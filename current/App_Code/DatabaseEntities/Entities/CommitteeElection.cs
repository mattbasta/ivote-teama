// CommitteeElection.cs
// Written by: Brian Fairservice
// Date Modified: 2/17/12
// TODO: Write static helper functions

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using NHibernate;
using NHibernate.Cfg;

namespace DatabaseEntities
{
    public enum ElectionPhase {WTSPhase,BallotPhase,VotePhase,
        ConflictPhase,ResultPhase};

    /// <summary>
    /// This class stores the information regarding an election to a committee.
    /// </summary>
    public class CommitteeElection
    {
        /// <summary>
        /// The unique, self-incrementing identifer of the election.
        /// </summary>
        public virtual int ID { get; private set; }
        /// <summary>
        /// The unique id of the committee this election pertains to.
        /// </summary>
        public virtual int Committee { get; set; }
        /// <summary>
        /// The date the election was started
        /// </summary>
        public virtual DateTime Started { get; set; }

        /// <summary>
        /// This value indicates the current phase of the election.
        /// </summary>
        public virtual ElectionPhase Phase { get; set; }

        /// <summary>
        /// This number indicates the number of positions which are open in the 
        /// committee this election pertains to.
        /// </summary>
        public virtual int VacanciesToFill {get; set;}

        /// <summary>
        /// This function sets the phase of the specified election.  You still 
        /// to call transaction.Commit() to ensure pending changes are saved.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="ID">The id of the election to edit.</param>
        /// <param name="electionPhase">The new phase of the election.</param>
        public static void SetPhase(ref ISession session, int ID, 
            ElectionPhase electionPhase)
        {
            // pull a list of all the committee elections from the database.
            var committees = session.CreateCriteria(typeof(CommitteeElection)).List<CommitteeElection>();
            for (int i = 0; i < committees.Count; i++)
            {
                // after searching through the list, if we've found the ID
                // of the comitttee election we want to edit, change the phase
                // to the one specified by the parameters, then save our changes.
                // then do some other stuff based on what the new phase is
                if (committees[i].ID == ID)
                {
                    committees[i].Phase = electionPhase;
                    session.SaveOrUpdate(committees[i]);

                    if (electionPhase == ElectionPhase.WTSPhase)
                    {
                        //TODO: Filter which users should be sent a WTS, instead of all.
                        List<User> userList = User.GetAllUsers(ref session);
                        nEmailHandler emailHandler = new nEmailHandler();
                        emailHandler.sendWTS(committees[i], userList);
                    }
                    else if (electionPhase == ElectionPhase.BallotPhase)
                    {
                        // not much needs to be done here
                    }
                    else if (electionPhase == ElectionPhase.VotePhase)
                    {
                        // distribute emails prompting faculty members to come 
                        // vote
                    }
                    else if (electionPhase == ElectionPhase.ConflictPhase)
                    {
                        // not much needs to be done here.
                    }
                    else if (electionPhase == ElectionPhase.ResultPhase)
                    {
                        // send out emails informing people of the results.
                        // send out emails teling NEC people to certify results
                    }

                    session.Flush();
                    return;
                }
            }
        }

        public static DateTime NextPhaseDate(ref ISession session, int ID)
        {
            // Grab the info about this committee election form the database.
            var committees = session.CreateCriteria(typeof(CommitteeElection)).List<CommitteeElection>();
            CommitteeElection election = null;
            for (int i = 0; i < committees.Count; i++)
            {
                if (committees[i].ID == ID)
                {
                    election = committees[i];
                    break;
                }
            }
            // If there was no election in the database with a matching id,
            // return MinValue (since the data will never be MinValue).
            if (election == null)
                return DateTime.MinValue;
            // People have two weeks to submit their WTS
            else if (election.Phase == ElectionPhase.WTSPhase)
                return election.Started.AddDays(14);
            // The NEC has one week to compose the ballot
            else if (election.Phase == ElectionPhase.BallotPhase)
                return election.Started.AddDays(14 + 7);
            // The voters have one week to cast their vote
            else if (election.Phase == ElectionPhase.VotePhase)
                return election.Started.AddDays(14 + 7);
            else
                // after that, there are no dead-line restrictions.
                return DateTime.MinValue;
        }

        /// <summary>
        /// Return what the next phase of the election ought to be.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="ID">The ID of the election.</param>
        public static ElectionPhase NextPhase(ref ISession session, int ID)
        {
            // pull a list of all the committee elections from the database.
            var committees = session.CreateCriteria(typeof(CommitteeElection)).List<CommitteeElection>();
            for (int i = 0; i < committees.Count; i++)
            {
                // find the election with the matching id
                if (committees[i].ID == ID)
                {
                    ElectionPhase toReturn = committees[i].Phase;
                    return ++toReturn;
                }
            }
            // Return if the specified election wasn't found.
            return 0;
        }

        /// <summary>
        /// Return a list of all nominations pertinent to this election.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="ID">The ID of the election.</param>
        /// <returns>A list of all nominations pertinent to the specified
        /// election, or an empty list.</returns>
        public static List<Nomination> GetNominations(ref ISession session,
            int ID)
        {
            var nominations = session.CreateCriteria(typeof(CommitteeElection)).List<Nomination>();
            List<Nomination> ret = new List<Nomination>();
            for (int i = 0; i < nominations.Count; i++)
            {
                if (nominations[i].Election == ID)
                    ret.Add(nominations[i]);
            }
            return ret;
        }

        /// <summary>
        /// This function returns a dictionary storing each candidate of this 
        /// and the number of votes that candidate one.  
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="ID">The ID of the election.</param>
        /// <returns>A dictionary storing each candidate and the
        /// number of votes they recieved. Dictionary keys are of the form 
        /// [User.FirstName + User.LastName].  The corresponding value
        /// is the number of votes.</returns>
        public static Dictionary<string, int> GetResults(ref ISession session,
            int ID)
        {
            var votes = session.CreateCriteria(typeof(BallotEntry)).List<BallotEntry>();
            Dictionary<string, int> ret = new Dictionary<string, int>();

            // Iterate through all the ballot entries
            for (int i = 0; i < votes.Count; i++)
            {
                // If the ballot entry is pertinent to this election
                if (votes[i].Election == ID)
                {
                    // Get the information for the candidate of this ballot entry
                    User thisUser = User.FindUser(ref session, votes[i].Candidate);
                    // If this candidate already has an entry in the return diciontary,
                    // increment the number of votes for him
                    if (ret.ContainsKey(thisUser.FirstName + thisUser.LastName))
                        ret[thisUser.FirstName + thisUser.LastName]++;
                    // Otherwise, add an entry for that candidate and set his number of votes
                    // to one
                    else
                        ret.Add(thisUser.FirstName + thisUser.LastName, 1);
                }
            }
            // After interating through all the votes, return the dictionary which
            // contains the candidates and their vote counts
            return ret;
        }

        /// <summary>
        ///  Marks a user as willing to serve in a committee election.
        ///  Don't forget to call transaction.Commit() to commit the pending
        ///  changes.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="candidate">The user ID who has submitted a WTS.</param>
        /// <param name="election">The pertinent election ID.</param>
        /// <param name="statement">THe statement provided by the user.</param>
        public static void WillingToServe(ref ISession session, int candidate, 
            int election, string statement)
        {
            CommitteeWTS toSubmit = new CommitteeWTS();
            toSubmit.Election = election;
            toSubmit.User = candidate;
            toSubmit.Statement = statement;

            session.SaveOrUpdate(toSubmit);
            session.Flush();
        }
    }
}

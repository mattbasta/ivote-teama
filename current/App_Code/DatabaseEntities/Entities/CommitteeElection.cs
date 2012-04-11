// CommitteeElection.cs
// Written by: Brian Fairservice
// Date Modified: 3/6/12
// TODO: Write static helper functions

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;

namespace DatabaseEntities
{
    public enum ElectionPhase
    {
        WTSPhase, NominationPhase, VotePhase, CertificationPhase,
        ConflictPhase, ClosedPhase
    };

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
        public virtual int PertinentCommittee { get; set; }

        /// <summary>
        /// The date the election was started
        /// </summary>
        public virtual DateTime Started { get; set; }

        /// <summary>
        /// The date the current phase was started.
        /// </summary>
        public virtual DateTime PhaseStarted { get; set; }

        /// <summary>
        /// This value indicates the current phase of the election.
        /// </summary>
        public virtual ElectionPhase Phase { get; set; }

        /// <summary>
        /// This number indicates the number of positions which are open in the
        /// committee this election pertains to.
        /// </summary>
        public virtual int VacanciesToFill { get; set; }

        /// <summary>
        /// This number represents how many days the administrator delayed or 
        /// rushed the election.
        /// </summary>
        public virtual int PhaseEndDelta { get; set; }

        /// <summary>
        /// The time that the delta was changed.
        /// </summary>

        // Methods
        /// <summary>
        /// Returns the election which is running for the specified committee.
        /// </summary>
        /// <param name="session">A valid session</param>
        /// <param name="committeeName">The name of the committee that the requested election is runnign for.</param>
        /// <returns>The CommitteeElection which is running for the specified election</returns>
        public static CommitteeElection FindElection(ISession session,
            string committeeName)
        {
            // get the committee object with the specified name
            Committee c = Committee.FindCommittee(session, committeeName);
            // now make a query for the election based off that committee's id
            var elections = session.CreateCriteria(typeof(CommitteeElection))
                .Add(Restrictions.Eq("PertinentCommittee", c.ID))
                .UniqueResult<CommitteeElection>();

            // and return it
            return elections;
        }

        /// <summary>
        /// Returns the election with the specified ID.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="id">The id of the election being searched for.</param>
        /// <returns>The CommitteeElection with the specified ID.</returns>
        public static CommitteeElection FindElection(ISession session,
            int id)
        {
            // form a query for the election with the given id
            var elections = session.CreateCriteria(typeof(CommitteeElection))
                .Add(Restrictions.Eq("ID", id))
                .UniqueResult<CommitteeElection>();

            // and return it
            return elections;
        }


        public virtual bool ShouldEnterNominationPhase(ISession session)
        {
            // if the number of WTS's submitted is less than twice the number of
            // vacnacies, no need for nomination phase
            return !(CommitteeWTS.FindCommitteeWTS(session, this.ID).Count <= this.VacanciesToFill * 2);
        }

        /// <summary>
        /// Creates a CommitteeElection object based off of a committee
        /// </summary>
        /// <param name="session">A valid session</param>
        /// <param name="committee">The committee the election pertains to</param>
        /// <returns>A committeeElection object to be saved to the database, or null if there were no vacancies</returns>
        public static CommitteeElection CreateElection(ISession session,
            Committee committee)
        {
            CommitteeElection ret = new CommitteeElection();
            ret.PertinentCommittee = committee.ID;
            ret.Started = DateTime.Now;
            ret.PhaseStarted = ret.Started;
            ret.Phase = ElectionPhase.WTSPhase;
            ret.VacanciesToFill = committee.NumberOfVacancies(session);
            ret.PhaseEndDelta = 0;
            // return null if there are no vacancies to fill or if there is
            // already an election for this committee
            if (ret.VacanciesToFill <= 0 || session.CreateCriteria(typeof(CommitteeElection))
                                                   .Add(Restrictions.Eq("PertinentCommittee", committee.ID))
                                                   .Add(Restrictions.Not(Restrictions.Eq("Phase", ElectionPhase.ClosedPhase)))
                                                   .UniqueResult<CommitteeElection>() != null)
                return null;
            else
                return ret;
        }
        /// <summary>
        /// This function sets the phase of the specified election.  You still
        /// to call transaction.Commit() to ensure pending changes are saved.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="id">The id of the election to edit.</param>
        /// <param name="electionPhase">The new phase of the election.</param>
        public virtual void SetPhase(ISession session,
            ElectionPhase electionPhase)
        {
            this.Phase = electionPhase;

            if (electionPhase == ElectionPhase.WTSPhase)
            {
                //TODO: Filter which users should be sent a WTS, instead of all.
                List<User> userList = User.GetAllUsers(session);
                nEmailHandler emailHandler = new nEmailHandler();
                emailHandler.sendWTS(this, userList);
            }
            else if (electionPhase == ElectionPhase.NominationPhase)
            {
                // distribute emails prompting faculty members to come
                // vote in the primary election
            }
            else if (electionPhase == ElectionPhase.VotePhase)
            {
                // distribute emails prompting faculty members to come
                // vote
            }
            else if (electionPhase == ElectionPhase.CertificationPhase)
            {
                // send out emails teling NEC people to certify results
            }
            else if (electionPhase == ElectionPhase.ConflictPhase)
            {
                ConflictLogic(session);
                // maybe send out emails telling admins / NEC that there are conflicts?
            }
            else if (electionPhase == ElectionPhase.ClosedPhase)
            {
                // not much needs to be done here.
            }
            // Store the current date in the PhaseStarted field
            this.PhaseStarted = DateTime.Now;
            this.PhaseEndDelta = 0;

            session.SaveOrUpdate(this);
            session.Flush();
        }

        public virtual DateTime NextPhaseDate(ISession session)
        {
            // People have two weeks to submit their WTS
            if (this.Phase == ElectionPhase.WTSPhase)
                return this.PhaseStarted.AddDays(14 + PhaseEndDelta);
            else if (this.Phase == ElectionPhase.NominationPhase)
                return this.PhaseStarted.AddDays(7 + PhaseEndDelta);
            // The voters have one week to cast their vote
            else if (this.Phase == ElectionPhase.VotePhase)
                return this.PhaseStarted.AddDays(7 + PhaseEndDelta);
            else
                // after that, there are no dead-line restrictions.
                return DateTime.MaxValue;
        }

        /// <summary>
        /// Return what the next phase of the election ought to be.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="ID">The ID of the election.</param>
        public virtual ElectionPhase NextPhase(ISession session)
        {
            ElectionPhase toReturn = this.Phase;
            if(toReturn != ElectionPhase.ClosedPhase)
                toReturn += 1;
            if (toReturn == ElectionPhase.NominationPhase &&
                this.ShouldEnterNominationPhase(session) == false)
                return ElectionPhase.VotePhase;
            else if (toReturn == ElectionPhase.ConflictPhase &&
                     ElectionConflict.FindElectionConflicts(session, ID).Count == 0)
                return ElectionPhase.ClosedPhase;
            else
                return toReturn;
        }

        /// <summary>
        /// Return a list of all nominations pertinent to this election.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="ID">The ID of the election.</param>
        /// <returns>A list of all nominations pertinent to the specified
        /// election, or an empty list.</returns>
        public static List<Nomination> GetNominations(ISession session,
            int id)
        {
            var nominations = session.CreateCriteria(typeof(Nomination))
                .Add(Restrictions.Eq("Election", id))
                .List<Nomination>();
            return nominations.ToList<Nomination>();
        }

        /// <summary>
        /// This function returns a dictionary storing each candidate of this
        /// and the number of votes that candidate one.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="ID">The ID of the election.</param>
        /// <returns>A dictionary storing each candidate and the
        /// number of votes they recieved. Dictionary keys are the user's email.  
        /// The corresponding value is the number of votes.</returns>
        public static Dictionary<string, int> GetResults(ISession session,
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
                    User thisUser = User.FindUser(session, votes[i].Candidate);
                    // If this candidate already has an entry in the return diciontary,
                    // increment the number of votes for him
                    if (ret.ContainsKey(thisUser.Email))
                        ret[thisUser.Email]++;
                    // Otherwise, add an entry for that candidate and set his number of votes
                    // to one
                    else
                        ret.Add(thisUser.Email, 1);
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
        public static void WillingToServe(ISession session, int candidate,
            int election, string statement)
        {
            CommitteeWTS toSubmit = new CommitteeWTS();
            toSubmit.Election = election;
            toSubmit.User = candidate;
            toSubmit.Statement = statement;

            session.SaveOrUpdate(toSubmit);
            session.Flush();
        }

        /// <summary>
        /// Adds conflicts to the database depending upon the current state of the election.
        /// </summary>
        /// <param name="session">A valid session.</param>
        protected virtual void ConflictLogic(ISession session)
        {
            ITransaction transaction = session.BeginTransaction();
            // Get the current committee
            Committee committee = Committee.FindCommittee(session, PertinentCommittee);

            // Get the users who won the election.
            Dictionary<string, int> winners = GetResults(session, ID);
            List<User> winningUsers = new List<User>();
            foreach (string email in winners.Keys)
            {
                winningUsers.Add(User.FindUser(session, email));
            }

            // Get the users on the committee.
            List<User> members = User.FindUsers(session, committee.Name);

            // List all of the departments currently present on the committee.
            // and use a parallel list to store the ID of the other department
            // member so we can add it to the conflict later.
            List<DepartmentType> departments = new List<DepartmentType>();
            List<int> secID = new List<int>();
            foreach (User i in members)
            {
                departments.Add(i.Department);
                secID.Add(i.ID);
            }

            // For each user who won, add a new conflict if their department
            // is already present on the list. Adding, departments as we go.
            // Also raise conflicts if the winning users hold officer positions,
            // or if they already serve on a committee.
            foreach (User i in winningUsers)
            {
                // check if they have a prior committment
                if (i.OfficerPosition != OfficerPositionType.None ||
                    i.CurrentCommittee != User.NoCommittee)
                {
                    ElectionConflict conflict = new ElectionConflict();
                    conflict.Election = ID;
                    conflict.FirstUser = i.ID;
                    conflict.SecUser = ElectionConflict.SecondUserNotApplicable;
                    conflict.Type = ConflictType.ElectedToMultipleCommittees;
                    session.SaveOrUpdate(conflict);
                }

                // check for department-based conflicts
                if(departments.Contains(i.Department))
                {
                    ElectionConflict conflict = new ElectionConflict();
                    conflict.Election = ID;
                    conflict.FirstUser = i.ID;
                    conflict.SecUser = secID[departments.IndexOf(i.Department)];
                    conflict.Type = ConflictType.TooManyDeptMembers;
                    session.SaveOrUpdate(conflict);
                }
            }
            session.Flush();
            NHibernateHelper.Finished(transaction);
        }

        /// <summary>
        /// Returns a list of all the users who were nominated for this election
        /// after accounting for the results of the primary election.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <returns>A list of all the nominees for this election.</returns>
        public virtual List<User> GetNominees(ISession session)
        {
            // Get users for each election
            List<User> users = DatabaseEntities.User.FindUsers(session, ID);
            Dictionary<int, int> nomCount = new Dictionary<int, int>();

            // Count nominations for each user.
            foreach (DatabaseEntities.User aUser in users)
                nomCount.Add(aUser.ID, 0);

            List<CommitteeWTSNomination> nominations =
                CommitteeWTSNomination.FindCommitteeWTSNominations(session, ID);
            foreach (CommitteeWTSNomination nom in nominations)
                nomCount[nom.Candidate]++;

            // the max amount of nominees is twice the number of vacancies
            // so find the twice-the-number-of-vacancies-highest number
            List<int> count = nomCount.Values.ToList();
            Committee committee = Committee.FindCommittee(session, this.PertinentCommittee);
            count.Sort();
            count.Reverse();
            
            int cutOff = -1;
            if (count.Count != 0) // vacancies times 2 minus 1 to make it zero based...
            {
                int num_vacs = committee.NumberOfVacancies(session) * 2 - 1;
                if(num_vacs > count.Count)
                    cutOff = -1;
                else
                    cutOff = count[num_vacs];
            }
            
            // Only add users to the list of nominees if they surpass the cutoff value
            List<User> ret = new List<User>();
            foreach (User user in users)
            {
                if (nomCount[user.ID] >= cutOff)
                    ret.Add(user);
            }
            return ret;
        }

        /// <summary>
        /// Removes the CommitteeWTS, CommitteeWTSNominations and BallotEntry
        /// objects which pertain to a user who had their WTS revoked.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="transaction">A transaction to perform the operation in.</param>
        /// <param name="user">The user who had their WTS revoked.</param>
        public virtual void RevokeWTS(ISession session, ITransaction transaction, int user)
        {
            // Find the committeeWTS.
            CommitteeWTS cWTS = CommitteeWTS.FindCommitteeWTS(session, ID, user);
            NHibernateHelper.Delete(session, cWTS);

            // Find all the WTSNominations
            List<CommitteeWTSNomination> cWTSNominations =
                CommitteeWTSNomination.FindCommitteeWTSNominations(session, ID);
            foreach (CommitteeWTSNomination nomination in cWTSNominations)
            {
                if (nomination.Candidate == user)
                    NHibernateHelper.Delete(session, nomination);
            }

            // Find all the BallotEntries
            List<BallotEntry> ballotEntries = 
                BallotEntry.FindBallotEntry(session, ID);
            foreach (BallotEntry entry in ballotEntries)
            {
                if (entry.Candidate == user)
                    NHibernateHelper.Delete(session, entry);
            }
        }

        /// <summary>
        /// Removes the CommitteeWTS, CommitteeWTSNominations and BallotEntry
        /// objects for all users, then destroys the election.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="transaction">A transaction to perform the operation in.</param>
        public virtual void DestroyElection(ISession session, ITransaction transaction)
        {
            // Find the committeeWTS.
            List<CommitteeWTS> cWTSes = CommitteeWTS.FindCommitteeWTS(session, ID);
            foreach(CommitteeWTS cWTS in cWTSes)
                NHibernateHelper.Delete(session, cWTS);

            // Find all the WTSNominations
            List<CommitteeWTSNomination> cWTSNominations =
                CommitteeWTSNomination.FindCommitteeWTSNominations(session, ID);
            foreach (CommitteeWTSNomination nomination in cWTSNominations)
                NHibernateHelper.Delete(session, nomination);

            // Find all the BallotEntries
            List<BallotEntry> ballotEntries = 
                BallotEntry.FindBallotEntry(session, ID);
            foreach (BallotEntry entry in ballotEntries)
            {
                NHibernateHelper.Delete(session, entry);
            }
            
            // Delete the election.
            NHibernateHelper.Delete(session, this);
        }
    }
}

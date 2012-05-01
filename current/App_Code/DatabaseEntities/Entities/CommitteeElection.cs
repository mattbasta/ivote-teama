// CommitteeElection.cs
// Written by: Brian Fairservice
// Date Modified: 3/6/12
// TODO: Write static helper functions

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using iTextSharp.text;
using iTextSharp.text.pdf;

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
        /// A boolean value representing whether the election is a special
        /// election.
        /// </summary>
        public virtual bool SpecialElection { get; set; }

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
            Committee com = Committee.FindCommittee(session, PertinentCommittee);

            if (electionPhase == ElectionPhase.WTSPhase)
            {
                List<User> userList = User.GetAllUsers(session);
                userList.RemoveAll(x => (!(!com.TenureRequired || x.IsTenured) ||
                                        !(!com.BargainingUnitRequired || x.IsBargainingUnit)));
                userList.RemoveAll(x => (x.CurrentCommittee == com.ID));
                nEmailHandler emailHandler = new nEmailHandler();
                emailHandler.sendGenericCommitteePhase(this, userList, "committeePhaseWTS");
            }
            else if (electionPhase == ElectionPhase.NominationPhase)
            {
                List<User> userList = User.GetAllUsers(session);

                nEmailHandler emailHandler = new nEmailHandler();
                userList.RemoveAll(x => (!x.CanVote));
                emailHandler.sendGenericCommitteePhase(this, userList, "committeePhaseNomination");
            }
            else if (electionPhase == ElectionPhase.VotePhase)
            {
                // if theres no need to enter the nomination phase,
                // automatically enter nominations for users who 
                //submitted wts
                if (!ShouldEnterNominationPhase(session))
                {
                    List<CommitteeWTS> wtses = CommitteeWTS.FindCommitteeWTS(session, ID);
                    foreach (CommitteeWTS wts in wtses)
                    {
                        ISession nomSession = NHibernateHelper.CreateSessionFactory().OpenSession();
                        Nomination nomination = new Nomination();
                        nomination.User = wts.User;
                        nomination.Election = ID;
                        nomSession.SaveOrUpdate(nomination);
                        nomSession.Flush();
                    }
                }
                // distribute emails prompting faculty members to come
                List<User> userList = User.GetAllUsers(session);
                userList.RemoveAll(x => (!(x.CanVote || x.IsAdmin)));

                nEmailHandler emailHandler = new nEmailHandler();
                emailHandler.sendGenericCommitteePhase(this, userList, "committeePhaseVote");
                // vote
            }
            else if (electionPhase == ElectionPhase.CertificationPhase)
            {
                ConflictLogic(session); // Get an idea of what the conflicts will be.
                List<User> userList = User.GetAllUsers(session);
                userList.RemoveAll(x => (!(x.IsNEC || x.IsAdmin)));

                nEmailHandler emailHandler = new nEmailHandler();
                emailHandler.sendGenericCommitteePhase(this, userList, "committeePhaseCertification");
            }
            else if (electionPhase == ElectionPhase.ConflictPhase)
            {
                ConflictLogic(session); // Re-calculate in case the admin fixed it up.
                // maybe send out emails telling admins / NEC that there are conflicts?
                List<User> userList = User.GetAllUsers(session);
                userList.RemoveAll(x => (!(x.IsNEC || x.IsAdmin)));

                nEmailHandler emailHandler = new nEmailHandler();
                emailHandler.sendGenericCommitteePhase(this, userList, "committeePhaseConflict");
            }
            else if (electionPhase == ElectionPhase.ClosedPhase)
            {
                // Put the users into the correct committee.
                Dictionary<string, int> winners = GetResults(session);
                List<User> winningUsers = new List<User>();
                foreach (string email in winners.Keys) {
                    User u = User.FindUser(session, email);
                    u.CurrentCommittee = PertinentCommittee;
                    session.SaveOrUpdate(u);
                }
            }
            // Store the current date in the PhaseStarted field
            this.PhaseStarted = DateTime.Now;
            this.PhaseEndDelta = 0;

            session.SaveOrUpdate(this);
            session.Flush();
        }

        public virtual DateTime NextPhaseDate(ISession session, bool real)
        {
            int delta = real ? 0 : PhaseEndDelta;
            // People have two weeks to submit their WTS
            if (this.Phase == ElectionPhase.WTSPhase)
                return this.PhaseStarted.AddDays(14 + delta);
            else if (this.Phase == ElectionPhase.NominationPhase)
                return this.PhaseStarted.AddDays(7 + delta);
            // The voters have one week to cast their vote
            else if (this.Phase == ElectionPhase.VotePhase)
                return this.PhaseStarted.AddDays(7 + delta);
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
        public virtual Dictionary<string, int> GetResults(ISession session)
        {
            var votes = session.CreateCriteria(typeof(BallotEntry))
                .Add(Restrictions.Eq("Election", this.ID))
                .List<BallotEntry>();
            Dictionary<string, int> ret = new Dictionary<string, int>();

            List<User> nominees = GetNominees(session);

            foreach (User i in nominees)
                ret.Add(i.Email, 0);

            Dictionary<int, string> emails = new Dictionary<int, string>();

            foreach (User i in nominees)
                emails.Add(i.ID, i.Email);

            // Iterate through all the ballot entries
            for (int i = 0; i < votes.Count; i++)
                // Increment the number of votes for the specified user.
                ret[emails[votes[i].Candidate]]++;

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
            List<ElectionConflict> conflicts = ElectionConflict.FindElectionConflicts(session, ID);
            foreach (ElectionConflict conflict in conflicts)
                NHibernateHelper.Delete(session, conflict);

            ITransaction transaction = session.BeginTransaction();
            // Get the current committee
            Committee committee = Committee.FindCommittee(session, PertinentCommittee);

            // Get the users who won the election.
            Dictionary<string, int> winners = GetResults(session);
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

            //List departments of nominees
            List<DepartmentType> departmentsWinning = new List<DepartmentType>();
            List<int> secIDWinning = new List<int>();
            foreach (User i in winningUsers)
            {
                departmentsWinning.Add(i.Department);
                secIDWinning.Add(i.ID);
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
                if (departments.Contains(i.Department))
                {
                    ElectionConflict conflict = new ElectionConflict();
                    conflict.Election = ID;
                    conflict.FirstUser = i.ID;
                    conflict.SecUser = secID[departments.IndexOf(i.Department)];
                    conflict.Type = ConflictType.TooManyDeptMembers;
                    session.SaveOrUpdate(conflict);
                }

                // check for department-based conflicts - nominees
                if (departmentsWinning.Contains(i.Department) && i.ID != secIDWinning[departmentsWinning.IndexOf(i.Department)])
                {
                    ElectionConflict conflict = new ElectionConflict();
                    conflict.Election = ID;
                    conflict.FirstUser = i.ID;
                    conflict.SecUser = secIDWinning[departmentsWinning.IndexOf(i.Department)];
                    secIDWinning.Remove(conflict.SecUser);
                    departmentsWinning.Remove(i.Department);
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
                if(aUser != null)
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
                if(num_vacs >= count.Count)
                    cutOff = -1;
                else
                    cutOff = count[Math.Max(num_vacs, count.Count - 1)];
            }
            
            // Only add users to the list of nominees if they surpass the cutoff value
            List<User> ret = new List<User>();
            foreach (User user in users)
            {
                if(user == null)
                    continue;
                if (nomCount[user.ID] >= cutOff)
                    ret.Add(user);
            }
            return ret;
        }
        
        public virtual int DaysRemainingInPhase(ISession session) {
            return (int)this.NextPhaseDate(session, false).Subtract(PhaseStarted).TotalDays;
        }
        
        public virtual int RealDaysRemainingInPhase(ISession session) {
            return (int)this.NextPhaseDate(session, true).Subtract(PhaseStarted).TotalDays;
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
            List<BallotEntry> ballotEntries = BallotEntry.FindBallotEntry(session, ID);
            foreach (BallotEntry entry in ballotEntries)
                NHibernateHelper.Delete(session, entry);

            // Find all the BallotEntries
            List<BallotFlag> ballotFlags = BallotFlag.FindBallotFlags(session, ID);
            foreach (BallotFlag flag in ballotFlags)
                NHibernateHelper.Delete(session, flag);
            
            // Find all the conflicts
            List<ElectionConflict> conflicts = ElectionConflict.FindElectionConflicts(session, ID);
            foreach (ElectionConflict conflict in conflicts)
                NHibernateHelper.Delete(session, conflict);
            
            // Delete the election.
            NHibernateHelper.Delete(session, this);
        }
        
        /// <summary>
        /// Generates a pdf of election results for this election.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="path">The path where the pdf will be saved.</param>
        /// <returns>The document which was created.</returns>
        public virtual Document GenerateResultsPDF(ISession session, string path)
        {
            Committee committee = Committee.FindCommittee(session, 
                PertinentCommittee);
            List<Certification> certifications = Certification.FindCertifications(session, ID);
            Dictionary<string, int> users = GetResults(session);

            var doc = new Document();
            PdfWriter.GetInstance(doc, 
                new FileStream(path, FileMode.Create));

            PdfPTable table = new PdfPTable(2);
            table.SpacingBefore = 30f;
            table.SpacingAfter = 30f;

            table.AddCell("Candidate");
            table.AddCell("Number of Votes");

            foreach (KeyValuePair<string, int> i in users)
            {
                User user = User.FindUser(session, i.Key);
                table.AddCell(user.FirstName + " " + user.LastName);
                table.AddCell(i.Value.ToString());
            }

            doc.Open();
            Font header = FontFactory.GetFont("Arial", 24, BaseColor.BLACK);
            doc.Add(new Phrase("APSCUF-KU Election Results\n", header));
            
            // The semester the election was started during
            string semester = "nothing";

            // The fall semester is between august 27 and december 17
            if (Started >= new DateTime(Started.Year, 8, 27) &&
                Started <= new DateTime(Started.Year, 12, 17))
                semester = " held during the Fall " + Started.Year.ToString() + " semester.";
            // The spring semester is between january 22 and may 12
            else if (Started >= new DateTime(Started.Year, 1, 22) &&
                Started <= new DateTime(Started.Year, 5, 12))
                semester = " held during the Spring " + Started.Year.ToString() + " semester.";
            else // otherwise just say what date it started.
                semester = " started on " + Started.ToString("d") + "."; 

            doc.Add(new Paragraph("The following results were collected during an election held to fill " + committee.NumberOfVacancies(session).ToString() + " vacancies in the " + committee.Name + semester));
            doc.Add(table);

            foreach(Certification i in certifications)
            {
                User certifyingUser = User.FindUser(session, i.User);
                Chunk sigLine = new Chunk("                                                  \n");
                sigLine.SetUnderline(0.5f, -1.5f);
                Phrase signatureArea = new Phrase();
                signatureArea.Add("I hereby certify the results of this election:\n");
                signatureArea.Add(sigLine);
                signatureArea.Add(certifyingUser.FirstName + " " + certifyingUser.LastName + "\n");
                signatureArea.Add(sigLine);
                signatureArea.Add("Date\n\n");

                doc.Add(signatureArea);
            }
            doc.Close();
            return doc;
        }

        /// <summary>
        /// Returns a list of all CommitteeWTS objects pertinent to a given election.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="election">The election id.</param>
        /// <returns>A list of all the CommitteeWTS objects pertinent to the specified election.</returns>
        public virtual List<CommitteeWTS> Nominees(ISession session)
        {
            // pull a list of all the Committee WTS' for the given election from the database.
            List<CommitteeWTS> wtses = session.CreateCriteria(typeof(CommitteeWTS))
                            .Add(Restrictions.Eq("Election", ID))
                            .List<CommitteeWTS>().ToList<CommitteeWTS>();
            List<CommitteeWTS> final_wtses = new List<CommitteeWTS>();
            foreach(CommitteeWTS wts in wtses)
                if(User.FindUser(session, wts.User) != null)
                    final_wtses.Add(wts);
            
            // return that list
            return final_wtses;
        }

    }
}

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

using NUnit.Framework;

namespace DatabaseEntities
{
    [TestFixture]
    class FunctionalTests
    {
        /// <summary>
        ///  totally clears everything in the database
        /// </summary>
        private static void Clear(ISession session, ITransaction transaction)
        {
            Console.Write("Clearing database.\n");
            // Clear out all the BallotEntrys
            var be = session.CreateCriteria(typeof(BallotEntry)).List<BallotEntry>();
            for (int i = 0; i < be.Count; i++)
            {
                NHibernateHelper.Delete(session, be[i]);
            }
            // Clear out all the BallotFlags
            var bf = session.CreateCriteria(typeof(BallotFlag)).List<BallotFlag>();
            for (int i = 0; i < bf.Count; i++)
            {
                NHibernateHelper.Delete(session, bf[i]);
            }
            // Clear out all the Commitees
            var c = session.CreateCriteria(typeof(Committee)).List<Committee>();
            for (int i = 0; i < c.Count; i++)
            {
                NHibernateHelper.Delete(session, c[i]);
            }
            // Clear out all the CommiteeElections
            var ce = session.CreateCriteria(typeof(CommitteeElection)).List<CommitteeElection>();
            for (int i = 0; i < ce.Count; i++)
            {
                NHibernateHelper.Delete(session, ce[i]);
            }
            // Clear out all the CommiteeWTSs
            var cwts = session.CreateCriteria(typeof(CommitteeWTS)).List<CommitteeWTS>();
            for (int i = 0; i < cwts.Count; i++)
            {
                NHibernateHelper.Delete(session, cwts[i]);
            }
            // Clear out all the CommiteeWTSNominations
            var cwtsn = session.CreateCriteria(typeof(CommitteeWTSNomination)).List<CommitteeWTSNomination>();
            for (int i = 0; i < cwtsn.Count; i++)
            {
                NHibernateHelper.Delete(session, cwtsn[i]);
            }
            // Clear out all the Nominations
            var n = session.CreateCriteria(typeof(Nomination)).List<Nomination>();
            for (int i = 0; i < n.Count; i++)
            {
                NHibernateHelper.Delete(session, n[i]);
            }
            // Clear out all the Users
            var u = session.CreateCriteria(typeof(User)).List<User>();
            for (int i = 0; i < u.Count; i++)
            {
                NHibernateHelper.Delete(session, u[i]);
            }
            transaction.Commit();
        }
        /// <summary>
        /// To test that committee elections can be initiated.
        /// </summary>
        /// <returns>True if the test succeeded, false otherwise</returns>
        [Test]
        public static void InitiateCommitteeElection()
        {
            Console.Write("Initial Committee Election:\n");
            // Set up pre-conditions
            ISessionFactory factory = NHibernateHelper.CreateSessionFactory();
            using (ISession session = factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    // clear the databse first
                    Clear(session, transaction);
                }
                using (ITransaction transaction = session.BeginTransaction())
                {
                    // add an admin user
                    Console.Write("Create admin user.\n");
                    User user = User.CreateUser("e", "f", "l", "p", "h", true, false,
                        false, false, false, DepartmentType.CSC, 
                        OfficerPositionType.None, false, -1);
                    session.SaveOrUpdate(user);

                    // create a committee: 4 positions, 3 filled
                    Console.Write("Create committee with 4 positions.\n");
                    Committee com = new Committee();
                    com.Name = "Acommittee";
                    com.PositionCount = 4;
                    session.SaveOrUpdate(com);

                    // add 3 users who are tenured and union members who are on
                    // the committee
                    Console.Write("Create 3 users and give them positions in the committee.\n");
                    User[] users = new User[3];
                    for (int i = 0; i < 3; i++)
                    {
                        users[i] = User.CreateUser("e", i.ToString() + "F",
                            i.ToString() + "L", "p", "h", false, false, true, true, 
                            false, DepartmentType.CSC, OfficerPositionType.None, true,
                            com.ID);
                        session.SaveOrUpdate(users[i]);
                    }
                    transaction.Commit();
                }
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Console.Write("Create an election based off the committee.\n");
                    Committee com = Committee.FindCommittee(session, "Acommittee");
                    CommitteeElection election = 
                        CommitteeElection.CreateElection(session, com);
                    session.SaveOrUpdate(election);
                    transaction.Commit();
                }
            }
            // Assertions
            using (ISession session = factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Console.Write("Election object's phase is WTSPhase: ");
                    Assert.AreEqual(ElectionPhase.WTSPhase, CommitteeElection.FindElection(session, "Acommittee").Phase);

                    int vacancies = CommitteeElection.FindElection(session, "Acommittee").VacanciesToFill;
                    Assert.AreEqual(1, vacancies);
                    
                    // Put email stuff here
                }
            }
        }

        /// <summary>
        /// To test that invalid elections cannot be created
        /// </summary>
        /// <returns>True if the test succeeded, flase otherwise</returns>
        [Test]
        public static void TestCommitteeCreationFailure()
        {
            Console.Write("Test Committee Election Failure:\n");
            ISessionFactory factory = NHibernateHelper.CreateSessionFactory();
            // Clear out database
            using (ISession session = factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Clear(session, transaction);
                }
            }

            // Pre-requisites
            using (ISession session = factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    // create a committee: 3 positions, 3 filled
                    Console.Write("Create committee with 3 positions.\n");
                    Committee com = new Committee();
                    com.Name = "Acommittee";
                    com.PositionCount = 3;
                    session.SaveOrUpdate(com);
                    // add 3 users who are tenured and union members who are on
                    // the committee
                    Console.Write("Create 3 users and give them positions in the committee.\n");
                    User[] users = new User[3];
                    for (int i = 0; i < 3; i++)
                    {
                        users[i] = User.CreateUser("e", i.ToString() + "F",
                            i.ToString() + "L", "p", "h", false, false, true, true,
                            false, DepartmentType.CSC, OfficerPositionType.None, true,
                            com.ID);
                        session.SaveOrUpdate(users[i]);
                    }
                    transaction.Commit();
                }
                // Assertions
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Console.Write("Attempting to create an election based off the committee.\n");
                    Committee com = Committee.FindCommittee(session, "Acommittee");
                    CommitteeElection election =
                        CommitteeElection.CreateElection(session, com);
                    Console.Write("CommitteeElection value (should be null): ");
                    Assert.AreEqual(null, election);
                }
            }
        }

        /// <summary>
        /// To test that users can be added to committees
        /// </summary>
        /// <returns>True if the test succeeded, false otherwise</returns>
        [Test]
        public static void AddMembersToCommittee()
        {
            Console.Write("Add members to committee:\n"); 
            
            ISessionFactory factory = NHibernateHelper.CreateSessionFactory();
            // Clear out database
            using (ISession session = factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Clear(session, transaction);
                }
            }
            
            // Pre-requisites
            using (ISession session = factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    // create a committee
                    Console.Write("Creat committee with 2 positions.\n");
                    Committee com = new Committee();
                    com.Name = "Acommittee";
                    com.PositionCount = 2;
                    session.SaveOrUpdate(com);

                    // add 2 users who are tenured and union members who are on
                    // the committee
                    Console.Write("Create 2 users and add them to the committee.\n");
                    for (int i = 0; i < 2; i++)
                    {
                        User user = User.CreateUser("e", i.ToString() + "F",
                            i.ToString() + "L", "p", "h", false, false, true, true,
                            false, DepartmentType.CSC, OfficerPositionType.None, true,
                            User.NoCommittee);
                        session.SaveOrUpdate(user);
                        user.AddToCommittee(session, "Acommittee");
                    }
                    transaction.Commit();
                }
            }

            // Assertions
            using (ISession session = factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    // assert that both users' committee fields reference the
                    // original committee
                    List<User> users = User.GetAllUsers(session);
                    Committee a = Committee.FindCommittee(session, "Acommittee");
                    Assert.AreEqual(a.ID, users[0].CurrentCommittee);
                    Assert.AreEqual(a.ID, users[1].CurrentCommittee);

                    Assert.AreEqual(0, a.NumberOfVacancies(session));
                    Assert.AreEqual(2, a.NumberOfPositions(session));
                }
            }
        }

        /// <summary>
        /// To test that users can be added to committees
        /// </summary>
        /// <returns>True if the test succeeded, false otherwise</returns>
        [Test]
        public static void AddMembersToSingleCommitteeOnly()
        {
            Console.Write("Add Members to Single Committee Only:\n");
            ISessionFactory factory = NHibernateHelper.CreateSessionFactory();
            // Clear out database
            using (ISession session = factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Clear(session, transaction);
                }
            }
            // Pre-requisites
            using (ISession session = factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    // create 1st committee
                    Console.Write("Create committee with 2 positions.\n");
                    Committee com = new Committee();
                    com.Name = "Acommittee";
                    com.PositionCount = 2;
                    session.SaveOrUpdate(com);

                    // add 2 users who are tenured and union members who are on
                    // the committee
                    Console.Write("Create 2 users and add them to the first committee.\n");
                    User[] users = new User[2];
                    for (int i = 0; i < 2; i++)
                    {
                        users[i] = User.CreateUser("e", i.ToString() + "F",
                            i.ToString() + "L", "p", "h", false, false, true, true,
                            false, DepartmentType.CSC, OfficerPositionType.None, true,
                            User.NoCommittee);
                        session.SaveOrUpdate(users[i]);
                        users[i].AddToCommittee(session, "Acommittee");
                    }

                    // create 2nd committee
                    Console.Write("Create another committee with 2 positions.\n");
                    Committee bcom = new Committee();
                    bcom.Name = "Bcommittee";
                    bcom.PositionCount = 2;
                    session.SaveOrUpdate(bcom);

                    transaction.Commit();
                }
            }
            // More pre-requisites
            using (ISession session = factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Console.Write("Try to add one of the users to the second committee.\n");
                    List<User> users = User.GetAllUsers(session);
                    User toAdd = users[0];
                    toAdd.AddToCommittee(session, "Bcommittee");
                    transaction.Commit();
                }
            }
            // Assertions
            using (ISession session = factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    // assert that both users' committee fields reference the
                    // original committee
                    List<User> users = User.GetAllUsers(session);
                    Committee a = Committee.FindCommittee(session, "Acommittee"),
                        b = Committee.FindCommittee(session, "Bcommittee");

                    Assert.AreEqual(a.ID, users[0].CurrentCommittee);
                    Assert.AreEqual(a.ID, users[1].CurrentCommittee);

                    Assert.AreEqual(0, a.NumberOfVacancies(session));
                    Assert.AreEqual(2, b.NumberOfVacancies(session));

                    Assert.AreEqual(2, a.NumberOfPositions(session));
                    Assert.AreEqual(0, b.NumberOfPositions(session));
                }
            }
        }

        /// <summary>
        /// To test that appropriate users can register as willing to serve.
        /// </summary>
        /// <returns>True if the test succeeded, false otherwise</returns>
        [Test]
        public static void RemoveMembersFromCommittees()
        {
            Console.Write("Remove members from committee:\n");

            ISessionFactory factory = NHibernateHelper.CreateSessionFactory();
            // Clear out database
            using (ISession session = factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Clear(session, transaction);
                }
            }
            // Pre-requisites
            using (ISession session = factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    // create a committee
                    Console.Write("Create committee with 2 positions.\n");
                    Committee com = new Committee();
                    com.Name = "Acommittee";
                    com.PositionCount = 2;
                    session.SaveOrUpdate(com);

                    // add 2 users who are tenured and union members who are on
                    // the committee
                    Console.Write("Create 2 users and add them to the committee.\n");
                    for (int i = 0; i < 2; i++)
                    {
                        User user = User.CreateUser("e", i.ToString() + "F",
                            i.ToString() + "L", "p", "h", false, false, true, true,
                            false, DepartmentType.CSC, OfficerPositionType.None, true,
                            User.NoCommittee);
                        session.SaveOrUpdate(user);
                        user.AddToCommittee(session, "Acommittee");
                    }
                    transaction.Commit();
                }
            }
            // More pre-requisites
            using (ISession session = factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Console.Write("Remove one of the users.\n");
                    List<User> users = User.GetAllUsers(session);
                    Committee c = Committee.FindCommittee(session, "Acommittee");
                    Committee.RemoveMember(session, users[0], c.ID);
                    session.SaveOrUpdate(users[0]);
                    transaction.Commit();
                }
            }
            // Assertions
            using (ISession session = factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    List<User> users = User.GetAllUsers(session);
                    Assert.AreEqual(User.NoCommittee, users[0].CurrentCommittee);

                    Assert.AreEqual(1, Committee.FindCommittee(session, "Acommittee").NumberOfVacancies(session));
                }
            }
        }

        /// <summary>
        /// To test that next_phase() produces the correct results when few enough users submit a WTS form to have a nomination phase.
        /// </summary>0
        /// <returns>True if the test succeeded, false otherwise</returns>
        [Test]
        public static void NominationPhaseSkipped()
        {
            Console.Write("Nomination Phase Skipped:\n");

            ISessionFactory factory = NHibernateHelper.CreateSessionFactory();
            // Clear out database
            using (ISession session = factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Clear(session, transaction);
                }
            }

            // Preconditions
            using (ISession session = factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    // create a committee
                    Console.Write("Create committee with 3 positions.\n");
                    Committee com = new Committee();
                    com.Name = "Acommittee";
                    com.PositionCount = 3;
                    session.SaveOrUpdate(com);

                    // add 2 users who are tenured and union members who are on
                    // the committee
                    Console.Write("Create 5 users and add 2 of them to the committee.\n");
                    for (int i = 0; i < 5; i++)
                    {
                        User user = User.CreateUser("e", i.ToString() + "F",
                            i.ToString() + "L", "p", "h", false, false, true, true,
                            false, DepartmentType.CSC, OfficerPositionType.None, true,
                            User.NoCommittee);
                        session.SaveOrUpdate(user);
                        if(i < 2)
                            user.AddToCommittee(session, "Acommittee");
                    }
                    // create an election for the committee
                    Console.Write("Create an election based off the committee.\n");
                    CommitteeElection election = 
                        CommitteeElection.CreateElection(session, com);
                    session.SaveOrUpdate(election);

                    session.Flush();

                    // submit WTS for 2 users
                    Console.Write("Submitting WTSes for 2 users.");
                    for (int i = 2; i < 4; i++)
                    {
                        CommitteeWTS wts = new CommitteeWTS();
                        wts.Election = election.ID;
                        wts.Statement = "thisisastatement.";
                        wts.User = i;
                        session.SaveOrUpdate(wts);
                        session.Flush();
                    }
                    session.Flush();
                    transaction.Commit();
                }
            }

            // Assertions
            using (ISession session = factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    CommitteeElection election = CommitteeElection.FindElection(session, "Acommittee");
                    ElectionPhase phase = election.NextPhase(session);

                    Assert.AreEqual(phase, ElectionPhase.VotePhase);
                }
            }
        }

        /// <summary>
        /// To test that next_phase() produces the correct results when enough WTS forms are submitted for a nomination phase to be held.
        /// </summary>
        /// <returns>True if the test succeeded, false otherwise</returns>
        [Test]
        public static void NominationPhaseNotSkipped()
        {
            Console.Write("Nomination Phase Not Skipped:\n");

            ISessionFactory factory = NHibernateHelper.CreateSessionFactory();
            // Clear out database
            using (ISession session = factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Clear(session, transaction);
                }
            }

            // Preconditions
            using (ISession session = factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    // create a committee
                    Console.Write("Create committee with 3 positions.\n");
                    Committee com = new Committee();
                    com.Name = "Acommittee";
                    com.PositionCount = 3;
                    session.SaveOrUpdate(com);

                    // add 2 users who are tenured and union members who are on
                    // the committee
                    Console.Write("Create 5 users and add 2 of them to the committee.\n");
                    for (int i = 0; i < 5; i++)
                    {
                        User user = User.CreateUser("e", i.ToString() + "F",
                            i.ToString() + "L", "p", "h", false, false, true, true,
                            false, DepartmentType.CSC, OfficerPositionType.None, true,
                            User.NoCommittee);
                        if (i < 2)
                            user.AddToCommittee(session, "Acommittee");
                        session.SaveOrUpdate(user);
                    }
                    // create an election for the committee
                    Console.Write("Create an election based off the committee.\n");
                    CommitteeElection election =
                        CommitteeElection.CreateElection(session, com);
                    session.SaveOrUpdate(election);

                    session.Flush();

                    // submit WTS for 3 users
                    Console.Write("Submitting WTSes for 3 users.");
                    for (int i = 2; i < 5; i++)
                    {
                        CommitteeWTS wts = new CommitteeWTS();
                        wts.Election = election.ID;
                        wts.Statement = "thisisastatement.";
                        wts.User = i;
                        session.SaveOrUpdate(wts);
                        session.Flush();
                    }
                    session.Flush();
                    transaction.Commit();
                }
            }

            // Assertions
            using (ISession session = factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    CommitteeElection election = CommitteeElection.FindElection(session, "Acommittee");
                    ElectionPhase phase = election.NextPhase(session);
                    Assert.AreEqual(ElectionPhase.NominationPhase, phase);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void NextPhaseBehavesProperly()
        {
            Console.Write("Next phase behaves properly:\n");

            ISessionFactory factory = NHibernateHelper.CreateSessionFactory();
            // Clear out database
            using (ISession session = factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Clear(session, transaction);
                }
            }

            // Preconditions
            using (ISession session = factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    // create a committee
                    Console.Write("Create committee with 3 positions.\n");
                    Committee com = new Committee();
                    com.Name = "Acommittee";
                    com.PositionCount = 3;
                    session.SaveOrUpdate(com);
                    transaction.Commit();
                }
            }
            using (ISession session = factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    // add 2 users who are tenured and union members who are on
                    // the committee
                    Console.Write("Create 2 users and add them to the committee.\n");
                    for (int i = 0; i < 2; i++)
                    {
                        User user = User.CreateUser("e", i.ToString() + "F",
                            i.ToString() + "L", "p", "h", false, false, true, true,
                            false, DepartmentType.CSC, OfficerPositionType.None, true,
                            User.NoCommittee);
                        user.AddToCommittee(session, "Acommittee");
                        session.SaveOrUpdate(user);
                    }

                    CommitteeElection election = CommitteeElection.CreateElection(session, Committee.FindCommittee(session, "Acommittee"));
                    election.Phase = ElectionPhase.NominationPhase;
                    session.SaveOrUpdate(election);
                    transaction.Commit();
                }
            }
            
            // Assertions
            using (ISession session = factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    CommitteeElection election = 
                        CommitteeElection.FindElection(session, "Acommittee");

                    Assert.AreEqual(ElectionPhase.VotePhase, election.NextPhase(session));
                    election.SetPhase(session, ElectionPhase.VotePhase);
                    transaction.Commit();
                }
            }
            using (ISession session = factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    CommitteeElection election =
                        CommitteeElection.FindElection(session, "Acommittee");

                    Assert.AreEqual(ElectionPhase.ConflictPhase, election.NextPhase(session));
                    election.SetPhase(session, ElectionPhase.ConflictPhase);
                    transaction.Commit();
                }
            }
            using (ISession session = factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    CommitteeElection election =
                        CommitteeElection.FindElection(session, "Acommittee");

                    Assert.AreEqual(ElectionPhase.ClosedPhase, election.NextPhase(session));

                    election.SetPhase(session, ElectionPhase.ClosedPhase);
                    transaction.Commit();
                }
            }
        }
    }
}
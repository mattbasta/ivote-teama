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

namespace DatabaseEntities
{
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
                NHibernateHelper.Delete(ref session, be[i]);
            }
            // Clear out all the BallotFlags
            var bf = session.CreateCriteria(typeof(BallotFlag)).List<BallotFlag>();
            for (int i = 0; i < bf.Count; i++)
            {
                NHibernateHelper.Delete(ref session, bf[i]);
            }
            // Clear out all the Commitees
            var c = session.CreateCriteria(typeof(Committee)).List<Committee>();
            for (int i = 0; i < c.Count; i++)
            {
                NHibernateHelper.Delete(ref session, c[i]);
            }
            // Clear out all the CommiteeElections
            var ce = session.CreateCriteria(typeof(CommitteeElection)).List<CommitteeElection>();
            for (int i = 0; i < ce.Count; i++)
            {
                NHibernateHelper.Delete(ref session, ce[i]);
            }
            // Clear out all the CommiteeWTSs
            var cwts = session.CreateCriteria(typeof(CommitteeWTS)).List<CommitteeWTS>();
            for (int i = 0; i < cwts.Count; i++)
            {
                NHibernateHelper.Delete(ref session, cwts[i]);
            }
            // Clear out all the CommiteeWTSNominations
            var cwtsn = session.CreateCriteria(typeof(CommitteeWTSNomination)).List<CommitteeWTSNomination>();
            for (int i = 0; i < cwtsn.Count; i++)
            {
                NHibernateHelper.Delete(ref session, cwtsn[i]);
            }
            // Clear out all the Nominations
            var n = session.CreateCriteria(typeof(Nomination)).List<Nomination>();
            for (int i = 0; i < n.Count; i++)
            {
                NHibernateHelper.Delete(ref session, n[i]);
            }
            // Clear out all the Users
            var u = session.CreateCriteria(typeof(User)).List<User>();
            for (int i = 0; i < u.Count; i++)
            {
                NHibernateHelper.Delete(ref session, u[i]);
            }
            transaction.Commit();
        }
        /// <summary>
        /// To test that committee elections can be initiated.
        /// </summary>
        /// <returns>True if the test succeeded, false otherwise</returns>
        public static bool InitiateCommitteeElection()
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
                    bool success = true;
                    Console.Write("Election object's phase is WTSPhase: ");
                    if (CommitteeElection.FindElection(session, "Acommittee").Phase == ElectionPhase.WTSPhase)
                        Console.Write("TRUE");
                    else
                    {
                        Console.Write("FALSE");
                        success = false;
                    }
                    int vacancies = CommitteeElection.FindElection(session, "Acommittee").VacanciesToFill;
                    Console.Write("\nElection object's vacancies to fill (should be 1): ");
                    Console.Write(vacancies);
                    Console.Write("\n");
                    if (vacancies != 1)
                        success = false;
                    
                    // Put email stuff here

                    Console.Write("Initiate Committee Election Complete\n\n");
                    return success;
                }
            }
        }

        /// <summary>
        /// To test that invalid elections cannot be created
        /// </summary>
        /// <returns>True if the test succeeded, flase otherwise</returns>
        public static bool TestCommitteeCreationFailure()
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
                using (ITransaction transaction = session.BeginTransaction())
                {
                    bool success = true;
                    Console.Write("Attempting to create an election based off the committee.\n");
                    Committee com = Committee.FindCommittee(session, "Acommittee");
                    CommitteeElection election =
                        CommitteeElection.CreateElection(session, com);
                    Console.Write("CommitteeElection value (should be null): ");
                    if (election == null)
                        Console.Write("null\n");
                    else
                    {
                        success = false;
                        Console.Write("not null\n");
                    }

                    // email stuff to go here

                    Console.Write("Test Committee Creation Failure Complete\n\n");
                    return success;
                }
            }
        }

        /// <summary>
        /// To test that users can be added to committees
        /// </summary>
        /// <returns>True if the test succeeded, false otherwise</returns>
        public static bool AddMembersToCommittee()
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
                    User[] users = new User[2];
                    for (int i = 0; i < 2; i++)
                    {
                        users[i] = User.CreateUser("e", i.ToString() + "F",
                            i.ToString() + "L", "p", "h", false, false, true, true,
                            false, DepartmentType.CSC, OfficerPositionType.None, true,
                            User.NoCommittee);
                        User.AddToCommittee(session, users[i], "Acommittee");
                        session.SaveOrUpdate(users[i]);
                    }
                    transaction.Commit();
                }
            }

            // Assertions
            using (ISession session = factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    bool success = true;
                    // assert that both users' committee fields reference the
                    // original committee
                    List<User> users = User.GetAllUsers(session);
                    Committee a = Committee.FindCommittee(session, "Acommittee");
                    if (users[0].CurrentCommittee == a.ID &&
                        users[1].CurrentCommittee == a.ID)
                        Console.Write("Both users reference the committee.\n");
                    else
                    {
                        Console.Write("One or both of the users does not reference the committee.\n");
                        success = false;
                    }
                    if (Committee.NumberOfVacancies(session, "Acommittee") == 0 &&
                        Committee.NumberOfPositions(session, "Acommittee") == 2)
                        Console.Write("The committee has the proper number of vacancies and positions.\n");
                    else
                    {
                        Console.Write("The committee has an improper number of vacancies or positions.\n");
                        success = false;
                    }
                    Console.Write("Add members to committee complete.\n\n");
                    return success;
                }
            }
        }

        /// <summary>
        /// To test that users can be added to committees
        /// </summary>
        /// <returns>True if the test succeeded, false otherwise</returns>
        public static bool AddMembersToSingleCommitteeOnly()
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
                        User.AddToCommittee(session, users[i], "Acommittee");
                        session.SaveOrUpdate(users[i]);
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
                    User.AddToCommittee(session, users[0], "Bcommittee");
                    session.SaveOrUpdate(users[0]);
                    transaction.Commit();
                }
            }
            // Assertions
            using (ISession session = factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    bool success = true;
                    // assert that both users' committee fields reference the
                    // original committee
                    List<User> users = User.GetAllUsers(session);
                    Committee a = Committee.FindCommittee(session, "Acommittee"),
                        b = Committee.FindCommittee(session, "Bcommittee");
                    if (users[0].CurrentCommittee == a.ID &&
                        users[1].CurrentCommittee == a.ID)
                        Console.Write("Both users reference original committee.\n");
                    else
                    {
                        Console.Write("AddToCommittee worked improperly.\n");
                        success = false;
                    }
                    if (Committee.NumberOfVacancies(session, "Acommittee") == 0 &&
                        Committee.NumberOfVacancies(session, "Bcommittee") == 2)
                        Console.Write("The first committee has 0 vacancies, the second has 2.\n");
                    else
                    {
                        Console.Write("The committees have an improper number of vacancies.\n");
                        success = false;
                    }
                    if (Committee.NumberOfPositions(session, "Acommittee") == 2 &&
                        Committee.NumberOfPositions(session, "Bcommittee") == 0)
                        Console.Write("The first committee has 0 positions filled, the second has 2.\n");
                    else
                    {
                        Console.Write("The committees have an improper number of positions filled.\n");
                        success = false;
                    }

                    Console.Write("Add Members to Single Committee Only Complete\n\n");

                    return success;
                }
            }

        }

        /// <summary>
        /// To test that appropriate users can register as willing to serve.
        /// </summary>
        /// <returns>True if the test succeeded, false otherwise</returns>
        public static bool RemoveMembersFromCommittees()
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
                    User[] users = new User[2];
                    for (int i = 0; i < 2; i++)
                    {
                        users[i] = User.CreateUser("e", i.ToString() + "F",
                            i.ToString() + "L", "p", "h", false, false, true, true,
                            false, DepartmentType.CSC, OfficerPositionType.None, true,
                            User.NoCommittee);
                        User.AddToCommittee(session, users[i], "Acommittee");
                        session.SaveOrUpdate(users[i]);
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
                    bool success = true;
                    List<User> users = User.GetAllUsers(session);
                    if (users[0].CurrentCommittee == User.NoCommittee)
                        Console.Write("User successfully removed.\n");
                    else
                    {
                        Console.Write("User unsuccessfully removed.\n");
                        success = false;
                    }
                    if (Committee.NumberOfVacancies(session, "Acommittee") == 1)
                        Console.Write("The committee has a single vacancy.\n");
                    else
                    {
                        Console.Write("The committee has the wrong number of vacancies.\n");
                        success = false;
                    }
                    Console.Write("Remove members from committees complete.\n\n");
                    return success;
                }
            }
        }

        /// <summary>
        /// To test that next_phase() produces the correct results when few enough users submit a WTS form to have a nomination phase.
        /// </summary>0
        /// <returns>True if the test succeeded, false otherwise</returns>
        public static bool NominationPhaseSkipped()
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
                    User[] users = new User[5];
                    for (int i = 0; i < 5; i++)
                    {
                        users[i] = User.CreateUser("e", i.ToString() + "F",
                            i.ToString() + "L", "p", "h", false, false, true, true,
                            false, DepartmentType.CSC, OfficerPositionType.None, true,
                            User.NoCommittee);
                        if(i < 2)
                            User.AddToCommittee(session, users[i], "Acommittee");
                        session.SaveOrUpdate(users[i]);
                    }
                    // create an election for the committee
                    Console.Write("Create an election based off the committee.\n");
                    CommitteeElection election = 
                        CommitteeElection.CreateElection(session, com);
                    election.Phase = ElectionPhase.BallotPhase;
                    session.SaveOrUpdate(election);

                    session.Flush();

                    // submit WTS for 2 users
                    Console.Write("Submitting WTSes for 2 users.");
                    for (int i = 2; i < 4; i++)
                    {
                        CommitteeWTS wts = new CommitteeWTS();
                        wts.Election = election.ID;
                        wts.Statement = "thisisastatement.";
                        wts.User = users[i].ID;
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
                    bool success = true;
                    CommitteeElection election = CommitteeElection.FindElection(session, "Acommittee");
                    ElectionPhase phase = CommitteeElection.NextPhase(session, election.ID);
                    if (phase == ElectionPhase.VotePhase)
                        Console.Write("The Next Phase correctly indicates the vote phase.\n");
                    else
                    {
                        Console.Write("The election did not skip the nomination phase.\n");
                        success = false;
                    }

                    Console.Write("Nomination Phase Skipped complete.\n\n");
                    return success;
                }
            }
        }

        /// <summary>
        /// To test that next_phase() produces the correct results when enough WTS forms are submitted for a nomination phase to be held.
        /// </summary>
        /// <returns>True if the test succeeded, false otherwise</returns>
        public static bool NominationPhaseNotSkipped()
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
                    User[] users = new User[5];
                    for (int i = 0; i < 5; i++)
                    {
                        users[i] = User.CreateUser("e", i.ToString() + "F",
                            i.ToString() + "L", "p", "h", false, false, true, true,
                            false, DepartmentType.CSC, OfficerPositionType.None, true,
                            User.NoCommittee);
                        if (i < 2)
                            User.AddToCommittee(session, users[i], "Acommittee");
                        session.SaveOrUpdate(users[i]);
                    }
                    // create an election for the committee
                    Console.Write("Create an election based off the committee.\n");
                    CommitteeElection election =
                        CommitteeElection.CreateElection(session, com);
                    election.Phase = ElectionPhase.BallotPhase;
                    session.SaveOrUpdate(election);

                    session.Flush();

                    // submit WTS for 3 users
                    Console.Write("Submitting WTSes for 3 users.");
                    for (int i = 2; i < 5; i++)
                    {
                        CommitteeWTS wts = new CommitteeWTS();
                        wts.Election = election.ID;
                        wts.Statement = "thisisastatement.";
                        wts.User = users[i].ID;
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
                    bool success = true;
                    CommitteeElection election = CommitteeElection.FindElection(session, "Acommittee");
                    ElectionPhase phase = CommitteeElection.NextPhase(session, election.ID);
                    if (phase == ElectionPhase.NominationPhase)
                        Console.Write("The Next Phase correctly indicates the nomination phase.\n");
                    else
                    {
                        Console.Write("The election skipped nomination phase.\n");
                        success = false;
                    }

                    Console.Write("Nomination Phase Not Skipped complete.\n\n");
                    return success;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool NextPhaseBehavesProperly()
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
                    User[] users = new User[2];
                    for (int i = 0; i < 2; i++)
                    {
                        users[i] = User.CreateUser("e", i.ToString() + "F",
                            i.ToString() + "L", "p", "h", false, false, true, true,
                            false, DepartmentType.CSC, OfficerPositionType.None, true,
                            User.NoCommittee);
                        User.AddToCommittee(session, users[i], "Acommittee");
                        session.SaveOrUpdate(users[i]);
                    }

                    CommitteeElection election = CommitteeElection.CreateElection(session, Committee.FindCommittee(session, "Acommittee"));
                    election.Phase = ElectionPhase.NominationPhase;
                    session.SaveOrUpdate(election);
                    transaction.Commit();
                }
            }
            
            // Assertions
            bool success = true;
            using (ISession session = factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    CommitteeElection election = 
                        CommitteeElection.FindElection(session, "Acommittee");

                    if (CommitteeElection.NextPhase(session, election.ID) == ElectionPhase.VotePhase)
                        Console.Write("Next phase is vote phase.\n");
                    else
                    {
                        Console.Write("Next phase is the wrong value.\n");
                        success = false;
                    }
                    CommitteeElection.SetPhase(session, election.ID, ElectionPhase.VotePhase);
                    transaction.Commit();
                }
            }
            using (ISession session = factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    CommitteeElection election =
                        CommitteeElection.FindElection(session, "Acommittee");

                    if (CommitteeElection.NextPhase(session, election.ID) == ElectionPhase.ConflictPhase)
                        Console.Write("Next phase is conflict phase.\n");
                    else
                    {
                        Console.Write("Next phase is the wrong value.\n");
                        success = false;
                    }
                    CommitteeElection.SetPhase(session, election.ID, ElectionPhase.ConflictPhase);
                    transaction.Commit();
                }
            }
            using (ISession session = factory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    CommitteeElection election =
                        CommitteeElection.FindElection(session, "Acommittee");

                    if (CommitteeElection.NextPhase(session, election.ID) == ElectionPhase.ResultPhase)
                        Console.Write("Next phase is result phase.\n");
                    else
                    {
                        Console.Write("Next phase is the wrong value.\n");
                        success = false;
                    }
                    CommitteeElection.SetPhase(session, election.ID, ElectionPhase.ResultPhase);
                    transaction.Commit();
                }
            }
            Console.Write("Next phase behaves properly complete.\n\n");
            return success;
        }
    }
}

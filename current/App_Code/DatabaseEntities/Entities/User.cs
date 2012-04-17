// User.cs
// Written by: Brian Fairservice.
// Date Modified: 3/6/12
// TODO:
// Complete the DepartmentType enumeration.  Not all departments are currently
// listed.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;

using System.Data.OleDb;
using System.Data;

namespace DatabaseEntities
{
    /// <summary>
    ///  This type enumerates all the different departments at the university.
    /// </summary>
    public enum DepartmentType
    {
        None, Staff, Other, ACC, AER, AMS, ANT, ARA, ART, ARC, ARU,
        ARH, ASE, AST, AVC, BTE, BIO, BUS, CHM, CHI, CDE, CDH, COU, CSC,
        CPY, CFT, CRJ, DAN, DVE, DVR, ECO, EDU, EDW, ELU, EGR, ENG, ENU,
        ENV, FIN, FAR, FAS, FLA, FRE, FRS, GEE, GEG, GEL, GER, HEA, HIS,
        HPD, HUM, ITC, INT, IST, LIB, MGM, MAR, MAT, MKT, MAU, MED,
        MIC, MIL, MLS, MCS, MUS, MUU, MUP, NSE, NUR, PLG, PHI, PED,
        PHY, POL, PRO, PSY, RAR, RSS, RUS, SCI, SCU, SEU, SSC, SSE, SPT,
        SSU, SWK, SWL, SOC, SPA, SPU, SPE, THE, TVR, UST, WRI, WST
    };

    /// <summary>
    /// This type enumerates the different officer positions in APSCUF 
    /// </summary>
    public enum OfficerPositionType
    {
        None, President, VicePresident, Secretary,
        Treasurer, Delegate, AlternateDelegate
    };

    /// <summary>
    /// This class stores all the attributes related to a user, as well as
    /// providing static methods which pull information about, and delete,
    /// entries in the user database.
    /// </summary>
    public class User
    {
        // Parameters
        /// <summary>
        /// The default password for imported users.
        /// </summary>
        public static string DefaultPassword = "ivoteteamaisawesome";
        /// <summary>
        /// The value to assign to User.CurrentCommittee if the user is not in a committee.
        /// </summary>
        public static int NoCommittee = -1;
        /// <summary>
        /// The self-incrementing, unique ID number for this user.
        /// </summary>
        public virtual int ID { get; private set; }
        /// <summary>
        /// The user's email.
        /// </summary>
        public virtual string Email { get; set; }
        /// <summary>
        /// The user's first name.
        /// </summary>
        public virtual string FirstName { get; set; }
        /// <summary>
        /// The user's last name.
        /// </summary>
        public virtual string LastName { get; set; }
        /// <summary>
        /// The password this user uses to access the site.
        /// </summary>
        public virtual String Password { get; set; }
        /// <summary>
        /// The string which will hopefully remind the user what their password
        /// is.
        /// </summary>
        public virtual string PasswordHint { get; set; }
        /// <summary>
        /// The timestamp of the user's last login.
        /// </summary>
        public virtual DateTime LastLogin { get; set; }
        /// <summary>
        /// True if the user is an administrator level user.\
        /// False otherwise.
        /// </summary>
        public virtual bool IsAdmin { get; set; }
        /// <summary>
        /// True if the user is an faculty level user.
        /// False otherwise.
        /// </summary>
        public virtual bool IsFaculty { get; set; }
        /// <summary>
        /// True if the user is an NEC level user.
        /// False otherwise.
        /// </summary>
        public virtual bool IsNEC { get; set; }
        /// <summary>
        ///  True if the user is a tenured faculty member.
        ///  False otherwise.
        /// </summary>
        public virtual bool IsTenured { get; set; }
        /// <summary>
        ///  True if the faculty-member is a union member.
        ///  False otherwise.
        /// </summary>
        public virtual bool IsUnion { get; set; }

        /// <summary>
        ///  True if the user is a member of a bargaining-unit committee.
        ///  False otherwise.
        /// </summary>
        public virtual bool IsBargainingUnit { get; set; }
        /// <summary>
        /// The department which the user is in.
        /// </summary>
        public virtual DepartmentType Department { get; set; }
        /// <summary>
        /// This value indicates if the user holds an officer position, 
        /// as well as what that position is if they do hold on.
        /// </summary>
        public virtual OfficerPositionType OfficerPosition { get; set; }
        /// <summary>
        /// True if the user can vote in elections.
        /// False otherwise.
        /// </summary>
        public virtual bool CanVote { get; set; }
        /// <summary>
        /// This value is a reference to the ID of the committee which the user 
        /// is serving on.  It is -1 if the user is not serving on a committee.
        /// </summary>
        public virtual int CurrentCommittee { get; set; }

        // Helper functions

        /// <summary>
        /// Creates a new user object with the specified values
        /// </summary>
        /// <param name="email">The user's email</param>
        /// <param name="first">The user's first name</param>
        /// <param name="last">The user's last name</param>
        /// <param name="password">The user's password to be hashed</param>
        /// <param name="passwordHint">The user's password hint</param>
        /// <param name="isAdmin">Whether ot not the user is an admin</param>
        /// <param name="isNEC">Whether or not the user is an NEC member</param>
        /// <param name="isFaculty">Whether or not the user is an faculty member</param>
        /// <param name="isTenured">Whether or not the user is tenured</param>
        /// <param name="isUnion">Whether or not the user is in APSCUF</param>
        /// <param name="isBargainingUnit">Whether or not the user is in a bargainingunit committee</param>
        /// <param name="department">The department the faculty member is in</param>
        /// <param name="officerPosition">The officer position of the user</param>
        /// <param name="canVote">Whether or not the user can vote</param>
        /// <param name="currentCommittee">The committee this user serves on</param>
        /// <returns>Returns a user object with the specified officer position</returns>
        public static User CreateUser(string email, string first, string last, string password,
            string passwordHint, bool isAdmin, bool isNEC, bool isFaculty,bool isTenured, bool isUnion,
            bool isBargainingUnit, DepartmentType department,
            OfficerPositionType officerPosition, bool canVote, int currentCommittee)
        {
            User ret = new User();
            ret.Email = email;
            ret.FirstName = first;
            ret.LastName = last;
            ret.Password = Hash(password);
            ret.PasswordHint = passwordHint;
            ret.IsAdmin = isAdmin;
            ret.IsNEC = isNEC;
            ret.IsFaculty = isFaculty;
            ret.IsTenured = isTenured;
            ret.IsUnion = isUnion;
            ret.IsBargainingUnit = isBargainingUnit;
            ret.Department = department;
            ret.OfficerPosition = officerPosition;
            ret.CanVote = canVote;
            ret.CurrentCommittee = currentCommittee;
            return ret;
        }

        /// <summary>
        /// Return a list of all the users in the User table.
        /// </summary>
        /// <param name="session">The current session.</param>
        /// <returns>A list of all the users in the table.</returns>
        public static List<User> GetAllUsers(ISession session)
        {
            return session.CreateCriteria(typeof(User)).List<User>().ToList();
        }
        /// <summary>
        /// Find a user with the specified ID in the database.
        /// </summary>
        /// <param name="Session">The current session.</param>
        /// <param name="ID">The ID of the user you are looking for.</param>
        /// <returns>The user with the matching ID, or null if none was found.</returns>
        public static User FindUser(ISession session, int id)
        {
            // use nhibernate to build the query
            return session.CreateCriteria(typeof(User))
                .Add(Restrictions.Eq("ID", id))
                .UniqueResult<User>();
        }

        /// <summary>
        /// Find a user with the specified name in the database.
        /// </summary>
        /// <param name="session">The current session.</param>
        /// <param name="email">The email you are searching for.</param>
        /// <returns>The user with the matching email, or null if none was found.</returns>
        public static User FindUser(ISession session, string email)
        {
            // use nhibernate to build the query
            return session.CreateCriteria(typeof(User))
                .Add(Restrictions.Eq("Email", email))
                .UniqueResult<User>();
        }

        /// <summary>
        /// Find a user with the specified name in the database.
        /// </summary>
        /// <param name="Session">The current session.</param>
        /// <param name="FirstName">The pertinent user's first name.</param>
        /// <param name="LastName">The pertinent user's last name.</param>
        /// <returns>The user with the matching name, or null if none was found.</returns>
        public static User FindUser(ISession session, string firstName,
            string lastName)
        {
            // use nhibernate to build the query
            return session.CreateCriteria(typeof(User))
                .Add(Restrictions.Eq("FirstName", firstName))
                .Add(Restrictions.Eq("LastName", lastName))
                .UniqueResult<User>();
        }

        /// <summary>
        /// Returns a list of users on the committee with the specified name.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="currentCommitteeName">The name of the pertinent committee.</param>
        /// <returns>A list of all the users on the specified committee.</returns>
        public static List<User> FindUsers(ISession session, 
            string currentCommitteeName)
        {
            // Get the id of the committee
            int committeeId = Committee.FindCommittee(session, 
                currentCommitteeName).ID;

            return session.CreateCriteria(typeof(User))
                .Add(Restrictions.Eq("CurrentCommittee", committeeId))
                .List<User>().ToList();
        }

        /// <summary>
        /// Returns a list of users on the committee specified.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="currentCommittee">The pertinent committee.</param>
        /// <returns>A list of all the users on the specified committee.</returns>
        public static List<User> FindUsers(ISession session, 
            Committee currentCommittee)
        {
            return session.CreateCriteria(typeof(User))
                .Add(Restrictions.Eq("CurrentCommittee", currentCommittee.ID))
                .List<User>().ToList();
        }

        /// <summary>
        /// Returns a list of users who have submitted WTS for a given election.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="id">The id of the pertinent election.</param>
        /// <returns>A list of users who have submitted wts for the given election.</returns>
        public static List<User> FindUsers(ISession session, int id)
        {
            List<CommitteeWTS> wts = CommitteeWTS.FindCommitteeWTS(session, id);
            List<User> users = new List<User>();
            foreach (CommitteeWTS cwts in wts)
            {
                users.Add(User.FindUser(session, cwts.User));
            }
            return users;
        }

        /// <summary>
        /// Returns a user with the specified email, if the correct password is
        /// supplied
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="email">The email of the pertinent user.</param>
        /// <param name="password">The supplied password.</param>
        /// <returns>A user class, or null if the email wasn't found, or if
        /// the password was wrong.</returns>
        public static User Authenticate(ISession session, string email,
            string password)
        {
            if (String.IsNullOrEmpty(password))
                return null;
            // pull a list of all the users from the database.
            var faculty = session.CreateCriteria(typeof(User)).List<User>();
            for (int i = 0; i < faculty.Count; i++)
            {
                if (String.IsNullOrEmpty(faculty[i].Password))
                    return null;

                if (faculty[i].Email.ToLower() == email.ToLower() && faculty[i].Password == Hash(password))
                    return faculty[i];
            }
            return null;
        }

        /// <summary>
        /// Hashes and then updates a user's password.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="ID">The ID of the user whose password is to be set.</param>
        /// <param name="password">The new password.</param>
        /// <param name="passwordHint">The new password hint.</param>
        public static void UpdatePassword(ISession session, int ID,
            string password, string passwordHint)
        {
            User user = FindUser(session, ID);
            user.Password = Hash(password);
            user.PasswordHint = passwordHint;
            session.SaveOrUpdate(user);
            session.Flush();
        }

        /// <summary>
        /// Hashes and then updates a user's password.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="ID">The ID of the user whose password is to be set.</param>
        /// <param name="password">The new password.</param>
        /// <param name="passwordHint">The new password hint.</param>
        public static void UpdatePassword(ISession session, User user,
            string password, string passwordHint)
        {
            user.Password = Hash(password);
            user.PasswordHint = passwordHint;
            session.SaveOrUpdate(user);
            session.Flush();
        }

        /// <summary>
        /// Adds the specified user to the specified committee
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="user">A reference to the user to be added</param>
        /// <param name="committee">The name of the committee the user is to be added to.</param>
        /// <returns>True if the operation was successful.</returns>
        public virtual bool AddToCommittee(ISession session, Committee com)
        {
            if (com != null)
            {
                // Note that the logical if then (A -> B) binary operation
                // is equivalent to (~A v B) 
                if ((!com.TenureRequired || this.IsTenured) && // logical if then
                    (!com.BargainingUnitRequired || this.IsBargainingUnit) && // logical if then
                    com.NumberOfVacancies(session) > 0)
                {
                    this.CurrentCommittee = com.ID;
                    session.SaveOrUpdate(this);
                    session.Flush();
                    return true;
                }
                else return false;
            }
            else
                return false;

        }

        /// <summary>
        /// Hashes a UTF8 string using SHA256. 
        /// </summary>
        /// <param name="toHash">The string which requires hashing.</param>
        /// <returns>The hash value of toHash.</returns>
        public static String Hash(string toHash)
        {
            SHA256 hasher = SHA256.Create();
            // Note: is this the correct encoding to use?  
            byte[] bytes = UTF8Encoding.ASCII.GetBytes(toHash);
            bytes = hasher.ComputeHash(bytes);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        /// <summary>
        /// Check if a user exists with specified email.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="email">Email</param>
        /// <returns>True if the email is found, false otherwise.</returns>
        public static bool CheckIfEmailExists(ISession session, string email)
        {
            // pull a list of all the users from the database.
            var faculty = session.CreateCriteria(typeof(User)).List<User>();
            for (int i = 0; i < faculty.Count; i++)
            {
                // find and return true for a matching email
                if (faculty[i].Email.ToLower() == email.ToLower())
                    return true;
            }
            // otherwise, return false
            return false;
        }

        /// <summary>
        /// Returns the DepartmentType enum which corresponds to the department
        /// name written in full in a string.
        /// </summary>
        /// <param name="name">The name of the department.</param>
        /// <returns>The enum value for the department.</returns>
        private static DepartmentType GetDepartment(string name)
        {
            // these strings come right out of the outlook database.
            switch (name)
            {
                case "ACADEMIC ADVISEMENT":
                    return DepartmentType.Other;
                case "ACADEMIC ENRICHMENT":
                    return DepartmentType.Other;
                case "ANTH/SOC":
                    return DepartmentType.ANT;
                case "ANTH/SOCIOLOGY":
                    return DepartmentType.ANT;
                case "ANTHROPOLOGY/SOC":
                    return DepartmentType.ANT;
                case "ANTHROPOLOGY/SOCIOLOGY":
                    return DepartmentType.ANT;
                case "ART ED/CRAFTS":
                    return DepartmentType.ARU;
                case "ATHLETICS":
                    return DepartmentType.PED;
                case "BIOLOGY":
                    return DepartmentType.BIO;
                case "BUSINESS ADMINISTRATION":
                    return DepartmentType.ACC;
                case "BUSINESS ADMINISTRATOIN": // typo is in the database
                    return DepartmentType.ACC;
                case "BUSINESS ADMINISTRATON": // typo is in the database
                    return DepartmentType.ACC;
                case "COMM STUDIES & THEATRE":
                    return DepartmentType.THE;
                case "COMMIUNICATION STUDIES": // this is how it is written in the database.
                    return DepartmentType.SPE;
                case "COMMUNICATIN STUDIES": // this error is present as well.
                    return DepartmentType.SPE;
                case "COMMUNICATION DESIGN":
                    return DepartmentType.CDE;
                case "COMMUNICATION SUDIES": // another spelling error
                    return DepartmentType.SPE;
                case "COMMUNICATIONS STUDIES":
                    return DepartmentType.SPE;
                case "COMMUNICTION STUDIES": // another spelling error
                    return DepartmentType.SPE;
                case "COMPUTER SCIENCE":
                    return DepartmentType.CSC;
                case "COUNS/HUM SERV":
                    return DepartmentType.COU;
                case "COUNSELING/HUM SERV":
                    return DepartmentType.COU;
                case"CRIMINAL JUSTICE":
                    return DepartmentType.CRJ;
                case "CRIMINAL JUSTICS":
                    return DepartmentType.CRJ; 
                case "COUNS/PSYCH SERV":
                    return DepartmentType.CPY;
                case "COUNS/PSYCH/SERV":
                    return DepartmentType.CPY;
                case "ELECTRONIC MEDIA":
                    return DepartmentType.TVR;
                case "ELEMENTARY ED":
                    return DepartmentType.ELU;
                case "ENGLISH":
                    return DepartmentType.ENG;
                case "FINE ARTS":
                    return DepartmentType.FAR;
                case "GEOGRAPHY":
                    return DepartmentType.GEG;
                case "HISTORY":
                    return DepartmentType.HIS;
                case "LIB SCI/INST TECH":
                    return DepartmentType.LIB;
                case "LIBRARY":
                    return DepartmentType.LIB;
                case "LIBRARY SCI/INST TECH":
                    return DepartmentType.LIB;
                case "LIBRARY SCIENCE/INST TECH":
                    return DepartmentType.LIB;
                case "MATHEMATICS":
                    return DepartmentType.MAT;
                case "MODERN LANGUAGE  STUDIES":
                    return DepartmentType.MLS;
                case "MODERN LANGUAGE STUDIES":
                    return DepartmentType.MLS;
                case "MUSIC":
                    return DepartmentType.MUS;
                case "NURSING":
                    return DepartmentType.NUR;
                case "PHILOSOPHY":
                    return DepartmentType.PHI;
                case "PHYSICAL  SCIENCES": // two spaces on purpose
                    return DepartmentType.PHY;
                case "PHYSICAL SCIENCE":
                    return DepartmentType.PHY;
                case "PHYSICAL SCIENCES":
                    return DepartmentType.PHY;
                case "POLITICAL SCIENCE":
                    return DepartmentType.POL;
                case "PROFESSIONAL STUDIES":
                    return DepartmentType.PRO;
                case "PSYCHOLOGY":
                    return DepartmentType.PSY;
                case "PYSCHOLOGY":
                    return DepartmentType.PSY;
                case "SECONDARY ED":
                    return DepartmentType.SEU;
                case "SOCIAL WORK":
                    return DepartmentType.SWK;
                case "SPECIAL ED":
                    return DepartmentType.SPU;
                case "SPECIAL EDUCATION":
                    return DepartmentType.SPU;
                case "SPORT MANAGEMENT":
                    return DepartmentType.SPT;
                default:
                    return DepartmentType.None;
            }
        }

        /// <summary>
        /// This function imports all the the users in a .accdb file which is
        /// formatted like the example file Karen gave to us.  The .accdb
        /// will just be on the server.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="filePath"></param>
        /// <returns>True if the import was successful, false otherwise.</returns>
        public static bool ImportUsers(ISession session, string filePath)
        {
            DataSet data = new DataSet();
            using (OleDbConnection connection =
                new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Persist Security Info=False;"))
            {
                OleDbCommand command = new OleDbCommand("SELECT * FROM Faculty",connection);
                OleDbDataAdapter adapter = new OleDbDataAdapter(command);

                try
                {
                    connection.Open();
                    adapter.Fill(data, "Faculty");
                }
                catch(Exception e)
                {
                    return false;
                }
                connection.Close();
            }
            DataRowCollection rows = data.Tables["Faculty"].Rows;
            foreach (DataRow row in rows)
            {
                User user = new User();
                user.LastName = row.Field<string>("Last");
                user.FirstName = row.Field<string>("First");
                
                user.Department = User.GetDepartment(row.Field<string>("Department"));
                if (user.Department == DepartmentType.None)
                    return false;


                // Im assuming N = not tenured
                // T = tenured
                // O = I don't know but im marking it in as not tenured for now.
                string tenured = row.Field<string>("Tenure");
                user.IsTenured = (tenured == "T") ? true : false;
                // this field is either 1 or 2.  Im assuming 2 means they aren't tenured
                user.IsUnion = (row.Field<Int16?>("Union") == 1) ? true : false;

                user.Email = row.Field<string>("Email");
                if(string.IsNullOrEmpty(user.Email))
                    user.Email = "Please retrieve the email for this user.";

                user.Password = User.Hash(User.DefaultPassword);
                user.PasswordHint = "Please see the email you recieved in regards to signing up on the iVote system.";
                user.LastLogin = DateTime.Now;
                user.OfficerPosition = OfficerPositionType.None;
                user.IsFaculty = true;
                user.IsAdmin = false;
                user.IsBargainingUnit = false;
                user.IsNEC = false;
                user.CanVote = true;
                user.CurrentCommittee = User.NoCommittee;


                session.SaveOrUpdate(user);
            }
            session.Flush();
            return true;
        }
    }
}

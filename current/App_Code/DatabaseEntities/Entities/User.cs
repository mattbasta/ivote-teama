// User.cs
// Written by: Brian Fairservice.
// Date Modified: 2/17/12
// TODO:
// Complete the DepartmentType enumeration.  Not all departments are currently
// listed.
// Finish writing static helper functions.

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
    /// <summary>
    ///  This type enumerates all the different departments at the university.
    /// </summary>
    public enum DepartmentType {CSC, MAT, PSY}; 

    /// <summary>
    /// This type enumerates the different officer positions in APSCUF 
    /// </summary>
    public enum OfficerPositionType {None, President, VicePresident, Secretary,
        Treasurer, Delegate, AlternateDelegate};

    /// <summary>
    /// This class stores all the attributes related to a user, as well as
    /// providing static methods which pull information about, and delete,
    /// entries in the user database.
    /// </summary>
    public class User
    {
        // Parameters
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
        public virtual string Password { get; set; }
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
        /// Return a list of all the users in the User table.
        /// </summary>
        /// <param name="session">The current session.</param>
        /// <returns>A list of all the users in the table.</returns>
        public static List<User> GetAllUsers(ref ISession session)
        {
            var users = session.CreateCriteria(typeof(User)).List<User>();
            return users.ToList();
        }
        /// <summary>
        /// Find a user with the specified ID in the database.
        /// </summary>
        /// <param name="Session">The current session.</param>
        /// <param name="ID">The ID of the user you are looking for.</param>
        /// <returns>The user with the matching ID, or null if none was found.</returns>
        public static User FindUser(ref ISession session, int ID)
        {
            // pull a list of all the users from the database.
            var faculty = session.CreateCriteria(typeof(User)).List<User>();
            for (int i = 0; i < faculty.Count; i++)
            {
                // find and return the user with a matching id
                if (faculty[i].ID == ID)
                    return faculty[i];
            }
            // otherwise, return null
            return null;
        }

        /// <summary>
        /// Find a user with the specified name in the database.
        /// </summary>
        /// <param name="session">The current session.</param>
        /// <param name="email">The email you are searching for.</param>
        /// <returns>The user with the matching email, or null if none was found.</returns>
        public static User FindUser(ref ISession session, string email)
        {
            // pull a list of all the users from the database.
            var faculty = session.CreateCriteria(typeof(User)).List<User>();
            for (int i = 0; i < faculty.Count; i++)
            {
                // find and return the user with a matching email
                if (faculty[i].Email == email)
                    return faculty[i];
            }
            // otherwise, return null
            return null;
        }

        /// <summary>
        /// Find a user with the specified name in the database.
        /// </summary>
        /// <param name="Session">The current session.</param>
        /// <param name="FirstName">The pertinent user's first name.</param>
        /// <param name="LastName">The pertinent user's last name.</param>
        /// <returns>The user with the matching name, or null if none was found.</returns>
        public static User FindUser(ref ISession session, string firstName, 
            string lastName)
        {
            // pull a list of all the users from the database.
            var faculty = session.CreateCriteria(typeof(User)).List<User>();
            for (int i = 0; i < faculty.Count; i++)
            {
                // find and return the user with a matching name
                if (faculty[i].FirstName == firstName && faculty[i].LastName == lastName)
                    return faculty[i];
            }
            // otherwise return null
            return null;
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
        public static User Authenticate(ref ISession session, string email, 
            string password)
        {
            // pull a list of all the users from the database.
            var faculty = session.CreateCriteria(typeof(User)).List<User>();
            for (int i = 0; i < faculty.Count; i++)
            {
                if (faculty[i].Email == email && faculty[i].Password == password)
                    return faculty[i];
            }
            return null;
        }
    }
}

// Committee.cs
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

namespace DatabaseEntities
{
    /// <summary>
    /// This class represents a bargaining unit comittee.
    /// </summary>
    public class Committee
    {
        /// <summary>
        /// The unique, self-incrementing identifer of the committee.
        /// </summary>
        public virtual int ID { get; private set; }
        /// <summary>
        /// The name of the comittee.
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// The total number of positions in the committee
        /// </summary>
        public virtual int PositionCount { get; set; }

        /// <summary>
        /// Returns a commitee object from the database based on its name.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="name">The name of the committee needed.</param>
        /// <returns>The committee object with the given name.</returns>
        public static Committee FindCommittee(ISession session, string name)
        {
            // pull a list of all the users from the database.
            var committees = session.CreateCriteria(typeof(Committee)).List<Committee>();
            for (int i = 0; i < committees.Count; i++)
            {
                // find and return the user with a matching email
                if (committees[i].Name == name)
                    return committees[i];
            }
            // otherwise, return null
            return null;
        }

        /// <summary>
        /// Returns a commitee object from the database based on its id.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="id">The id of the election being seached for.</param>
        /// <returns>The committee object with the given id.</returns>
        public static Committee FindCommittee(ISession session, int id)
        {
            // pull a list of all the users from the database.
            var committees = session.CreateCriteria(typeof(Committee)).List<Committee>();
            for (int i = 0; i < committees.Count; i++)
            {
                // find and return the user with a matching email
                if (committees[i].ID == id)
                    return committees[i];
            }
            // otherwise, return null
            return null;
        }

        /// <summary>
        /// This function calculates the number of vancies in a given committee.
        /// </summary>
        /// <param name="session">A valid sesssion.</param>
        /// <param name="name">The name of the committee in question</param>
        /// <returns>The number of vacancies in the committee, or -1 if there was a problem.</returns>
        public static int NumberOfVacancies(ISession session, string name)
        {
            Committee com = Committee.FindCommittee(session, name);
            List<User> users = User.GetAllUsers(session);

            if (com == null)
                return -1;

            // this number represents the number of people serving on the committee
            int members = 0;

            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].CurrentCommittee == com.ID)
                    members++;
            }
            return com.PositionCount - members;
        }

        /// <summary>
        /// This function calculates the number of positions which are currently filled in the committee.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="name">The name of the committee in question.</param>
        /// <returns>The number of positions users are filling in the committee, or -1 if there was a problem.</returns>
        public static int NumberOfPositions(ISession session, string name)
        {
            Committee com = Committee.FindCommittee(session, name);
            List<User> users = User.GetAllUsers(session);

            if (com == null)
                return -1;

            // this number represents the number of people serving on the committee
            int members = 0;

            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].CurrentCommittee == com.ID)
                    members++;
            }
            return members;
        }

        /// <summary>
        /// This function takes the specified user off the committee with the 
        /// specified ID.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="user">The user to remove</param>
        /// <param name="id">The ID of the committee the user is being removed from.</param>
        public static void RemoveMember(ISession session, User user, int id)
        {
            user.CurrentCommittee = User.NoCommittee;
            session.SaveOrUpdate(user);
            session.Flush();
        }
    }
}

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
using NHibernate.Criterion;

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
        /// The description of the comittee.
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// The total number of positions in the committee
        /// </summary>
        public virtual int PositionCount { get; set; }
        /// <summary>
        /// Whether members must be in the bargaining unit
        /// </summary>
        public virtual bool BargainingUnitRequired { get; set; }
        /// <summary>
        /// Whether members must be tenured
        /// </summary>
        public virtual bool TenureRequired { get; set; }
        /// <summary>
        /// Returns a commitee object from the database based on its name.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="name">The name of the committee needed.</param>
        /// <returns>The committee object with the given name, or null if none was found</returns>
        public static Committee FindCommittee(ISession session, string name)
        {
            // Formulate a query for the committee based on the committee name
            return session.CreateCriteria(typeof(Committee))
                .Add(Restrictions.Eq("Name", name))
                .UniqueResult<Committee>();
        }

        /// <summary>
        /// Returns a commitee object from the database based on its id.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="id">The id of the election being seached for.</param>
        /// <returns>The committee object with the given id, or null if none was found.</returns>
        public static Committee FindCommittee(ISession session, int id)
        {
            // formulate a query for the committee based off the id
            return session.CreateCriteria(typeof(Committee))
                .Add(Restrictions.Eq("ID", id))
                .UniqueResult<Committee>();
        }

        /// <summary>
        /// Returns the number of committees that have vacancies.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <returns>An integer describing the number of committees found.</returns>
        public static int NumberOfWaitingCommittees(ISession session)
        {
            // formulate a query for the committees
            var committees = session.CreateCriteria(typeof(Committee))
                    .List<Committee>();
            int count = 0;
            for (int i = 0; i < committees.Count; i++)
                if (committees[i].NumberOfVacancies(session) > 0)
                    count++;
            return count - session.CreateCriteria(typeof(CommitteeElection))
                                   .Add(Restrictions.Not(Restrictions.Eq("Phase", ElectionPhase.ClosedPhase)))
                                   .List().Count;
        }

        /// <summary>
        /// Returns whether the committee has an ongoing election.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <returns>A bool of whether the committee is in an election</returns>
        public virtual bool InElection(ISession session)
        {
            return session.CreateCriteria(typeof(CommitteeElection))
                                   .Add(Restrictions.Eq("PertinentCommittee", ID))
                                   .Add(Restrictions.Not(Restrictions.Eq("Phase", ElectionPhase.ClosedPhase)))
                                   .List().Count > 0;
        }

        /// <summary>
        /// This function calculates the number of vancies in a given committee.
        /// </summary>
        /// <param name="session">A valid sesssion.</param>
        /// <param name="name">The name of the committee in question</param>
        /// <returns>The number of vacancies in the committee, or -1 if there was a problem.</returns>
        public virtual int NumberOfVacancies(ISession session)
        {
            List<User> users = User.GetAllUsers(session);

            // this number represents the number of people serving on the committee
            int members = 0;
            for (int i = 0; i < users.Count; i++)
                if (users[i].CurrentCommittee == this.ID)
                    members++;
            return this.PositionCount - members;
        }

        /// <summary>
        /// This function calculates the number of positions which are currently filled in the committee.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="name">The name of the committee in question.</param>
        /// <returns>The number of positions users are filling in the committee, or -1 if there was a problem.</returns>
        public virtual int NumberOfPositions(ISession session)
        {
            return this.PositionCount;
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

        /// <summary>
        /// Detects conflicts within a given committee.  For use with the committees dashboard only.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="id">The ID of the committee in question.</param>
        /// <returns>A list of strings containing information (in english) about any conflicts</returns>
        public static List<string> FindConflicts(ISession session, Committee committee)
        {
            List<string> ret = new List<string>();
            List<User> members = User.FindUsers(session, committee.Name);
            // Find if multiple members of the same department are on the committee
            List<DepartmentType> departments = new List<DepartmentType>();
            foreach (User i in members)
            {
                if (departments.Contains(i.Department))
                    ret.Add("Warning! Multiple faculty members in the " + i.Department.ToString() + " department are on this committee!");
                else departments.Add(i.Department);
            }
            return ret;
        }

        /// <summary>
        /// Indicates whether or not there is a member in a given department within a givin committee.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="committee">The committee in question.</param>
        /// <param name="department">The department to find in the committee.</param>
        /// <returns></returns>
        public static bool DepartmentRepresented(ISession session, Committee committee, DepartmentType department)
        {
            List<User> members = User.FindUsers(session, committee.Name);
            foreach (User i in members)
            {
                if (i.Department == department)
                    return true;
            }
            return false;
        }
    }
}
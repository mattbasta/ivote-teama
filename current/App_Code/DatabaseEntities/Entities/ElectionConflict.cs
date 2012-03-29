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
    /// This enum represents the different types of conflicts which may arise at the
    /// end of an election.
    /// </summary>
    public enum ConflictType
    {
        ElectedToMultipleCommittees,
        TooManyDeptMembers
    }

    /// <summary>
    /// This class represents an entry for an election conflict in the database.
    /// </summary>
    public class ElectionConflict
    {
        /// <summary>
        /// Flags SecUser if the conflict type doesn't need a second user
        /// ID.
        /// </summary>
        public static int SecondUserNotApplicable = -1;
        
        /// <summary>
        /// The unique, auto-incrementing ID of the election conflict.
        /// </summary>
        public virtual int ID { get; private set; }
        
        /// <summary>
        /// The ID of the election to which this conflict is pertinent.
        /// </summary>
        public virtual int Election { get; set; }

        /// <summary>
        /// The type of the conflict
        /// </summary>
        public virtual ConflictType Type { get; set; }
        
        /// <summary>
        /// The ID of the first user for which this conflict has arisen.
        /// </summary>
        public virtual int FirstUser { get; set; }
        
        /// <summary>
        /// The ID of the second user involved in the conflict.  Only used
        /// for TooManyDeptMembers type conflicts.  This value equals
        /// Electionconflict.SecondUserNotApplicable if this is a different
        /// conflict type.
        /// </summary>
        public virtual int SecUser { get; set; }

        /// <summary>
        /// Returns a list of all the election conflicts which are pertinent to a given election.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="electionID">The ID of the pertinent election.</param>
        /// <returns>A list of all the ElectionConflicts pertinent to the election.</returns>
        public static List<ElectionConflict> FindElectionConflicts(ISession session,
            int electionID)
        {
            var conflicts = session.CreateCriteria(typeof(ElectionConflict))
                .Add(Restrictions.Eq("Election", electionID))
                .List<ElectionConflict>();

            return conflicts.ToList<ElectionConflict>();
        }

        /// <summary>
        /// Returns the election conflict object with the specified ID.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="id">The ID of the election conflict object to be retrieved.</param>
        /// <returns>The election conflict object.</returns>
        public static ElectionConflict FindElectionConflict(ISession session,
            int id)
        {
            var conflicts = session.CreateCriteria(typeof(ElectionConflict))
                .Add(Restrictions.Eq("ID", id))
                .UniqueResult<ElectionConflict>();

            return conflicts;
        }
    }
}
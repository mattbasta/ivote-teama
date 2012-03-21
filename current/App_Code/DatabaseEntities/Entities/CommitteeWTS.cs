// CommitteeWTS.cs
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
    /// This class represents a CommitteeWTS as it is stored in the database.
    /// It stores which users have submitted willingness-to-serve forms for
    /// which elections, along with their statement.
    /// </summary>
    public class CommitteeWTS
    {
        /// <summary>
        /// The unique, self-incrementing identifer of the CommitteeWTS.
        /// </summary>
        public virtual int ID { get; private set; }

        /// <summary>
        /// The id of the user who stated they were willing to serve.
        /// </summary>
        public virtual int User { get; set; }
        /// <summary>
        /// This is a reference to the unique ID of the election to which this
        /// WTS is pertinent.
        /// </summary>
        public virtual int Election { get; set; }
        /// <summary>
        /// This string contains the user's statement which they submitted along
        /// with the WTS.
        /// </summary>
        public virtual string Statement { get; set; }

        /// <summary>
        /// Returns a list of all CommitteeWTS objects pertinent to a given election.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="election">The election id.</param>
        /// <returns>A list of all the CommitteeWTS objects pertinent to the specified election.</returns>
        public static List<CommitteeWTS> FindCommitteeWTS(ISession session,
            int election)
        {
            // pull a list of all the Committee WTS'  for the given election from the database.
            var entries = session.CreateCriteria(typeof(CommitteeWTS))
                .Add(Restrictions.Eq("Election", election))
                .List<CommitteeWTS>();

            // return that list
            return entries.ToList<CommitteeWTS>();
        }
    }
}

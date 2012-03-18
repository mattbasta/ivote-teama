// Nominations.cs
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
    /// This class represents a nomination of a user in an election.
    /// </summary>
    public class Nomination
    {
        /// <summary>
        /// The unique, self-incrementing identifer of the nomination.
        /// </summary>
        public virtual int ID { get; private set; }
        /// <summary>
        /// This is a reference to the unique ID of the election to which this
        /// nomination is pertinent.
        /// </summary>
        public virtual int Election { get; set; }
        /// <summary>
        /// This is a reference to the unique ID of the user who was nominated.
        /// </summary>
        public virtual int User { get; set; }

        /// <summary>
        /// Returns a list of all Nomination objects pertinent to a given election.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="election">The election id.</param>
        /// <returns>A list of all the Nomination objects pertinent to the specified election.</returns>
        public static List<Nomination> FindNomination(ISession session,
            int election)
        {
            // pull a list of all the ballot entriess from the database.
            var entries = session.CreateCriteria(typeof(Nomination)).List<Nomination>();
            List<Nomination> ret = new List<Nomination>();
            for (int i = 0; i < entries.Count; i++)
            {
                if (entries[i].Election == election)
                    ret.Add(entries[i]);
            }
            return ret;
        }
    }
}

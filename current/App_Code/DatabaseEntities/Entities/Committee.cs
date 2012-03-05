// Committee.cs
// Written by: Brian Fairservice
// Date Modified: 2/17/12
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
        /// Find a Committee with the specified ID in the database.
        /// </summary>
        /// <param name="Session">The current session.</param>
        /// <param name="ID">The ID of the Committee you are looking for.</param>
        /// <returns>The Committee with the matching ID, or null if none was found.</returns>
        public static Committee FindCommittee(ref ISession session, int ID)
        {
            // pull a list of all the users from the database.
            var committees = session.CreateCriteria(typeof(Committee)).List<Committee>();
            for (int i = 0; i < committees.Count; i++)
            {
                // find and return the user with a matching id
                if (committees[i].ID == ID)
                    return committees[i];
            }
            // otherwise, return null
            return null;
        }
    }
}

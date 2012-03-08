// BallotEntry.cs
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
    /// This class represents a ballot-entry as it is stored in the database.
    /// A ballot entry represents a vote for a nominee in a given election.
    /// </summary>
    public class BallotEntry
    {
        /// <summary>
        /// The unique, self-incrementing identifer of the ballot-entry.
        /// </summary>
        public virtual int ID { get; private set; }
        /// <summary>
        /// This is a reference to the unique ID of the election to which this
        /// ballot-entry is pertinent.
        /// </summary>
        public virtual int Election { get; set; }
        /// <summary>
        /// This is a reference to the id of the candidate who was voted for.
        /// </summary>
        public virtual int Candidate { get; set; }

        /// <summary>
        /// Returns all the ballot entries pertinent to a given election.
        /// </summary>
        /// <param name="session">A reference to the session.</param>
        /// <param name="election">The pertinent election.</param>
        /// <returns>A list of all ballot entries which apply to the specified election.</returns>
        public static List<BallotEntry> FindBallotEntry(ISession session, 
            int election)
        {
            // pull a list of all the ballot entriess from the database.
            var entries = session.CreateCriteria(typeof(BallotEntry)).List<BallotEntry>();
            List<BallotEntry> ret = new List<BallotEntry>();
            for (int i = 0; i < entries.Count; i++)
            {
                if (entries[i].Election == election)
                    ret.Add(entries[i]);
            }
            return ret;
        }
    }
}

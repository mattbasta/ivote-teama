// BallotFlag.cs
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
    /// This class represents a ballot-flag in the database.  Ballot flags are
    /// used to track which users have voted already in a given election.
    /// </summary>
    public class BallotFlag
    {
        /// <summary>
        /// The unique, self-incrementing identifer of the ballotflag.
        /// </summary>
        public virtual int ID { get; private set; }
        /// <summary>
        /// This is a reference to the unique ID of the election to which this
        /// ballotflag is pertinent.
        /// </summary>
        public virtual int Election { get; set; }
        /// <summary>
        /// This is a reference to the unique ID of the user who voted.
        /// </summary>
        public virtual int User { get; set; }

        /// <summary>
        /// Returns a list of ballot flags which are pertinent to a given election
        /// </summary>
        /// <param name="session">A valid session</param>
        /// <param name="election">The id of the election.</param>
        /// <returns>A list of BallotFlags pertinent to the specified election.</returns>
        public static List<BallotFlag> FindBallotFlag(ISession session,
            int election)
        {
            // pull a list of all the ballot entriess from the database.
            var entries = session.CreateCriteria(typeof(BallotFlag)).List<BallotFlag>();
            List<BallotFlag> ret = new List<BallotFlag>();
            for (int i = 0; i < entries.Count; i++)
            {
                if (entries[i].Election == election)
                    ret.Add(entries[i]);
            }
            return ret;
        }
    }
}

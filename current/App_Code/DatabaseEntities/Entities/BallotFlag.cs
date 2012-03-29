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
using NHibernate.Criterion;

namespace DatabaseEntities
{
    /// <summary>
    /// This class represents a ballot-flag in the database. Ballot flags are
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
        public static List<BallotFlag> FindBallotFlags(ISession session,
            int election)
        {
            // formulate a query for all the ballot flags which are pertinent
            // to the specified election
            var entries = session.CreateCriteria(typeof(BallotFlag))
                .Add(Restrictions.Eq("Election", election))
                .List<BallotFlag>();

            // and then return a list of all the pertinent ballot flags
            return entries.ToList<BallotFlag>();
        }

        /// <summary>
        /// Finds a ballot flag pertaining to a given election, which flags
        /// that a given user has already voted
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="election">The pertinent election.</param>
        /// <param name="user">The user to check for.</param>
        /// <returns>A ballot flag indicated that the specified user has voted in the specified election, or null.</returns>
        public static BallotFlag FindBallotFlag(ISession session, int election,
            int user)
        {
            // formulate a query for all the ballot flags which are pertinent
            // to the specified election and specified user
            var entry = session.CreateCriteria(typeof(BallotFlag))
                .Add(Restrictions.Eq("Election", election))
                .Add(Restrictions.Eq("User", user))
                .UniqueResult<BallotFlag>();

            return entry;
        }
    }
}
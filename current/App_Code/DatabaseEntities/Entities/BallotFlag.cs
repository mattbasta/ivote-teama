// BallotFlag.cs
// Written by: Brian Fairservice
// Date Modified: 2/17/12
// TODO: Write static helper functions

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}

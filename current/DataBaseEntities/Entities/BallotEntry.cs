// BallotEntry.cs
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
    }
}

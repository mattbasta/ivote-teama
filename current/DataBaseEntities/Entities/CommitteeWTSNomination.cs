// CommitteeWTSNominations.cs
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
    /// This class represents a nomination for a user to fill a position in a
    /// committee.
    /// </summary>
    public class CommitteeWTSNomination
    {
        /// <summary>
        /// The unique, self-incrementing identifer of the CommitteeWTSNomination.
        /// </summary>
        public virtual int ID { get; private set; }
        /// <summary>
        /// This is a reference to the unique ID of the election to which this
        /// nomination is pertinent.
        /// </summary>
        public virtual int Election { get; set; }
        /// <summary>
        /// The id of the user being nominated.
        /// </summary>
        public virtual int Candidate { get; set; }

        /// <summary>
        /// The id of the user who submitted the nomination.
        /// </summary>
        public virtual int Voter { get; set; }
    }
}

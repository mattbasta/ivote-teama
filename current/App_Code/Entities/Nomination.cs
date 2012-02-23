// Nominations.cs
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
    }
}

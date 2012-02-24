// Committee.cs
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
    }
}

// CommitteeWTS.cs
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
    /// This class represents a CommitteeWTS as it is stored in the database.
    /// It stores which users have submitted willingness-to-serve forms for
    /// which elections, along with their statement.
    /// </summary>
    public class CommitteeWTS
    {
        /// <summary>
        /// The unique, self-incrementing identifer of the CommitteeWTS.
        /// </summary>
        public virtual int ID { get; private set; }

        /// <summary>
        /// The id of the user who stated they were willing to serve.
        /// </summary>
        public virtual int User { get; set; }
        /// <summary>
        /// This is a reference to the unique ID of the election to which this
        /// WTS is pertinent.
        /// </summary>
        public virtual int Election { get; set; }
        /// <summary>
        /// This string contains the user's statement which they submitted along
        /// with the WTS.
        /// </summary>
        public virtual string Statement { get; set; }
    }
}

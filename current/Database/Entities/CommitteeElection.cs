// CommitteeElection.cs
// Written by: Brian Fairservice
// Date Modified: 2/17/12
// TODO: Write static helper functions

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseEntities
{
    public enum ElectionPhase {WTSPhase,NominationPhase,PetitionPhase,VotePhase,
        ConflictPhase,ResultPhase};

    /// <summary>
    /// This class stores the information regarding an election to a committee.
    /// </summary>
    public class CommitteeElection
    {
        /// <summary>
        /// The unique, self-incrementing identifer of the election.
        /// </summary>
        public virtual int ID { get; private set; }
        /// <summary>
        /// The unique id of the committee this election pertains to.
        /// </summary>
        public virtual int Committee { get; set; }
        /// <summary>
        /// The date the election was started
        /// </summary>
        public virtual DateTime Started { get; set; }

        /// <summary>
        /// This value indicates the current phase of the election.
        /// </summary>
        public virtual ElectionPhase Phase { get; set; }

        /// <summary>
        /// This number indicates the number of positions which are open in the 
        /// committee this election pertains to.
        /// </summary>
        public virtual int VacanciesToFill {get; set;}
    }
}

// CommitteeWTSNominationMap.cs
// Written by: Brian Fairservice
// Date Modified: 2/17/12
// TODO:

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentNHibernate.Mapping;

namespace DatabaseEntities
{
    /// <summary>
    /// This class explicates the Fluent NHibernate mapping for the 
    /// CommitteeWTSNomination class onto the database.
    /// </summary>
    public class CommitteeWTSNominationMap : ClassMap<CommitteeWTSNomination>
    {
        public CommitteeWTSNominationMap()
        {
            // Specify the name of the table WTS nominations's will be 
            // inserted into.
            Table("CommitteeWTSNominations");
            // ID is a self-incrementing, unique identifier for the WTS 
            // nomination.
            Id(x => x.ID);
            // The rest of the parameters will be fluently auto-mapped, and
            // cannot be null.
            Map(x => x.Election)
                .Not.Nullable();
            Map(x => x.Candidate)
                .Not.Nullable();
            Map(x => x.Voter)
                .Not.Nullable();
        }
    }
}

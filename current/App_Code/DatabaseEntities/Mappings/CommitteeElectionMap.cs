// CommitteeElectionMap.cs
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
    public class CommitteeElectionMap : ClassMap<CommitteeElection>
    {
        public CommitteeElectionMap()
        {
            // Specify the name of the table WTS nominations's will be
            // inserted into.
            Table("CommitteeElections");
            // ID is a self-incrementing, unique identifier for the election.
            Id(x => x.ID);
            // The rest of the parameters will be fluently auto-mapped, and
            // cannot be null.
            Map(x => x.PertinentCommittee)
                .Not.Nullable();
            Map(x => x.Started)
                .Not.Nullable();
            Map(x => x.PhaseStarted)
                .Not.Nullable();
            // This value is an enum type. By default, NHibernate maps enums to
            // strings in the database.
            // We can use .CustomType<TYPE>() to change that.
            Map(x => x.Phase)
                .Not.Nullable();
            Map(x => x.VacanciesToFill)
                .Not.Nullable();
            Map(x => x.PhaseEndDelta)
                .Not.Nullable();
        }
    }
}
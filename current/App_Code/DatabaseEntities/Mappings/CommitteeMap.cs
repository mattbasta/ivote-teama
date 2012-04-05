// CommitteeMap.cs
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
    /// This class explicates the Fluent NHibernate mapping for the Committee
    /// class onto the database.
    /// </summary>
    public class CommitteeMap : ClassMap<Committee>
    {
        public CommitteeMap()
        {
            // Specify the name of the table committees will be inserted into.
            Table("Committees");
            // ID is a self-incrementing, unique identifier for the comittee.
            Id(x => x.ID);
            // The rest of the parameters will be fluently auto-mapped, and
            // cannot be null.
            Map(x => x.Name)
                .Not.Nullable();
            Map(x => x.Description);
            Map(x => x.PositionCount)
                .Not.Nullable();
        }
    }
}
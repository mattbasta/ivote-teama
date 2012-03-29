using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentNHibernate.Mapping;

namespace DatabaseEntities
{
    /// <summary>
    /// This class outlines the classmap for ElectionConflicts.
    /// </summary>
    public class ElectionConflictMap : ClassMap<ElectionConflict>
    {
        public ElectionConflictMap()
        {
            // Specify the name of the table which will store ElectionConflicts.
            Table("ElectionConflicts");

            // The unique, aut-incrementing ID of the election conflict.
            Id(x => x.ID);

            // The ID of the election to which the conflict is pertinent
            Map(x => x.Election)
                .Not.Nullable();
            // The type of the conflict.
            Map(x => x.Type)
                .Not.Nullable();
            // The ID of the first user who is involved in the conflict.
            Map(x => x.FirstUser)
                .Not.Nullable();
            // The ID of the second user who is involved in the conflict.
            Map(x => x.SecUser)
                .Not.Nullable();
        }
    }
}


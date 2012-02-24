// NominationMap.cs
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
    /// This class explicates the Fluent NHibernate mapping for the Nomination
    /// class onto the database.
    /// </summary>
    public class NominationMap : ClassMap<Nomination>
    {
        public NominationMap()
        {
            // Specify the name of the table nominations will be inserted into.
            Table("Nominations");
            // ID is a self-incrementing, unique identifier for the nomination.
            Id(x => x.ID);
            // The rest of the parameters will be fluently auto-mapped, and
            // cannot be null.
            Map(x => x.Election)
                .Not.Nullable();
            Map(x => x.User)
                .Not.Nullable();
        }
    }
}

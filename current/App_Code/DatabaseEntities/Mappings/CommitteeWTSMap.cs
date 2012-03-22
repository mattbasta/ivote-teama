// CommitteeWTSMap.cs
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
    /// This class explicates the Fluent NHibernate mapping for the CommitteeWTS
    /// class onto the database.
    /// </summary>
    public class CommitteeWTSMap : ClassMap<CommitteeWTS>
    {
        public CommitteeWTSMap()
        {
            // Specify the name of the table WTS's will be inserted into.
            Table("CommitteeWTSes");
            // ID is a self-incrementing, unique identifier for the Committee WTS.
            Id(x => x.ID);
            // The rest of the parameters will be fluently auto-mapped, and
            // cannot be null.
            Map(x => x.User)
                .Not.Nullable();
            Map(x => x.Election)
                .Not.Nullable();
            Map(x => x.Statement)
                .Not.Nullable()
                .CustomSqlType("text");
        }
    }
}

// BallotFlag.cs
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
    /// This class explicates the Fluent NHibernate mapping for the BallotFlag
    /// class onto the database.
    /// </summary>
    public class BallotFlagMap : ClassMap<BallotFlag>
    {
        public BallotFlagMap()
        {
            // Specify the name of the table ballot-flags will be inserted into.
            Table("BallotFlags");
            // ID is a self-incrementing, unique identifier for the ballot-flag.
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


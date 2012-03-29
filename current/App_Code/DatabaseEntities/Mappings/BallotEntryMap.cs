// BallotEntryMap.cs
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
    /// This class explicates the Fluent NHibernate mapping for the BallotEntry
    /// class onto the database.
    /// </summary>
    public class BallotEntryMap : ClassMap<BallotEntry>
    {
        public BallotEntryMap()
        {
            // Specify the name of the table ballot-entries will be inserted into.
            Table("BallotEntries");
            // ID is a self-incrementing, unique identifier for the ballot-entry.
            Id(x => x.ID);
            // The rest of the parameters will be fluently auto-mapped, and
            // cannot be null.
            Map(x => x.Election)
                .Not.Nullable();
            Map(x => x.Candidate)
                .Not.Nullable();
        }
    }
}


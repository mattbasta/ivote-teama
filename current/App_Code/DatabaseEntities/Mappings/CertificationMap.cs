using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentNHibernate.Mapping;

namespace DatabaseEntities
{
    public class CertificationMap : ClassMap<Certification>
    {
        public CertificationMap()
        {
            // Specify the name of the table nominations will be inserted into.
            Table("Certifications");
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
// UserMap.cs
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
    /// This class explicates the Fluent NHibernate mapping for the User class
    /// onto the database.
    /// </summary>
    public class UserMap : ClassMap<User>
    {
        /// <summary>
        /// Maps the class onto the database.
        /// </summary>
        public UserMap()
        {
            // Specify the name of the table users will be inserted into.
            Table("Users");
            // ID is an auto-incrementing identifer which is unique to each user.
            Id(x => x.ID);
            // The rest of the User parameters will be fluently auto-mapped, and
            // cannot be null.
            Map(x => x.Email)
                .Not.Nullable();
            Map(x => x.FirstName)
                .Not.Nullable();
            Map(x => x.LastName)
                .Not.Nullable();
            Map(x => x.Password)
                .Not.Nullable();
            Map(x => x.PasswordHint)
                .Not.Nullable();
            Map(x => x.LastLogin)
                .Not.Nullable();
            Map(x => x.IsAdmin)
                .Not.Nullable();
            Map(x => x.IsNEC)
                .Not.Nullable();
            Map(x => x.IsTenured)
                .Not.Nullable();
            Map(x => x.IsUnion)
                .Not.Nullable();
            Map(x => x.IsBargainingUnit)
                .Not.Nullable();
            // The following two parameter mappings are defined as enums in the
            // User class. By default, NHibernate will handle these as strings
            // in the database.  We can use .CustomType<TYPE>() to change that.
            Map(x => x.Department)
                .Not.Nullable();
            Map(x => x.OfficerPosition)
                .Not.Nullable();
            Map(x => x.CanVote)
                .Not.Nullable();
            Map(x => x.CurrentCommittee)
                .Not.Nullable();
        }
    }
}

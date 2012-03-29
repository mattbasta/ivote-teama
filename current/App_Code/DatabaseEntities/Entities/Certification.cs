using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;

namespace DatabaseEntities
{
    /// <summary>
    /// This class represents the certification an NEC member gives to election
    /// results.  Any given election ought to have 3 certifications associated 
    /// with it.
    /// </summary>
    public class Certification
    {
        /// <summary>
        /// The unique, self-incrementing identifer of the certification.
        /// </summary>
        public virtual int ID { get; private set; }
        /// <summary>
        /// This is a reference to the unique ID of the election to which this
        /// certification is pertinent.
        /// </summary>
        public virtual int Election { get; set; }
        /// <summary>
        /// This is a reference to the unique ID of the user who certified 
        /// the election.
        /// </summary>
        public virtual int User { get; set; }

        /// <summary>
        /// Returns a list of all Certification objects pertinent to a given election.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="election">The election id.</param>
        /// <returns>A list of all the Certification objects pertinent to the specified election.</returns>
        public static List<Certification> FindCertifications(ISession session,
            int election)
        {
            // pull a list of the nominations regarding the given election form the database
            var entries = session.CreateCriteria(typeof(Certification))
                .Add(Restrictions.Eq("Election", election))
                .List<Certification>();

            // and return that very same list
            return entries.ToList<Certification>();
        }

        /// <summary>
        /// Returns a certification object if the specified user certified the specified election.
        /// </summary>
        /// <param name="session">A valid session.</param>
        /// <param name="election">The election id.</param>
        /// <param name="user">The NEC user.</param>
        /// <returns>The certification object, or null if it does not exist.</returns>
        public static Certification FindCertification(ISession session,
            int election, int user)
        {
            // pull a list of the nominations regarding the given election form the database
            var entry = session.CreateCriteria(typeof(Certification))
                .Add(Restrictions.Eq("Election", election))
                .Add(Restrictions.Eq("User", user))
                .UniqueResult<Certification>();

            // and return that very same list
            return entry;
        }
    }
}
// NhibernateHelper.cs
// Written by: Brian Fairservice.
// Date Modified: 2/17/12
// TODO: Remove wrapper functions?

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

namespace DatabaseEntities
{
    /// <summary>
    /// This static helper class will provide methods to open a session,
    /// as well as to generate the database schema.
    /// </summary>
    static class NHibernateHelper
    {
        /// <summary>
        /// This is the connection string used to connect to the MySQL database.
        /// </summary>
        public static string ConnectionString
            = "Server=localhost;Database=testdb;Uid=root;Pwd=password;";

        /// <summary>
        /// This method acquires a mysql session factory so we can make changes
        /// to the databse after acquiring sessions.
        /// </summary>
        /// <returns>
        /// A valid session factory.
        /// </returns>
        public static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                    .Database(MySQLConfiguration.Standard.ConnectionString(ConnectionString))
                    .Mappings(m => m.FluentMappings.Add<UserMap>())
                    .Mappings(m => m.FluentMappings.Add<CommitteeMap>())
                    .Mappings(m => m.FluentMappings.Add<NominationMap>())
                    .Mappings(m => m.FluentMappings.Add<BallotFlagMap>())
                    .Mappings(m => m.FluentMappings.Add<BallotEntryMap>())
                    .Mappings(m => m.FluentMappings.Add<CommitteeWTSMap>())
                    .Mappings(m => m.FluentMappings.Add<CommitteeWTSNominationMap>())
                    .Mappings(m => m.FluentMappings.Add<CommitteeElectionMap>())
                    .BuildSessionFactory();
        }

        /// <summary>
        /// This method acquires a mysql session factory so we can make 
        /// acquire a session to make changes to the database. It also 
        /// automatically generates our database schema based off of the 
        /// fluent mappings.
        /// </summary>
        /// <returns>
        /// A valid session factory.
        /// </returns>
        public static ISessionFactory CreateSessionFactoryAndGenerateSchema()
        {
            return Fluently.Configure()
                    .Database(MySQLConfiguration.Standard.ConnectionString(ConnectionString))
                    .Mappings(m => m.FluentMappings.Add<UserMap>())
                    .Mappings(m => m.FluentMappings.Add<CommitteeMap>())
                    .Mappings(m => m.FluentMappings.Add<NominationMap>())
                    .Mappings(m => m.FluentMappings.Add<BallotFlagMap>())
                    .Mappings(m => m.FluentMappings.Add<BallotEntryMap>())
                    .Mappings(m => m.FluentMappings.Add<CommitteeWTSMap>())
                    .Mappings(m => m.FluentMappings.Add<CommitteeWTSNominationMap>())
                    .Mappings(m => m.FluentMappings.Add<CommitteeElectionMap>())
                    .ExposeConfiguration(cfg =>
                        {
                            new SchemaExport(cfg).Create(false, true);
                        })
                    .BuildSessionFactory();
        }


        // The following wrapper functions are in place to standardize interactions with
        // the database through this class.
        /// <summary>
        /// This function deletes an object from the pertinent table.
        /// </summary>
        /// <param name="session">The current session.</param>
        /// <param name="toDelete">The object to delete.</param>
        public static void Delete(ISession session, Object toDelete)
        {
            session.Delete(toDelete);
            session.Flush();
        }

        /// <summary>
        /// This function saves an object to the database, or, if the specified object
        /// shares an ID with an entry already in the table, that entry's values are 
        /// changed to the values in toSave.
        /// </summary>
        /// <param name="session">The current session.</param>
        /// <param name="toSave">The object to insert or edit.</param>
        public static void UpdateDatabase(ISession session, Object toSave)
        {
            session.SaveOrUpdate(toSave);
            session.Flush();
        }

        /// <summary>
        /// This function completes the current transaction with the database.  Call it
        /// when you are done operating on the database.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        public static void Finished(ITransaction transaction)
        {
            transaction.Commit();
        }

    }
}

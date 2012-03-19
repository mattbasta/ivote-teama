using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Specialized;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using NHibernate;
using System.Collections.Generic;

/// <summary>
/// Summary description for iVoteRoleProvider
/// </summary>
public class iVoteRoleProvider : RoleProvider
{
    private databaseLogic dbLogic = new databaseLogic();

 
    public override void Initialize(string name, NameValueCollection configuration)
    {
        try
        {
            base.Initialize(name, configuration);
        }
        catch
        {
        }
    }

    public override string ApplicationName
    {
        get
        {
            return "iVoteSystem";
        }
        set
        {
        }
    }

    public override void AddUsersToRoles(string[] usernames, string[] rolenames)
    {
        throw new Exception("Method not implemented.");
    }

    public override void CreateRole(string role)
    {
        throw new Exception("Method not implemented.");
    }

    public override bool DeleteRole(string role, bool throwOnPopulatedRole)
    {
        throw new Exception("Method not implemented.");
    }

    public override string[] FindUsersInRole(string role, string user)
    {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        List<DatabaseEntities.User> users = DatabaseEntities.User.GetAllUsers(session);

        List<string> usersInRole = new List<string>();

        foreach (DatabaseEntities.User userTemp in users)
        {
            if (userTemp.Email.ToLower() != user.ToLower())
                continue;

            if (role == "admin" && userTemp.IsAdmin)
                usersInRole.Add(userTemp.Email);
            if (role == "nec" && userTemp.IsNEC)
                usersInRole.Add(userTemp.Email);
            if (role == "faculty" && userTemp.IsUnion)
                usersInRole.Add(userTemp.Email);
        }

        return usersInRole.ToArray<string>();
    }

    public override string[] GetAllRoles()
    {
        throw new Exception("Method not implemented.");
    }

    public override string[] GetRolesForUser(string user)
    {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        DatabaseEntities.User nUser = DatabaseEntities.User.FindUser(session, user);

        List<string> userRoles = new List<string>();

        if (nUser.IsAdmin)
            userRoles.Add("admin");
        if (nUser.IsNEC)
            userRoles.Add("nec");
        if (nUser.IsUnion)
            userRoles.Add("faculty");

        return userRoles.ToArray<string>();
    }

    public override string[] GetUsersInRole(string role)
    {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        List<DatabaseEntities.User> users = DatabaseEntities.User.GetAllUsers(session);

        List<string> usersInRole = new List<string>();

        foreach (DatabaseEntities.User userTemp in users)
        {
            if (role == "admin" && userTemp.IsAdmin)
                usersInRole.Add(userTemp.Email);
            if (role == "nec" && userTemp.IsNEC)
                usersInRole.Add(userTemp.Email);
            if (role == "faculty" && userTemp.IsUnion)
                usersInRole.Add(userTemp.Email);
        }

        return usersInRole.ToArray<string>();
    }

    public override Boolean IsUserInRole(string user, string role)
    {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        DatabaseEntities.User nUser = DatabaseEntities.User.FindUser(session, user);

        if (role == "admin" && nUser.IsAdmin)
            return true;
        else if (role == "nec" && nUser.IsNEC)
            return true;
        else if (role == "faculty" && nUser.IsUnion)
            return true;
        else
            return false;

    }

    public override void RemoveUsersFromRoles(string[] users, string[] roles)
    {
        throw new Exception("Method not implemented.");
    }

    public override bool RoleExists(string role)
    {
        throw new Exception("Method not implemented.");
    }

}

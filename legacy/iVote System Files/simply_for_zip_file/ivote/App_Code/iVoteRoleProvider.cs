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
        catch (Exception e)
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
        dbLogic.addUsersToRoles(usernames, rolenames);
    }

    public override void CreateRole(string role)
    {
        dbLogic.addRole(role);
    }

    public override bool DeleteRole(string role, bool throwOnPopulatedRole)
    {
        return false;
    }

    public override string[] FindUsersInRole(string role, string user)
    {
        return dbLogic.findUsersInRole(role, user);
    }

    public override string[] GetAllRoles()
    {
        return dbLogic.getRoles();
    }

    public override string[] GetRolesForUser(string user)
    {
        return dbLogic.getUserRoles(user);
    }

    public override string[] GetUsersInRole(string role)
    {
        return dbLogic.getUsersInRole(role);
    }

    public override Boolean IsUserInRole(string user, string role)
    {
        return dbLogic.isUserInRole(user, role);
    }

    public override void RemoveUsersFromRoles(string[] users, string[] roles)
    {
        dbLogic.removeUsersFromRoles(users, roles);
    }

    public override bool RoleExists(string role)
    {
        return dbLogic.roleExists(role);
    }

}
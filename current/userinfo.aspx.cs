using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class wwwroot_phase1aSite_userinfo : System.Web.UI.Page
{
    databaseLogic dbLogic = new databaseLogic();
    iVoteRoleProvider roleProvider = new iVoteRoleProvider();
    
    //global variable "only" used for changing users roles
    String id = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        id = getQueryValue();

        if (!Page.IsPostBack)
        {
            getSpecificInfo(id);
            HiddenFieldID.Value = id;
            LinkButtonChangeEmail.OnClientClick = "javascript:return confirm('Are you sure you want to change this persons email?\nEmail messages the system's only method to contact the specified user, so please make sure the new address your saving valid.')";
            ButtonDelete.OnClientClick = "javascript:return confirm('Are you sure what want to PERMANENTLY delete this user account?')";
        }
        
        //Populate dropdown menu from DepartmentType enum.
        foreach (DatabaseEntities.DepartmentType dept in Enum.GetValues(typeof(DatabaseEntities.DepartmentType)))
        {
            DeptDropDown.Items.Add(dept.ToString());
        }

    }

    protected String getQueryValue()
    {
        if (Request.QueryString.ToString() != "")
        {
            String[] commandArgs = Request.QueryString.ToString().Split(new char[] { '=' }); //splits querystring into variable name and value
            return commandArgs[1]; //returns query value
        }
        else
            return "";
    }

    //Gets info for a user
    protected void getSpecificInfo(string id) 
    {
        dbLogic.selectUserInfoFromUnionId(id);
        DataSet infoSet = dbLogic.getResults();
        DataRow dr = infoSet.Tables["query"].Rows[0];
        //attach data to external schema

        Page.Title = "Edit info for " + dr["first_name"].ToString() + " " + dr["last_name"].ToString();

        //name, first and last
        Email.Text += dr["email"].ToString();
        FirstName.Text = dr["first_name"].ToString();
        LastName.Text = dr["last_name"].ToString();
        Phone.Text = dr["phone"].ToString();
        DeptDropDown.SelectedItem.Text = dr["department"].ToString();
        LabelFullname.Text = "Edit " + dr["first_name"].ToString() + " " + dr["last_name"].ToString() + "'s information in the field below.";
    
        //add role of the user
        string username = dbLogic.returnUsernameFromUnionID(id);
        HiddenFieldUsername.Value = username;

        if(roleProvider.IsUserInRole(username, "faculty"))
        {
            RadioButtonListRoles.SelectedValue = "faculty";
            //previousRole = "faculty";
        }
        else if(roleProvider.IsUserInRole(username, "nec"))
        {
            RadioButtonListRoles.SelectedValue = "nec";
            //previousRole = "pres";
        }
        else if (roleProvider.IsUserInRole(username, "admin"))
        {
            RadioButtonListRoles.SelectedValue = "admin";
            //previousRole = "admin";
        }
    }

    public void setUpRoles()
    {
        String[] userArray = { HiddenFieldUsername.Value };
        String[] roleArray = { RadioButtonListRoles.SelectedValue };
        String[] previousRoleArray = { "admin", "faculty", "nec" };

        roleProvider.RemoveUsersFromRoles(userArray, previousRoleArray); //removes user from previous role
        roleProvider.AddUsersToRoles(userArray, roleArray); //adds user's new role
    }

    //returns user to orginal page
    protected void returnToUsersPage(object sender, EventArgs e)
    {
        Response.Redirect("users.aspx");
    }


    //ADD CHECK AGAINST EMAIL IN THE DATABASE
    protected void ButtonSave_Clicked(object sender, EventArgs e)
    {
        string[] union = { LastName.Text, FirstName.Text, Email.Text, Phone.Text, DeptDropDown.SelectedValue };
        dbLogic.updateUser(HiddenFieldID.Value, union);
        setUpRoles();
        Response.Redirect("users.aspx");
        
    }

    protected void LinkButtonChangeEmail_Clicked(object sender, EventArgs e)
    {
        if (Email.Enabled == false)
        {
            Email.Enabled = true;
            LinkButtonChangeEmail.Text = "Lock";
        }
        else
        {
            Email.Enabled = false;
            LinkButtonChangeEmail.Text = "Change";
        }
    }


    protected void ButtonDelete_Clicked(object sender, EventArgs e)
    {
        dbLogic.deleteAccountCompletely(id);
        Response.Redirect("users.aspx");
    }
}
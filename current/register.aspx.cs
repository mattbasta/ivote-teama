using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DatabaseEntities;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using NHibernate;

public partial class Account_Register : System.Web.UI.Page
{
    databaseLogic dbLogic = new databaseLogic();
    VerifyEmail sendEmail = new VerifyEmail();
    iVoteRoleProvider roleProvider = new iVoteRoleProvider();
    protected void Page_Load(object sender, EventArgs e)
    {
        //Populate dropdown menu from DepartmentType enum.
        foreach (DatabaseEntities.DepartmentType dept in Enum.GetValues(typeof(DatabaseEntities.DepartmentType)))
        {
            DeptDropDown.Items.Add(dept.ToString());
        }
    }

    protected void submit(object sender, EventArgs e)
    {
        //make sure the page is valid before processing
        if (Page.IsValid)
        {
            ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();

            if (DatabaseEntities.User.CheckIfEmailExists(ref session, Email.Text)) //checks if new user's email address already exists
            {
                LabelFeedback.Text = "Email Address/User already exists in dabase records.";
            }
            else
            {
                string[] union = { LastName.Text, FirstName.Text, Email.Text, Phone.Text, DeptDropDown.SelectedValue };
                //string[] username = Email.Text.Split(new char[] { '@' }); //Take only first part of users email and make it username (ex. asd123@yahoo.com. username = asd123)
                string user = Email.Text;

                dbLogic.insertUnion(union);  //Insert into Union_members table
                int unionID = dbLogic.returnLastUnionAdded();

                //dbLogic.insertUser(unionID, user);  //Insert into User table
                User nUser = CreateUser(Email.Text, "", FirstName.Text, LastName.Text, DeptDropDown.SelectedValue);
                DatabaseEntities.NHibernateHelper.UpdateDatabase(session, nUser);

                string[] emailAddress = new string[1];
                emailAddress[0] = Email.Text;

                //email message sent to new user
                String emailMessage = "";
                emailMessage += "Hello " + FirstName.Text + " " + LastName.Text + ",<br /><br />";
                emailMessage += "The APSCUF-KU iVote system administrator has added you as a new user!<br /><br />";
                emailMessage += "<u>You MUST verify your new account before it can be fully activated!</u> <br /><br />";
                emailMessage += "Please record your new username for the iVote System below. <br />";
                emailMessage += "(You will use this username to log onto the system.)<br /><br />";
                emailMessage += "Username: <b>" + user + "</b> <br /><br />";

                // passes arguments to this class where it will send the email
                sendEmail.verify(unionID, emailAddress, emailMessage);

                //make form label invisible
                lblForm.Visible = false;
                //make confirmation message label visible
                lblConfirm.Visible = true;

            }
        }
    }

    private User CreateUser(String email, String passwordHint, String firstName, String lastName, String department)
    {
        DatabaseEntities.User user = new DatabaseEntities.User();
        user.FirstName = firstName;
        user.LastName = lastName;
        user.Email = email;
        user.Password = DatabaseEntities.User.Hash("");
        user.PasswordHint = passwordHint;
        user.CanVote = true;
        user.CurrentCommittee = -1;
        user.Department = DepartmentType.CSC;
        user.IsAdmin = (RadioButtonListRoles.SelectedValue == "admin");
        user.IsBargainingUnit = false;
        user.IsNEC = (RadioButtonListRoles.SelectedValue == "nec");
        user.IsTenured = false;
        user.IsUnion = (RadioButtonListRoles.SelectedValue == "faculty");
        user.LastLogin = DateTime.Now;
        user.CanVote = true;

        DepartmentType nDepartment = (DepartmentType)Enum.Parse(typeof(DepartmentType), department);

        user.Department = nDepartment;

        return user;

    }
}
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
    VerifyEmail sendEmail = new VerifyEmail();
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!Page.IsPostBack) {
            ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
            //Populate dropdown menu from DepartmentType enum.
            foreach (DatabaseEntities.DepartmentType dept in Enum.GetValues(typeof(DatabaseEntities.DepartmentType)))
                DeptDropDown.Items.Add(dept.ToString());
                
            foreach(DatabaseEntities.Committee c in session.CreateCriteria(typeof(DatabaseEntities.Committee)).List())
                CurrentCommittee.Items.Add(new ListItem(c.Name, c.ID.ToString()));
        }
   }

    protected void submit(object sender, EventArgs e)
    {
        //make sure the page is valid before processing
        if (Page.IsValid)
        {
            ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
            ITransaction transaction = session.BeginTransaction();

            if (DatabaseEntities.User.CheckIfEmailExists(session, Email.Text)) //checks if new user's email address already exists
                LabelFeedback.Text = "Email Address/User already exists in dabase records.";
                SuccessPanel.Visible = false;
            else
            {
                string user = Email.Text;

                User nUser = CreateUser(Email.Text, FirstName.Text, LastName.Text, DeptDropDown.SelectedValue);
                
                // Set up the user permissions and such.
                nUser.IsAdmin = IsAdmin.Checked;
                nUser.IsNEC = IsNEC.Checked;
                nUser.IsFaculty = IsFaculty.Checked;
                nUser.IsTenured = IsTenured.Checked;
                nUser.IsUnion = IsUnion.Checked;
                nUser.IsBargainingUnit = IsBU.Checked;
                
                nUser.CanVote = CanVote.Checked;
                
                DatabaseEntities.NHibernateHelper.UpdateDatabase(session, nUser);
               

                //email message sent to new user
                String emailMessage = "";
                emailMessage += "Hello " + FirstName.Text + " " + LastName.Text + ",<br /><br />";
                emailMessage += "The APSCUF-KU election administration has added you as a new user!<br /><br />";
                emailMessage += "<u>You MUST verify your new account before it can be fully activated!</u> <br /><br />";
                emailMessage += "Please record your new username for the iVote System below. <br />";
                emailMessage += "(You will use this username to log onto the system.)<br /><br />";
                emailMessage += "Username: <b>" + user + "</b> <br /><br />";

                // passes arguments to this class where it will send the email
                string[] emailAddress = { user };

                //Grab user again to update ID
                User testUser = DatabaseEntities.User.FindUser(session, nUser.Email);
                sendEmail.verify(testUser.ID, emailAddress, emailMessage);

                NHibernateHelper.Finished(transaction);

                SuccessPanel.Visible = true;
            }
        }
    }
    
    private User CreateUser(String email, String firstName, String lastName, String department)
    {
        DatabaseEntities.User user = new DatabaseEntities.User();
        user.FirstName = firstName;
        user.LastName = lastName;
        user.Email = email;
        user.Password = DatabaseEntities.User.Hash("");
        user.PasswordHint = "";

        user.LastLogin = DateTime.Now;

        user.CurrentCommittee = Convert.ToInt32(CurrentCommittee.SelectedValue);
        user.Department = (DepartmentType)Enum.Parse(typeof(DepartmentType), department);

        return user;
    }
}
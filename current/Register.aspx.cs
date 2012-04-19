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
            Committee committee = Committee.FindCommittee(session, int.Parse(CurrentCommittee.SelectedValue));
            
            ITransaction transaction = session.BeginTransaction();

            if (DatabaseEntities.User.CheckIfEmailExists(session, Email.Text)) {
                //checks if new user's email address already exists
                EmailExistsPanel.Visible = true;
                SuccessPanel.Visible = false;
                ConflictPanel.Visible = false;
            } else if(committee != null &&
                      Committee.DepartmentRepresented(session, committee, (DepartmentType)Enum.Parse(typeof(DepartmentType), DeptDropDown.SelectedValue)))
            {
                ConflictPanel.Visible = true;
                EmailExistsPanel.Visible = false;
                InElectionPanel.Visible = false;
                SuccessPanel.Visible = false;
            } else if(committee != null && committee.InElection(session)) {
                InElectionPanel.Visible = true;
                EmailExistsPanel.Visible = false;
                ConflictPanel.Visible = false;
                SuccessPanel.Visible = false;
            } else {
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
               
                User testUser = DatabaseEntities.User.FindUser(session, nUser.Email);

                nEmailHandler emailer = new nEmailHandler();
                emailer.sendConfirmationEmail(testUser, "Welcome to the APSCUF iVote System", "userRegister");

                NHibernateHelper.Finished(transaction);

                SuccessPanel.Visible = true;
                ConflictPanel.Visible = false;
                
                FirstName.Text = "";
                LastName.Text = "";
                Email.Text = "";
                
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
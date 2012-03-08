using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DatabaseEntities;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using NHibernate;
using NHibernate.Cfg;

public partial class tester : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        DatabaseEntities.NHibernateHelper.CreateSessionFactoryAndGenerateSchema();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        emailer emailTest = new emailer();
        String[] addresses = new String[1];
        addresses[0] = email.Text;
        emailTest.sendEmailToList(addresses, "This is a test.", "Test Message");
    }
    protected void Button3_Click(object sender, EventArgs e)
    {


        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        ITransaction transaction = session.BeginTransaction();

        DatabaseEntities.User user = new DatabaseEntities.User();
        user.FirstName = "Default";
        user.LastName = "Admin";
        user.Email = "test@btx.me";
        user.Password = DatabaseEntities.User.Hash("testpassword");
        user.PasswordHint = "";
        user.CanVote = true;
        user.CurrentCommittee = -1;
        user.Department = DepartmentType.CSC;
        user.IsAdmin = true;
        user.IsBargainingUnit = false;
        user.IsNEC = false;
        user.IsTenured = true;
        user.IsUnion = true;
        user.LastLogin = DateTime.Now;
        //user.OfficerPosition = OfficerPositionType.Delegate;
        user.CanVote = true;



        DatabaseEntities.NHibernateHelper.UpdateDatabase(session, user);

        DatabaseEntities.NHibernateHelper.Finished(transaction);



    }
    protected void Button5_Click(object sender, EventArgs e)
    {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        ITransaction transaction = session.BeginTransaction();

        User user = DatabaseEntities.User.FindUser(session, "test@btx.me");

        DatabaseEntities.NHibernateHelper.Delete(session, user);
        DatabaseEntities.NHibernateHelper.Finished(transaction);
    }

    protected void Button4_Click(object sender, EventArgs e)
    {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        ITransaction transaction = session.BeginTransaction();

        List<User> userList = DatabaseEntities.User.GetAllUsers(session);
        DatabaseEntities.NHibernateHelper.Finished(transaction);

        foreach (User user in userList)
        {
            Label1.Text += user.Email + "<br />";
        }

        

    }
   protected void Button7_Click(object sender, EventArgs e)
    {
        Label2.Text = DatabaseEntities.User.Hash(preHash.Text);
    }
    protected void Button8_Click(object sender, EventArgs e)
    {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        DatabaseEntities.User testUser = DatabaseEntities.User.Authenticate(session,authEmail.Text,authPassword.Text);

        if (testUser == null)
        {
            Label3.Text = "Invalid";
        }
        else
        {
            Label3.Text = "Valid";
        }
    }
    protected void Button9_Click(object sender, EventArgs e)
    {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        ITransaction transaction = session.BeginTransaction();

        DatabaseEntities.CommitteeElection ce = new DatabaseEntities.CommitteeElection();
        ce.PertinentCommittee = 1;
        ce.VacanciesToFill = 2;
        ce.Started = DateTime.Now;

        DatabaseEntities.NHibernateHelper.UpdateDatabase(session, ce);

        DatabaseEntities.NHibernateHelper.Finished(transaction);
    }
    protected void Button10_Click(object sender, EventArgs e)
    {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        ITransaction transaction = session.BeginTransaction();

        DatabaseEntities.CommitteeElection.SetPhase(session, 1, DatabaseEntities.ElectionPhase.WTSPhase);

        DatabaseEntities.NHibernateHelper.Finished(transaction);
    }
}

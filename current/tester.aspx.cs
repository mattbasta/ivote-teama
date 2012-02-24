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
        user.FirstName = "FirstTest";
        user.LastName = "LastTest";
        user.Email = "test@btx.me";
        user.Password = DatabaseEntities.User.Hash("testpassword");
        user.PasswordHint = "This is a hint.";
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

        User user = DatabaseEntities.User.FindUser(ref session, "test@btx.me");

        DatabaseEntities.NHibernateHelper.Delete(session, user);
        DatabaseEntities.NHibernateHelper.Finished(transaction);
    }

    protected void Button4_Click(object sender, EventArgs e)
    {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        ITransaction transaction = session.BeginTransaction();

        List<User> userList = DatabaseEntities.User.GetAllUsers(ref session);
        DatabaseEntities.NHibernateHelper.Finished(transaction);

        foreach (User user in userList)
        {
            Label1.Text += user.Email + "<br />";
        }

        

    }
}
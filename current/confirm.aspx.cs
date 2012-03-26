using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using DatabaseEntities;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using NHibernate;

public partial class confirm : System.Web.UI.Page
{

    String fullCode = "";
    databaseLogic dbLogic = new databaseLogic();
    protected void Page_Load(object sender, EventArgs e)
    {

        String code = Request.QueryString["x"]; //retrieves query value from url
        String idunionQuery = "SELECT iduser FROM email_verification WHERE code_verified=" + code + ";";

        //if there is a query code in the url
        if (code != "")
        {
            //retrieves timestamp from db (for testing purposes). if code is incorrect will return empty string
            String id = dbLogic.checkConfirmCode(code);

            if (id != "")
            {
                PanelHide.Visible = true;
                PanelError.Visible = false;
                //if code exists
                DatabaseEntities.User user = getUser(int.Parse(id));

                LabelFeedback.Text = "Hello <b>" + user.FirstName + " " + user.LastName + "</b>. Thank you for verifying your account.<br />Please set up your new password below to complete your new account's activation.";
                HiddenFieldPassword.Value = user.Email;
                fullCode = code;
            }
            else
            {
                //if code does not exist
                LabelFeedback.Text = "<b>Sorry, there seems to have been an error...</b>";
                PanelHide.Visible = false;
                PanelError.Visible = true;
            }
        }
        else
        {
            //if query string does not exist
            LabelFeedback.Text = "<b>A problem has occurred.</b>";
            PanelHide.Visible = false;
            PanelError.Visible = true;
        }

    }

    protected void ButtonSave_Clicked(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();

            DatabaseEntities.User user = DatabaseEntities.User.FindUser(session, HiddenFieldPassword.Value);
            DatabaseEntities.User.UpdatePassword(session, user, TextBoxPassword.Text, "");

            dbLogic.deleteCode(fullCode); //deletes code row from database

            //Make form invisible
            PanelHide.Visible = false;
            //display confirmation message
            lblConfirm.Visible = true;

        }

    }


    // Gets user object form database based on ID
    protected DatabaseEntities.User getUser(int id)
    {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        DatabaseEntities.User user = DatabaseEntities.User.FindUser(session, id);

        return user;
    }
}

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
        
        String code = getQueryValue(); //retrieves query value from url
        String idunionQuery = "SELECT iduser FROM email_verification WHERE code_verified=" + code + ";";
        String[] FirstLast = new String[2];

        //if there is a query code in the url
        if (code != "")
        {
            //retrieves timestamp from db (for testing purposes). if code is incorrect will return empty string
            String name = dbLogic.checkConfirmCode(code);

            if (name != "")
            {
                PanelHide.Visible = true;
                //if code exists    
                //Pull First and Last name from union_members and add to congrats message!
                String getNames = "SELECT last_name, first_name, email FROM union_members WHERE idunion_members='" + name + "';";
                FirstLast = getNameOfUser(getNames);
                LabelFeedback.Text = "Hello <b>" + FirstLast[1] + " " + FirstLast[0] + "</b>. Thank you for verifying your account.<br />Please set up your new password below to complete your new account's activation.";
                HiddenFieldPassword.Value = FirstLast[2];
                fullCode = code;
            }
            else
            {
                //if code does not exist
                LabelFeedback.Text = "<b>Sorry, there seems to have been an error...</b>";
                PanelHide.Visible = false;
                PanelHide.Enabled = false;
            }
        }
        else
        {
            //if query string does not exist
            LabelFeedback.Text = "<b>Sorry, there seems to have been an error...</b>";
            PanelHide.Visible = false;
            PanelHide.Enabled = false;
        }

        //LabelFeedback.Text = dbLogic.checkConfirmCode(code) + "!";
    }

    //gets the query value ONLY, if it exists
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

    protected void ButtonSave_Clicked(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();

            DatabaseEntities.User user = DatabaseEntities.User.FindUser(ref session, HiddenFieldPassword.Value);
            DatabaseEntities.User.UpdatePassword(ref session, user, TextBoxPassword.Text, "");

            //dbLogic.updatePassword(HiddenFieldPassword.Value, encryptionHelper.encrypt(TextBoxPassword.Text)); //inserts new encrypted password
            dbLogic.deleteCode(fullCode); //deletes code row from database

            //Make form invisible
            lblForm.Visible = false;
            //display confirmation message
            lblConfirm.Visible = true;
             
        }
        
    }


    // gets first and last name of user
    protected String[] getNameOfUser(String id)
    {
        DataSet ds = dbLogic.getFirstAndLast(id);
        String[] name = new String[3];
        name[0] = ds.Tables[0].Rows[0].ItemArray[0].ToString();
        name[1] = ds.Tables[0].Rows[0].ItemArray[1].ToString();
        name[2] = ds.Tables[0].Rows[0].ItemArray[2].ToString();

        return name;
    }
}
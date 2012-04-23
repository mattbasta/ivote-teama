using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Cfg;

public partial class wwwroot_experimental_petition : System.Web.UI.Page
{
    databaseLogic dbLogic = new databaseLogic();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //Binds positions to the dropdownlist in the popup
            dbLogic.selectAllAvailablePositions();
            DataSet positionSet = dbLogic.getResults();
            DropDownListPostions.DataSource = positionSet;
            DropDownListPostions.DataTextField = positionSet.Tables[0].Columns[2].ToString();
            DropDownListPostions.DataBind();
        }
    }

    protected void search(object sender, EventArgs e)
    {
         string query = txtSearch.Text;

        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        ListViewUsers.DataSource = session.CreateCriteria(typeof(DatabaseEntities.User))
                .Add(Restrictions.Or(Restrictions.Like("FirstName", "%" + query + "%"),
                                     Restrictions.Like("LastName", "%" + query + "%")))
                .List<DatabaseEntities.User>();

        ListViewUsers.DataBind();

        ListViewUsers.Visible = true;

        if (txtSearch.Text != "")
            btnViewAll.Visible = true;
    }

    protected void ListViewUsers_ItemCommand(Object sender, ListViewCommandEventArgs e)
    {        
        if (String.Equals(e.CommandName, "nominate"))
        {
            ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
            DatabaseEntities.User userObject = DatabaseEntities.User.FindUser(session, int.Parse(e.CommandArgument.ToString()));

            LabelChoosPosition.Text = "Please select the position you would<br /> like " + userObject.FirstName + " " + userObject.LastName + " to be petitioned for:";
            ButtonSubmit.OnClientClick = "return confirm('Are you sure you want to start this petition for " + userObject.FirstName + " " + userObject.LastName + "?\\n(If accepted, you will not be able to withdraw your petition  later.)')";
            HiddenFieldName.Value = userObject.FirstName + " " + userObject.LastName;
            HiddenFieldId.Value = e.CommandArgument.ToString();

            PopupControlExtender1.Show();
        }
    }

    protected void clear(object sender, EventArgs e)
    {
        ListViewUsers.Visible = false;
        btnViewAll.Visible = false;
    }

    protected void ButtonSubmit_Clicked(object sender, EventArgs e)
    {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        DatabaseEntities.User userObject = DatabaseEntities.User.FindUser(session, HttpContext.Current.User.Identity.Name);

        string[] petitionInfo = {HiddenFieldId.Value, DropDownListPostions.SelectedItem.Text , userObject.ID.ToString()};


        //submit request
        if (!dbLogic.isUserEnteringPetitionTwice(petitionInfo)) //checks if user has already entered this petition
        {
            if (dbLogic.countPetitionsForPerson(petitionInfo) >= 10)
            {
                LabelFeedback.Text = "Thank you for your submission. This person already has enough petition signatures to be on the ballot for this election.";
            }
            else if (dbLogic.countPetitionsForPerson(petitionInfo) == 9)
            {
                dbLogic.insertPetition(petitionInfo);
                //move data to nomination table
                LabelFeedback.Text = "Petition signature Submitted. " + HiddenFieldName.Value + " now has enough signatures to be on the ballot.";

                //add data to nominate_accept table
                string[] acceptInfo = { HiddenFieldId.Value, "0", DropDownListPostions.SelectedItem.Text, "1" };
                dbLogic.insertNominationAcceptFromPetition(acceptInfo);
            }
            else
            {
                dbLogic.insertPetition(petitionInfo);
                if (dbLogic.countPetitionsForPerson(petitionInfo) == 1)
                {
                    LabelFeedback.Text = "Petition request submitted for " + HiddenFieldName.Value + " to be on the next ballod for " + DropDownListPostions.SelectedItem.Text + ".";
                }
                else
                {
                    LabelFeedback.Text = "Petition signature Submitted for " + HiddenFieldName.Value + " to be on the next ballod for " + DropDownListPostions.SelectedItem.Text + ".";
                }
            }
        }
        else
        {
            LabelFeedback.Text = "Submission rejected. You have already signed a petition for " + HiddenFieldName.Value + " to be on the next ballod for " + DropDownListPostions.SelectedItem.Text + ".";
        }

            //LabelFeedback.Text = HiddenFieldId.Value + ", " + DropDownListPostions.SelectedItem.Text + ", " + dbLogic.returnUnionIDFromUsername(HttpContext.Current.User.Identity.Name) + "<br />" + dbLogic.countPetitionsForPerson(petitionInfo);
        
        //reset form
        txtSearch.Text = "";
        ListViewUsers.Visible = false;
        btnViewAll.Visible = false;
        PopupControlExtender1.Hide();

           
    }

    protected void ButtonCancel_Clicked(object sender, EventArgs e)
    {
        PopupControlExtender1.Hide();
    }
}
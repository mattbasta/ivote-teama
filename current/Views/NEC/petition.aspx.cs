using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class NEC_petition : System.Web.UI.Page
{
    databaseLogic dbLogic = new databaseLogic();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //notifications
            checkForNomination();

            //petition form
            dbLogic.selectAllAvailablePositions();
            DataSet positionSet = dbLogic.getResults();
            DropDownListPostions.DataSource = positionSet;
            DropDownListPostions.DataTextField = positionSet.Tables[0].Columns[1].ToString();
            DropDownListPostions.DataBind();
        }
    }

    //For server transferring with hyperlinks
    protected void transfer(object sender, CommandEventArgs e)
    {
        Server.Transfer(e.CommandArgument.ToString());
    }

    //check if the user has a nomination pending
    protected void checkForNomination()
    {
        if (dbLogic.isUserNominatedPending(dbLogic.returnUnionIDFromUsername(HttpContext.Current.User.Identity.Name)))
        {
            PanelNominationPending.Enabled = true;
            PanelNominationPending.Visible = true;
            nom_pending.Enabled = true;
            nom_pending.Visible = true;
        }
    }



    //Petition component code

    protected void search(object sender, EventArgs e)
    {
        dbLogic.SelectPeopleFromSearch(txtSearch.Text);
        DataSet emailSet = dbLogic.getResults();
        ListViewUsers.DataSource = emailSet;
        ListViewUsers.DataBind();
        ListViewUsers.Visible = true;

        //loadConfirmPopup();

        if (txtSearch.Text != "")
            btnViewAll.Visible = true;
    }

    protected void ListViewUsers_ItemCommand(Object sender, ListViewCommandEventArgs e)
    {
        if (String.Equals(e.CommandName, "nominate"))
        {
            LabelChoosPosition.Text = "Please select the position you would<br /> like " + dbLogic.selectFullName(e.CommandArgument.ToString()) + " to be petitioned for:";
            ButtonSubmit.OnClientClick = "return confirm('Are you sure you want to start this petition for " + dbLogic.selectFullName(e.CommandArgument.ToString()) + "?\\n(If accepted, you will not be able to withdraw your petition  later.)')";
            HiddenFieldName.Value = dbLogic.selectFullName(e.CommandArgument.ToString());
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
        string[] petitionInfo = { HiddenFieldId.Value, DropDownListPostions.SelectedItem.Text, dbLogic.returnUnionIDFromUsername(HttpContext.Current.User.Identity.Name) };

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
            }
            else
            {
                dbLogic.insertPetition(petitionInfo);
                if (dbLogic.countPetitionsForPerson(petitionInfo) == 1)
                {
                    LabelFeedback.Text = "Petition request submitted for " + HiddenFieldName.Value + " to be on the next ballot for " + DropDownListPostions.SelectedItem.Text + ".";
                }
                else
                {
                    LabelFeedback.Text = "Petition signature Submitted for " + HiddenFieldName.Value + " to be on the next ballot for " + DropDownListPostions.SelectedItem.Text + ".";
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
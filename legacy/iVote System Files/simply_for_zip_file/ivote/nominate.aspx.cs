using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class wwwroot_phase1aSite_nominate : System.Web.UI.Page
{
    databaseLogic dbLogic = new databaseLogic();
    emailer emailHelper = new emailer();
    protected void Page_PreRender(object sender, EventArgs e)
    {

    }


    protected void Page_Load(object sender, EventArgs e)
    {
        string fullPath = Request.PathInfo;
        string username = Page.User.Identity.Name.ToString();
        if (!Page.IsPostBack)
        {
            string[] categoryList;
            categoryList = fullPath.Substring(1).TrimEnd('/').Split('/');
            string queryVal = categoryList[0]; //variable name of querystring
            string positionTitle = dbLogic.selectPositionFromID(queryVal);
            LabelHeader.Text = positionTitle + " Nomination";
            LabelExplain.Text = "Please search for a faculty member to nominate for  " + positionTitle + " below:";
            HiddenFieldPosition.Value = positionTitle;
        }
    }


    protected void search(object sender, EventArgs e)
    {
        dbLogic.SelectPeopleFromSearch(txtSearch.Text);
        DataSet emailSet = dbLogic.getResults();
        ListViewUsers.DataSource = emailSet;
        ListViewUsers.DataBind();
        ListViewUsers.Visible = true;

        foreach (ListViewDataItem myItem in ListViewUsers.Items)
        {
            Button Nominate = (Button)myItem.FindControl("ButtonNominate");
            //If the current user's name appears in the results list, the nominate button is disabled
            if(Nominate.CommandArgument.ToString() == dbLogic.returnUnionIDFromUsername(HttpContext.Current.User.Identity.Name).ToString()) 
            {
                Nominate.ToolTip = "To nominate yourself, click \"Nominate Yourself\" on the homepage";
                Nominate.Enabled = false;
            }
        }
        //loadConfirmPopup();

        if (txtSearch.Text != "")
            btnViewAll.Visible = true;
    }

    protected void ListViewUsers_ItemCommand(Object sender, ListViewCommandEventArgs e)
    {
        if (String.Equals(e.CommandName, "nominate"))
        {
            LabelComplete.Text = "Thank you for nominating " + dbLogic.selectFullName(e.CommandArgument.ToString()) + " for " + HiddenFieldPosition.Value;

            PanelSearch.Visible = false;
            PanelComplete.Visible = true;
            PanelComplete.Enabled = true;

            string[] info = { e.CommandArgument.ToString(), dbLogic.returnUnionIDFromUsername(HttpContext.Current.User.Identity.Name), HiddenFieldPosition.Value };

            //inserts data into the db
            dbLogic.insertNominationAccept(info);
        
            string[] emailAddress = {dbLogic.selectEmailFromID(Convert.ToInt16(e.CommandArgument))};

            //send an email to the user nominated
            emailHelper.sendEmailToList(emailAddress, dbLogic.selectFullName(dbLogic.returnUnionIDFromUsername(HttpContext.Current.User.Identity.Name)) + " has nominated you for the position of " + HiddenFieldPosition.Value + " for the next voting period!<br /><br />To be fully nominated you must first accept this nomination,<br /> then fill out the digital \"Willingness To Serve\" form.<br />To accept (or reject) this nomination, log into the <a href=\"www.kenneronline.com/testsite/\" target=\"_blank\"> Kutztown iVote website</a>.<br /><br />The iVote System Team", "You've been nominated for " + HiddenFieldPosition.Value);
        }
    }

    /*
    protected void loadConfirmPopup()
    {
        foreach (ListViewDataItem myItem in ListViewUsers.Items)
        {
            //gets important info from each contact row
            Button button = (Button)myItem.FindControl("ButtonNominate");
            Label fullname = (Label)myItem.FindControl("LabelName");
            HiddenField position = (HiddenField)myItem.FindControl("HiddenFieldPosition");

            button.OnClientClick = "javascript:return " + "confirm('Are you sure you want to nominate " + fullname.Text + " for " + position.Value + "?')";
        }
    }
    */
    protected void clear(object sender, EventArgs e)
    {
        ListViewUsers.Visible = false;
        btnViewAll.Visible = false;
    }

}
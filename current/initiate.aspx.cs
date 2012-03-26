using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//Created by Adam Blank for CSC 354

public partial class initiate : System.Web.UI.Page
{
    databaseLogic dbLogic = new databaseLogic(); //for adding new positions
    ArrayList PageArrayList, VoteTypeList, DescriptionList, PluralityNumber, VoteNumber; //for adding new positions

    protected void Page_Load(object sender, EventArgs e)
    {
        if (ViewState["arrayListInViewState"] != null)
        {
            PageArrayList = (ArrayList)ViewState["arrayListInViewState"];
            VoteTypeList = (ArrayList)ViewState["voteListInViewState"];
            DescriptionList = (ArrayList)ViewState["descListInViewState"];
            PluralityNumber = (ArrayList)ViewState["plurNumListInViewState"];
            VoteNumber = (ArrayList)ViewState["VotesNumListInViewState"];
        }
        else
        {
            // ArrayList isn't in view state, so it must be created and populated.
            PageArrayList = new ArrayList();
            VoteTypeList = new ArrayList();
            DescriptionList = new ArrayList();
            PluralityNumber = new ArrayList();
            VoteNumber = new ArrayList();
        }
    }

    void Page_PreRender(object sender, EventArgs e)
    {
        ViewState.Add("arrayListInViewState", PageArrayList);
        ViewState.Add("voteListInViewState", VoteTypeList);
        ViewState.Add("descListInViewState", DescriptionList);
        ViewState.Add("plurNumListInViewState", PluralityNumber);
        ViewState.Add("VotesNumListInViewState", VoteNumber);
    }

    //adds new position unless user has plurality selected
    protected void addPosition_clicked(object sender, EventArgs e)
    {
        if (checkTitle(positionText.Text))
        {
            lblPosError.Visible = false;
            lblPosError.Text = "";
            if (voteMethodList.SelectedItem.Text == "Plurality")
                ModalPopupExtender1.Show();
            else
            {
                VoteNumber.Add("1");
                PluralityNumber.Add("1");
                buildPosition();
            }
            EmptyPositions.Visible = false;
        }
        else
        {
            lblPosError.Visible = true;
            lblPosError.Text = "<span style=\"color:red;\">You cannot insert the same position twice, or insert an untitled position.</span><br/>";
            UpdateTable();
        }
    }

    //checkTitle (Validation function)
    //checks to make sure a duplicate title is not being inserted
    //also checks to make sure the title isn't null.
    private bool checkTitle(string title)
    {
        //Check against the arraylist
        for (int i = 0; i < PageArrayList.Capacity; i++)
            if (PageArrayList[i].ToString().ToLower() == title.ToLower())
                return false;

        //check if null
        if (title.Trim() == "")
            return false;

        return true;
    }

    //adds new position with number of sub positions available (for plurality)
    protected void ButtonPluralityComplete_clicked(object sender, EventArgs e)
    {

        if (!(Convert.ToInt32(DropDownListPlurality.SelectedItem.Text) < Convert.ToInt32(DropDownListVoting.SelectedItem.Text)))
        {
            ModalPopupExtender1.Hide();
            buildPosition();
            PluralityNumber.Add(DropDownListPlurality.SelectedItem.Text);
            VoteNumber.Add(DropDownListVoting.SelectedItem.Text);
            DropDownListPlurality.SelectedIndex = 0;
            popupError.Text = "";
        }
        else
        {
            popupError.Text = "<span style=\"color:red;\">Number of votes may not exceed number of positions available.</span>";
            UpdateTable();
        }
    }

    protected void ButtonPluralityCancel_clicked(object sender, EventArgs e)
    {
        popupError.Text = "";
        ModalPopupExtender1.Hide();
    }

    //for adding new positions
    protected void buildPosition()
    {
        PageArrayList.Add(positionText.Text);
        VoteTypeList.Add(voteMethodList.SelectedItem.Text);
        DescriptionList.Add(Description.Text);

        //shows the pannel with the positions list 
        positionsList.Visible = true;
        UpdateTable();

        positionText.Text = "";
        Description.Text = "";
    }

    //for adding new positions
    public void UpdateTable()
    {
    
        System.Collections.IEnumerator pageEnum = PageArrayList.GetEnumerator();
        System.Collections.IEnumerator voteEnum = VoteTypeList.GetEnumerator();
        
        while(pageEnum.MoveNext() && voteEnum.MoveNext()) {
            TableRow tr = new TableRow();
            TableCell name, vote_type;
            
            name = new TableCell();
            name.Controls.Add(new LiteralControl(pageEnum.Current.ToString()));
            tr.Cells.Add(name);
            
            vote_type = new TableCell();
            vote_type.Controls.Add(new LiteralControl(voteEnum.Current.ToString()));
            tr.Cells.Add(vote_type);
            
            PositionTable.Controls.Add(tr);
        }
    }

/*
    //clears the positions list so the user can start over
    protected void LinkButtonClear_Clicked(object sender, EventArgs e)
    {
        list.Text = "";
        PageArrayList = new ArrayList();
        VoteTypeList = new ArrayList();
        DescriptionList = new ArrayList();
        PluralityNumber = new ArrayList();
        VoteNumber = new ArrayList();
    }*/

    protected void ButtonSave_Clicked(object sender, EventArgs e)
    {
           if (PageArrayList.Count == 0 || VoteTypeList.Count == 0)
           {
               LabelFeedback.Text = "You must enter at least one position!";
               return;
           }

           dbLogic.createTimeline();

           //Inserts positions
           dbLogic.addPos(PageArrayList, VoteTypeList, DescriptionList, PluralityNumber, VoteNumber);
           
           Response.Redirect("/officer_election.aspx");
           
    }
}
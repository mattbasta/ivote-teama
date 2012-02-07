using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//Created by Adam Blank for CSC 354

public partial class controlRoom : System.Web.UI.Page
{
    databaseLogic dbLogic = new databaseLogic(); //for adding new positions
    ArrayList PageArrayList, VoteTypeList, DescriptionList, PluralityNumber, VoteNumber; //for adding new positions
    int counter; //for adding new positions
    DateTime nomination;
    DateTime petition;
    DateTime vote;
    DateTime voteEnd;

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!Page.IsPostBack)
        //{
            //lblForm.Visible = true;
            //for adding new positions
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
        //}
    }

    void Page_PreRender(object sender, EventArgs e)
    {
        //for adding new positions
        // Save PageArrayList before the page is rendered.
        //if (!Page.IsPostBack)
        //{
            ViewState.Add("arrayListInViewState", PageArrayList);
            ViewState.Add("voteListInViewState", VoteTypeList);
            ViewState.Add("descListInViewState", DescriptionList);
            ViewState.Add("plurNumListInViewState", PluralityNumber);
            ViewState.Add("VotesNumListInViewState", VoteNumber);
        //}
    }

    //adds new position unless user has plurality selected
    protected void addPosition_clicked(object sender, EventArgs e)
    {
        if (checkTitle())
        {
            lblPosError.Visible = false;
            lblPosError.Text = "";
            if (voteMethodList.SelectedItem.Text == "Plurality")
            {
                LabelPlurality.Text = "Please select the number of available positions for <u>" + positionText.Text + "</u>.<br /> (If there is only 1 available position then please choose a different tally method instead of plurality.)";
                ModalPopupExtender1.Show();
            }
            else
            {
                VoteNumber.Add("1");
                PluralityNumber.Add("1");
                buildPosition();
            }
        }
        else
        {
            lblPosError.Visible = true;
            lblPosError.Text = "<span style=\"color:red;\">You cannot insert the same position twice, or insert an untitled position.</span><br/>";
        }
    }

    //checkTitle (Validation function)
    //checks to make sure a duplicate title is not being inserted
    //also checks to make sure the title isn't null.
    private bool checkTitle()
    {
        //return flag
        bool flag = true;

        //Check against the arraylist
        for (int i = 0; i < PageArrayList.Capacity; i++)
        {
            if (PageArrayList[i].ToString().ToLower() == positionText.Text.ToLower())
                flag = false;
        }

        //check if null
        if (positionText.Text == "")
            flag = false;

        //make sure isn't all spaces
        bool allSpaces = true;
        for (int i = 0; i < positionText.Text.Length; i++)
        {
            if (!(positionText.Text[i] == ' '))
                allSpaces = false;
        }
        if (allSpaces)
            flag = false;

        //return
        return flag;
    }

    //adds new position with number of sub positions available (for plurality)
    protected void ButtonPluralityComplete_clicked(object sender, EventArgs e)
    {

        /*super quick work-around to random bug involving ArrayList and dropdownlist (Should be resolved in the future)
        if (DropDownListPlurality.SelectedItem.Text == "2")
            number = "2";
        else if (DropDownListPlurality.SelectedItem.Text == "3")
            number = "3";
        else if (DropDownListPlurality.SelectedItem.Text == "4")
            number = "4";
        else if (DropDownListPlurality.SelectedItem.Text == "5")
            number = "5";
        else if (DropDownListPlurality.SelectedItem.Text == "6")
            number = "6";
        else if (DropDownListPlurality.SelectedItem.Text == "7")
            number = "7";
        else if (DropDownListPlurality.SelectedItem.Text == "8")
            number = "8";
        else if (DropDownListPlurality.SelectedItem.Text == "9")
            number = "9";
        else if (DropDownListPlurality.SelectedItem.Text == "10")
            number = "10";

        if (number != "")
        {*/
        if (!(Convert.ToInt32(DropDownListPlurality.SelectedItem.Text) < Convert.ToInt32(DropDownListVoting.SelectedItem.Text)))
        {
            ModalPopupExtender1.Hide();
            buildPosition();
            PluralityNumber.Add(DropDownListPlurality.SelectedItem.Text);
            VoteNumber.Add(DropDownListVoting.SelectedItem.Text);
            DropDownListPlurality.SelectedIndex = 0;
        }
        else
        {
            popupError.Text = "<span style=\"color:red;\">Number of votes may not exceed number of positions available.</span>";
        }
        //}
    }

    //for adding new positions
    protected void buildPosition()
    {
        PageArrayList.Add(positionText.Text);
        VoteTypeList.Add(voteMethodList.SelectedItem.Text);
        DescriptionList.Add(Description.Text);

        //shows the pannel with the positions list 
        positionsList.Visible = true;
        //displays "clear" button if list is populated
        if (LinkButtonClear.Visible == false) 
        {
            LinkButtonClear.Visible = true;
        }
        PrintValues(PageArrayList, VoteTypeList);

        positionText.Text = "";
        Description.Text = "";

    }


    //for adding new positions
    public void PrintValues(IEnumerable myList, IEnumerable myVoteList)
    {
        list.Text = "<table class=\"simpleGrid\" style=\"width: 60%\"><tr><th>Position Title</th><th>Tally Method</th><th></th></tr>";
        LabelEndTable.Visible = true;
        System.Collections.IEnumerator myEnumerator = myList.GetEnumerator();
        System.Collections.IEnumerator myVoteEnumerator = myVoteList.GetEnumerator();
        while (myEnumerator.MoveNext() && myVoteEnumerator.MoveNext())
            list.Text += "<tr><td>" + myEnumerator.Current + "</td><td>" + myVoteEnumerator.Current + "</td></tr>";
    }

    //clears the positions list so the user can start over
    protected void LinkButtonClear_Clicked(object sender, EventArgs e)
    {
        list.Text = "";
        LabelEndTable.Visible = false;
        PageArrayList = new ArrayList();
        VoteTypeList = new ArrayList();
        DescriptionList = new ArrayList();
        PluralityNumber = new ArrayList();
        VoteNumber = new ArrayList();
        LinkButtonClear.Visible = false;
    }

    protected void ButtonSave_Clicked(object sender, EventArgs e)
    {
       //if (Page.IsValid )
        //{

            int hourNomination = Convert.ToInt32(TextBoxTimeHourNomination.Text);
            int hourAccept1 = Convert.ToInt32(TextBoxTimeHourAccept1.Text);
            int hourAccept2 = Convert.ToInt32(TextBoxTimeHourAccept1.Text);
            int hourPetition = Convert.ToInt32(TextBoxTimeHourPetition.Text);
            int hourVote = Convert.ToInt32(TextBoxTimeHourVote.Text);
            int hourVoteEnd = Convert.ToInt32(TextBoxTimeHourVoteEnd.Text);
            string strHourNomination = "00";
            string strHourAccept1 = "00";
            string strHourAccept2 = "00";
            string strHourPetition = "00";
            string strHourVote = "00";
            string strHourVoteEnd = "00";

            if (hourNomination >= 3)
            {
                strHourNomination = "";
                hourNomination = hourNomination - 3;
                if (hourNomination < 10)
                    strHourNomination = "0";
                strHourNomination += hourNomination.ToString();
            }
            if (hourAccept1 >= 3)
            {
                strHourAccept1 = "";
                hourAccept1 = hourAccept1 - 3;
                if (hourAccept1 < 10)
                    strHourAccept1 = "0";
                strHourAccept1 += hourAccept1.ToString();
            }
            if (hourPetition >= 3)
            {
                strHourPetition = "";
                hourPetition = hourPetition - 3;
                if (hourPetition < 10)
                    strHourPetition = "0";
                strHourPetition += hourPetition.ToString();
            }
            if (hourAccept2 >= 3)
            {
                strHourAccept2 = "";
                hourAccept2 = hourAccept2 - 3;
                if (hourAccept2 < 10)
                    strHourAccept2 = "0";
                strHourAccept2 += hourAccept2.ToString();
            }
            if (hourVote >= 3)
            {
                strHourVote = "";
                hourVote = hourVote - 3;
                if (hourVote < 10)
                    strHourVote = "0";
                strHourVote += hourVote.ToString();
            }
            if (hourVoteEnd >= 3)
            {
                strHourVoteEnd = "";
                hourVoteEnd = hourVoteEnd - 3;
                if (hourVoteEnd < 10)
                    strHourVoteEnd = "0";
                strHourVoteEnd += hourVoteEnd.ToString();
            }

            //LabelFeedback.Text = "Nom: " + hourNomination.ToString() + ". Pet = " + hourPetition.ToString() + ". Vote: " + hourVote.ToString() + ". VoteEnd: " + hourVoteEnd.ToString();
            //create a list of dates
           String[] Dates = { TextBoxDateNomination.Text, TextBoxDateAccept1.Text, TextBoxDatePetition.Text, TextBoxDateAccept2.Text, TextBoxDateVote.Text, TextBoxDateVoteEnd.Text};
           String[] Times = { strHourNomination + ":" + TextBoxTimeMinNomination.Text, strHourAccept1 + ":" + TextBoxTimeMinAccept1.Text, strHourPetition + ":" + TextBoxTimeMinPetition.Text, strHourAccept2 + ":" + TextBoxTimeMinAccept2.Text, strHourVote + ":" + TextBoxTimeMinVote.Text, strHourVoteEnd + ":" + TextBoxTimeMinVoteEnd.Text };

           if (PageArrayList.Count == 0 || VoteTypeList.Count == 0)
           {
               LabelFeedback.Text = "You must enter at least one position!";
               return;
           }
           
           //create timeline

           //reset database for reseeding
           dbLogic.resetElection();

           //Insert new election into the election table
           dbLogic.insertElection();
           dbLogic.createTimeline(Dates, Times, dbLogic.returnLatestElectionId());

           //Inserts positions
           dbLogic.addPos(PageArrayList, VoteTypeList, DescriptionList, PluralityNumber, VoteNumber);

           //Display feedback
           positionsAdd.Visible = false;
           positionsList.Visible = false;
           PanelComplete.Visible = true;
           PanelCreateElection.Visible = false;
           //LabelFeedback.Text = "Election Successfully Processed and Created.";

           //Create the datetime value to display to the user
           nomination = createDateTime(TextBoxDateNomination.Text, strHourNomination + ":" + TextBoxTimeMinNomination.Text);
           petition = createDateTime(TextBoxDatePetition.Text, strHourPetition + ":" + TextBoxTimeMinPetition.Text);
           vote = createDateTime(TextBoxDateVote.Text, strHourVote + ":" + TextBoxTimeMinVote.Text);
           voteEnd = createDateTime(TextBoxDateVoteEnd.Text, strHourVoteEnd + ":" + TextBoxTimeMinVoteEnd.Text);

           //Create timeline information for admin

           LabelFinalTimeline.Text = ""; //clear any prior text
           LabelFinalTimeline.Text += "The nomination phase will offically begin on <u>" + getLongDateString(nomination) + "</u> in " + getTimeSpanBetweenTwoDates(DateTime.Now, nomination) + ".<br /><br />";
           LabelFinalTimeline.Text += "The nomination phase will end on <u>" + getLongDateString(petition) + "</u>.<br /><br />";
           LabelFinalTimeline.Text += "The petition phase will offically begin on <u>" + getLongDateString(petition) + "</u> in " + getTimeSpanBetweenTwoDates(DateTime.Now, petition) + ".<br /><br />";
           LabelFinalTimeline.Text += "The petition phase will end on <u>" + getLongDateString(vote) + "</u>.<br /><br />";
           LabelFinalTimeline.Text += "The voting phase will offically begin on <u>" + getLongDateString(vote) + "</u> in " + getTimeSpanBetweenTwoDates(DateTime.Now, vote) + ".<br /><br />";
           LabelFinalTimeline.Text += "The voting phase will end on <u>" + getLongDateString(voteEnd) + "</u>.<br /><br /><hr />";
           LabelFinalTimeline.Text += "<span style=\"font-size: 14px; font-weight: bold\">The span of each phase, in the election process, is as follows:</span><br /><br />";
           LabelFinalTimeline.Text += "Users will have <u>" + getTimeSpanBetweenTwoDates(nomination, petition) + "</u> to nominate and accept nominations<br /><br />";
           LabelFinalTimeline.Text += "Users will have <u>" + getTimeSpanBetweenTwoDates(petition, vote) + "</u> after the nomination phase to petition someone<br /><br />";
           LabelFinalTimeline.Text += "Users will have <u>" + getTimeSpanBetweenTwoDates(vote, voteEnd) + "</u> to submit their offical ballot for the election<br /><br /><br />";
           LabelFinalTimeline.Text += "You, and the NEC president, will be able to view the offical results of the election on:<br /> ";
           LabelFinalTimeline.Text += "<b>" + getLongDateString(vote) + " at " + voteEnd.ToLongTimeString() + ".</b>";
        //}
    }

    protected string getTimeSpanBetweenTwoDates(DateTime dateBegin, DateTime dateEnd)
    {
        long timeBetween = dateEnd.Ticks - dateBegin.Ticks;
        TimeSpan elapsedSpan = new TimeSpan(timeBetween);
        string span = "";
        if (elapsedSpan.Days > 0)
        {
            span += elapsedSpan.Days.ToString() + " day(s)";
        }
        if (elapsedSpan.Hours > 0)
        {
            if (elapsedSpan.Days > 0)
                span += ", ";
            span += elapsedSpan.Hours.ToString() + " hour(s)";
        }
        if (elapsedSpan.Minutes > 0)
        {
            if((elapsedSpan.Days > 0) || (elapsedSpan.Hours > 0))
                span += ", ";
            span += elapsedSpan.Minutes.ToString() + " minute(s)";
        }
        return span;
    }

    //creates a datetime from the strings
    protected DateTime createDateTime(string date, string time)
    {
        string[] formats = {"M/d/yyyy H:mm:ss tt", "M/d/yyyy HH:mm tt", 
                           "MM/dd/yyyy HH:mm:ss", "M/d/yyyy H:mm:ss", 
                           "M/d/yyyy HH:mm tt", "M/d/yyyy HH tt", 
                           "M/d/yyyy H:mm", "M/d/yyyy H:mm", 
                           "MM/dd/yyyy HH:mm", "M/dd/yyyy HH:mm"};
        DateTime newDate = DateTime.ParseExact(date + " " + time, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
        return newDate;
    }

    protected string getLongDateString(DateTime newDate)
    {
        return newDate.DayOfWeek + ", " + newDate.Month + "/" + newDate.Day;
    }

    protected void updateSpanNominate(object sender, EventArgs e)
    {

        TextBoxTimeHourAccept1.Enabled = true;
        TextBoxTimeMinAccept1.Enabled = true;
        TextBoxDateAccept1.Enabled = true;

        LabelWhenNominate.Text = "";
        LabelWhenAccept1.Text = "Now enter the date and time for this phase.";

        /*int hour = Convert.ToInt16(TextBoxTimeHourNomination.Text);
        hour = hour - 3; // IMPORTANT: Needed to fix the fact the the server is in a different time zone, should be removed if system is ever hosted at KU
        
        nomination = createDateTime(TextBoxDateNomination.Text, hour.ToString() + ":" + TextBoxTimeMinNomination.Text);
        LabelWhenNominate.Text = "Phase begins in " + getTimeSpanBetweenTwoDates(DateTime.Now, nomination);
         */
    }

    protected void updateSpanAccept1(object sender, EventArgs e)
    {

        TextBoxTimeHourPetition.Enabled = true;
        TextBoxTimeMinPetition.Enabled = true;
        TextBoxDatePetition.Enabled = true;

        LabelWhenNominate.Text = "";
        LabelWhenPetition.Text = "Now enter the date and time for this phase.";

        /*int hour = Convert.ToInt16(TextBoxTimeHourNomination.Text);
        hour = hour - 3; // IMPORTANT: Needed to fix the fact the the server is in a different time zone, should be removed if system is ever hosted at KU
        
        nomination = createDateTime(TextBoxDateNomination.Text, hour.ToString() + ":" + TextBoxTimeMinNomination.Text);
        LabelWhenNominate.Text = "Phase begins in " + getTimeSpanBetweenTwoDates(DateTime.Now, nomination);
         */
    }


    protected void updateSpanPetition(object sender, EventArgs e)
    {
        TextBoxTimeHourAccept2.Enabled = true;
        TextBoxTimeMinAccept2.Enabled = true;
        TextBoxDateAccept2.Enabled = true;

        LabelWhenAccept2.Text = "Now enter the date and time for this phase.";
        LabelWhenPetition.Text = "";
        /*
        int hour = Convert.ToInt16(TextBoxTimeHourPetition.Text);
        hour = hour - 3; // IMPORTANT: Needed to fix the fact the the server is in a different time zone, should be removed if system is ever hosted at KU

        petition = createDateTime(TextBoxDatePetition.Text, hour.ToString() + ":" + TextBoxTimeMinPetition.Text);
        LabelWhenPetition.Text = "Phase begins in " + getTimeSpanBetweenTwoDates(DateTime.Now, petition);
        */
    }

    protected void updateSpanAccept2(object sender, EventArgs e)
    {
        TextBoxTimeHourVote.Enabled = true;
        TextBoxTimeMinVote.Enabled = true;
        TextBoxDateVote.Enabled = true;

        LabelWhenVote.Text = "Now enter the date and time for this phase.";
        LabelWhenPetition.Text = "";
        /*
        int hour = Convert.ToInt16(TextBoxTimeHourPetition.Text);
        hour = hour - 3; // IMPORTANT: Needed to fix the fact the the server is in a different time zone, should be removed if system is ever hosted at KU

        petition = createDateTime(TextBoxDatePetition.Text, hour.ToString() + ":" + TextBoxTimeMinPetition.Text);
        LabelWhenPetition.Text = "Phase begins in " + getTimeSpanBetweenTwoDates(DateTime.Now, petition);
        */
    }

    protected void updateSpanVote(object sender, EventArgs e)
    {
        TextBoxTimeHourVoteEnd.Enabled = true;
        TextBoxTimeMinVoteEnd.Enabled = true;
        TextBoxDateVoteEnd.Enabled = true;

        LabelWhenVoteEnd.Text = "Finally, enter the date and time for the voting phase to end.";
        LabelWhenVote.Text = "";
        /*
        int hour = Convert.ToInt16(TextBoxTimeHourVote.Text);
        hour = hour - 3; // IMPORTANT: Needed to fix the fact the the server is in a different time zone, should be removed if system is ever hosted at KU

        vote = createDateTime(TextBoxDateVote.Text, hour.ToString() + ":" + TextBoxTimeMinVote.Text);
        LabelWhenVote.Text = "Phase begins in " + getTimeSpanBetweenTwoDates(DateTime.Now, vote);
        */
    }

    protected void updateSpanVoteEnd(object sender, EventArgs e)
    {
        LabelWhenVoteEnd.Text = "";

        /*
        int hour = Convert.ToInt16(TextBoxTimeHourVoteEnd.Text);
        hour = hour - 3; // IMPORTANT: Needed to fix the fact the the server is in a different time zone, should be removed if system is ever hosted at KU

        voteEnd = createDateTime(TextBoxDateVoteEnd.Text, hour.ToString() + ":" + TextBoxTimeMinVoteEnd.Text);
        LabelWhenVoteEnd.Text = "Phase begins in " + getTimeSpanBetweenTwoDates(DateTime.Now, voteEnd);
         */
    }

    protected void ButtonSendEmail_clicked(object sender, EventArgs e)
    {
        emailer emailSender = new emailer();
        string[] emailAddress = new string[1]; 
        emailAddress[0] = dbLogic.selectEmailFromID(Convert.ToInt16(dbLogic.returnUnionIDFromUsername(HttpContext.Current.User.Identity.Name)));
        emailSender.sendEmailToList(emailAddress, LabelFinalTimeline.Text, "Official Timeline For New Election");

        LabelFeedback.Text = "Email message successfully sent. Please wait 5-10 minutes for email to appear in your inbox.";
        ButtonSendEmail.Enabled = false;
    }
}
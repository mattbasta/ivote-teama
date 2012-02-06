using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class timeline : System.Web.UI.Page
{
    databaseLogic dbLogic = new databaseLogic();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void ButtonSave_Clicked(object sender, EventArgs e)
    {
        if (TextBoxDateNomination.Text != "")
        {
            dbLogic.updateTimeline(TextBoxDateNomination.Text, TextBoxTimeNomination.Text, "nullphase");
        }
        if (TextBoxDatePetition.Text != "")
        {
            dbLogic.updateTimeline(TextBoxDatePetition.Text, TextBoxTimePetition.Text, "nominate");
        }
        if (TextBoxDateVote.Text != "")
        {
            dbLogic.updateTimeline(TextBoxDateVote.Text, TextBoxTimeVote.Text, "petition");
        }
        if (TextBoxDateVoteEnd.Text != "")
        {
            dbLogic.updateTimeline(TextBoxDateVoteEnd.Text, TextBoxTimeVoteEnd.Text, "vote");
        }

      
    }

    protected string getTimeSpan(string date, string time)
    {
        DateTime newDate = DateTime.ParseExact(date + " " + time, "MM/dd/yyyy HH:mm", null);
        //DateTime now = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
        //DateTime correctTimeZoneDate = TimeZoneInfo.ConvertTime(newDate, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
        long timeBetween = newDate.Ticks - DateTime.Now.Ticks;
        TimeSpan elapsedSpan = new TimeSpan(timeBetween);
        return "Phase Starts in " + elapsedSpan.Days.ToString() + " days, " + elapsedSpan.Hours.ToString() + " hours, " + elapsedSpan.Minutes.ToString() + " minutes.";
    }

    protected void updateSpanNominate(object sender, EventArgs e)
    {
        LabelWhenNominate.Text = getTimeSpan(TextBoxDateNomination.Text, TextBoxTimeNomination.Text);
    }

    protected void updateSpanPetition(object sender, EventArgs e)
    {
        LabelWhenPetition.Text = getTimeSpan(TextBoxDatePetition.Text, TextBoxTimePetition.Text);
    }

    protected void updateSpanVote(object sender, EventArgs e)
    {
        LabelWhenVote.Text = getTimeSpan(TextBoxDateVote.Text, TextBoxTimeVote.Text);
    }

    protected void updateSpanVoteEnd(object sender, EventArgs e)
    {
        LabelWhenVoteEnd.Text = getTimeSpan(TextBoxDateVoteEnd.Text, TextBoxTimeVoteEnd.Text);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class wwwroot_phase1aSite_users : System.Web.UI.Page
{
    databaseLogic dbLogic = new databaseLogic();
    voteCounter vc = new voteCounter();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            
        }
    }

    protected void extend_OnClick(object sender, EventArgs e)
    {
        //end.Text = 
        //vc.extendVotingWeek();
        /*
        int newDay = 0, newMonth = 0, newYear = 0;
        //get end date of voting
        dbLogic.getEndDate("vote");
        DataSet phaseSet = dbLogic.getResults();
        DataRow dr = phaseSet.Tables["query"].Rows[0];
        DateTime newDate = createDateTime(dr["datetime_end"].ToString());

        DateTime finalDate = newDate.AddDays(7.0);
        //long timeBetween = dateEnd.Ticks - dateBegin.Ticks;
        //TimeSpan elapsedSpan = new TimeSpan(timeBetween);
        //string span = "";

        dbLogic.updateTimeline2(finalDate.ToString(), "vote");*/

        dbLogic.updateVotePhase();
    }

    protected DateTime createDateTime(string datetime)
    {
        string[] formats = {"M/d/yyyy H:mm:ss tt", "M/d/yyyy HH:mm tt", 
                           "MM/dd/yyyy HH:mm:ss", "M/d/yyyy H:mm:ss", 
                           "M/d/yyyy HH:mm tt", "M/d/yyyy HH tt", 
                           "M/d/yyyy H:mm", "M/d/yyyy H:mm", 
                           "MM/dd/yyyy HH:mm", "M/dd/yyyy HH:mm", "MM/d/yyyy HH:mm:ss tt"};
        DateTime newDate = DateTime.ParseExact(datetime, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
        return newDate;
    }
}
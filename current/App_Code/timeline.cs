using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for timeline
/// </summary>
public class timeline
{
    databaseLogic dbLogic = new databaseLogic();
    public string currentPhase = "";

    public timeline()
	{
        currentPhase = checkPhase();
	}

    //check which phase a user is currently in 
    public string checkPhase()
    {
        string phase = "";

        string[] formats = {"M/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm tt", 
                           "MM/dd/yyyy hh:mm:ss", "M/d/yyyy h:mm:ss", 
                           "M/d/yyyy hh:mm tt", "M/d/yyyy hh tt", 
                           "M/d/yyyy h:mm", "M/d/yyyy h:mm", 
                           "MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm"};

        dbLogic.selectPhaseNamesAndDates();
        DataSet phaseSet = dbLogic.getResults();
        DataRow dr;
        for (int i = 0; i < phaseSet.Tables["query"].Rows.Count; i++)
        {
            dr = phaseSet.Tables["query"].Rows[i];
            DateTime phaseDatetime = DateTime.ParseExact(dr["datetime_end"].ToString(), formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
            int result = DateTime.Compare(phaseDatetime, DateTime.Now); //compares the phase datetime with the current datetime
            
            //now is before the phase
            if (result > 0)
            {
                if (phase == "")
                    phase = dr["name_phase"].ToString(); //if 
            }
            //now is exactly the phase (HIGHLY unlikely)
            else if (result == 0)
            {
                checkPhase(); //re-runs check phase until it doesn't get this result (being that DateTime.Now will change eventaully)
            }
            //now is after the phase
            else 
            {

            }
        }
        return phase;
    }

    //checks if it's the first time the specified phase has occured in the timeline
    public bool firstTimeOccurence(string phase_name)
    {
        if (dbLogic.currentPhase() == phase_name) //not first time
        {
            return false;
        }
        else //first time
        {
            //goes into timeline table and turns off all phases except the new current phase
            dbLogic.selectPhaseNames();
            DataSet phaseSet = dbLogic.getResults();
            DataRow dr;
            for (int i = 0; i < phaseSet.Tables["query"].Rows.Count; i++)
            {
                dr = phaseSet.Tables["query"].Rows[i];
                if (dr["name_phase"].ToString() == phase_name)
                {
                    dbLogic.turnOnPhase(phase_name);
                }
                else
                {
                    dbLogic.turnOffPhase(dr["name_phase"].ToString());
                }
            }
            return true;
        }
    }

    //get a list of the phases
    public DataSet getPhaseNames()
    {
        dbLogic.selectPhaseNames();
        return dbLogic.getResults();
    }

    //get a list of the dates of each phase
    public DataSet getDates()
    {
        dbLogic.selectPhaseDates();
        return dbLogic.getResults();
    }
    
    //set the date of a specific phase
    public void setDateForPhase(string phase)
    {

    }


    public bool changePhaseToCurrent(string phase)
    {
        if(phase == "nomination")
        {
            dbLogic.turnOnPhase("nomination");
            dbLogic.turnOffPhase("petition");
            dbLogic.turnOffPhase("vote");
        }
        else if(phase == "petition")
        {
            dbLogic.turnOffPhase("nomination");
            dbLogic.turnOnPhase("petition");
            dbLogic.turnOffPhase("vote");
        }
         else if(phase == "vote")
        {
             dbLogic.turnOffPhase("nomination");
            dbLogic.turnOffPhase("petition");
            dbLogic.turnOnPhase("vote");
        }
        
        if (dbLogic.currentPhase() == phase)
            return true;
        else
            return false;
    }

    //manually change now to a specific phase
    //public void manualChangeToCurrent(string phase)

    //check if a phase exists
    public bool DoesPhaseExist(string phase)
    {
        return true;
    }

}
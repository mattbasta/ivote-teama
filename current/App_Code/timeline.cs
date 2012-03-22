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
        currentPhase = dbLogic.currentPhase();
	}

    //checks if it's the first time the specified phase has occured in the timeline
    public bool firstTimeOccurence(string phase_name)
    {
        if (dbLogic.currentPhase() == phase_name) //not first time
            return false;
        else //first time
        {
            //goes into timeline table and turns off all phases except the new current phase
            dbLogic.selectPhaseNames();
            DataSet phaseSet = dbLogic.getResults();
            DataRow dr;
            for (int i = 0; i < phaseSet.Tables["query"].Rows.Count; i++)
            {
                String row_name = phaseSet.Tables["query"].Rows[i]["name_phase"].ToString();
                if (row_name == phase_name)
                    dbLogic.turnOnPhase(row_name);
                else
                    dbLogic.turnOffPhase(row_name);
            }
            return true;
        }
    }

    public bool changePhaseToCurrent(string phase)
    {
        string[] iter_phases = {"nominate", "accept1", "petition", "accept2", "vote", "slate", "approval", "result"};
        foreach(string i in iter_phases)
        {
            if(phase == i) dbLogic.turnOnPhase(i);
            else dbLogic.turnOffPhase(i);
        }
        
        return dbLogic.currentPhase() == phase;
    }

}
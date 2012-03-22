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
    VerifyEmail email = new VerifyEmail();
    public string currentPhase = "";

    public timeline()
	{
        currentPhase = dbLogic.currentPhase();
	}

    public bool changePhaseToCurrent(string phase, bool force=false)
    {
        if(dbLogic.currentPhase() == phase)
            return false;
        
        string[] iter_phases = {"nominate", "accept1", "petition", "accept2", "vote", "slate", "approval", "result"};
        int changed_to = 0;
        for(int i = 0; i < iter_phases.Length; i++)
        {
            if(phase == iter_phases[i])
            {
                dbLogic.turnOnPhase(iter_phases[i], DateTime.Now);
                changed_to = i;
            }
            else dbLogic.turnOffPhase(iter_phases[i]);
        }
        
        if(dbLogic.canSkipPhase() && !force)
            return changePhaseToCurrent(iter_phases[changed_to + 1]);
        
        switch(phase)
        {
            case "nominate":
                email.phaseNomination();
                break;
            case "accept1":
                email.phaseAccept1();
                break;
            case "slate":
                email.phaseSlate();
                break;
            case "petition":
                email.phasePetition();
                dbLogic.phase_ClearNullNominations();
                break;
            case "accept2":
                email.phaseAccept2();
                break;
            case "approval":
                email.phaseApproval();
                break;
            case "vote":
                email.phaseVote();
                dbLogic.phase_ClearNullNominations();
                break;
            case "result":
                email.phaseResults();
                break;
        }
        
        return true;
    }

    public int daysRemaining()
    {
        string[] iter_phases = {"nominate", "accept1", "petition", "accept2", "vote", "slate", "approval"};
        int[] phase_durations = {7, 7, 7, 7, 7, 7, 7};
        for(int i = 0; i < iter_phases.Length; i++)
            if(currentPhase == iter_phases[i])
                return (int)DateTime.Now.Subtract(dbLogic.currentPhaseEndDateTime()).TotalDays;
        return 0;
    }

}
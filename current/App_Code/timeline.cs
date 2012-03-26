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
        
        string[] iter_phases = {"nominate", "accept1", "slate", "petition", "accept2", "approval", "vote", "result"};
        int changed_to = 0;
        for(int i = 0; i < iter_phases.Length; i++)
        {
            if(phase == iter_phases[i])
            {
                dbLogic.turnOnPhase(iter_phases[i]);
                changed_to = i;
                break;
            }
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

    public bool bumpPhase()
    {
        string[] iter_phases = {"nominate", "accept1", "slate", "petition", "accept2", "approval", "vote", "result"};
        for(int i = 0; i < iter_phases.Length - 1; i++)
            if(currentPhase == iter_phases[i])
                return changePhaseToCurrent(iter_phases[i + 1]);
        return false;
    }

    public int daysRemaining()
    {
        string[] iter_phases = {"nominate", "accept1", "slate", "petition", "accept2", "approval", "vote"};
        int[] phase_durations = {7, 7, 7, 7, 7, 7, 7};
        for(int i = 0; i < iter_phases.Length; i++)
            if(currentPhase == iter_phases[i])
                return (int)dbLogic.currentPhaseEndDateTime().AddDays(phase_durations[i]).Subtract(DateTime.Now).TotalDays;
        return 0;
    }

}
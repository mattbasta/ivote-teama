using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public class election
{
    private string phase;
    private int id;
    private string electionName;
    private timeline time = new timeline();
    
    public election(string PHASE, int ID, string NAME)
	{
        phase = PHASE;
        id = ID;
        electionName = NAME;
	}

    
    //********* methods for phase *************//
    
    public string checkPhase()
    {
        return phase;
    }

    public void updatePhase(string newPhase)
    {
        phase = newPhase;
    }


    //********* methods for id ****************//

    public int checkID()
    {
        return id;
    }

    public void updateID(int ID)
    {
        id = ID;
    }


    //********** methods for electionName **********//

    public string checkName()
    {
        return electionName;
    }

    public void updateName(string name)
    {
        electionName = name;
    }

}
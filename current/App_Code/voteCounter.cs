using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Data;
using System.Web;


public class voteCounter
{
    databaseLogic dbLogic = new databaseLogic();
    timeline phases = new timeline();

    DataSet ds;                   // DataSet that is meant to contain all of the information from the table
    DataSet dsForElection;        // DataSet that will hold the positions for this election
    ArrayList userIDs;            // userIDs is an ArrayList of all of the users in the election for the current position
    ArrayList userVotes;          // userVotes is meant to be a parallel ArrayList to userIDs to keep track of that users votes
    int totalForPosition;         // keeps track of the total votes for the current position
    string query;

    public voteCounter()
	{
        userIDs = new ArrayList();
        userVotes = new ArrayList();
        totalForPosition = 0;

        dbLogic.selectAllAvailablePositions();
        dsForElection = dbLogic.getResults();
	}


    // main function that will be the only public function to cycle though the positions and types in election positions
    //      table and use a switch to dictate which way will be used to tally.
    public void tally()
    {
        int i = 0;    
        while(i < dsForElection.Tables[0].Rows.Count)
        {
                switch (dsForElection.Tables[0].Rows[i].ItemArray[3].ToString())
                {
                    case "Simple":
                        classic(dsForElection.Tables[0].Rows[i].ItemArray[2].ToString());
                        break;
                    case "Plurality":
                        plural(dsForElection.Tables[0].Rows[i].ItemArray[2].ToString());
                        break;
                    case "Majority":
                        majority(dsForElection.Tables[0].Rows[i].ItemArray[2].ToString());
                        break;
                }
                i++;
        }
    }


    // method to invoke a voting process based on the "Simple" voting style
    //      returns userID
    protected void classic(string position)
    {
        grabTableInfo(position);
        try
        {
            dbLogic.insertWinners(position, (int)ds.Tables[0].Rows[0].ItemArray[0]);
        }
        catch
        {
        }
    }

    // method to invoke a voting process based on the Plurality voting style
    //      returns a string array of userID's
    protected void plural(string position)
    {
        ArrayList topVotedUsers = new ArrayList();

        grabTableInfo(position);
        try
        {
            setParalellArrays();
            setTotalVotesForPosition();

            int numPositionSpots = (int)dsForElection.Tables[0].Rows[0].ItemArray[6]; //counter
            while (numPositionSpots > 0)
            {
                dbLogic.insertWinners(position, (int)userIDs[numPositionSpots]);
                numPositionSpots--;
            }
        }
        catch
        { }
    }

    // method to invoke a voting process based on the Majority voting style
    //      returns userID
    //
    // NOTE: need clarification from Karen on how Majority will work
    //
    protected void majority(string position)
    {
        grabTableInfo(position);
        try
        {
            setParalellArrays();
            setTotalVotesForPosition();

            if ((Convert.ToDouble(userVotes[0]) / Convert.ToDouble(totalForPosition)) <= .5)
            {
                int lowestVoteID = (userVotes.Count - 1);
                clearForMajority((int)userIDs[lowestVoteID], position);
                dbLogic.updateVotePhase();
            }
            else
            {
                dbLogic.insertWinners(position, (int)userIDs[0]);
            }
        }
        catch
        { }

    }
    
    /************* Helper methods **************/

    // this will be used for getting the total votes for the position
    protected void setTotalVotesForPosition()
    {
        for(int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            totalForPosition += (int)ds.Tables[0].Rows[i].ItemArray[2];
        }
    }

    protected void setParalellArrays()
    {
        for(int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            userIDs.Add(ds.Tables[0].Rows[i].ItemArray[0]);
            userVotes.Add(ds.Tables[0].Rows[i].ItemArray[2]);
        }
    }


    protected void clearForMajority(int id, string position)
    {
        string query = "DELETE FROM wts WHERE idunion_members=" + id + " AND position='" + position + "';";
        dbLogic.genericQueryDeleter(query);
        query = "DELETE FROM tally WHERE position='" + position + "';";
        dbLogic.genericQueryDeleter(query);
        query = "DELETE FROM election_position WHERE position<>'" + position + "';";
        dbLogic.genericQueryDeleter(query);
        query = "DELETE FROM flag_voted";
        dbLogic.genericQueryDeleter(query);
    }

    protected void grabTableInfo(string position)
    {
        query = "SELECT * FROM tally WHERE position='" + position + "' ORDER BY count DESC;";
        dbLogic.genericQuerySelector(query);
        ds = dbLogic.getResults();
    }

    protected DateTime createDateTime(string datetime)
    {
        string[] formats = {"M/d/yyyy H:mm:ss tt", "M/d/yyyy HH:mm tt", 
                           "MM/dd/yyyy HH:mm:ss", "M/d/yyyy H:mm:ss", 
                           "M/d/yyyy HH:mm tt", "M/d/yyyy HH tt", 
                           "M/d/yyyy H:mm", "M/d/yyyy H:mm", 
                           "MM/dd/yyyy HH:mm", "M/dd/yyyy HH:mm", "MM/d/yyyy HH:mm"};
        DateTime newDate = DateTime.ParseExact(datetime, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
        return newDate;
    }
}
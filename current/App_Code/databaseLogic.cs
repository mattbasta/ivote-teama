/// <summary>
/// 
/// Created by Adam Blank, 9/17/2011, databaseLogic.cs
/// Database for iVote system for CSC354
/// 
/// Last modified: 2/27/2012 by Andrew
/// 
/// </summary> 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Text.RegularExpressions;

using DatabaseEntities;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using NHibernate;
using NHibernate.Criterion;


public class databaseLogic
{

    private string hostname;
    private string user;
    private string database;
    private string port;
    private string password;
    private string connectionString;
    private MySqlConnection connection;
    private MySqlCommand cmd;
    private DataSet ds;
    private MySqlDataAdapter adapter;
    public string testing;

    public databaseLogic()
    {
        //Pull MySQL connection info from web.config
        hostname = System.Configuration.ConfigurationManager.AppSettings["mysqlHost"];
        database = System.Configuration.ConfigurationManager.AppSettings["mysqlDB"];
        user = System.Configuration.ConfigurationManager.AppSettings["mysqlUser"];
        password = System.Configuration.ConfigurationManager.AppSettings["mysqlPassword"];

        port = "3306";

        connectionString = "server=" + hostname + ";user=" + user + ";database=" + database + ";port=" + port + ";password=" + password + ";Allow zero Datetime=true";
        cmd = new MySqlCommand();
    }

    //replace invalid characters with empty strings
    public string CleanInput(string strIn)
    {
        // TODO: Test this method.
        //return MySqlHelper.EscapeString(strIn);
        return Regex.Replace(strIn, @"[^\w\.@-]", @" ");
    }

    //open the connection to the database
    private void openConnection()
    {
        connection = new MySqlConnection(connectionString);
        // If we fail to open the connection, let that failure trickle down to
        // a more specific error handler.
        connection.Open();
    }

    //^^^^^^^^^^generic database methods^^^^^^^^^^

    //generic selector method
    public void genericQuerySelector(string query)
    {
        openConnection();
        adapter = new MySqlDataAdapter(query, connection);
        ds = new DataSet();
        adapter.Fill(ds, "query");
        closeConnection();
    }

    //generic counter method
    public int genericQueryCounter(string query)
    {
        try
        {
            openConnection();
            int count = -1;
            adapter = new MySqlDataAdapter(query, connection);
            ds = new DataSet();
            adapter.Fill(ds, "query");
            count = ds.Tables[0].Rows.Count;
            return count;
        }
        catch
        { return -1; }
        finally
        {
            closeConnection();
        }
    }

    //generic updater method
    public void genericQueryUpdater(string query)
    {
        openConnection();
        cmd = new MySqlCommand(query, connection);
        cmd.CommandType = CommandType.Text;
        cmd.ExecuteNonQuery();
        closeConnection();
    }

    //generic delete method
    public void genericQueryDeleter(string query)
    {
        openConnection();
        cmd = new MySqlCommand(query, connection);
        cmd.CommandType = CommandType.Text;
        cmd.ExecuteNonQuery();
        closeConnection();
    }

    //generic insert method
    public void genericQueryInserter(string query)
    {
        openConnection();
        cmd.Connection = connection;
        cmd.CommandText = query;
        cmd.ExecuteNonQuery();
        closeConnection();
    }

    // close the connection to the database
    private void closeConnection()
    {
        connection.Close();
    }

    //return the results retrieved by a previously called function
    public DataSet getResults()
    {
        return ds;
    }

    //^^^^^^^^^^user methods^^^^^^^^^^


    [System.Obsolete("Use NHibernate-backed DB instead.")]
    public string selectFullName(string id)
    {
        try
        {
            openConnection();
            adapter = new MySqlDataAdapter("Select CONCAT (first_name, ' ', last_name) AS fullname FROM Where idunion_members = '" + id + "';", connection);
            ds = new DataSet();
            adapter.Fill(ds, "query");
            return ds.Tables[0].Rows[0].ItemArray[0].ToString();
        }
        catch
        { return ""; }
        finally
        {
            closeConnection();
        }
    }

    //select a user in the user table based on the union_members key
    [System.Obsolete("Use NHibernate-backed DB instead.")]
    public void selectUserInfoFromUnionId(String id)
    {
        string query = "SELECT * FROM union_members WHERE idunion_members = '" + id + "';";
        genericQuerySelector(query);
    }

    //select a users email in the union_members table based on the unionID 
    [System.Obsolete("Use NHibernate-backed DB instead.")]
    public string selectEmailFromID(int id)
    {
        openConnection();
        try
        {
            adapter = new MySqlDataAdapter("SELECT email FROM union_members WHERE idunion_members='" + id + "' ;", connection);
            ds = new DataSet();
            adapter.Fill(ds, "blah");
            string pictureURL = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            closeConnection();
            return pictureURL;
        }
        catch
        {
            closeConnection();
        }
        return "";
    }

    [System.Obsolete("Use NHibernate-backed DB instead.")]
    public string[] getEmails()
    {
        openConnection();
        try
        {
            adapter = new MySqlDataAdapter("select email from union_members;", connection);
            ds = new DataSet();
            adapter.Fill(ds, "email");
            closeConnection();
            return parseTable();
        }
        catch
        {
            closeConnection();
        }

        return null;
    }

    //retrieve only the ADMIN role emails
    public string[] getAdminEmails(ISession session)
    {
        IList<DatabaseEntities.User> nec_users = session.CreateCriteria(typeof(DatabaseEntities.User))
                                                .Add(Restrictions.Eq("IsAdmin", true))
                                                .List<DatabaseEntities.User>();
        string[] emails = new string[nec_users.Count];
        for (int i = 0; i < nec_users.Count; i++)
            emails[i] = nec_users[i].Email;
        return emails;
    }

    //retrieve only the NEC role emails
    public string[] getNECEmails(ISession session)
    {
        IList<DatabaseEntities.User> nec_users = session.CreateCriteria(typeof(DatabaseEntities.User))
                                                        .Add(Restrictions.Eq("IsNEC", true))
                                                        .List<DatabaseEntities.User>();
        string[] emails = new string[nec_users.Count];
        for(int i = 0; i < nec_users.Count; i++)
            emails[i] = nec_users[i].Email;
        return emails;
    }


    //retrieve only the emails for a NULL accept/reject
    public string[] getNullEmails()
    {
        openConnection();
        try
        {
            string query = "SELECT UM.Email FROM users UM, nomination_accept NA WHERE NA.accepted is NULL AND UM.ID=NA.idunion_to";
            adapter = new MySqlDataAdapter(query, connection);
            ds = new DataSet();
            adapter.Fill(ds, "email");
            closeConnection();
            return parseTable();
        }
        catch
        {
            closeConnection();
        }

        return null;
    }


    //select a users unionID in the user table based on the username 
    [System.Obsolete("Use NHibernate-backed DB instead.")]
    public int selectIDFromEmail(ISession session, String email)
    {
        DatabaseEntities.User user =
                session.CreateCriteria(typeof(DatabaseEntities.User))
                       .Add(Restrictions.Eq("Email", email))
                       .UniqueResult<DatabaseEntities.User>();
        if(user == null)
            return -1;
        return user.ID;
    }

    [System.Obsolete("Use NHibernate-backed DB instead.")]
    public DataSet getFirstAndLast(String query)
    {
        openConnection();
        try
        {
            adapter = new MySqlDataAdapter(query, connection);
            ds = new DataSet();
            adapter.Fill(ds, "query");
            closeConnection();
            return ds;
        }
        catch { return null; }
        finally
        {
            closeConnection();
        }
    }

    //^^^^^^^^^^email_verification methods^^^^^^^^^^

    //update based on primary key

    //insert verification codes
    public void insertCodes(int ID, String code1, String code2)
    {
        genericQueryInserter("INSERT INTO email_verification (iduser, code_verified, code_rejected, datetime_sent) VALUES (" + ID + ", '" + code1 + "', '" + code2 + "', NOW());");
    }

    public void insertCodes(string[] code)
    {
        genericQueryInserter("INSERT INTO email_verification (iduser, code_verified, code_rejected, datetime_sent) VALUES (" + code[0] + ", '" + code[1] + "', '" + code[2] + "', NOW());");
    }

    //deletes a code based on the code
    public void deleteCode(String code1)
    {
        genericQueryInserter("DELETE FROM email_verification WHERE code_verified = '" + code1 + "';");
    }

    //select a verified code

    //check if verified code is in table and returnes user id if it is. (***CHANGED AGAIN idemail_verification to iduser***)
    public string checkConfirmCode(string code)
    {
        try
        {
            openConnection();
            adapter = new MySqlDataAdapter("SELECT iduser FROM email_verification WHERE code_verified='" + code + "' ORDER BY iduser DESC;", connection);
            ds = new DataSet();
            adapter.Fill(ds, "email_verification");
            return ds.Tables[0].Rows[0].ItemArray[0].ToString();
        }
        catch { return ""; }
        finally
        {
            closeConnection();
        }
    }

    //^^^^^^^^^^petition methods^^^^^^^^^^

    //select all
    public void selectAllPetitions(String id)
    {
        genericQuerySelector("SELECT * FROM petition;");
    }

    //insert
    public void insertPetition(string[] petition)
    {
        genericQueryInserter("INSERT INTO petition (idunion_members, positions, idum_signedby) VALUES (" + petition[0] + ", '" + petition[1] + "', " + petition[2] + ");");
    }

    //check if user (who is about to insert a petition) has already inserted a petition for that person + position combo
    public bool isUserEnteringPetitionTwice(string[] petition)
    {
        string query = "SELECT * FROM petition WHERE idunion_members = " + petition[0] + " AND positions = '" + petition[1] + "' AND idum_signedby = " + petition[2] + ";";
        return genericQueryCounter(query) != 0; //if there are no rows in the datadata set created, result is false
    }

    //count how many petition there are for one person + position
    public int countPetitionsForPerson(string[] petition)
    {
        return genericQueryCounter("SELECT * FROM petition WHERE idunion_members = " + petition[0] + " AND positions = '" + petition[1] + "';");
    }

    //Select the idposition from positions using the position title
    public string selectIDFromPosition(string position)
    {
        openConnection();
        try
        {
            string query = "SELECT idelection_position FROM election_position WHERE position = '" + position + "';";
            adapter = new MySqlDataAdapter(query, connection);
            ds = new DataSet();
            adapter.Fill(ds, "blah");
            string pictureURL = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            closeConnection();
            return pictureURL;
        }
        catch
        {
            closeConnection();
        }
        return "";
    }


    //^^^^^^^^^^positions methods^^^^^^^^^^

    //Select all positions
    public void selectAllPositions()
    {
        genericQuerySelector("SELECT * FROM positions");
    }

    //Select all available positions
    public void selectAllAvailablePositions()
    {
        genericQuerySelector("SELECT * FROM election_position");
    }

    //Select the position title from positions using the idposition
    public string selectPositionFromID(string id)
    {
        try
        {
            openConnection();
            adapter = new MySqlDataAdapter("SELECT position FROM election_position WHERE idelection_position = '" + id + "';", connection);
            ds = new DataSet();
            adapter.Fill(ds, "blah");
            return ds.Tables[0].Rows[0].ItemArray[0].ToString();
        }
        catch { return ""; }
        finally
        {
            closeConnection();
        }

    }

    //^^^^^^^^^^tally methods^^^^^^^^^^

    //used to initialize a new tally row
    public void insertNewTally(string[] votingInfo)
    {
        genericQueryInserter("INSERT INTO tally (id_union, position, count) VALUES (" + votingInfo[0] + ", '" + votingInfo[1] + "', 0)");
    }

    //updates the count for a specified tally row
    public void updateTally(string[] votingInfo)
    {
        genericQueryUpdater("UPDATE tally SET count = count + 1 WHERE id_union = " + votingInfo[0] + " AND position = '" + votingInfo[1] + "';");
    }

    public void selectTallyInfoForPosition(string position)
    {
        // TODO: Update this to the new DB stuff.
        genericQuerySelector("SELECT T.*, CONCAT(UM.FirstName,' ', UM.LastName) AS fullname FROM tally T, users UM WHERE T.position = '" + position + "' AND UM.ID = T.id_union;");
    }

    //^^^^^^^^^^flag_voted methods^^^^^^^^^^
    public void insertFlagVoted(int id, string code)
    {
        genericQueryInserter("INSERT INTO flag_voted (idunion_members, code_confirm) VALUES (" + id.ToString() + ", '" + code + "')");
    }

    public bool isUserNewVoter(int id)
    {
        return genericQueryCounter("SELECT idunion_members FROM flag_voted WHERE idunion_members=" + id.ToString() + ";") == 0;
    }

    //^^^^^^^^^^wts methods^^^^^^^^^^

    //selects all info for nomination approval, including the users first and last name returned as a data row "full name"
    public void selectInfoForApprovalTable()
    {
        genericQuerySelector("SELECT * FROM wts;");
    }

    //update the the eligiblity
    public void updateEligible(string id, string approval)
    {
        genericQuerySelector("UPDATE wts SET eligible = " + approval + " WHERE wts_id = '" + id + "';");
    }


    //insert into the wts table
    public void insertIntoWTS(string id, string statement, string position)
    {
        genericQueryInserter("INSERT INTO wts (idunion_members, position, date_applied, statement) VALUES ('" + id + "', '" + position + "', NOW() ,'" + statement + "');");
    }

    public void selectDetailFromWTS(string id)
    {
        genericQuerySelector("SELECT * FROM wts WHERE idunion_members = " + id + ";");
    }


    public bool isUserWTS(int id, string position)
    {
        try
        {
            openConnection();
            adapter = new MySqlDataAdapter("SELECT * FROM wts WHERE idunion_members=" + id.ToString() + " AND position='" + position + "';", connection);
            ds = new DataSet();
            adapter.Fill(ds, "email_verification");
            return ds.Tables[0].Rows[0].ItemArray[0].ToString() != "";
        }
        catch { return false; }
        finally
        {
            closeConnection();
        }
    }

    //^^^^^^^^^^timeline^^^^^^^^^^
    public void selectPhaseNames()
    {
        genericQuerySelector("SELECT name_phase FROM timeline;");
    }

    public void updateTimeline(string date, string time, string phase)
    {
        genericQueryInserter("UPDATE timeline SET datetime_end = STR_TO_DATE('" + date + " " + time + "','%m/%d/%Y %H:%i') WHERE name_phase = '" + phase + "';");
    }

    public void updateVotePhase()
    {
        genericQueryInserter("UPDATE timeline SET datetime_end = DATE_ADD(datetime_end,INTERVAL 7 DAY) WHERE name_phase = 'vote';");
    }

    public void turnOnPhase(string phase)
    {
        genericQueryUpdater("UPDATE timeline SET iscurrent = 0 WHERE 1;");
        genericQueryUpdater("UPDATE timeline SET iscurrent = 1, datetime_end = NOW() WHERE name_phase = '" + phase + "';");
    }

    public string currentPhase()
    {
        try
        {
            openConnection();
            adapter = new MySqlDataAdapter("SELECT name_phase FROM timeline WHERE iscurrent = 1;", connection);
            ds = new DataSet();
            adapter.Fill(ds, "blah");
            return ds.Tables[0].Rows[0].ItemArray[0].ToString();
        }
        catch { return ""; }
        finally
        {
            closeConnection();
        }
    }

    public DateTime currentPhaseEndDateTime()
    {
        try
        {
            openConnection();
            adapter = new MySqlDataAdapter("SELECT datetime_end FROM timeline WHERE iscurrent = 1;", connection);
            ds = new DataSet();
            adapter.Fill(ds, "blah");
            return DateTime.Parse(ds.Tables[0].Rows[0].ItemArray[0].ToString());
        }
        catch { return DateTime.Now; }
        finally
        {
            closeConnection();
        }
    }

    //^^^^^^^^^^nomination_accept^^^^^^^^^^

    //inserts nomination into db
    public void insertNominationAccept(string[] accept)
    {
        genericQueryInserter("INSERT INTO nomination_accept (idunion_to, idunion_from, position) VALUES ('" + accept[0] + "','" + accept[1] + "','" + accept[2] + "');");
    }

    public void insertNominationAcceptFromPetition(string[] accept)
    {
        genericQueryInserter("INSERT INTO nomination_accept (idunion_to, idunion_from, position, from_petition) VALUES ('" + accept[0] + "','" + accept[1] + "','" + accept[2] + "', " + accept[3] + ");");
    }

    //******NOTE***** The user will have multiple positions that this can be true for, might want to modify ******NOTE******
    public bool isUserNominatedPending(int id)
    {
        try
        {
            openConnection();
            adapter = new MySqlDataAdapter("SELECT * FROM nomination_accept WHERE idunion_to=" + id.ToString() + " AND accepted IS NULL;", connection);
            ds = new DataSet();
            adapter.Fill(ds, "email_verification");
            return ds.Tables[0].Rows[0].ItemArray[0].ToString() != "";
        }
        catch
        { return false; }
        finally
        {
            closeConnection();
        }

    }

    public bool isUserNominatedFromPetitionPending(int id)
    {
        try
        {
            openConnection();
            adapter = new MySqlDataAdapter("SELECT * FROM nomination_accept WHERE idunion_to=" + id.ToString() + " AND from_petition = 1 AND accepted IS NULL;", connection);
            ds = new DataSet();
            adapter.Fill(ds, "email_verification");
            return ds.Tables[0].Rows[0].ItemArray[0].ToString() != "";
        }
        catch
        { return false; }
        finally
        {
            closeConnection();
        }

    }

    //checks if the user already has an entry for the position
    public bool isUserNominated(int id, string position)
    {
        try
        {
            openConnection();
            adapter = new MySqlDataAdapter("SELECT * FROM nomination_accept WHERE idunion_to=" + id.ToString() + " AND position='" + position + "';", connection);
            ds = new DataSet();
            adapter.Fill(ds, "email_verification");
            closeConnection();
            return ds.Tables[0].Rows[0].ItemArray[0].ToString() != "";
        }
        catch
        { return false; }
        finally
        {
            closeConnection();
        }

    }

    public string selectDescriptionFromPositionName(string posName)
    {
        try
        {
            openConnection();
            adapter = new MySqlDataAdapter("SELECT description FROM election_position WHERE position='" + posName + "';", connection);
            ds = new DataSet();
            adapter.Fill(ds, "email_verification");
            return ds.Tables[0].Rows[0].ItemArray[0].ToString();
        }
        catch
        { return ""; }
        finally
        {
            closeConnection();
        }

    }

    //user has accepted nomination
    public void userAcceptedNom(string id, string position)
    {
        genericQueryUpdater("UPDATE nomination_accept SET accepted='1' WHERE idunion_to='" + id + "' AND position='" + position + "';");
    }

    //user has rejected nomination
    public void userRejectedNom(string id, string position)
    {
        genericQueryUpdater("UPDATE nomination_accept SET accepted='0' WHERE idunion_to='" + id + "' AND position='" + position + "';");
    }


    //get all nominations that pertain to a user
    public void selectAllUserNoms(string id)
    {
        genericQuerySelector("SELECT * FROM nomination_accept WHERE idunion_to = " + id + " AND accepted IS NULL;");
    }

    //^^^^^^^^^^^^^^election methods^^^^^^^^^^^^^^^^^^^
    public void insertElection()
    {
        genericQueryInserter("INSERT INTO election (name) VALUES ('Fall 2011');");
        genericQueryInserter("INSERT INTO flag_NEC values(1,1,0,0)");
    }

    public string returnLatestElectionId()
    {
        try
        {
            adapter = new MySqlDataAdapter("SELECT idelection FROM election ORDER BY idelection DESC;", connection);
            ds = new DataSet();
            adapter.Fill(ds, "email_verification");
            return ds.Tables[0].Rows[0].ItemArray[0].ToString();
        }
        catch
        { return ""; }
        finally
        {
            closeConnection();
        }
    }

    //^^^^^^^^^^^ballot methods^^^^^^^^^^^^^^^

    //get all the info to populate the ballot
    public void selectAllForBallot(string position)
    {
        genericQuerySelector("SELECT * FROM wts WHERE eligible=1 AND position='" + CleanInput(position) + "';");
    }

    //counts how many people are nominated for a position
    public int countHowManyCandidatesForPosition(string position)
    {
        return genericQueryCounter("SELECT * FROM wts WHERE eligible=1 AND position='" + position + "';");
    }

    public bool IsThereCandidatesForPoisition(string position)
    {
        // TODO: Update this
        return genericQueryCounter("SELECT WTS.idunion_members,  CONCAT(UM.FirstName,' ', UM.LastName) AS fullname " +
                                   "FROM wts WTS, users UM " +
                                   "WHERE (WTS.eligible=1 AND wts.idunion_members = UM.ID AND WTS.position='" + position + "');") > 0;
    }

    //gets all the current election positions
    public void selectAllBallotPositions()
    {
        genericQuerySelector("SELECT * FROM election_position;");
    }


    //^^^^^^^^^^^^^^adding positions to an election methods^^^^^^^^^^^^^^^^

    //adds the positions to positions table
    public void addPos(ArrayList positions, ArrayList vote, ArrayList description, ArrayList num, ArrayList votes)
    {
        genericQueryDeleter("TRUNCATE TABLE election_position;");
        for (int i = 0; i < positions.Count; i++)
            genericQueryInserter("INSERT INTO election_position (position, tally_type, description, slots_plurality, votes_allowed) VALUES ('" + positions[i].ToString() + "', '" + vote[i].ToString() + "', '" + description[i].ToString() + "', " + num[i].ToString() + ", " + votes[i].ToString() + ");");
    }


    //^^^^^^^^^^^^^^methods for the results of an election^^^^^^^^^^^^^^^^^^^

    //gets position and winner from results table
    public void getPosAndWinner()
    {
        // TODO: Update this
        genericQuerySelector("SELECT position, id_union FROM results;");
    }

    public void insertWinners(string position, int id)
    {
        genericQueryInserter("INSERT INTO results (position, id_union) VALUES ('" + position + "', " + id + ");");
    }

    public bool checkNecApprove()
    {
        genericQuerySelector("SELECT approve FROM flag_nec where approve = 1;");
        DataSet ds = getResults();
        return ds.Tables[0].Rows.Count > 0;
    }

    public void insertNecApprove()
    {
        genericQueryUpdater("update flag_nec set approve = 1;");
    }

    public bool checkSlateApprove()
    {
        genericQuerySelector("select * from flag_nec where slate = 1");
        return getResults().Tables[0].Rows.Count > 0;
    }

    public void approveSlate()
    {
        genericQueryUpdater("update flag_nec set slate = 1;");
    }

    //^^^^^^^^^^role and login provider methods (DO NOT MODIFY)^^^^^^^^^^

    //EXTRA STUFF NEEDED FOR OTHER QUERIES
    private string[] parseTable()
    {
        // parse a datatable containing 1 column of data (i.e., 1 column selected for multiple records)

        string[] retVal = new String[ds.Tables[0].Rows.Count];
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            retVal[i] = (string)ds.Tables[0].Rows[i].ItemArray[0];
        return retVal;
    }

    //check to see if there are any pending eligibility forms to be completed
    public int returnEligibilityCount()
    {
        return genericQueryCounter("select * from wts where eligible is NULL;");
    }

    //create timeline
    public void createTimeline()
    {
        int id_num;
        openConnection();
        
        resetElection();
        
        genericQueryInserter("INSERT INTO election (name) VALUES ('Officer Election');" +
                             "INSERT INTO flag_NEC values(1,1,0,0)");
        
        adapter = new MySqlDataAdapter("select * from timeline;", connection);
        ds = new DataSet();
        adapter.Fill(ds, "blah");
        
        if (Convert.ToInt32((ds.Tables[0].Rows.Count)) == 0)
            id_num = 1;
        else
        {
            adapter = new MySqlDataAdapter("select idtimeline from timeline order by idtimeline desc limit 1;", connection);
            ds = new DataSet();
            adapter.Fill(ds, "blah");
            id_num = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[0]) + 1;
        }
        //inserts//
        string[] iter_phases = {"nullphase", "nominate", "accept1", "slate", "petition", "accept2", "approval", "vote", "result"};

        foreach(string phase in iter_phases)
            genericQueryInserter("INSERT INTO timeline (idelection, name_phase, datetime_end, iscurrent) VALUES (1, '" + phase + "', NOW(), " + (phase == "nominate" ? '1' : '0') + ");");
    }

    /********************************
     * Reset Election
     * Truncates the following tables:
     * nomination_accept
     * timeline
     * election
     * petition
     * election_position
     * results
     * wts
     * flag_nec
     * flag_voted
     * tally
     * *****************************/
    public void resetElection()
    {
        //open connection
        openConnection();

        //set tables to truncate
        String[] tables = {"nomination_accept", "timeline", "election", "petition", "election_position",
            "results", "wts", "flag_nec", "flag_voted", "tally"};

        //truncate nomination_accept
        for (int i = 0; i < tables.Length; i++)
        {
            cmd.CommandText = "truncate table " + tables[i];
            cmd.Connection = connection;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
        }

        //close connection
        closeConnection();
    }

    /********************************************
     * phase_ClearNullNominations
     * Clears all NULL nominations that were not
     * accepted in time for the old phase.
     * ******************************************/
    public void phase_ClearNullNominations()
    {
        //open connection
        openConnection();

        //execute query
        cmd.CommandText = "delete from nomination_accept where accepted is NULL;";
        cmd.Connection = connection;
        cmd.CommandType = CommandType.Text;
        cmd.ExecuteNonQuery();

        //close connection
        closeConnection();
    }

    /*********************************************
     * canSkipPhase
     * Returns true if accept phase can be skipped.
     * ******************************************/
    public bool canSkipPhase()
    {
        string cp = currentPhase();
        if(cp == "approval")
            return canSkipAdminPhase();
        else if(cp != "accept1")
            return false;
        
        openConnection();
        adapter = new MySqlDataAdapter("select * from nomination_accept where accepted is NULL;", connection);
        ds = new DataSet();
        adapter.Fill(ds, "blah");
        //query to see if any nominations are unaccepted
        return Convert.ToInt32((ds.Tables[0].Rows.Count)) > 0;
    }

    /*********************************************
     * canSkipAdminPhase
     * Returns true if approval phase can be skipped.
     * ******************************************/
    public bool canSkipAdminPhase()
    {
        openConnection();
        adapter = new MySqlDataAdapter("select * from wts where eligible is NULL;", connection);
        ds = new DataSet();
        adapter.Fill(ds, "blah");
        //query to see if any nominations are unaccepted
        return Convert.ToInt32((ds.Tables[0].Rows.Count)) > 0;
    }

    public void createSchema()
    {
        String rawShema = File.ReadAllText(HttpContext.Current.Server.MapPath("~/App_Data/sql/officer-schema.sql"));

        openConnection();
        cmd.Connection = connection;
        cmd.CommandText = rawShema;
        cmd.ExecuteNonQuery();
        closeConnection();
    }
    
}




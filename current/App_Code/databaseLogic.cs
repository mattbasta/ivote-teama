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
    private string commandString;
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
        
        try
        {
            connection.Open();
        }
        catch
        {
        }
    }

    //^^^^^^^^^^generic database methods^^^^^^^^^^

    //generic selector method
    public void genericQuerySelector(string query)
    {
        openConnection();
        try
        {
            commandString = query;
            adapter = new MySqlDataAdapter(commandString, connection);
            ds = new DataSet();
            adapter.Fill(ds, "query");
            closeConnection();
        }
        catch
        {
            closeConnection();
        }
    }

    //generic counter method
    public int genericQueryCounter(string query)
    {
        int count = -1;
        openConnection();
        try
        {
            commandString = query;
            adapter = new MySqlDataAdapter(commandString, connection);
            cmd.CommandType = CommandType.Text;
            ds = new DataSet();
            adapter.Fill(ds, "query");
            count = ds.Tables[0].Rows.Count;
            //count = Convert.ToInt32(cmd.ExecuteScalar());
            closeConnection();
            return count;
        }
        catch
        {
            closeConnection();
        }
        //this would be an error..
        return count;
    }

    //generic updater method
    public void genericQueryUpdater(string query)
    {
        openConnection();
        try
        {
            commandString = query;
            cmd = new MySqlCommand(commandString, connection);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            closeConnection();
        }
        catch
        {
            closeConnection();
        }
    }

    //generic delete method
    public void genericQueryDeleter(string query)
    {
        openConnection();
        commandString = query;
        try
        {
            cmd = new MySqlCommand(commandString, connection);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            closeConnection();
        }
        catch
        {
            closeConnection();
        }
    }

    //generic insert method
    public void genericQueryInserter(string query)
    {
        openConnection();
        try
        {
            commandString = query;
            cmd.Connection = connection;
            cmd.CommandText = commandString;
            cmd.ExecuteNonQuery();
            closeConnection();
        }
        catch
        {
            closeConnection();
        }
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


    //update a password
    public void updatePassword(string id, string password)
    {
        string query = "UPDATE user SET password='" + password + "' WHERE idunion_members=" + id + ";";
        genericQueryUpdater(query);
    }

    public void updatePassword2(string id, string password)
    {
        string query = "UPDATE user SET password='" + password + "' WHERE username='" + id + "';";
        genericQueryUpdater(query);
    }


    //insert a new user
    //    First value is 0 for auto-increment
    public void insertUser(int ID, string user)
    {
        string query = "INSERT INTO user (idunion_members, username) VALUES (" + ID + ", '" + user + "');";
        genericQueryInserter(query);
    }
    //insert a union member
    //    First value is 0 for auto-increment
    public void insertUnion(string[] union)
    {
        string query = "INSERT INTO union_members (last_name, first_name, email, department) VALUES ('" + union[0] + "', '" + union[1] + "', '" + union[2] + "', '" + union[3] + "');";
        genericQueryInserter(query);
    }

    //returns the users unionID based on the username
    public string returnUnionIDFromUsername(string username)
    {
        openConnection();
        try
        {
            commandString = "SELECT idunion_members FROM union_members WHERE email = '" + username + "';";
            adapter = new MySqlDataAdapter(commandString, connection);
            ds = new DataSet();
            adapter.Fill(ds, "id");
            string id = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            closeConnection();
            return id;
        }
        catch
        {
            closeConnection();
        }
        return "";
    }

    //returns the users username based on their union_members id
    public string returnUsernameFromUnionID(string unionID)
    {
        openConnection();
        try
        {
            commandString = "SELECT username FROM user WHERE idunion_members = '" + unionID + "';";
            adapter = new MySqlDataAdapter(commandString, connection);
            ds = new DataSet();
            adapter.Fill(ds, "username");
            string username = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            closeConnection();
            return username;
        }
        catch
        {
            closeConnection();
        }
        return "";
    }


    //(CODE FIX 10:48am, 10/4, Included idunion_members after ORDER BY to return correct value)
    public int returnLastUnionAdded()
    {
        openConnection();
        try
        {
            commandString = "SELECT idunion_members FROM union_members ORDER BY idunion_members DESC;";
            adapter = new MySqlDataAdapter(commandString, connection);
            ds = new DataSet();
            adapter.Fill(ds, "blah");
            int pictureURL = (int)ds.Tables[0].Rows[0].ItemArray[0];
            closeConnection();
            return pictureURL;
        }
        catch
        {
            closeConnection();
        }
        return -1;
    }

    public int returnLastUserAdded()
    {
        openConnection();
        try
        {
            commandString = "SELECT iduser FROM user ORDER BY DESC;";
            adapter = new MySqlDataAdapter(commandString, connection);
            ds = new DataSet();
            adapter.Fill(ds, "blah");
            int pictureURL = (int)ds.Tables[0].Rows[0].ItemArray[0];
            closeConnection();
            return pictureURL;
        }
        catch
        {
            closeConnection();
        }
        return -1;
    }

    public DataSet returnAllUsers()
    {
        openConnection();
        try
        {
            commandString = "SELECT * FROM union_members ORDER BY DESC;";
            adapter = new MySqlDataAdapter(commandString, connection);
            ds = new DataSet();
            adapter.Fill(ds);
            closeConnection();
            return ds;
        }
        catch
        {
            closeConnection();
        }
        return null;
    }

    public string selectFullName(string id)
    {
        openConnection();
        try
        {
            commandString = "Select CONCAT (first_name, ' ', last_name) AS fullname FROM union_members Where idunion_members = '" + id + "';";
            adapter = new MySqlDataAdapter(commandString, connection);
            ds = new DataSet();
            adapter.Fill(ds, "query");
            string name = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            closeConnection();
            return name;
        }
        catch
        {
            closeConnection();
        }
        return "";
    }

    //delete a user
    public void deleteUser(string iduser)
    {
        string query = "DELETE FROM user WHERE iduser = '" + iduser + "';";
        genericQueryDeleter(query);
    }

    //completely deletes a user from database
    public void deleteAccountCompletely(string idunion)
    {
        string query = "DELETE FROM user WHERE idunion_members = '" + idunion + "';";
        genericQueryDeleter(query);

        query = "DELETE FROM union_members WHERE idunion_members = '" + idunion + "';";
        genericQueryDeleter(query);
    }

    public void updateUser(string ID, string[] UserInfo)
    {
        string query = "update union_members set last_name='" + UserInfo[0] + "', first_name='" + UserInfo[1] + "', email='" + UserInfo[2] + "', phone='" + UserInfo[3] + "', department='" + UserInfo[4] + "' WHERE idunion_members = " + ID + ";";
        genericQueryUpdater(query);

    }

    //^^^^^^^^^^union_memebers methods^^^^^^^^^^

    //select all user info from union_members
    public void selectAllUserInfo()
    {
        string query = "SELECT * FROM union_members ORDER BY last_name ASC;";
        genericQuerySelector(query);
    }

    //select a user in the user table based on the union_members key 
    public void selectUserInfoFromUnionId(String id)
    {
        string query = "SELECT * FROM union_members WHERE idunion_members = '" + id + "';";
        genericQuerySelector(query);
    }


    //select a users email in the union_members table based on the unionID 
    public string selectEmailFromID(int id)
    {
        openConnection();
        try
        {
            commandString = "SELECT email FROM union_members WHERE idunion_members='" + id + "' ;";
            adapter = new MySqlDataAdapter(commandString, connection);
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

    public string[] getEmails()
    {
        openConnection();
        try
        {
            commandString = "select email from union_members;";
            adapter = new MySqlDataAdapter(commandString, connection);
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
    public string[] getAdminEmails()
    {
        openConnection();
        try
        {
            commandString = "SELECT UM.email FROM union_members UM, roles_users RU, user U WHERE RU.role='admin' AND  RU.username=U.username  AND U.idunion_members=UM.idunion_members";
            adapter = new MySqlDataAdapter(commandString, connection);
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

    //retrieve only the NEC role emails
    public string[] getNECEmails()
    {
        openConnection();
        try
        {
            commandString = "SELECT UM.email FROM union_members UM, roles_users RU, user U WHERE RU.role='admin' AND  RU.username=U.username  AND U.idunion_members=UM.idunion_members";
            adapter = new MySqlDataAdapter(commandString, connection);
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


    //retrieve only the emails for a NULL accept/reject
    public string[] getNullEmails()
    {
        openConnection();
        try
        {
            commandString = "SELECT UM.email FROM union_members UM, nomination_accept NA WHERE NA.accepted is NULL AND UM.idunion_members=NA.idunion_to";
            adapter = new MySqlDataAdapter(commandString, connection);
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
    public int selectIDFromEmail(String email)
    {
        openConnection();
        try
        {
            commandString = "SELECT idunion_members FROM union_members WHERE email='" + email + "' ;";
            adapter = new MySqlDataAdapter(commandString, connection);
            ds = new DataSet();
            adapter.Fill(ds, "blah");
            int pictureURL = (int)ds.Tables[0].Rows[0].ItemArray[0];
            closeConnection();
            return pictureURL;
        }
        catch
        {
            closeConnection();
        }
        return -1;
    }

    public void SelectPeopleFromSearch(string criteria)
    {
        string query = "SELECT * FROM union_members WHERE MATCH (first_name, last_name, department) AGAINST ('" + criteria + "');";
        genericQuerySelector(query);
    }


    public DataSet getFirstAndLast(String query)
    {
        openConnection();
        try
        {
            commandString = query;
            adapter = new MySqlDataAdapter(commandString, connection);
            ds = new DataSet();
            adapter.Fill(ds, "query");
            closeConnection();
            return ds;
        }
        catch
        {
            closeConnection();
        }
        return null;
    }

    //^^^^^^^^^^email_verification methods^^^^^^^^^^

    ////select all email addresses from union_members
    //public void selectAllEmail()
    //{
    //    string query = "SELECT email FROM union_members;";
    //    genericQuerySelector(query);
    //}

    ////deletes all email addresses from union_members
    //public void deleteAllEmail()
    //{
    //    string query = "DELETE FROM union_members WHERE email LIKE '%@live.kutztown.edu'";
    //    genericQueryDeleter(query);
    //}

    ////insert an email address based on primary key
    //public void insertEmailByKey(int ID, String newEmail)
    //{
    //    string query = "INSERT INTO union_members.email WHERE ID = idunion_members";

    //   // INSERT INTO union_members
    //   // VALUES ('newEmail')
    //   // WHERE idunion_members = ID;
    //}

    //update based on primary key

    //insert verification codes
    public void insertCodes(int ID, String code1, String code2)
    {
        string query = "INSERT INTO email_verification (iduser, code_verified, code_rejected, datetime_sent) VALUES (" + ID + ", '" + code1 + "', '" + code2 + "', NOW());";
        genericQueryInserter(query);
    }

    public void insertCodes(string[] code)
    {
        string query = "INSERT INTO email_verification (iduser, code_verified, code_rejected, datetime_sent) VALUES (" + code[0] + ", '" + code[1] + "', '" + code[2] + "', NOW());";
        genericQueryInserter(query);
    }

    //deletes a code based on the code
    public void deleteCode(String code1)
    {
        string query = "DELETE FROM email_verification WHERE code_verified = '" + code1 + "';";
        genericQueryInserter(query);
    }

    //select a verified code

    //check if verified code is in table and returnes user id if it is. (***CHANGED AGAIN idemail_verification to iduser***)
    public string checkConfirmCode(string code)
    {
        openConnection();
        try
        {
            commandString = "SELECT iduser FROM email_verification WHERE code_verified='" + code + "' ORDER BY iduser DESC;";
            adapter = new MySqlDataAdapter(commandString, connection);
            ds = new DataSet();
            adapter.Fill(ds, "email_verification");
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

/*    //checks if email exists in the database already (when adding a new user)
    //returns true if email is found in db
    public bool checkIfEmailExists(string newEmail)
    {
        openConnection();
        try
        {
            commandString = "SELECT idunion_members FROM union_members WHERE email='" + newEmail + "';";
            adapter = new MySqlDataAdapter(commandString, connection);
            ds = new DataSet();
            adapter.Fill(ds, "email_verification");
            string email = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            closeConnection();
            if (email != "")
                return true;
            else
                return false;
        }
        catch
        {
            closeConnection();
            return false;
        }

    }
*/

    //^^^^^^^^^^petition methods^^^^^^^^^^

    //select all
    public void selectAllPetitions(String id)
    {
        string query = "SELECT * FROM petition;";
        genericQuerySelector(query);
    }

    //insert
    public void insertPetition(string[] petition)
    {
        string query = "INSERT INTO petition (idunion_members, positions, idum_signedby) VALUES (" + petition[0] + ", '" + petition[1] + "', " + petition[2] + ");";
        genericQueryInserter(query);
    }

    //check if user (who is about to insert a petition) has already inserted a petition for that person + position combo
    public bool isUserEnteringPetitionTwice(string[] petition)
    {
        string query = "SELECT * FROM petition WHERE idunion_members = " + petition[0] + " AND positions = '" + petition[1] + "' AND idum_signedby = " + petition[2] + ";";

        if (genericQueryCounter(query) == 0) //if there are no rows in the datadata set created, result is false
            return false;
        else
            return true;
    }

    //count how many petition there are for one person + position
    public int countPetitionsForPerson(string[] petition)
    {
        string query = "SELECT * FROM petition WHERE idunion_members = " + petition[0] + " AND positions = '" + petition[1] + "';";
        return genericQueryCounter(query);
    }


    //^^^^^^^^^^positions methods^^^^^^^^^^

    //Select all positions
    public void selectAllPositions()
    {
        string query = "SELECT * FROM positions";
        genericQuerySelector(query);
    }

    //Select all available positions
    public void selectAllAvailablePositions()
    {
        string query = "SELECT * FROM election_position";
        genericQuerySelector(query);
    }



    //Select the idposition from positions using the position title
    public string selectIDFromPosition(string position)
    {
        openConnection();
        try
        {
            commandString = "SELECT idelection_position FROM election_position WHERE position = '" + position + "';";
            adapter = new MySqlDataAdapter(commandString, connection);
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


    //Select the position title from positions using the idposition
    public string selectPositionFromID(string id)
    {

        openConnection();
        try
        {
            commandString = "SELECT position FROM election_position WHERE idelection_position = '" + id + "';";
            adapter = new MySqlDataAdapter(commandString, connection);
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

    //^^^^^^^^^^tally methods^^^^^^^^^^

    //used to initialize a new tally row
    public void insertNewTally(string[] votingInfo)
    {
        string query = "INSERT INTO tally (id_union, position, count) VALUES (" + votingInfo[0] + ", '" + votingInfo[1] + "', 0)";
        genericQueryInserter(query);
    }

    //updates the count for a specified tally row
    public void updateTally(string[] votingInfo)
    {
        string query = "UPDATE tally SET count = count + 1 WHERE id_union = " + votingInfo[0] + " AND position = '" + votingInfo[1] + "';";
        genericQueryUpdater(query);
    }

    public void selectTallyInfoForPosition(string position)
    {
        string query = "SELECT T.*, CONCAT(UM.first_name,' ', UM.last_name) AS fullname FROM tally T, union_members UM WHERE T.position = '" + position + "' AND UM.idunion_members = T.id_union;;";
        genericQuerySelector(query);
    }

    //^^^^^^^^^^flag_voted methods^^^^^^^^^^
    public void insertFlagVoted(string id, string code)
    {
        string query = "INSERT INTO flag_voted (idunion_members, code_confirm) VALUES (" + id + ", '" + code + "')";
        genericQueryInserter(query);
    }

    public bool isUserNewVoter(string id)
    {
        string query = "SELECT idunion_members FROM flag_voted WHERE idunion_members=" + id + ";";

        if (genericQueryCounter(query) == 0)
            return true;
        else
            return false;
    }

    //^^^^^^^^^^wts methods^^^^^^^^^^

    //selects all info for nomination approval, including the users first and last name returned as a data row "full name"
    public void selectInfoForApprovalTable()
    {
        string query = "SELECT WTS.*,  CONCAT(UM.first_name,' ', UM.last_name) AS fullname FROM wts WTS, union_members UM WHERE UM.idunion_members = WTS.idunion_members;";
        genericQuerySelector(query);
    }

    //update the the eligiblity
    public void updateEligible(string id, string approval)
    {
        string query = "UPDATE wts SET eligible = " + approval + " WHERE wts_id = '" + id + "';";
        genericQuerySelector(query);
    }


    //insert into the wts table
    public void insertIntoWTS(string id, string statement, string position)
    {
        string query = "INSERT INTO wts (idunion_members, position, date_applied, statement) VALUES ('" + id + "', '" + position + "', NOW() ,'" + statement + "');";
        genericQueryInserter(query);
    }

    public void selectDetailFromWTS(string id)
    {
        string query = "SELECT WTS.*,  CONCAT(UM.first_name,' ', UM.last_name) AS fullname FROM wts WTS, union_members UM WHERE UM.idunion_members = WTS.idunion_members AND WTS.idunion_members = " + id + ";";
        genericQuerySelector(query);
    }


    public bool isUserWTS(string id, string position)
    {
        openConnection();
        try
        {
            commandString = "SELECT * FROM wts WHERE idunion_members=" + id + " AND position='" + position + "';";
            adapter = new MySqlDataAdapter(commandString, connection);
            ds = new DataSet();
            adapter.Fill(ds, "email_verification");
            string email = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            closeConnection();
            if (email != "")
                return true;
            else
                return false;
        }
        catch
        {
            closeConnection();
            return false;
        }
    }

    //^^^^^^^^^^timeline^^^^^^^^^^
    public void selectPhaseNames()
    {
        string query = "SELECT name_phase FROM timeline;";
        genericQuerySelector(query);
    }

    public void selectPhaseDates()
    {
        string query = "SELECT datetime_end FROM timeline;";
        genericQuerySelector(query);
    }

    public void selectPhaseNamesAndDates()
    {
        string query = "SELECT name_phase, datetime_end FROM timeline;";
        genericQuerySelector(query);
    }

    public void selectPhaseDate(string phase)
    {
        string query = "SELECT datetime_end FROM timeline WHERE name_phase = '" + phase + "';";
        genericQuerySelector(query);
    }

    public void insertTimeline()
    {
        string query = "";
        genericQueryInserter(query);
    }

    public void updateTimeline(string date, string time, string phase)
    {
        string query = "UPDATE timeline SET datetime_end = STR_TO_DATE('" + date + " " + time + "','%m/%d/%Y %H:%i') WHERE name_phase = '" + phase + "';";
        genericQueryInserter(query);
    }

    public void updateVotePhase()
    {
        string query = "UPDATE timeline SET datetime_end =  DATE_ADD(datetime_end,INTERVAL 7 DAY) WHERE name_phase = 'vote';";
        genericQueryInserter(query);
    }

    public void turnOnPhase(string phase)
    {
        string query = "UPDATE timeline set iscurrent= 1 WHERE name_phase = '" + phase + "';";
        genericQueryUpdater(query);
    }

    public void turnOffPhase(string phase)
    {
        string query = "UPDATE timeline set iscurrent= 0 WHERE name_phase = '" + phase + "';";
        genericQueryUpdater(query);
    }

    public string currentPhase()
    {
        openConnection();
        try
        {
            commandString = "SELECT name_phase FROM timeline WHERE iscurrent = 1;";
            adapter = new MySqlDataAdapter(commandString, connection);
            ds = new DataSet();
            adapter.Fill(ds, "blah");
            string phase = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            closeConnection();
            return phase;
        }
        catch
        {
            closeConnection();
        }
        return "";
    }

    //^^^^^^^^^^nomination_accept^^^^^^^^^^

    //inserts nomination into db
    public void insertNominationAccept(string[] accept)
    {
        string query = "INSERT INTO nomination_accept (idunion_to, idunion_from, position) VALUES ('" + accept[0] + "','" + accept[1] + "','" + accept[2] + "');";
        genericQueryInserter(query);
    }

    public void insertNominationAcceptFromPetition(string[] accept)
    {
        string query = "INSERT INTO nomination_accept (idunion_to, idunion_from, position, from_petition) VALUES ('" + accept[0] + "','" + accept[1] + "','" + accept[2] + "', " + accept[3] + ");";
        genericQueryInserter(query);
    }

    //******NOTE***** The user will have multiple positions that this can be true for, might want to modify ******NOTE******
    public bool isUserNominatedPending(string id)
    {
        openConnection();
        try
        {
            commandString = "SELECT * FROM nomination_accept WHERE idunion_to=" + id + " AND accepted IS NULL;";
            adapter = new MySqlDataAdapter(commandString, connection);
            ds = new DataSet();
            adapter.Fill(ds, "email_verification");
            string email = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            closeConnection();
            if (email != "")
                return true;
            else
                return false;
        }
        catch
        {
            closeConnection();
            return false;
        }

    }

    public bool isUserNominatedFromPetitionPending(string id)
    {
        openConnection();
        try
        {
            commandString = "SELECT * FROM nomination_accept WHERE idunion_to=" + id + " AND from_petition = 1 AND accepted IS NULL;";
            adapter = new MySqlDataAdapter(commandString, connection);
            ds = new DataSet();
            adapter.Fill(ds, "email_verification");
            string email = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            closeConnection();
            if (email != "")
                return true;
            else
                return false;
        }
        catch
        {
            closeConnection();
            return false;
        }

    }

    //checks if the user already has an entry for the position
    public bool isUserNominated(string id, string position)
    {
        openConnection();
        try
        {
            commandString = "SELECT * FROM nomination_accept WHERE idunion_to=" + id + " AND position='" + position + "';";
            adapter = new MySqlDataAdapter(commandString, connection);
            ds = new DataSet();
            adapter.Fill(ds, "email_verification");
            string email = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            closeConnection();
            if (email != "")
                return true;
            else
                return false;
        }
        catch
        {
            closeConnection();
            return false;
        }

    }

    public string selectDescriptionFromPositionName(string posName)
    {
        openConnection();
        try
        {
            commandString = "SELECT description FROM election_position WHERE position='" + posName + "';";
            adapter = new MySqlDataAdapter(commandString, connection);
            ds = new DataSet();
            adapter.Fill(ds, "email_verification");
            closeConnection();
            return ds.Tables[0].Rows[0].ItemArray[0].ToString();

        }
        catch
        {
            closeConnection();
            return "";
        }

    }

    //user has accepted nomination
    public void userAcceptedNom(string id, string position)
    {
        string query = "UPDATE nomination_accept SET accepted='1' WHERE idunion_to='" + id + "' AND position='" + position + "';";
        genericQueryUpdater(query);
    }

    //user has rejected nomination
    public void userRejectedNom(string id, string position)
    {
        string query = "UPDATE nomination_accept SET accepted='0' WHERE idunion_to='" + id + "' AND position='" + position + "';";
        genericQueryUpdater(query);
    }


    //get all nominations that pertain to a user
    public void selectAllUserNoms(string id)
    {
        string query = "SELECT * FROM nomination_accept WHERE idunion_to = " + id + " AND accepted IS NULL;";
        genericQuerySelector(query);
    }

    //^^^^^^^^^^^^^^election methods^^^^^^^^^^^^^^^^^^^
    public void insertElection()
    {
        string query = "INSERT INTO election (name) VALUES ('Fall 2011');";
        genericQueryInserter(query);
        query = "INSERT INTO flag_NEC values(1,1,0,0)";
        genericQueryInserter(query);
    }

    public string returnLatestElectionId()
    {
        try
        {
            commandString = "SELECT idelection FROM election ORDER BY idelection DESC;";
            adapter = new MySqlDataAdapter(commandString, connection);
            ds = new DataSet();
            adapter.Fill(ds, "email_verification");
            closeConnection();
            return ds.Tables[0].Rows[0].ItemArray[0].ToString();
        }
        catch
        {
            closeConnection();
            return "";
        }
    }

    //^^^^^^^^^^^ballot methods^^^^^^^^^^^^^^^

    //get all the info to populate the ballot
    public void selectAllForBallot(string position)
    {
        string query = "SELECT WTS.idunion_members,  CONCAT(UM.first_name,' ', UM.last_name) AS fullname " +
                       "FROM wts WTS, union_members UM " +
                       "WHERE (WTS.eligible=1 AND wts.idunion_members = UM.idunion_members AND WTS.position='" + position + "');";
        genericQuerySelector(query);
    }

    //counts how many people are nominated for a position
    public int countHowManyCandidatesForPosition(string position)
    {
        string query = "SELECT WTS.idunion_members,  CONCAT(UM.first_name,' ', UM.last_name) AS fullname " +
                       "FROM wts WTS, union_members UM " +
                       "WHERE (WTS.eligible=1 AND wts.idunion_members = UM.idunion_members AND WTS.position='" + position + "');";
        return genericQueryCounter(query);
    }

    public bool IsThereCandidatesForPoisition(string position)
    {
        string query = "SELECT WTS.idunion_members,  CONCAT(UM.first_name,' ', UM.last_name) AS fullname " +
                       "FROM wts WTS, union_members UM " +
                       "WHERE (WTS.eligible=1 AND wts.idunion_members = UM.idunion_members AND WTS.position='" + position + "');";
        int check = genericQueryCounter(query);
        if (check > 0)
            return true;
        else
            return false;
    }

    //gets all the current election positions
    public void selectAllBallotPositions()
    {
        string query = "SELECT * FROM election_position;";
        genericQuerySelector(query);
    }


    //^^^^^^^^^^^^^^adding positions to an election methods^^^^^^^^^^^^^^^^

    //adds the positions to positions table
    public void addPos(ArrayList positions, ArrayList vote, ArrayList description, ArrayList num, ArrayList votes)
    {
        string query = "TRUNCATE TABLE election_position;";
        genericQueryDeleter(query);
        int i;
        for (i = 0; i < positions.Count; i++)
        {
            query = "INSERT INTO election_position (position, tally_type, description, slots_plurality, votes_allowed) VALUES ('" + positions[i].ToString() + "', '" + vote[i].ToString() + "', '" + description[i].ToString() + "', " + num[i].ToString() + ", " + votes[i].ToString() + ");";
            genericQueryInserter(query);
        }
    }


    //^^^^^^^^^^^^^^methods for the results of an election^^^^^^^^^^^^^^^^^^^

    //gets position and winner from results table
    public void getPosAndWinner()
    {
        string query = "SELECT R.position, R.id_union, CONCAT(UM.first_name,' ', UM.last_name) AS fullname FROM results R, union_members UM  WHERE UM.idunion_members = R.id_union;";
        genericQuerySelector(query);
    }

    public void insertWinners(string position, int id)
    {
        string query = "INSERT INTO results (position, id_union) VALUES ('" + position + "', " + id + ");";
        genericQueryInserter(query);
    }

    public bool checkNecApprove()
    {
        string query = "SELECT approve FROM flag_nec where approve = 1;";
        genericQuerySelector(query);
        DataSet ds = getResults();
        if (ds.Tables[0].Rows.Count > 0)
            return true;
        else
            return false;
    }

    public void insertNecApprove()
    {
        string query = "update flag_nec set approve = 1;";
        genericQueryUpdater(query);
    }

    public bool checkSlateApprove()
    {
        string query = "select * from flag_nec where slate = 1";
        genericQuerySelector(query);
        DataSet ds = getResults();
        if (ds.Tables[0].Rows.Count > 0)
            return true;
        else
            return false;
    }

    public void approveSlate()
    {
        string query = "update flag_nec set slate = 1;";
        genericQueryUpdater(query);
    }

    //^^^^^^^^^^role and login provider methods (DO NOT MODIFY)^^^^^^^^^^

    //retrieve roles for a given user. implemented because required by overload for role provider
    public string[] getUsersInRole(string role)
    {

        openConnection();
        try
        {
            commandString = "SELECT Username from roles_users WHERE role = '" + role + "';";
            adapter = new MySqlDataAdapter(commandString, connection);
            ds = new DataSet();
            adapter.Fill(ds, "usersinroles");
            closeConnection();
            return parseTable();
        }
        catch
        {
            closeConnection();
        }
        return null;
    }

    //find users belonging to a given role. implemented because required by overload for role provider
    public string[] findUsersInRole(string role, string user)
    {
        openConnection();
        try
        {
            commandString = "Select username from roles_users where username = " + user + " and role = " + role + ";";
            adapter = new MySqlDataAdapter(commandString, connection);
            ds = new DataSet();
            adapter.Fill(ds, "usersinroles");
            closeConnection();
            return parseTable();
        }
        catch
        {
            closeConnection();
        }
        return null;
    }

    //get the roles attached to the input user. implemented because required by overload for role provider
    public string[] getUserRoles(string user)
    {

        openConnection();
        try
        {
            commandString = "SELECT role FROM roles_users WHERE username = '" + user + "';";
            adapter = new MySqlDataAdapter(commandString, connection);
            ds = new DataSet();
            adapter.Fill(ds, "usersinroles");
            closeConnection();
            return parseTable();
        }
        catch
        {
            closeConnection();
        }

        return null;
    }

    //take a set of roles away from a set of users. implemented because required by overload for role provider
    public void removeUsersFromRoles(string[] users, string[] roles)
    {
        try
        {
            for (int i = 0; i < users.Length; i++)
            {
                for (int j = 0; j < roles.Length; j++)
                {
                    openConnection();
                    commandString = "DELETE FROM roles_users WHERE username = '" + users[i] + "' and role = '" + roles[j] + "';";
                    cmd.CommandText = commandString;
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    closeConnection();
                }
            }
        }
        catch
        {
            closeConnection();
            //testing = "remove users from roles: " + e.Message;
        }
    }

    public void addRole(string role)
    {
        //only exists because required by overload for role provider, obvisouly doesn't do anything
    }

    // get all roles from the membership provider tables. implemented because required by overload for role provider
    public string[] getRoles()
    {
        openConnection();
        try
        {
            commandString = "SELECT roles from roles;";
            adapter = new MySqlDataAdapter(commandString, connection);
            ds = new DataSet();
            adapter.Fill(ds, "roles");
            closeConnection();
            return parseTable();
        }
        catch
        {
            closeConnection();
        }

        return null;
    }

    //check if a given role exists. implemented because required by overload for role provider
    public bool roleExists(string role)
    {

        openConnection();
        try
        {
            commandString = "Select roles from roles where role = " + role + ";";
            adapter = new MySqlDataAdapter(commandString, connection);
            ds = new DataSet();
            adapter.Fill(ds, "roles");
            closeConnection();
            return (ds.Tables.Count > 0);
        }
        catch
        {
            closeConnection();
        }
        return false;
    }

    //get users for verifying login information
    public void getUsersL()
    {
        openConnection();
        try
        {
            commandString = "SELECT username, password FROM user;";
            adapter = new MySqlDataAdapter(commandString, connection);
            ds = new DataSet();
            adapter.Fill(ds, "users");
            closeConnection();
        }
        catch
        {
            closeConnection();
        }
    }

    // batch encryption update
    // KEEP AS IS
    public void updateUserPasswords(string[] users, string[] passwords)
    {

        openConnection();
        try
        {
            for (int i = 0; i < users.Length; i++)
            {
                commandString = "update user set password='" + passwords[i] + "' where username = '" + users[i] + "';";
                cmd = new MySqlCommand(commandString, connection);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

            }
            closeConnection();
        }
        catch
        {
            closeConnection();
        }
    }

    //retrieve password for login
    public DataTable getPassword(string username)
    {
        openConnection();
        try
        {
            commandString = "SELECT password from user WHERE username=@whoAmI;";
            adapter = new MySqlDataAdapter(commandString, connection);
            adapter.SelectCommand.Parameters.AddWithValue("@whoAmI", username);
            ds = new DataSet();
            adapter.Fill(ds, "user");
            closeConnection();
            return ds.Tables.Count > 0 ? ds.Tables[0] : null;
        }
        catch
        {
            closeConnection();
            return null;
        }
    }

    //add a set of roles to a set of users
    public void addUsersToRoles(string[] users, string[] roles)
    {


        try
        {
            for (int i = 0; i < users.Length; i++)
            {
                for (int j = 0; j < roles.Length; j++)
                {
                    if (!(isUserInRole(users[i], roles[j])))
                    {
                        openConnection();
                        commandString = "INSERT INTO roles_users (username, role) VALUES ('" + users[i] + "', '" + roles[j] + "');";
                        cmd.CommandText = commandString;
                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        closeConnection();
                    }
                }
            }
        }
        catch(Exception e)
        {
            closeConnection();
            testing = "add users to roles_users: " + e.Message;
        }
    }

    public bool isUserInRole(string username, string role)
    {
        openConnection();
        commandString = "Select idroles_users FROM roles_users WHERE username = '" + username + "' and role = '" + role + "';";
        adapter = new MySqlDataAdapter(commandString, connection);
        ds = new DataSet();
        adapter.Fill(ds, "usersinroles_users");
        closeConnection();
        return (ds.Tables[0].Rows.Count > 0);
    }


    //EXTRA STUFF NEEDED FOR OTHER QUERIES
    private string[] parseTable()
    {
        // parse a datatable containing 1 column of data (i.e., 1 column selected for multiple records)

        string[] retVal = new String[ds.Tables[0].Rows.Count];
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            retVal[i] = (string)ds.Tables[0].Rows[i].ItemArray[0];
        }
        return retVal;
    }
    /* Update user fields
    public void UpdateUserFields(int ID, string[] UserInfo)
    {
                string query = "update users set email='" + UserInfo[0] + "',last_name='" + UserInfo[1] + "',first_name='" + UserInfo[2] + "',phone='" + UserInfo[3] + "',department='" + UserInfo[4] + "' where id = '" + ID + "';";
                genericQueryUpdater(string query);

    }*/


    //check to see if there are any pending eligibility forms to be completed
    public int returnEligibilityCount()
    {
        commandString = "select * from wts where eligible is NULL;";
        int count = genericQueryCounter(commandString);
        return count;
    }

    //create timeline
    public void createTimeline(String[] Date, String[] Time, string ElectionNum)
    {
        int id_num;
        openConnection();
        commandString = "select * from timeline;";
        adapter = new MySqlDataAdapter(commandString, connection);
        ds = new DataSet();
        adapter.Fill(ds, "blah");
        int count = Convert.ToInt32((ds.Tables[0].Rows.Count));
        if (count == 0)
            id_num = 1;
        else
        {
            commandString = "select idtimeline from timeline order by idtimeline desc limit 1;";
            adapter = new MySqlDataAdapter(commandString, connection);
            ds = new DataSet();
            adapter.Fill(ds, "blah");
            id_num = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[0]) + 1;
        }
        //inserts//
        //null phase
        commandString = "Insert into timeline values(" + id_num.ToString() + "," + ElectionNum + "," + "'nullphase'," + "STR_TO_DATE('" + Date[0] + " " + Time[0] + "','%m/%d/%Y %H:%i'), '0');";
        cmd.CommandText = commandString;
        cmd.Connection = connection;
        cmd.CommandType = CommandType.Text;
        cmd.ExecuteNonQuery();
        id_num++;

        //nominate phase
        commandString = "Insert into timeline values(" + id_num.ToString() + "," + ElectionNum + "," + "'nominate'," + "STR_TO_DATE('" + Date[1] + " " + Time[1] + "','%m/%d/%Y %H:%i'), '0');";
        cmd.CommandText = commandString;
        cmd.Connection = connection;
        cmd.CommandType = CommandType.Text;
        cmd.ExecuteNonQuery();
        id_num++;

        //first acceptance phase
        commandString = "Insert into timeline values(" + id_num.ToString() + "," + ElectionNum + "," + "'accept1'," + "STR_TO_DATE('" + Date[2] + " " + Time[2] + "','%m/%d/%Y %H:%i'), '0');";
        cmd.CommandText = commandString;
        cmd.Connection = connection;
        cmd.CommandType = CommandType.Text;
        cmd.ExecuteNonQuery();
        id_num++;

        //slate
        //this phase is ended when slate is approved
        commandString = "Insert into timeline values(" + id_num.ToString() + "," + ElectionNum + "," + "'slate'," + "STR_TO_DATE('01/01/2100 00:00','%m/%d/%Y %H:%i'), '0');";
        cmd.CommandText = commandString;
        cmd.Connection = connection;
        cmd.CommandType = CommandType.Text;
        cmd.ExecuteNonQuery();
        id_num++;

        //petition phase
        commandString = "Insert into timeline values(" + id_num.ToString() + "," + ElectionNum + "," + "'petition'," + "STR_TO_DATE('" + Date[3] + " " + Time[3] + "','%m/%d/%Y %H:%i'), '0');";
        cmd.CommandText = commandString;
        cmd.Connection = connection;
        cmd.CommandType = CommandType.Text;
        cmd.ExecuteNonQuery();
        id_num++;

        //second acceptance phase
        commandString = "Insert into timeline values(" + id_num.ToString() + "," + ElectionNum + "," + "'accept2'," + "STR_TO_DATE('" + Date[4] + " " + Time[4] + "','%m/%d/%Y %H:%i'), '0');";
        cmd.CommandText = commandString;
        cmd.Connection = connection;
        cmd.CommandType = CommandType.Text;
        cmd.ExecuteNonQuery();
        id_num++;

        //approval
        //this phase is ended when all eligibility is checked.
        commandString = "Insert into timeline values(" + id_num.ToString() + "," + ElectionNum + "," + "'approval'," + "STR_TO_DATE('01/01/2100 00:00','%m/%d/%Y %H:%i'), '0');";
        cmd.CommandText = commandString;
        cmd.Connection = connection;
        cmd.CommandType = CommandType.Text;
        cmd.ExecuteNonQuery();
        id_num++;

        //vote
        commandString = "Insert into timeline values(" + id_num.ToString() + "," + ElectionNum + "," + "'vote'," + "STR_TO_DATE('" + Date[5] + " " + Time[5] + "','%m/%d/%Y %H:%i'), '0');";
        cmd.CommandText = commandString;
        cmd.Connection = connection;
        cmd.CommandType = CommandType.Text;
        cmd.ExecuteNonQuery();
        id_num++;

        //results
        //this phase is ended by a button push, so throw in a fake end date 1/1/2100 at 00:00
        commandString = "Insert into timeline values(" + id_num.ToString() + "," + ElectionNum + "," + "'result'," + "STR_TO_DATE('01/01/2100 00:00','%m/%d/%Y %H:%i'), '0');";
        cmd.CommandText = commandString;
        cmd.Connection = connection;
        cmd.CommandType = CommandType.Text;
        cmd.ExecuteNonQuery();

        //end inserts

        //string IDQuery = "select idtimeline from timeline order by idtimeline desc limit 1;";
        //string query = "UPDATE timeline SET datetime_end = STR_TO_DATE('" + date + " " + time + "','%m/%d/%Y %H:%i') WHERE name_phase = '" + phase + "';";
        //string query = "INSERT INTO timeline VALUES(
    }

    //End the results phase  
    //sets the end date of the result phase to 01/01/1900 00:00
    public void admin_EndResultsPhase() //DEPRECIATED
    {
        //depreciated function
        //now used as alias
        killPhase("result");
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
            commandString = "truncate table " + tables[i];
            cmd.CommandText = commandString;
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

        //query string
        commandString = "delete from nomination_accept where accepted is NULL;";

        //execute query
        cmd.CommandText = commandString;
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
        openConnection();
        commandString = "select * from nomination_accept where accepted is NULL;";
        adapter = new MySqlDataAdapter(commandString, connection);
        ds = new DataSet();
        adapter.Fill(ds, "blah");
        int count = Convert.ToInt32((ds.Tables[0].Rows.Count));
        //query to see if any nominations are unaccepted

        if (count > 0)
            return false;
        else
            return true;
    }

    /*********************************************
     * canSkipAdminPhase
     * Returns true if approval phase can be skipped.
     * ******************************************/
    public bool canSkipAdminPhase()
    {
        openConnection();
        commandString = "select * from wts where eligible is NULL;";
        adapter = new MySqlDataAdapter(commandString, connection);
        ds = new DataSet();
        adapter.Fill(ds, "blah");
        int count = Convert.ToInt32((ds.Tables[0].Rows.Count));
        //query to see if any nominations are unaccepted

        if (count > 0)
            return false;
        else
            return true;
    }

    /********************************************
     * killPhase
     * Kills a phase by name
     * *****************************************/
    public void killPhase(string phaseName)
    {
        //open connection
        openConnection();

        //send command
        //note:  For concurrent elections, this will need to update for the correct election.  This
        //current updates any elections in the table.
        commandString = "update timeline set datetime_end = '1900-01-01 00:00:00' where name_phase = '" + phaseName + "'";
        cmd.CommandText = commandString;
        cmd.Connection = connection;
        cmd.CommandType = CommandType.Text;
        cmd.ExecuteNonQuery();

        //close connection
        closeConnection();
    }

    /***********************************************
     * getEndDate
     * Returns a phase's end-date
     * ********************************************/
    public void getEndDate(string phase)
    {
        string query = "select datetime_end from timeline where name_phase = '" + phase + "';";
        genericQuerySelector(query);

        /*//open connection
        openConnection();

        //select data
        commandString = "select datetime_end from timeline where name_phase = '" + phase + "';";
        adapter = new MySqlDataAdapter(commandString, connection);
        ds = new DataSet();
        adapter.Fill(ds, "blah");
        string enddate = ds.Tables[0].Rows[0].ItemArray[0].ToString();

        //close connection
        closeConnection();

        //return the date
        return enddate;*/
    }
}  //END CLASS




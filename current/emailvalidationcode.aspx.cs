using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class emailvalidationcode : System.Web.UI.Page
{
    databaseLogic dbLogic = new databaseLogic(); //initializes new db logic instance

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        String confirmUrl = "confirm.aspx"; //current landing page for test cofirmation
        String code_confirm = getRanString(); //the confirmation code needed by user to activate the account
        String code_reject = getRanString(); //the code needed if person sent email is not person who sent verification code

        insertCodesIntoDb(code_confirm, code_reject); //sends codes to inserter for db

        //displays codes for user
        Label1.Text = "Copy and paste into address bar:<br /><b>http://" + System.Configuration.ConfigurationManager.AppSettings["baseUrl"] + "/" + confirmUrl + "?x=" + code_confirm + "</b>";
        Button1.Enabled = false; //disables button
    }

    //returns random regular expression 64 length string
    protected String getRanString()
    {
        String uncleanRandomString = System.Web.Security.Membership.GeneratePassword(62, 0);
        uncleanRandomString = uncleanRandomString.Replace("!", "a");
        uncleanRandomString = uncleanRandomString.Replace("@", "2");
        uncleanRandomString = uncleanRandomString.Replace("#", "c");
        uncleanRandomString = uncleanRandomString.Replace("$", "4");
        uncleanRandomString = uncleanRandomString.Replace("%", "3");
        uncleanRandomString = uncleanRandomString.Replace("^", "i");
        uncleanRandomString = uncleanRandomString.Replace("&", "a");
        uncleanRandomString = uncleanRandomString.Replace("*", "9");
        uncleanRandomString = uncleanRandomString.Replace("(", "g");
        uncleanRandomString = uncleanRandomString.Replace(")", "m");
        uncleanRandomString = uncleanRandomString.Replace("_", "d");
        uncleanRandomString = uncleanRandomString.Replace("-", "5");
        uncleanRandomString = uncleanRandomString.Replace("+", "p");
        uncleanRandomString = uncleanRandomString.Replace("=", "q");
        uncleanRandomString = uncleanRandomString.Replace("[", "w");
        uncleanRandomString = uncleanRandomString.Replace("{", "t");
        uncleanRandomString = uncleanRandomString.Replace("]", "r");
        uncleanRandomString = uncleanRandomString.Replace("}", "f");
        uncleanRandomString = uncleanRandomString.Replace(";", "8");
        uncleanRandomString = uncleanRandomString.Replace(":", "z");
        uncleanRandomString = uncleanRandomString.Replace("<", "x");
        uncleanRandomString = uncleanRandomString.Replace(">", "0");
        uncleanRandomString = uncleanRandomString.Replace("|", "v");
        uncleanRandomString = uncleanRandomString.Replace(".", "b");
        uncleanRandomString = uncleanRandomString.Replace("/", "y");
        uncleanRandomString = uncleanRandomString.Replace("?", "t");
        return uncleanRandomString;
    }

    //inserts data into the database
    protected void insertCodesIntoDb(String code1, String code2)
    {
        String[] code = new String[3];

        code[0] = "1"; //supposed to be persons new username, will be added later
        code[1] = code1;
        code[2] = code2;

        //inserts values into db
        dbLogic.insertCodes(code);
    }
}

// Membershp Provider coded with guide from MSDN: http://msdn.microsoft.com/en-us/library/6tc47t75(VS.80).aspx
using System.Web.Security;
using System.Configuration.Provider;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Data;
using System.Configuration;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;



/// <summary>
/// Summary description for iVoteLoginProvider
/// </summary>
public class iVoteLoginProvider : MembershipProvider
{
    private databaseLogic dblogic = new databaseLogic();

    private int passwordLength = 8;
    private string eventSource = "iVoteProvider";
    private string eventLog = "Application";
    private string exceptionMessage = "Exception, please check Application Event Log";
    private string applicationName = "iVoteSystem";
    private string pregex = "*";
    private bool enablePasswordReset = true;
    private bool enablePasswordRetrieval = true;
    private bool questionAndAnser = true;
    private bool uniqueEmail = false;
    private int maxInvalidAttempts = 10;
    private int maxAttemptLockWindow = 5;
    private MembershipPasswordFormat format;

    private MachineKeySection machineKey;


    public iVoteLoginProvider()
    {
        Configuration config = WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
        machineKey = (MachineKeySection)config.GetSection("system.web/machineKey");
    }

    private string GetConfigValue(string configValue, string defaultValue)
    {
        if (String.IsNullOrEmpty(configValue))
        {
            return defaultValue;
        }
        return configValue;
    }

    public override bool EnablePasswordReset
    {
        get
        {
           return enablePasswordReset; 
        }
    }

    public override bool EnablePasswordRetrieval
    {
        get
        {
            return enablePasswordRetrieval;
        }
    }
    
    public override bool RequiresQuestionAndAnswer
    {
        get
        {
            return questionAndAnser;
        }
    }


    public void fixUser()
    {
        UnlockUser("");
    }

    public override bool RequiresUniqueEmail
    {
        get
        {
            return uniqueEmail;
        }
    }

    public override MembershipPasswordFormat PasswordFormat
    {
        // changed to encrypted to incorporate security
        get
        {
            //return MembershipPasswordFormat.Clear;
            return MembershipPasswordFormat.Encrypted;
        }
    }

    public override int MaxInvalidPasswordAttempts
    {
        get
        {
            //Allow 3 tries for user to get into iVote System
            return maxInvalidAttempts;
        }
    }

    public override int PasswordAttemptWindow
    {
        get
        {
            //locks user our for 5 minutes after 3 tries
            return maxAttemptLockWindow;
        }
    }

    public override string ApplicationName
    {
        get
        {
            return applicationName;
        }
        set
        {
            applicationName = value;
        }
    }

    public override string PasswordStrengthRegularExpression
    {
        get { return pregex; }
        
    }

    public void encryptAll()
    {
        
        dblogic.getUsersL();
        DataSet results = dblogic.getResults();
        string[] encryptMe = new string[results.Tables[0].Rows.Count];
        string[] users = new string[encryptMe.Length];
        for (int i = 0; i < results.Tables[0].Rows.Count; i++)
        {
            users[i] = (string)results.Tables[0].Rows[i].ItemArray[0];
            encryptMe[i] = encrypt((string)results.Tables[0].Rows[i].ItemArray[1]);
        }
        dblogic.updateUserPasswords(users, encryptMe);
    }

    public string encrypt(string what)
    {
        string answer = "test";

        answer = Convert.ToBase64String(EncryptPassword(Encoding.Unicode.GetBytes(what)));

        return answer;
    }

    private string decrypt(string what)
    {
        string answer = "";

        answer = Encoding.Unicode.GetString(DecryptPassword(Convert.FromBase64String(what)));

        return answer;
    }

    public override int MinRequiredNonAlphanumericCharacters
    {
        get { return 0; }
    }

    public override int MinRequiredPasswordLength
    {
        get { return 8  ; }
    }

    public override void UpdateUser(MembershipUser updatedInforamtion)
    {
    }

    public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
    {
        status  = MembershipCreateStatus.Success;
        return null;
    }

    public override bool DeleteUser(string username, bool deleteAllRelatedData)
    {
        return true;
    }



    public override bool ValidateUser(string username, string password)
    {
        
        dblogic.getUsersL();
        string temp;
        DataSet results = dblogic.getResults();
        DataTable users = results.Tables[0];
        for (int i = 0; i < users.Rows.Count; i++)
        {
            temp = encrypt(password);
            if (((string)users.Rows[i].ItemArray[0]).Equals(username) )
            {
                //make sure password is not null
                if (!users.Rows[i].ItemArray[1].Equals(DBNull.Value))
                {
                    if (((string)users.Rows[i].ItemArray[1]).Equals(temp))
                        return true;
                }
            }
        }
        return false;
    }


    public override MembershipUser GetUser(Object providerUserKey, bool userIsOnline)
    {
        return null;
    }

    public override MembershipUser GetUser(string username, bool userIsOnline)
    {
        return null;
    }

    public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
    {
        totalRecords = 0;
        return null;
    }

    public override int GetNumberOfUsersOnline()
    {
        return 0;
    }

    public override string ResetPassword(string userName, string answer)
    {
        return "";
    }

    public override string GetPassword(string userName, string passwordAnswer)
    {
        DataTable dt =dblogic.getPassword(userName);
        if (dt != null)
        {
            return decrypt((string)dt.Rows[0].ItemArray[0]);
        }
        else
        {
            return "";
        }
    }

    public override string GetUserNameByEmail(string emailAddress)
    {
        return "";
    }

    public override bool ChangePassword(string username, string oldPassword, string newPassword)
    {
        throw new NotImplementedException("membership provider change password not implemented");
    }

    public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
    {
        return true;
    }

    public override MembershipUserCollection FindUsersByName(string matchThisName, int pageIndex, int pageSize, out int totalRecords)
    {
        totalRecords = 0;
        return null;
    }

    public override MembershipUserCollection FindUsersByEmail(string emailtomatch, int pageIndex, int pageSize, out int totalRecords)
    {
        totalRecords = 0;
        return null;
    }

    public override bool UnlockUser(string userName)
    {
        return true;
    }
}
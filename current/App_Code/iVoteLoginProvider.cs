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
using DatabaseEntities;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using NHibernate;


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
        //machineKey = (MachineKeySection)config.GetSection("system.web/machineKey");
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



    public override bool ValidateUser(string email, string password)
    {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        DatabaseEntities.User testUser = DatabaseEntities.User.Authenticate(session, email, password);

        return (testUser != null);
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
        return "";
    }

    public override string GetUserNameByEmail(string emailAddress)
    {
        return "";
    }

    public override bool ChangePassword(string email, string oldPassword, string newPassword)
    {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        DatabaseEntities.User user = DatabaseEntities.User.Authenticate(session, email, oldPassword);

        if (user == null)
            return false;

        DatabaseEntities.User.UpdatePassword(session, user, newPassword, "");

        return true;
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

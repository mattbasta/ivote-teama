using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using NHibernate;
using NHibernate.Cfg;

public partial class experimental_WTS : System.Web.UI.Page
{

    databaseLogic dbLogic = new databaseLogic();
    DataSet ds = new DataSet();
    string query = "";
    string id;
    string[] info;
    DatabaseEntities.CommitteeElection electionObject;
    DatabaseEntities.User userObject;
    protected void Page_Load(object sender, EventArgs e)
    {

        string position = Request.QueryString["position"];
        int positionID;
        string committee = Request.QueryString["committee"];
        int committeeID;


        //Election ID is a number

        //Check if is legacy officer election
        if (position != null && int.TryParse(position, out positionID))
        {
            string queryVal = positionID.ToString(); //variable name of querystring
            string positionTitle = dbLogic.selectPositionFromID(queryVal);
            LabelHeader.Text = positionTitle + " Willingness To Serve Form:";
            LabelExplain.Text = "If you are willing to hold a place in office as " + positionTitle + ", please fill out the following form:";
            HiddenFieldPosition.Value = positionTitle;

            ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
            userObject = DatabaseEntities.User.FindUser(session, Page.User.Identity.Name.ToString());
            id = userObject.ID.ToString();

            ds = dbLogic.getResults();
            info = new string[3] { id, id, HiddenFieldPosition.Value };

            Name.Text = userObject.FirstName + " " + userObject.LastName;
            Dept.Text = userObject.Department.ToString();

            if (!String.IsNullOrEmpty(positionTitle))
                Submit.Enabled = true;
        }
        //Check if is committee election
        else if (committee != null && int.TryParse(committee, out committeeID))
        {
            //Open session
            ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
            ITransaction transaction = session.BeginTransaction();

            //Check if election exists
            electionObject = DatabaseEntities.CommitteeElection.FindElection(session, committeeID);

            if (electionObject != null && electionObject.Phase == DatabaseEntities.ElectionPhase.WTSPhase)
            {
                //Election is valid and in WTS phase.

                //Lookup committee
                DatabaseEntities.Committee committeeObject = DatabaseEntities.Committee.FindCommittee(session, electionObject.PertinentCommittee);


                //Get user
                userObject = DatabaseEntities.User.FindUser(session, Page.User.Identity.Name.ToString());
                Name.Text = userObject.FirstName + " " + userObject.LastName;
                Dept.Text = userObject.Department.ToString();

                //Check if WTS already exists
                List<DatabaseEntities.CommitteeWTS> wtsList = DatabaseEntities.CommitteeWTS.FindCommitteeWTS(session, electionObject.ID);
                bool wtsAlreadySubmitted = false;
                foreach (DatabaseEntities.CommitteeWTS wts in wtsList)
                {
                    if (wts.Election == electionObject.ID && wts.User == userObject.ID)
                        wtsAlreadySubmitted = true;
                }

                string positionTitle = committeeObject.Name;
                HiddenFieldPosition.Value = positionTitle;
                LabelHeader.Text = positionTitle + " Willingness To Serve Form:";

                //Display
                if (!wtsAlreadySubmitted)
                {
                    //No previous wts
                    LabelExplain.Text = "If you are willing to hold a position in the " + positionTitle + " Committee , please fill out the following form:";
                    Submit.Enabled = true;
                }
                else
                {
                    //Previous wts
                    LabelExplain.Text = "You have already submit a willingness to serve form for this election.";
                    Submit.Enabled = false;
                }

            }


            DatabaseEntities.NHibernateHelper.Finished(transaction);
        }




    }

    protected bool checkAccept()
    {
        if (Accept.Checked == true)
            return true;
        else
            return false;
    }

    protected void submit(object sender, EventArgs e)
    {

        if (checkAccept())
        {
            if (electionObject == null)
            {
                //Handle legacy WTS
                if (!dbLogic.isUserWTS(int.Parse(id), HiddenFieldPosition.Value))
                {
                    dbLogic.insertIntoWTS(id, Statement.Text, HiddenFieldPosition.Value);
                    if (!dbLogic.isUserNominated(int.Parse(id), HiddenFieldPosition.Value))
                    {
                        dbLogic.insertNominationAccept(info);
                    }
                    dbLogic.userAcceptedNom(id, HiddenFieldPosition.Value);
                    AcceptError.Visible = false;
                    UpdatePanel1.Visible = false;
                    LabelExplain.Visible = false;
                    Confirm.Visible = true;
                }
                else
                {
                    LabelExplain.Text = "You have already applied for this position.";
                    UpdatePanel1.Visible = false;
                }
            }
            else
            {
                //Handle committee WTS
                ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
                ITransaction transaction = session.BeginTransaction();

                //Create WTS
                DatabaseEntities.CommitteeElection.WillingToServe(session, userObject.ID, electionObject.ID, Statement.Text);

                DatabaseEntities.NHibernateHelper.Finished(transaction);

                AcceptError.Visible = false;
                UpdatePanel1.Visible = false;
                LabelExplain.Visible = false;
                Confirm.Visible = true;
            }

        }
        else
        {
            AcceptError.Visible = true;
        }
    }

    protected bool checkWTS()
    {
        return dbLogic.isUserWTS(int.Parse(id), HiddenFieldPosition.Value);
    }
}
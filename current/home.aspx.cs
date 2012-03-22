using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Web.UI.WebControls;
using System.Data;

using DatabaseEntities;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Cfg;

public partial class home : System.Web.UI.Page
{
    //OBJECT DECLARATION
    databaseLogic dbLogic = new databaseLogic();
    timeline phases = new timeline();
    VerifyEmail email = new VerifyEmail();
    voteCounter vc = new voteCounter();
    ArrayList positions = new ArrayList();
    
    bool is_admin;

    protected void Page_Load(object sender, EventArgs e)
    {
        is_admin = User.IsInRole("admin");
        if(is_admin)
            AdminTabs.Visible = true;
        
        // Set up the officer election bit.
        setView();
        
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        ITransaction transaction = session.BeginTransaction();
        
        IList<DatabaseEntities.CommitteeElection> committee_elections;
        if(!is_admin)
            committee_elections =
                    session.CreateCriteria(typeof(DatabaseEntities.CommitteeElection))
                            .Add(Restrictions.Not(Restrictions.Eq("Phase", DatabaseEntities.ElectionPhase.ClosedPhase)))
                            .List<DatabaseEntities.CommitteeElection>();
        else
            committee_elections =
                    session.CreateCriteria(typeof(DatabaseEntities.CommitteeElection))
                            .List<DatabaseEntities.CommitteeElection>();
        
        if(committee_elections.Count == 0)
            CommitteeElectionStatus.Visible = true;
        else
        {
            CommitteeElectionRepeater.DataSource = committee_elections;
            CommitteeElectionRepeater.DataBind();
        }
        
        if(is_admin)
        {
            int waiting_committees = DatabaseEntities.Committee.NumberOfWaitingCommittees(session);
            if(waiting_committees > 0)
            {
                WaitingCommittees.Visible = true;
                WaitingCommittees.Text = waiting_committees.ToString();
            }
        }
        
        DatabaseEntities.NHibernateHelper.Finished(transaction);
    }
    
    public bool IsAdmin() {return is_admin;}
    
    public string GetName(object ID) {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        return DatabaseEntities.Committee.FindCommittee(session, (int)ID).Name;
    }
    
    public int GetCommitteePhaseProgress(DatabaseEntities.CommitteeElection election) {
        /*
        WTSPhase, NominationPhase, VotePhase,
            CertificationPhase, ConflictPhase, ClosedPhase
        */
        switch(election.Phase) {
            case DatabaseEntities.ElectionPhase.WTSPhase:
                return 5;
            case DatabaseEntities.ElectionPhase.NominationPhase:
                return 20;
            case DatabaseEntities.ElectionPhase.VotePhase:
                return 40;
            case DatabaseEntities.ElectionPhase.CertificationPhase:
                return 70;
            case DatabaseEntities.ElectionPhase.ConflictPhase:
                return 80;
            case DatabaseEntities.ElectionPhase.ClosedPhase:
                return 100;
            default:
                return 0;
        }
    }
    
    public string GetCommitteeProgressStatus(DatabaseEntities.CommitteeElection election) {
        string output = "progress ";
        
        if(election.Phase == DatabaseEntities.ElectionPhase.ClosedPhase)
            return output + "progress-info";
        
        // Make the bar red if it's over time.
        if(election.NextPhaseDate(DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession()) < DateTime.Now)
            output += "progress-danger ";
        
        switch(election.Phase) {
            case DatabaseEntities.ElectionPhase.WTSPhase:
            case DatabaseEntities.ElectionPhase.NominationPhase:
            case DatabaseEntities.ElectionPhase.VotePhase:
                output += "progress-striped";
                break;
            case DatabaseEntities.ElectionPhase.CertificationPhase:
            case DatabaseEntities.ElectionPhase.ConflictPhase:
                output += "progress-warning";
                break;
        }
        return output;
    }
    
    public string GetCommitteePhaseMessage(DatabaseEntities.CommitteeElection election) {
        switch(election.Phase) {
            case DatabaseEntities.ElectionPhase.WTSPhase:
                return "Now accepting Willingness to Serve applications.";
            case DatabaseEntities.ElectionPhase.NominationPhase:
                return "The primary voting period is currently in progress.";
            case DatabaseEntities.ElectionPhase.VotePhase:
                return "General voting is now open.";
            case DatabaseEntities.ElectionPhase.CertificationPhase:
                return "Results are currently being certified by the NEC.";
            case DatabaseEntities.ElectionPhase.ConflictPhase:
                return "Election result conflicts are awaiting resolution.";
            case DatabaseEntities.ElectionPhase.ClosedPhase:
                return "The election is closed.";
        }
        throw new Exception("Unexpected election phase.");
    }

    //sends user to homepage based on current phase and the users role
    protected void setView()
    {
        switch(phases.currentPhase) {
            case "nominate":
                OfficerNominate.Visible = true;
                break;
            case "accept1":
                OfficerNominationAccept.Visible = true;
                break;
            case "slate":
                OfficerSlate.Visible = true;
                break;
            case "petition":
                OfficerPetition.Visible = true;
                break;
            case "accept2":
                OfficerPetitionAccept.Visible = true;
                break;
            case "approval":
                OfficerApproval.Visible = true;
                break;
            case "vote":
                OfficerVoting.Visible = true;
                break;
            case "result":
                OfficerResults.Visible = true;
                break;
            default:
                OfficerStateless.Visible = true;
                break;
        }
    }

}
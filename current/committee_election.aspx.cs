using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Security;

using DatabaseEntities;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Cfg;

public partial class committee_election : System.Web.UI.Page
{
    private Committee committee;
    private CommitteeElection election;
    private DatabaseEntities.User user;

    protected void Page_Load(object sender, EventArgs e)
    {
        int eid;
        if (Request.QueryString["id"] != null)
            eid = int.Parse(Request.QueryString["id"]);
        else
            throw new HttpException(400, "Invalid election ID");

        ISession session = NHibernateHelper.CreateSessionFactory().OpenSession();

        // grab the objects based off the committee ID
        election = CommitteeElection.FindElection(session, eid);
        committee = Committee.FindCommittee(session, election.PertinentCommittee);

        user = DatabaseEntities.User.FindUser(session, User.Identity.Name);
        
        // If the user isn't an admin or nec...
        if (user.CanVote)
        {
            // expose the pertinent panel based on the state of the election.
            switch (election.Phase)
            {
                case ElectionPhase.WTSPhase:
                    FacultyWTS.Visible = true;
                    break;
                case ElectionPhase.NominationPhase:
                    FacultyNomination.Visible = true;
                    break;
                case ElectionPhase.VotePhase:
                    FacultyVote.Visible = true;
                    break;
            }
        }
        
        if(user.IsNEC && election.Phase == ElectionPhase.CertificationPhase)
            ActivateTab("CertificationPhase");
        if(user.IsAdmin) {
            DaysLeftInPhase(session);
            
            ActivateTab(election.Phase.ToString());
            
            if(election.Phase >= ElectionPhase.ClosedPhase)
                closed_tab.Visible = true;
            if(election.Phase >= ElectionPhase.ConflictPhase)
                conflicts_tab.Visible = true;
            if(election.Phase >= ElectionPhase.CertificationPhase)
                certifications_tab.Visible = true;
            if(election.Phase >= ElectionPhase.VotePhase)
                votes_tab.Visible = true;
            if(election.Phase >= ElectionPhase.NominationPhase)
                nominations_tab.Visible = true;
            if(election.Phase >= ElectionPhase.WTSPhase)
                wts_tab.Visible = true;
                
            switch(election.Phase) {
                case ElectionPhase.WTSPhase:
                    PhaseLiteral.Text = "WTS Phase";
                    break;
                case ElectionPhase.NominationPhase:
                    PhaseLiteral.Text = "Nomination Phase";
                    break;
                case ElectionPhase.VotePhase:
                    PhaseLiteral.Text = "Voting Phase";
                    break;
                case ElectionPhase.CertificationPhase:
                    PhaseLiteral.Text = "Certification Phase";
                    break;
                case ElectionPhase.ConflictPhase:
                    PhaseLiteral.Text = "Conflict Resolution Phase";
                    break;
                case ElectionPhase.ClosedPhase:
                    PhaseLiteral.Text = "Closed";
                    CancelElection.Visible = false;
                    JulioButtonHider.Visible = false;
                    break;
            }
        }

    }
    
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if(user.IsAdmin && election.Phase != ElectionPhase.ClosedPhase) {
            JulioButtonPanel.Visible = true;
            JulioButtonPhase.SelectedValue = election.Phase.ToString();
        }
    }
    
    private void DaysLeftInPhase(ISession session)
    {
        DaysRemaining.Text = "The election is closed.";
        if(election.Phase != ElectionPhase.ClosedPhase)
        {
            int days_remaining = (int)election.NextPhaseDate(session).Subtract(election.PhaseStarted).TotalDays;
            if(days_remaining > 0)
                DaysRemaining.Text = days_remaining.ToString() + " day(s) remaining for this phase.";
            else
                DaysRemaining.Text = "This phase is " + (days_remaining * -1 + 1).ToString() + " day(s) overdue.";
        }
    }
    
    private void ActivateTab(string tab_name) {
        AdminTabs.Visible = true;
        closed_tab.Attributes["class"] = "";
        conflicts_tab.Attributes["class"] = "";
        certifications_tab.Attributes["class"] = "";
        votes_tab.Attributes["class"] = "";
        nominations_tab.Attributes["class"] = "";
        wts_tab.Attributes["class"] = "";
        
        AdminWTSPanel.Visible = false;
        AdminNominationsPanel.Visible = false;
        AdminVotingPanel.Visible = false;
        AdminCertificationPanel.Visible = false;
        AdminConflictPanel.Visible = false;
        AdminClosedPanel.Visible = false;
        
        switch(tab_name) {
            case "WTSPhase":
                wts_tab.Visible = true;
                wts_tab.Attributes["class"] = "active";
                AdminWTSPanel.Visible = true;
                break;
            case "NominationPhase":
                nominations_tab.Visible = true;
                nominations_tab.Attributes["class"] = "active";
                AdminNominationsPanel.Visible = true;
                break;
            case "VotePhase":
                votes_tab.Visible = true;
                votes_tab.Attributes["class"] = "active";
                AdminVotingPanel.Visible = true;
                break;
            case "CertificationPhase":
                certifications_tab.Visible = true;
                certifications_tab.Attributes["class"] = "active";
                AdminCertificationPanel.Visible = true;
                break;
            case "ConflictPhase":
                conflicts_tab.Visible = true;
                conflicts_tab.Attributes["class"] = "active";
                AdminConflictPanel.Visible = true;
                break;
            case "ClosedPhase":
                closed_tab.Visible = true;
                closed_tab.Attributes["class"] = "active";
                AdminClosedPanel.Visible = true;
                break;
            default:
                throw new HttpException(500, "Unexpected election phase.");
        }
    }

    protected void JulioButton_Clicked(Object sender, EventArgs e)
    {
        ISession session = NHibernateHelper.CreateSessionFactory().OpenSession();
        ElectionPhase next_phase = election.NextPhase(session);
        election.SetPhase(session, next_phase);
        
        Response.Redirect("/committee_election.aspx?id=" + election.ID.ToString());
    }

    protected void JulioButtonCustom_Clicked(Object sender, EventArgs e)
    {
        ISession session = NHibernateHelper.CreateSessionFactory().OpenSession();
        ElectionPhase next_phase = (ElectionPhase)Enum.Parse(typeof(ElectionPhase), JulioButtonPhase.SelectedValue);
        election.SetPhase(session, next_phase);
        
        Response.Redirect("/committee_election.aspx?id=" + election.ID.ToString());
    }

    protected void Tab_Clicked(Object sender, EventArgs e)
    {
        ActivateTab(((LinkButton)sender).CommandName);
    }
}
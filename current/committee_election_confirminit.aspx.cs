using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
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

public partial class wwwroot_phase1aSite_committee_election_confirminit : System.Web.UI.Page
{
    Int32 committee_id = -1;
    DatabaseEntities.Committee committee;

    protected void Page_Load(object sender, EventArgs e)
    {
        String committee_id_raw = Request.QueryString["id"];
        if (committee_id_raw == null)
            throw new System.Web.HttpException(400, "Missing committee ID");
        
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        
        try {
            committee_id = Int32.Parse(committee_id_raw);
            committee = DatabaseEntities.Committee.FindCommittee(session, committee_id);
            if(committee == null)
                throw new Exception("Committee not found.");
        } catch {
            throw new System.Web.HttpException(400, "Invalid committee ID");
        }
        
        CheckForExistingElection(session);

    }
    
    public bool CheckForExistingElection(ISession session) {
        if(committee.InElection(session)) {
            Response.Redirect("/committee_election.aspx?id=" + committee.GetElection(session).ID.ToString());
            return false;
        }
        return true;
    }

    protected void StartElection_Clicked(object sender, EventArgs e)
    {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        
        if(!CheckForExistingElection(session))
            return;
        
        
        ITransaction transaction = session.BeginTransaction();
        
        DatabaseEntities.CommitteeElection election =
                DatabaseEntities.CommitteeElection.CreateElection(session, committee);
        session.SaveOrUpdate(election);

        //SetPhase to WTS to send emails
        election.SetPhase(session ,ElectionPhase.WTSPhase);
        
        DatabaseEntities.NHibernateHelper.Finished(transaction);
        
        if(election != null)
            Response.Redirect("committee_election.aspx?id=" + election.ID);
        else
            throw new HttpException(500, "Null was returned for election object.");
    }
}
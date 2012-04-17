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

public partial class wwwroot_phase1aSite_committee_election_manage : System.Web.UI.Page
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
        
        CommName1.Text = committee.Name;
        CommName2.Text = committee.Name;
        
        if(!Page.IsPostBack) {
            GridViewUsers.DataSource = DatabaseEntities.User.FindUsers(session, committee);
            GridViewUsers.DataBind();
        }
    }

    protected void CreateVacancies_Clicked(object sender, EventArgs e)
    {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        ITransaction transaction = session.BeginTransaction();
        
        int count = 0;
        foreach(GridViewRow gvr in GridViewUsers.Rows) {
            CheckBox cbr = (CheckBox)gvr.FindControl("CBRemove");
            if(cbr.Checked) {
                HiddenField uid_field = (HiddenField)gvr.FindControl("UserID");
                int uid = int.Parse(uid_field.Value);
                
                DatabaseEntities.User u = DatabaseEntities.User.FindUser(session, uid);
                u.CurrentCommittee = -1;
                session.SaveOrUpdate(u);
                
                count++;
            }
        }
        
        DatabaseEntities.NHibernateHelper.Finished(transaction);
        
        failure_panel.Visible = count == 0;
        if(count > 0)
            Response.Redirect("committee_election_confirminit.aspx?id=" + committee.ID);
    }

    protected void sorting(object sender, GridViewSortEventArgs e)
    {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        ICriteria criteria = session.CreateCriteria(typeof(DatabaseEntities.User))
                                        .Add(Restrictions.Eq("CurrentCommittee", committee.ID))
                                        .AddOrder(new Order(e.SortExpression.ToString(), true));
        
        GridViewUsers.DataSource = criteria.List<DatabaseEntities.User>();
        GridViewUsers.DataBind();
    }
}
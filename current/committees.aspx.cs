﻿using System;
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

public partial class wwwroot_phase1aSite_committees : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        ITransaction transaction = session.BeginTransaction();
        
        var committees = session.CreateCriteria(typeof(DatabaseEntities.Committee)).List<DatabaseEntities.Committee>();
        for(int i = 0; i < committees.Count; i++)
        {
            DatabaseEntities.Committee committee = committees[i];
            int vacancy_count = committee.PositionCount -
                                session.CreateCriteria(typeof(DatabaseEntities.User))
                                           .Add(Restrictions.Eq("CurrentCommittee", committee.ID))
                                           .List().Count;
            IList<DatabaseEntities.CommitteeElection> active_election =
                    session.CreateCriteria(typeof(DatabaseEntities.CommitteeElection))
                               .Add(Restrictions.Eq("PertinentCommittee", committee.ID))
                               .List<DatabaseEntities.CommitteeElection>();
            
            TableRow tr = new TableRow();
            
            TableCell name, positions, vacancies, status;
            
            name = new TableCell();
            name.Controls.Add(new LiteralControl(committee.Name));
            tr.Cells.Add(name);
            
            positions = new TableCell();
            positions.Controls.Add(new LiteralControl(committee.PositionCount + " Positions"));
            tr.Cells.Add(positions);
            
            vacancies = new TableCell();
            vacancies.Controls.Add(new LiteralControl(vacancy_count + " Vacancies"));
            tr.Cells.Add(vacancies);
            
            status = new TableCell();
            if(vacancy_count == 0)
                status.Controls.Add(new LiteralControl("Too few vacancies for election."));
            else
            {
                if(active_election.Count == 0)
                {
                    Button start_election_button = new Button();
                    start_election_button.Text = "Initiate Election";
                    start_election_button.ID = "initiate_election_" + committee.ID;
                    start_election_button.CssClass = "btn btn-small btn-success";
                    start_election_button.OnClientClick =
                            "window.location.href = \"/committee_election_confirminit.aspx?id=" +
                            committee.ID +
                            "\";return false;";
                    
                    status.Controls.Add(start_election_button);
                }
                else
                {
                    Button visit_election = new Button();
                    visit_election.Text = "Visit Election";
                    visit_election.OnClientClick = "window.location.href = \"/committee_election.aspx?id=" +
                                                   active_election[0].ID + "\";return false;";
                    
                    status.Controls.Add(visit_election);
                }
            }
            
            tr.Cells.Add(status);
            
            CommitteeTable.Rows.Add(tr);
        }
        
        DatabaseEntities.NHibernateHelper.Finished(transaction);
    }
}
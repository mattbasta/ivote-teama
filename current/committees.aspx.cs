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
            int vacancy_count = committee.NumberOfVacancies(session);
            IList<DatabaseEntities.CommitteeElection> active_election =
                    session.CreateCriteria(typeof(DatabaseEntities.CommitteeElection))
                               .Add(Restrictions.Eq("PertinentCommittee", committee.ID))
                               .Add(Restrictions.Not(
                                        Restrictions.Eq("Phase", DatabaseEntities.ElectionPhase.ClosedPhase)))
                               .List<DatabaseEntities.CommitteeElection>();
            
            TableRow tr = new TableRow();
            
            TableCell name, positions, vacancies, members, status;
            
            name = new TableCell();
            name.Controls.Add(new LiteralControl(committee.Name));
            tr.Cells.Add(name);
            
            positions = new TableCell();
            positions.Controls.Add(new LiteralControl(committee.PositionCount + " Members"));
            tr.Cells.Add(positions);
            
            vacancies = new TableCell();
            vacancies.Controls.Add(new LiteralControl(vacancy_count + " Vacancies"));
            tr.Cells.Add(vacancies);

            List<User> users = DatabaseEntities.User.FindUsers(session, committee.Name);
            string toAdd = "";
            for(int j = 0; j < users.Count; j++)
            {
                toAdd += users[j].FirstName + " " + users[j].LastName + ((j != users.Count - 1) ? ("<br />"):(""));
            }
            members = new TableCell();
            members.Controls.Add(new LiteralControl(toAdd));
            tr.Cells.Add(members);
            
            status = new TableCell();
            
            if(vacancy_count > 0) {
                if(active_election.Count == 0)
                {
                    Button start_election_button = new Button();
                    start_election_button.Text = "Initiate Election";
                    start_election_button.ID = "initiate_election_" + committee.ID;
                    start_election_button.CssClass = "btn btn-success";
                    start_election_button.OnClientClick =
                            "window.location.href = \"/committee_election_confirminit.aspx?id=" +
                            committee.ID +
                            "\";return false;";
                    
                    status.Controls.Add(start_election_button);
                }
                else
                {
                    Button visit_election = new Button();
                    visit_election.CssClass = "btn";
                    visit_election.Text = "Visit Election";
                    visit_election.OnClientClick = "window.location.href = \"/committee_election.aspx?id=" +
                                                   active_election[0].ID + "\";return false;";
                    
                    status.Controls.Add(visit_election);
                }
            }

            if(active_election.Count == 0 && vacancy_count < committee.PositionCount) {
                HyperLink manage_committee_btn = new HyperLink();
                manage_committee_btn.CssClass = "btn";
                manage_committee_btn.Text = "Manage Membership";
                manage_committee_btn.ID = "manage_committee_" + committee.ID;
                manage_committee_btn.NavigateUrl = "/committee_election_manage.aspx?id=" + committee.ID;
                status.Controls.Add(manage_committee_btn);
            } else if(vacancy_count == 0)
                status.Controls.Add(new LiteralControl("Too few vacancies for election"));

            List<string> conflicts = Committee.FindConflicts(session, committee);
            foreach (string j in conflicts)
                status.Controls.Add(new LiteralControl(j));

            tr.Cells.Add(status);
            
            CommitteeTable.Rows.Add(tr);
        }
        
        DatabaseEntities.NHibernateHelper.Finished(transaction);
    }
}
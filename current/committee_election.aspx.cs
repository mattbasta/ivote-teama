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

namespace current
{
    public partial class committee_election : System.Web.UI.Page
    {
        private Committee committee;
        private CommitteeElection election;

        protected void Page_Load(object sender, EventArgs e)
        {
            int CommitteeID = -1;
            if (Request.QueryString["cid"] != null)
                CommitteeID = int.Parse(Request.QueryString["cid"]);
            else ;// TODO: Handle error if there is no CID in the url

            ISession session = NHibernateHelper.CreateSessionFactory().OpenSession();
            ITransaction transaction = session.BeginTransaction();

            // grab the objects based off the committee ID
            committee = Committee.FindCommittee(session, CommitteeID);
            election = CommitteeElection.FindElection(session, committee.Name);

            bool isAdmin = false;

            foreach (string aString in Roles.GetRolesForUser())
            {
                // If the user is an admin or NEC, expose the tabbed admin / NEC panels
                if (aString == "admin" || aString == "nec")
                {
                    // if we're in the closed phase, instead of exposing the tabbed panels
                    // just expose the closed phase panel
                    if (election.Phase == ElectionPhase.ClosedPhase)
                        ClosePanel.Visible = true;
                    else
                        AdminTabs.Visible = true;
                    isAdmin = true;
                }
            }
            // If the user isn't an admin or nec...
            if (isAdmin == false)
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

        }
    }
}
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

public partial class officer_election : System.Web.UI.Page
{
    //OBJECT DECLARATION
    public databaseLogic dbLogic = new databaseLogic();
    timeline phases = new timeline();
    voteCounter vc = new voteCounter();
    ArrayList positions = new ArrayList();
    
    public DatabaseEntities.User user;
    
    bool is_admin;

    private DatabaseEntities.User GetUser(ISession session) {
        return DatabaseEntities.User.FindUser(session, User.Identity.Name);
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        user = GetUser(session);
        is_admin = user.IsAdmin;
    }
    
    protected void Page_PreRender(object sender, EventArgs e)
    {
        setView();
        try {
            JulioButtonPhase.SelectedValue = phases.currentPhase;
        } catch(ArgumentOutOfRangeException aoore) {} // This shouldn't matter.
    }
    
    protected string GetName(int UserID) {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        User u = DatabaseEntities.User.FindUser(session, UserID);
        return u.FirstName + " " + u.LastName;
    }
    
    public bool IsAdmin() {return is_admin;}
    
    private void DaysLeftInPhase()
    {
        if(phases.currentPhase == "nullphase" ||
           phases.currentPhase == "")
            DaysRemaining.Text = "No election is currently in progress.";
        else if(phases.currentPhase == "result")
            DaysRemaining.Text = "The election is closed.";
        else
        {
            int days_remaining = phases.daysRemaining();
            if(days_remaining > 0)
                DaysRemaining.Text = (days_remaining + 1).ToString() + " day(s) remaining for this phase.";
            else
                DaysRemaining.Text = "This phase is " + (days_remaining * -1 + 1).ToString() + " day(s) overdue.";
        }
    }

    //sends user to homepage based on current phase and the users role
    protected void setView()
    {
        CancelElection.Visible = true;
        InitiateNewElection.Visible = false;
        
        // Reset the panel visibility.
        OfficerNominate.Visible = false;
        OfficerNominationAccept.Visible = false;
        OfficerSlate.Visible = false;
        OfficerPetition.Visible = false;
        OfficerPetitionAccept.Visible = false;
        OfficerApproval.Visible = false;
        OfficerVoting.Visible = false;
        OfficerResults.Visible = false;
        OfficerStateless.Visible = false;
        
        JulioButtonHider.Visible = is_admin;
        JulioButton.Visible = true;
        
        switch(phases.currentPhase) {
            case "nominate":
                OfficerNominate.Visible = true;
                NominateUpdatePanel.Visible = user.CanVote;
                PhaseLiteral.Text = "Nomination Phase";
                
                dbLogic.selectAllAvailablePositions();
                DataSet postionsSet = dbLogic.getResults();
                GridViewPositions.DataSource = postionsSet;
                GridViewPositions.DataBind();
                
                break;
            case "accept1":
                OfficerNominationAccept.Visible = true;
                PhaseLiteral.Text = "Nomination Acceptance Phase";
                break;
            case "slate":
                OfficerSlate.Visible = true;
                PhaseLiteral.Text = "Slate Phase";
                
                functions_slate.Visible = user.IsAdmin;
                nec_approval_form.Visible = user.IsNEC;
                
                if (dbLogic.checkSlateApprove())
                {
                    phases.bumpPhase();
                    Response.Redirect("/officer_election.aspx");
                }
                
                break;
            case "petition":
                OfficerPetition.Visible = true;
                PhaseLiteral.Text = "Petition Phase";
                
                dbLogic.selectAllAvailablePositions();
                DataSet positionSet = dbLogic.getResults();
                DropDownListPostions.DataSource = positionSet;
                DropDownListPostions.DataTextField = positionSet.Tables[0].Columns[2].ToString();
                DropDownListPostions.DataBind();
                
                break;
            case "accept2":
                OfficerPetitionAccept.Visible = true;
                PhaseLiteral.Text = "Petition Acceptance Phase";
                break;
            case "approval":
                OfficerApproval.Visible = true;
                PhaseLiteral.Text = "Approval Phase";
                break;
            case "vote":
                OfficerVoting.Visible = true;
                PhaseLiteral.Text = "General Voting Phase";
                
                UpdatePanel3.Visible = user.CanVote;
                
                if (dbLogic.isUserNewVoter(user.ID)) {
                    votehider.Visible = true;
                    dbLogic.selectAllBallotPositions();
                    SlateView.DataSource = dbLogic.getResults();
                    SlateView.DataBind();
                }
                else
                {
                    LabelFeedbackVote2.Text = "You have already voted for this election.";
                    votehider.Visible = false;
                    ButtonSubmitVotes.Visible = false;
                }
                
                break;
            case "result":
                OfficerResults.Visible = true;
                PhaseLiteral.Text = "Result Phase";
                // You can't cancel an election that isn't happening.
                CancelElection.Visible = false;
                JulioButtonHider.Visible = false;
                InitiateNewElection.Visible = true;
                
                bindPositions();
                
                break;
            default:
                OfficerStateless.Visible = true;
                FunctionsStateless.Visible = user.IsAdmin;
                PhaseLiteral.Text = "Inactive";
                
                // You can't cancel an election that isn't happening.
                CancelElection.Visible = false;
                JulioButtonHider.Visible = false;
                InitiateNewElection.Visible = true;
                break;
        }
        
        DaysLeftInPhase();
        
        if(phases.currentPhase != "vote" &&
           phases.currentPhase != "result") {
            checkForNomination();
            if(is_admin)
                checkForEligibility();
        }
        
        if (is_admin)
        {
            functions_nominate.Visible = true;
            functions_accept1.Visible = true;
            functions_approval.Visible = true;
            functions_petition.Visible = true;
            functions_accept2.Visible = true;
            functions_voting.Visible = true;
            //results
            if (phases.currentPhase == "result")
                adminEnd.Visible = dbLogic.checkNecApprove();
        }
        
        if (User.IsInRole("nec"))
        {
            if (phases.currentPhase == "slate") {
                dbLogic.selectAllBallotPositions();
                DataSet emailSet = dbLogic.getResults();
                ListViewPositions2.DataSource = emailSet;
                ListViewPositions2.DataBind();
                
                btnApprove.Visible = true;
                
            }
            //results
            else if (phases.currentPhase == "result")
            {
                //this is for NEC role
                necApprove.Visible = !dbLogic.checkNecApprove();
            }
        }
    }

    protected void JulioButton_Clicked(Object sender, EventArgs e)
    {
        phases.bumpPhase();
        Response.Redirect("/officer_election.aspx");
    }

    protected void JulioButtonCustom_Clicked(Object sender, EventArgs e)
    {
        phases.changePhaseToCurrent(JulioButtonPhase.SelectedValue, true);
        Response.Redirect("/officer_election.aspx?" + JulioButtonPhase.SelectedValue);
    }

    //gridview actions
    protected void GridViewPositions_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if(e.CommandName == "nom_me")
            Response.Redirect("/WTS.aspx?position=" + e.CommandArgument.ToString());
        else if(e.CommandName == "nom_other")
            Response.Redirect("/Nominate.aspx?position=" + e.CommandArgument.ToString());
        
    }

    //check if the user has a nomination pending
    protected void checkForNomination()
    {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        DatabaseEntities.User user = GetUser(session);
        if (dbLogic.isUserNominatedPending(user.ID))
        {
            PanelNominationPending.Visible = true;
            nom_pending.Visible = true;
        }
    }

    //check to see if there is any pending eligibility
    protected void checkForEligibility()
    {
        if (dbLogic.returnEligibilityCount() > 0)
        {
            PanelNominationPending.Visible = true;
            elig_pending.Visible = true;
        }
    }

    /********************************************
     * SLATE APPROVE
     * FUNCTIONALITY
     * *****************************************/
    //for slate display
    protected void ListViewPositions2_ItemCommand(Object sender, ListViewCommandEventArgs e)
    {
        if (String.Equals(e.CommandName, "position"))
        {
            dbLogic.selectAllForBallot(e.CommandArgument.ToString());
            DataSet emailSet = dbLogic.getResults();
            ListViewPeople2.DataSource = emailSet;
            ListViewPeople2.DataBind();
            PanelPeople2.Visible = true;
            PanelSelect2.Visible = false;
            HiddenFieldCurrentPosition2.Value = e.CommandArgument.ToString();
            LabelFeedback2.Text = "Select a candidate for this position.";
        }
    }

    protected void ListViewPeople2_ItemCommand(Object sender, ListViewCommandEventArgs e)
    {
        if (String.Equals(e.CommandName, "id"))
        {

            dbLogic.selectDetailFromWTS(e.CommandArgument.ToString());
            DataSet infoSet = dbLogic.getResults();
            DataRow dr = infoSet.Tables["query"].Rows[0];

            LabelStatement2.Text = "\"" + dr["statement"].ToString() + "\"";
            HiddenFieldName2.Value = GetName(int.Parse(dr["idunion_members"].ToString()));
            HiddenFieldId2.Value = e.CommandArgument.ToString();

            if (LabelStatement2.Text == "\"\"")
                LabelStatement2.Text = GetName(int.Parse(dr["idunion_members"].ToString())) + " did not include a personal statement.";

            PanelSelect2.Visible = true;
            LabelFeedback2.Text = "";
        }
    }

    protected void btnApprove_OnClick(object sender, EventArgs e)
    {
        phases.bumpPhase();
        Response.Redirect("/officer_election.aspx");
    }

    /********************************************
     * PETITION
     * FUNCTIONALITY
     * *****************************************/
    //Petition component code
    protected void search(object sender, EventArgs e)
    {
        string query = txtSearch.Text;
    
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        ListViewUsers.DataSource = session.CreateCriteria(typeof(DatabaseEntities.User))
                .Add(Restrictions.Or(Restrictions.Like("FirstName", "%" + query + "%"),
                                     Restrictions.Like("LastName", "%" + query + "%")))
                .List<DatabaseEntities.User>();
        ListViewUsers.DataBind();
        ListViewUsers.Visible = true;

        //loadConfirmPopup();

        btnViewAll.Visible = txtSearch.Text != "";
    }

    protected void ListViewUsers_ItemCommand(Object sender, ListViewCommandEventArgs e)
    {
        if (String.Equals(e.CommandName, "nominate"))
        {
            ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
            DatabaseEntities.User user = DatabaseEntities.User.FindUser(session, int.Parse(e.CommandArgument.ToString()));
            LabelChoosPosition.Text = "Please select the position you would<br /> like " + user.FirstName + " " + user.LastName + " to be petitioned for:";
            ButtonSubmit.OnClientClick = "return confirm('Are you sure you want to start this petition for " + user.FirstName + " " + user.LastName + "?\\n(If accepted, you will not be able to withdraw your petition  later.)')";
            HiddenFieldName.Value = user.FirstName + " " + user.LastName;
            HiddenField1.Value = e.CommandArgument.ToString();
            PopupControlExtender1.Show();
        }
    }
    protected void clear(object sender, EventArgs e)
    {
    /*
        ListViewUsers.Visible = false;
        btnViewAll.Visible = false;
    */
    }

    protected void ButtonSubmit_Clicked(object sender, EventArgs e)
    {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        DatabaseEntities.User user = GetUser(session);
        string[] petitionInfo = { HiddenField1.Value, DropDownListPostions.SelectedItem.Text, user.ID.ToString() };//

        //submit request
        if (!dbLogic.isUserEnteringPetitionTwice(petitionInfo)) //checks if user has already entered this petition
        {
            if (dbLogic.countPetitionsForPerson(petitionInfo) >= 4) //MUST BE CHANGED BACK TO 10
                LabelFeedback.Text = "Thank you for your submission. This person already has enough petition signatures to be on the ballot for this election.";
            else if (dbLogic.countPetitionsForPerson(petitionInfo) == 3) //MUST BE CHANGED BACK TO 9
            {
                dbLogic.insertPetition(petitionInfo);
                //move data to nomination table
                LabelFeedback.Text = "Petition signature submitted. " + HiddenFieldName.Value + " now has enough signatures to be on the ballot.";

                //add data to nominate_accept table
                string[] acceptInfo = { HiddenField1.Value, "0", DropDownListPostions.SelectedItem.Text, "1" };
                dbLogic.insertNominationAcceptFromPetition(acceptInfo);
            }
            else
            {
                dbLogic.insertPetition(petitionInfo);
                if (dbLogic.countPetitionsForPerson(petitionInfo) == 1)
                    LabelFeedback.Text = "Petition started for " + HiddenFieldName.Value + " to be on the next ballot for " + DropDownListPostions.SelectedItem.Text + ".";
                else
                    LabelFeedback.Text = "Petition signature submitted for " + HiddenFieldName.Value + " to be on the next ballot for " + DropDownListPostions.SelectedItem.Text + ".";
           }
        }
        else
            LabelFeedback.Text = "Submission rejected. You have already signed a petition for " + HiddenFieldName.Value + " to be on the next ballod for " + DropDownListPostions.SelectedItem.Text + ".";

        //LabelFeedback.Text = HiddenFieldId.Value + ", " + DropDownListPostions.SelectedItem.Text + ", " + user.ID + "<br />" + dbLogic.countPetitionsForPerson(petitionInfo);

        //reset form
        txtSearch.Text = "";
        ListViewUsers.Visible = false;
        btnViewAll.Visible = false;
        PopupControlExtender1.Hide();

    }

    protected void ButtonCancel_Clicked(object sender, EventArgs e)
    {
        PopupControlExtender1.Hide();
    }

    /********************************************
     * VOTING
     * FUNCTIONALITY
     * *****************************************/

    protected void SlateView_ItemDataBound(Object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType != ListViewItemType.DataItem) return;
        
        // Fuck everything about this. Seriously, whoever designed the DB is a fucking moron.
        HiddenField position_id = (HiddenField)e.Item.FindControl("Position");
        dbLogic.selectAllForPosition(position_id.Value);
        
        ListView candidates = (ListView)e.Item.FindControl("candidates");
        candidates.DataSource = dbLogic.getResults();
        candidates.DataBind();
        
    }
    protected void ButtonSubmitVotes_Clicked(object sender, EventArgs e)
    {
        int votingCounter = 0;
        bool pluralityNotOccuring = true;
        
        // Loop once to validate
        foreach (ListViewDataItem myItem in SlateView.Items)
        {
            HiddenField position = (HiddenField)myItem.FindControl("Position");
            HiddenField position_type = (HiddenField)myItem.FindControl("PositionType");
            
            bool voted = false;
            
            if(position_type.Value != "Plurality") {
                ListView candidates = (ListView)myItem.FindControl("candidates");
                foreach (ListViewDataItem candidate in candidates.Items) {
                    RadioButton r = (RadioButton)candidate.FindControl("checker");
                    if(r.Checked) {
                        voted = true;
                        break;
                    }
                }
                if(!voted)
                    LabelFeedbackVote2.Text = "You did not cast a vote for " + position.Value;
            } else {
                HiddenField votes_allowed_field = (HiddenField)myItem.FindControl("VotesAllowed");
                int votes_allowed = int.Parse(votes_allowed_field.Value);
                ListView candidates = (ListView)myItem.FindControl("candidates_plurality");
                int votes_cast = 0;
                foreach (ListViewDataItem candidate in candidates.Items) {
                    CheckBox r = (CheckBox)candidate.FindControl("checker");
                    if(r.Checked)
                        votes_cast++;
                }
                if(votes_cast < votes_allowed)
                    LabelFeedbackVote2.Text = "You did not cast enough votes for " + position.Value;
                else if(votes_cast > votes_allowed)
                    LabelFeedbackVote2.Text = "You cast too many votes for " + position.Value;
                
                voted = votes_cast == votes_allowed;
            }
            
            if(!voted)
                return;
        }
        
        // Loop again to store
        foreach (ListViewDataItem myItem in SlateView.Items)
        {
            HiddenField position = (HiddenField)myItem.FindControl("Position");
            HiddenField position_type = (HiddenField)myItem.FindControl("PositionType");
            
            ListView candidates = (ListView)myItem.FindControl(
                (position_type.Value != "Plurality") ? "candidates" : "candidates_plurality");
            if(position_type.Value != "Plurality") {
                foreach (ListViewDataItem candidate in candidates.Items) {
                    HiddenField cid = (HiddenField)candidate.FindControl("CandidateID");
                    int candidate_id = int.Parse(cid.Value);
                    RadioButton r = (RadioButton)candidate.FindControl("checker");
                    if(r.Checked) {
                        dbLogic.insertNewVote(candidate_id, position.Value);
                        break;
                    }
                }
            } else {
                foreach (ListViewDataItem candidate in candidates.Items) {
                    CheckBox r = (CheckBox)candidate.FindControl("checker");
                    if(r.Checked) {
                        HiddenField cid = (HiddenField)candidate.FindControl("CandidateID");
                        int candidate_id = int.Parse(cid.Value);
                        dbLogic.insertNewVote(candidate_id, position.Value);
                    }
                }
            }
        }

        //create confirmation code
        string code = System.Convert.ToBase64String(
                          System.Text.Encoding.UTF8.GetBytes(
                              System.Web.Security.Membership.GeneratePassword(10, 0)));

        //set flag that user has voted
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        DatabaseEntities.User user = GetUser(session);
        dbLogic.insertFlagVoted(user.ID, code);

        //hide slate
        votehider.Visible = false;

        //give feedback
        LabelFeedbackVote2.Text = "Your ballot has been successfully submitted and processed.<br /><br /> Your confirmation code is: <b>" + code + "</b>";
    }
    
    /*********************************************
     * RESULTS
     * FUNCTIONALITY
     * ******************************************/
    protected void bindPositions()
    {
        
        DataSet ds = new DataSet();
        dbLogic.genericQuerySelector("SELECT * FROM election_position;");
        ds = dbLogic.getResults();
        positions.Clear();
        for(int i = 0; i < ds.Tables[0].Rows.Count; i++)
            positions.Add(ds.Tables[0].Rows[i].ItemArray[2]);

        vc.tally();

        //binds result data
        dbLogic.getPosAndWinner();
        ds = dbLogic.getResults();
        resultList.DataSource = ds;
        resultList.DataBind();

        //if user is not admin, cannot see "view result detail" row
        resultList.Columns[2].Visible = User.IsInRole("admin") || User.IsInRole("nec");
    }

    //sends user to 
    protected void resultList_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (String.Equals(e.CommandName, "position"))
            Response.Redirect("ResultDetail.aspx/" + e.CommandArgument.ToString());
    }

    //nec must approve nomination before use
    protected void necButton_OnClick(Object sender, EventArgs e)
    {
        dbLogic.insertNecApprove();
        necApprove.Visible = false;
        necButton.Visible = false;
    }

    protected void adminButton_OnClick(Object sender, EventArgs e)
    {
        //dbLogic.turnOffPhase("result");
        dbLogic.resetElection();
        Response.Redirect("/officer_election.aspx");
    } 
}
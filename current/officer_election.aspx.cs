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
    databaseLogic dbLogic = new databaseLogic();
    timeline phases = new timeline();
    VerifyEmail email = new VerifyEmail();
    voteCounter vc = new voteCounter();
    ArrayList positions = new ArrayList();
    
    bool is_admin;

    protected void Page_Load(object sender, EventArgs e)
    {
        setView();
        is_admin = User.IsInRole("admin");
        if(is_admin)
            AdminTabs.Visible = true;
        
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        
        if(is_admin)
        {
            int waiting_committees = DatabaseEntities.Committee.NumberOfWaitingCommittees(session);
            if(waiting_committees > 0)
            {
                WaitingCommittees.Visible = true;
                WaitingCommittees.Text = waiting_committees.ToString();
            }
        }
        
    }
    
    public bool IsAdmin() {return is_admin;}

    //sends user to homepage based on current phase and the users role
    protected void setView()
    {
        //if they are faculty
        if (User.IsInRole("faculty"))
        {
            //nominatation
            if (phases.currentPhase == "nominate")
            {
                //if this is the first time the phase has occured, then a set group of site functions occur
                if (phases.firstTimeOccurence(phases.currentPhase))
                    email.phaseNomination();

                //page load data
                loadData(); //call method to load office position data

                //notifications
                checkForNomination();

                //set label visible
                OfficerNominate.Visible = true;
            }
            //accept1
            else if (phases.currentPhase == "accept1")
            {
                //make sure the phase can't be skipped
                if (dbLogic.canSkipPhase())
                {
                    dbLogic.killPhase("accept1");
                    Response.Redirect("home.aspx");
                }
              
                if (phases.firstTimeOccurence(phases.currentPhase))
                    //email notification
                    email.phaseAccept1();

                //notifications
                checkForNomination();

                //set label visible
                OfficerNominationAccept.Visible = true;
            }
            //slate
            else if (phases.currentPhase == "slate")
            {
 
                //make sure phase can't be ended, then display stateless
                if (dbLogic.checkSlateApprove())
                {
                    dbLogic.killPhase("slate");
                    Response.Redirect("home.aspx");
                }
                
                
                if (phases.firstTimeOccurence(phases.currentPhase))
                    //email notification
                    email.phaseSlate();
                
                //make label visible
                OfficerStateless.Visible = true;
            }
            //petition
            else if (phases.currentPhase == "petition")
            {
                //if this is the first time the phase has occured, then a set group of site functions occur
                if (phases.firstTimeOccurence(phases.currentPhase))
                {
                    email.phasePetition();
                    dbLogic.phase_ClearNullNominations();
                }
                //load data
                dbLogic.selectAllAvailablePositions();
                DataSet positionSet = dbLogic.getResults();
                DropDownListPostions.DataSource = positionSet;
                DropDownListPostions.DataTextField = positionSet.Tables[0].Columns[2].ToString();
                DropDownListPostions.DataBind();


                //notifications
                checkForNomination();

                //set label
                lblPetition.Visible = true;
            }
            //accept 2
            else if (phases.currentPhase == "accept2")
            {
                //make sure the phase can't be skipped
                if (dbLogic.canSkipPhase())
                {
                    dbLogic.killPhase("accept2");
                    Response.Redirect("home.aspx");
                }

                if (phases.firstTimeOccurence(phases.currentPhase))
                    //email notification
                    email.phaseAccept2();
                
                //notifications
                checkForNomination();

                //set label visible
                lblAccept2.Visible = true;
            }
            else if (phases.currentPhase == "approval")
            {
                //make sure phase can't be ended, then display stateless
                if (dbLogic.canSkipAdminPhase())
                {
                    dbLogic.killPhase("approval");
                    Response.Redirect("home.aspx");
                }

                if (phases.firstTimeOccurence(phases.currentPhase))
                    //email notification
                    email.phaseApproval();

                //make label visible
                OfficerStateless.Visible = true;
            }
            //voting
            else if (phases.currentPhase == "vote")
            {
                //if this is the first time the phase has occured, then a set group of site functions occur
                if (phases.firstTimeOccurence(phases.currentPhase))
                {
                    email.phaseVote();
                    dbLogic.phase_ClearNullNominations();
                }
                //load data
                //voting functionality
                dbLogic.selectAllBallotPositions();
                DataSet emailSet = dbLogic.getResults();
                ListViewPositions.DataSource = emailSet;
                ListViewPositions.DataBind();

                if (dbLogic.isUserNewVoter(dbLogic.returnUnionIDFromUsername(HttpContext.Current.User.Identity.Name)))
                    PanelSlateWrapper.Visible = true;
                else
                {
                    PanelSlateWrapper.Visible = false;
                    LabelFeedbackVote2.Text = "You have already voted for this election.";
                    LabelFeedback.Text = "You have already voted for this election.";
                }

                //no notifications for this phase

                //set label visible
                lblVoting.Visible = true;
            }
            //results
            else if (phases.currentPhase == "result")
            {
                //if this is the first time the phase has occured, then a set group of site functions occur
                if (phases.firstTimeOccurence(phases.currentPhase))
                    email.phaseResults();

                //make phase visible
                lblResults.Visible = true;
                lblResults.Enabled = true;

                //results functionality
                bindPositions();

            }
            //stateless
            else
                //make label visible
                OfficerStateless.Visible = true;
        }
        else if (User.IsInRole("admin"))
        {
            //nominatation
            if (phases.currentPhase == "nominate")
            {
                //if this is the first time the phase has occured, then a set group of site functions occur
                if (phases.firstTimeOccurence(phases.currentPhase))
                    email.phaseNomination();
                
                //page load data
                loadData(); //call method to load office position data

                //notifications
                checkForNomination();
                checkForEligibility();

                //set label visible
                OfficerNominate.Visible = true;
                functions_nominate.Visible = true;
            }
            //accept1
            else if (phases.currentPhase == "accept1")
            {
                //make sure the phase can't be skipped
                if (dbLogic.canSkipPhase())
                {
                    dbLogic.killPhase("accept1");
                    Response.Redirect("home.aspx");
                }

                if (phases.firstTimeOccurence(phases.currentPhase))
                    //email notification
                    email.phaseAccept1();

                //notifications
                checkForNomination();
                checkForEligibility();

                //set label visible
                OfficerNominationAccept.Visible = true;
                functions_accept1.Visible = true;
            }
            //slate
            else if (phases.currentPhase == "slate")
            {
                PanelSlateWrapper2.Visible = false;
                LabelFeedback2.Text = "";

                //make sure phase can't be ended, then display stateless
                if (dbLogic.checkSlateApprove())
                {
                    dbLogic.killPhase("slate");
                    Response.Redirect("home.aspx");
                }
                checkForEligibility();

                if (phases.firstTimeOccurence(phases.currentPhase))
                    //email notification
                    email.phaseSlate();
                
                //make label visible
                lblSlate.Visible = true;
                functions_slate.Visible = true;
            } //petition
            else if (phases.currentPhase == "petition")
            {
                //if this is the first time the phase has occured, then a set group of site functions occur
                if (phases.firstTimeOccurence(phases.currentPhase))
                {
                    email.phasePetition();
                    dbLogic.phase_ClearNullNominations();
                }
                //load data
                dbLogic.selectAllAvailablePositions();
                DataSet positionSet = dbLogic.getResults();
                DropDownListPostions.DataSource = positionSet;
                DropDownListPostions.DataTextField = positionSet.Tables[0].Columns[2].ToString();
                DropDownListPostions.DataBind();

                //notifications
                checkForNomination();
                checkForEligibility();

                //set label
                lblPetition.Visible = true;
                functions_petition.Visible = true;
            }

            else if (phases.currentPhase == "accept2")
            {
                //make sure the phase can't be skipped
                bool canSkip = dbLogic.canSkipPhase();
                if (canSkip)
                {
                    dbLogic.killPhase("accept2");
                    Response.Redirect("home.aspx");
                }

                if (phases.firstTimeOccurence(phases.currentPhase))
                {
                    //email notification
                    email.phaseAccept2();
                }
                
                //notifications
                checkForNomination();
                checkForEligibility();

                //set label visible
                lblAccept2.Visible = true;
                functions_accept2.Visible = true;
            }
            //approval
            else if (phases.currentPhase == "approval")
            {
                //make sure phase can't be ended, then display stateless
                if (dbLogic.canSkipAdminPhase())
                {
                    dbLogic.killPhase("approval");
                    Response.Redirect("home.aspx");
                }

                if (phases.firstTimeOccurence(phases.currentPhase))
                    //email notification
                    email.phaseApproval();

                checkForEligibility();
                //make label visible
                lblApproval.Visible = true;
                functions_approval.Visible = true;
            }
            //voting
            else if (phases.currentPhase == "vote")
            {
                //if this is the first time the phase has occured, then a set group of site functions occur
                if (phases.firstTimeOccurence(phases.currentPhase))
                {
                    email.phaseVote();
                    dbLogic.phase_ClearNullNominations();
                }
                //load data
                //voting functionality
                dbLogic.selectAllBallotPositions();
                DataSet emailSet = dbLogic.getResults();
                ListViewPositions.DataSource = emailSet;
                ListViewPositions.DataBind();

                if (dbLogic.isUserNewVoter(dbLogic.returnUnionIDFromUsername(HttpContext.Current.User.Identity.Name)))
                    PanelSlateWrapper.Visible = true;
                else
                {
                    PanelSlateWrapper.Visible = false;
                    LabelFeedbackVote2.Text = "You have already voted for this election.";
                    LabelFeedback.Text = "You have already voted for this election.";
                }


                //no notifications for this phase

                //set label visible
                lblVoting.Visible = true;
                functions_voting.Visible = true;
            }
            //results
            else if (phases.currentPhase == "result")
            {
                //if this is the first time the phase has occured, then a set group of site functions occur
                if (phases.firstTimeOccurence(phases.currentPhase))
                    email.phaseResults();

                    //make phase visible
                lblResults.Visible = true;
                lblResults.Enabled = true;

                //results functionality
                bindPositions();

                //admin functionality
                if (dbLogic.checkNecApprove())
                    //if nec approved, then the admin can end the election
                    adminButton.Visible = true;
                else
                    adminEnd.Visible = true;

                    }
            //stateless
            else
            {
                //Response.Redirect("Views/Admin/stateless.aspx");
                OfficerStateless.Visible = true;
                functions_stateless.Visible = true;
            }
        }
        else if (User.IsInRole("nec"))
        {
            //nominatation
            if (phases.currentPhase == "nominate")
            {
                //if this is the first time the phase has occured, then a set group of site functions occur
                if (phases.firstTimeOccurence(phases.currentPhase))
                    email.phaseNomination();
                //page load data
                loadData(); //call method to load office position data

                //notifications
                checkForNomination();

                //set label visible
                OfficerNominate.Visible = true;
            }
            //accept1
            else if (phases.currentPhase == "accept1")
            {
                //make sure the phase can't be skipped
                if (dbLogic.canSkipPhase())
                {
                    dbLogic.killPhase("accept1");
                    Response.Redirect("home.aspx");
                }

                if (phases.firstTimeOccurence(phases.currentPhase))
                    //email notification
                    email.phaseAccept1();
                
                //notifications
                checkForNomination();

                //set label visible
                OfficerNominationAccept.Visible = true;
            }
            //slate
            else if (phases.currentPhase == "slate")
            {
                //load data
                dbLogic.selectAllBallotPositions();
                DataSet emailSet = dbLogic.getResults();
                ListViewPositions2.DataSource = emailSet;
                ListViewPositions2.DataBind();

                if (dbLogic.isUserNewVoter(dbLogic.returnUnionIDFromUsername(HttpContext.Current.User.Identity.Name)))
                    PanelSlateWrapper2.Visible = true;
                else
                {
                    PanelSlateWrapper2.Visible = false;
                    LabelFeedback2.Text = "You have already voted for this election.";
                }

                //make sure phase can't be ended, then display stateless
                if (dbLogic.checkSlateApprove())
                {
                    dbLogic.killPhase("slate");
                    Response.Redirect("home.aspx");
                }

                if (phases.firstTimeOccurence(phases.currentPhase))
                    //email notification
                    email.phaseSlate();
                

                //make label visible
                lblSlate.Visible = true;
                btnApprove.Visible = true;

            }
            //petition
            else if (phases.currentPhase == "petition")
            {
                //if this is the first time the phase has occured, then a set group of site functions occur
                if (phases.firstTimeOccurence(phases.currentPhase))
                {
                    email.phasePetition();
                    dbLogic.phase_ClearNullNominations();
                }
                //load data
                dbLogic.selectAllAvailablePositions();
                DataSet positionSet = dbLogic.getResults();
                DropDownListPostions.DataSource = positionSet;
                DropDownListPostions.DataTextField = positionSet.Tables[0].Columns[2].ToString();
                DropDownListPostions.DataBind();


                //notifications
                checkForNomination();

                //set label
                lblPetition.Visible = true;
            }
            //accept 2
            else if (phases.currentPhase == "accept2")
            {
                //make sure the phase can't be skipped
                bool canSkip = dbLogic.canSkipPhase();
                if (canSkip)
                {
                    dbLogic.killPhase("accept2");
                    Response.Redirect("home.aspx");
                }

                if (phases.firstTimeOccurence(phases.currentPhase))
                    //email notification
                    email.phaseAccept2();
                
                //notifications
                checkForNomination();

                //set label visible
                lblAccept2.Visible = true;
            }
            //approval
            else if (phases.currentPhase == "approval")
            {
                //make sure phase can't be ended, then display stateless
                bool canSkip1 = dbLogic.canSkipAdminPhase();
                if (canSkip1)
                {
                    dbLogic.killPhase("approval");
                    Response.Redirect("home.aspx");
                }

                if (phases.firstTimeOccurence(phases.currentPhase))
                {
                    //email notification
                    email.phaseApproval();

                }

                //make label visible
                OfficerStateless.Visible = true;
            }
            //voting
            else if (phases.currentPhase == "vote")
            {
                //if this is the first time the phase has occured, then a set group of site functions occur
                if (phases.firstTimeOccurence(phases.currentPhase))
                {
                    email.phaseVote();
                    dbLogic.phase_ClearNullNominations();
                }
                //load data
                //voting functionality
                dbLogic.selectAllBallotPositions();
                DataSet emailSet = dbLogic.getResults();
                ListViewPositions.DataSource = emailSet;
                ListViewPositions.DataBind();

                if (dbLogic.isUserNewVoter(dbLogic.returnUnionIDFromUsername(HttpContext.Current.User.Identity.Name)))
                {
                    PanelSlateWrapper.Visible = true;
                    foreach (ListViewDataItem myItem in ListViewPositions.Items)
                    {
                        LinkButton LinkButtonPosition = (LinkButton)myItem.FindControl("LinkButtonPostions");
                        Label votedFor = (Label)myItem.FindControl("LabelVoted");
                        Label votedForExtra = (Label)myItem.FindControl("LabelVotedExtra");
                        HiddenField votedId = (HiddenField)myItem.FindControl("HiddenFieldVotedId");

                        if (!dbLogic.IsThereCandidatesForPoisition(LinkButtonPosition.Text))
                        {
                            LinkButtonPosition.Enabled = false;
                            votedFor.Text = "No candidates available";
                            votedId.Value = "0";
                        }
                    }
                }

                else
                {
                    PanelSlateWrapper.Visible = false;
                    LabelFeedbackVote2.Text = "You have already voted for this election.";
                    LabelFeedback.Text = "You have already voted for this election.";
                }


                //no notifications for this phase

                //set label visible
                lblVoting.Visible = true;
            }

            //results
            else if (phases.currentPhase == "result")
            {
                //if this is the first time the phase has occured, then a set group of site functions occur
                if (phases.firstTimeOccurence(phases.currentPhase))
                {
                    email.phaseResults();
                }
                //make phase visible
                lblResults.Visible = true;

                //results functionality
                bindPositions();

                //this is for NEC role
                if (!dbLogic.checkNecApprove())
                {
                    necApprove.Visible = true;
                    necButton.Visible = true;
                }
            }
            //stateless
            else
            {
                //Response.Redirect("Views/NEC/stateless.aspx");
                //make label visible
                OfficerStateless.Visible = true;
            }
        }
    }

    //For server transferring with hyperlinks
    protected void transfer(object sender, CommandEventArgs e)
    {
        Response.Redirect(e.CommandArgument.ToString());
    }


    /************************************************
     * NOMINATION
     * FUNCTIONALITY
     * *********************************************/
    //load office position data
    protected void loadData()
    {
        dbLogic.selectAllAvailablePositions();
        DataSet postionsSet = dbLogic.getResults();
        GridViewPositions.DataSource = postionsSet;
        GridViewPositions.DataBind();
    }

    //gridview actions
    protected void GridViewPositions_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (String.Equals(e.CommandName, "positions"))
        {
            PanelSelected.Visible = true; //makes the panel visible to the user
            LabelSelected.Text = "<h3>Info for " + e.CommandArgument + ":</h3>" +
                                 "<p>" + dbLogic.selectDescriptionFromPositionName(e.CommandArgument.ToString()) + "</p>";
            ButtonWTS.Text = "Nominate me for " + e.CommandArgument;
            ButtonNominate.Text = "Nominate a user for " + e.CommandArgument;
            HiddenFieldID.Value = dbLogic.selectIDFromPosition(e.CommandArgument.ToString());
            ButtonWTS.Enabled = !dbLogic.isUserNominated(dbLogic.returnUnionIDFromUsername(User.Identity.Name),
                                                         e.CommandArgument.ToString()) || dbLogic.isUserWTS(dbLogic.returnUnionIDFromUsername(User.Identity.Name),
                                                                                                            e.CommandArgument.ToString());
        }
    }

    //check if the user has a nomination pending
    protected void checkForNomination()
    {
        if (dbLogic.isUserNominatedPending(dbLogic.returnUnionIDFromUsername(HttpContext.Current.User.Identity.Name)))
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

    protected void next(Object sender, EventArgs e)
    {
        Response.Redirect("/WTS.aspx?position=" + HiddenFieldID.Value);
    }

    protected void nominate(Object sender, EventArgs e)
    {
        Response.Redirect("/nominate.aspx/" + HiddenFieldID.Value);
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
            HiddenFieldName2.Value = dr["fullname"].ToString();
            HiddenFieldId2.Value = e.CommandArgument.ToString();

            if (LabelStatement2.Text == "\"\"")
                LabelStatement2.Text = dr["fullname"].ToString() + " did not include a personal statement.";

            PanelSelect2.Visible = true;
            LabelFeedback2.Text = "";
        }
    }

    protected void btnApprove_OnClick(object sender, EventArgs e)
    {
        dbLogic.approveSlate();
        Response.Redirect("home.aspx");
    }



    /********************************************
     * PETITION
     * FUNCTIONALITY
     * *****************************************/
    //Petition component code

    protected void search(object sender, EventArgs e)
    {
        dbLogic.SelectPeopleFromSearch(txtSearch.Text);
        DataSet emailSet = dbLogic.getResults();
        ListViewUsers.DataSource = emailSet;
        ListViewUsers.DataBind();
        ListViewUsers.Visible = true;

        //loadConfirmPopup();

        if (txtSearch.Text != "")
            btnViewAll.Visible = true;
    }

    protected void ListViewUsers_ItemCommand(Object sender, ListViewCommandEventArgs e)
    {
        if (String.Equals(e.CommandName, "nominate"))
        {
            LabelChoosPosition.Text = "Please select the position you would<br /> like " + dbLogic.selectFullName(e.CommandArgument.ToString()) + " to be petitioned for:";
            ButtonSubmit.OnClientClick = "return confirm('Are you sure you want to start this petition for " + dbLogic.selectFullName(e.CommandArgument.ToString()) + "?\\n(If accepted, you will not be able to withdraw your petition  later.)')";
            HiddenFieldName.Value = dbLogic.selectFullName(e.CommandArgument.ToString());
            HiddenField1.Value = e.CommandArgument.ToString();//

            PopupControlExtender1.Show();
        }
    }

    protected void clear(object sender, EventArgs e)
    {
        ListViewUsers.Visible = false;
        btnViewAll.Visible = false;
    }

    protected void ButtonSubmit_Clicked(object sender, EventArgs e)
    {
        string[] petitionInfo = { HiddenField1.Value, DropDownListPostions.SelectedItem.Text, dbLogic.returnUnionIDFromUsername(HttpContext.Current.User.Identity.Name) };//

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

        //LabelFeedback.Text = HiddenFieldId.Value + ", " + DropDownListPostions.SelectedItem.Text + ", " + dbLogic.returnUnionIDFromUsername(HttpContext.Current.User.Identity.Name) + "<br />" + dbLogic.countPetitionsForPerson(petitionInfo);

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
    //functionality for voting

    protected void ListViewPositions_ItemCommand(Object sender, ListViewCommandEventArgs e)
    {
        if (String.Equals(e.CommandName, "position"))
        {
            dbLogic.selectAllForBallot(e.CommandArgument.ToString());
            DataSet emailSet = dbLogic.getResults();
            ListViewPeople.DataSource = emailSet;
            ListViewPeople.DataBind();
            PanelPeople.Visible = true;
            PanelSelect.Visible = false;
            HiddenFieldCurrentPosition.Value = e.CommandArgument.ToString();
            LabelFeedbackVote2.Text = "Select a candidate for this position.";

            //resolves plurality error
            foreach (ListViewDataItem myItem3 in ListViewPositions.Items)
            {
                LinkButton Position = (LinkButton)myItem3.FindControl("LinkButtonPostions");
                HiddenField number = (HiddenField)myItem3.FindControl("HiddenFieldVoteNumber");
                if (dbLogic.countHowManyCandidatesForPosition(Position.Text) < Convert.ToInt16(number.Value))
                    number.Value = dbLogic.countHowManyCandidatesForPosition(Position.Text).ToString();
            }

            //turns off cadidates in personlistview if they have been selected for plurality
            foreach (ListViewDataItem myItem in ListViewPeople.Items)
            {
                LinkButton Person = (LinkButton)myItem.FindControl("LinkButtonPostions");
                foreach (ListViewDataItem myItem2 in ListViewPositions.Items)
                {
                    LinkButton LinkButtonPosition = (LinkButton)myItem2.FindControl("LinkButtonPostions");
                    Label votedFor = (Label)myItem2.FindControl("LabelVoted");
                    HiddenField allCandidates = (HiddenField)myItem2.FindControl("HiddenFieldAllCandidates");

                    if (e.CommandArgument.ToString() == LinkButtonPosition.Text)
                    {
                        string[] candidates = allCandidates.Value.Split(new char[] { '%' }); //splits querystring into variable name and value
                        for (int i = 1; i < candidates.Length; i++)
                            if (candidates[i] == Person.CommandArgument)
                                Person.Enabled = false;
                    }
                }
            }
        }
    }

    protected void ListViewPeople_ItemCommand(Object sender, ListViewCommandEventArgs e)
    {
        if (String.Equals(e.CommandName, "id"))
        {
            dbLogic.selectDetailFromWTS(e.CommandArgument.ToString());
            DataSet infoSet = dbLogic.getResults();
            DataRow dr = infoSet.Tables["query"].Rows[0];

            ButtonVote.Text = "Vote for " + dr["fullname"].ToString();
            LabelStatement.Text = "\"" + dr["statement"].ToString() + "\"";
            ButtonVote.OnClientClick = "return confirm('Are you sure you want to vote for " + dr["fullname"].ToString() + " for " + HiddenFieldCurrentPosition.Value + "?')";
            HiddenFieldName.Value = dr["fullname"].ToString();
            HiddenField2.Value = e.CommandArgument.ToString();

            if (LabelStatement.Text == "\"\"")
                LabelStatement.Text = dr["fullname"].ToString() + " did not include a personal statement.";

            PanelSelect.Visible = true;
            LabelFeedbackVote2.Text = "";
        }
    }

    protected void ButtonVote_Clicked(object sender, EventArgs e)
    {
        int votingCounter = 0;
        bool pluralityNotOccuring = true;
        foreach (ListViewDataItem myItem in ListViewPositions.Items)
        {
            LinkButton LinkButtonPosition = (LinkButton)myItem.FindControl("LinkButtonPostions");
            Label votedFor = (Label)myItem.FindControl("LabelVoted");
            Label votedForExtra = (Label)myItem.FindControl("LabelVotedExtra");
            HiddenField votedId = (HiddenField)myItem.FindControl("HiddenFieldVotedId");
            HiddenField number = (HiddenField)myItem.FindControl("HiddenFieldVoteNumber");
            HiddenField allCandidates = (HiddenField)myItem.FindControl("HiddenFieldAllCandidates");

            if (LinkButtonPosition.Text == HiddenFieldCurrentPosition.Value)
            {
                //for plurality
                if (Convert.ToInt16(number.Value) > 1)
                {
                    number.Value = (Convert.ToInt16(number.Value) - 1).ToString(); //decrements counter for plurality
                    LabelFeedbackVote2.Text = "Selection for " + HiddenFieldCurrentPosition.Value + " stored. You can select " + number.Value + " more cadidates for this position (Due to this position being tallied via \"plurality\").";
                    pluralityNotOccuring = false;
                }
                else
                    LinkButtonPosition.Enabled = false;
                
                votedFor.Text = HiddenFieldName.Value;
                votedForExtra.Text = "Selected ";
                votedId.Value = HiddenField2.Value;
                allCandidates.Value = allCandidates.Value + "%" + HiddenField2.Value;
                //LinkButtonPosition.Enabled = false;
                PanelPeople.Visible = false;
                PanelSelect.Visible = false;
            }

            if (!LinkButtonPosition.Enabled)
                votingCounter++;
        }

        int numberOfVotesLeft = ListViewPositions.Items.Count - votingCounter;

        if (votingCounter < 1 && pluralityNotOccuring)
            LabelFeedbackVote2.Text = "Selection for " + HiddenFieldCurrentPosition.Value + " stored. There are " + numberOfVotesLeft.ToString() + " more positions you can vote on before you can submit your ballot.";
        else
            ButtonSubmitVotes.Visible = true;
    }

    protected void ButtonSubmitVotes_Clicked(object sender, EventArgs e)
    {
        foreach (ListViewDataItem myItem in ListViewPositions.Items)
        {
            LinkButton LinkButtonPosition = (LinkButton)myItem.FindControl("LinkButtonPostions");
            HiddenField votedId = (HiddenField)myItem.FindControl("HiddenFieldVotedId"); //quick fix, should be changed later (too many hiddenfields)
            HiddenField allCandidates = (HiddenField)myItem.FindControl("HiddenFieldAllCandidates");

            string[] candidates = allCandidates.Value.Split(new char[] { '%' }); //splits querystring into variable name and value

            for (int i = 1; i < candidates.Length; i++)
            {
                string[] votedInfo = { candidates[i], LinkButtonPosition.Text };
                dbLogic.insertNewTally(votedInfo); //initializes tally row (if already initialized, nothing will occur)
                dbLogic.updateTally(votedInfo);
            }
        }

        //create confirmation code
        string code = System.Convert.ToBase64String(System.Web.Security.Membership.GeneratePassword(10, 0));

        //set flag that user has voted
        dbLogic.insertFlagVoted(dbLogic.returnUnionIDFromUsername(HttpContext.Current.User.Identity.Name), code);

        //hide slate
        PanelSlateWrapper.Visible = false;

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
        String query = "SELECT * FROM election_position;";
        dbLogic.genericQuerySelector(query);
        ds = dbLogic.getResults();
        for(int i = 0; i < ds.Tables[0].Rows.Count; i++)
            positions.Add(ds.Tables[0].Rows[i].ItemArray[2]);

        vc.tally();

        //binds result data
        dbLogic.getPosAndWinner();
        ds = dbLogic.getResults();
        resultList.DataSource = ds;
        resultList.DataBind();

        //if user is not admin, cannot see "view result detail" row
        if (!User.IsInRole("admin") || !User.IsInRole("nec"))
            resultList.Columns[2].Visible = false;
    }

    //sends user to 
    protected void resultList_RowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (String.Equals(e.CommandName, "position"))
            Response.Redirect("resultDetail.aspx/" + dbLogic.selectIDFromPosition(e.CommandArgument.ToString()));
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
        dbLogic.killPhase("result");
        Response.Redirect("home.aspx");
    } 
}
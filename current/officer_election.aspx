<%@ Page Title="Officer Election" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="officer_election.aspx.cs" Inherits="officer_election" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <ul class="breadcrumb">
        <li><a href="/home.aspx">Home</a> <span class="divider">/</span></li>
        <li class="active">Officer Election</li>
    </ul>
    
    <!--Notifications-->
    <asp:Panel ID="PanelNominationPending" Visible="false" runat="server">
        <div id="notifications">
            <asp:Panel id="nom_pending" class="alert alert-info" Visible="false" runat="server">
                <strong>Notification</strong>
                <p>You have a nomination pending!</p>
                <a href="/Nominations.aspx" class="btn btn-primary">View Nomination</a>
            </asp:Panel>
            <asp:Panel ID="elig_pending" class="alert" Visible="false" runat="server">
                <strong>Notification</strong>
                <p>There are eligibility forms that must be approved.</p>
                <a href="/ApproveNominations.aspx" class="btn btn-warning">View Forms</a>
            </asp:Panel>
        </div>
    </asp:Panel>
    
    <div class="page-header">
        <h1>Officer Election</h1>
    </div>
    
    <asp:Panel runat="server" ID="JulioButtonPanel" CssClass="well">
        <asp:Hyperlink ID="CancelElection" Visible="true" Text="Cancel Election"
                NavigateUrl="/Terminate.aspx" runat="server" CssClass="btn btn-danger pull-right" />
        <asp:Hyperlink ID="InitiateNewElection" Visible="false" Text="Start New Election"
                NavigateUrl="/Initiate.aspx" runat="server" CssClass="btn btn-success pull-right" />
        <p><big><strong>Current Phase:</strong> <asp:Literal ID="PhaseLiteral" Text="Inactive" runat="server" /></big></p>
        <p><asp:Literal ID="DaysRemaining" Text="No election is currently in progress." runat="server" /></p>
        <asp:Panel runat="server" ID="JulioButtonHider" CssClass="form form-inline juliobuttonbox">
            <asp:Button runat="server" ID="JulioButton" Text="Switch to Next Phase"
                    CssClass="btn btn-primary btn-small" OnClick="JulioButton_Clicked" />
            or switch to
            <asp:DropDownList runat="server" ID="JulioButtonPhase">
                <asp:ListItem Value="nominate">Nomination</asp:ListItem>
                <asp:ListItem Value="accept1">Nomination Acceptance</asp:ListItem>
                <asp:ListItem Value="slate">Slate</asp:ListItem>
                <asp:ListItem Value="petition">Petition</asp:ListItem>
                <asp:ListItem Value="accept2">Petition Acceptance</asp:ListItem>
                <asp:ListItem Value="approval">Approval</asp:ListItem>
                <asp:ListItem Value="vote">Voting</asp:ListItem>
            </asp:DropDownList>
            <asp:Button runat="server" ID="JulioButtonCustom" Text="Switch"
                    CssClass="btn" OnClick="JulioButtonCustom_Clicked" />
        </asp:Panel>
        <asp:Panel ID="FunctionsStateless" Visible="false" runat="server" CssClass="juliobuttonbox">
            <a class="btn btn-primary" href="/initiate.aspx">Initiate Officer Election</a>
        </asp:Panel>
    </asp:Panel>
    
    <asp:ScriptManager runat="server" />
    
    <asp:Panel ID="OfficerStateless" Visible="false" runat="server">
        <div class="alert alert-info">
            <strong>No Active Officer Election</strong>
            There are currently no active election phases. This could mean that there is no election running, or that there is no action required on your part at this time.
        </div>
    </asp:Panel>
    
    
    <asp:Panel ID="OfficerNominate" Visible="false" runat="server">
        <p>The officer election is currently in the <b>nomination phase</b>. During this period, you may nominate yourself or other faculty members.</p>

        <asp:Panel id="functions_nominate" style="margin-top:10px;" visible="false" runat="server">
            <a class="btn" href="/ApproveNominations.aspx">Approve Eligibility</a>
        </asp:Panel>
    
        <!-- TODO: For users with can_vote=False, do not show this section. -->
        <asp:UpdatePanel ID="NominateUpdatePanel" runat="server">
            <ContentTemplate>
                <p>The following positions are up for election:</p>
                
                <!-- List of positions -->
                <asp:GridView ID="GridViewPositions" AutoGenerateColumns="false" OnRowCommand="GridViewPositions_RowCommand" CssClass="table table-bordered" runat="server">
                     <Columns>
                        <asp:BoundField HeaderText="Postion Name" DataField="position" NullDisplayText="Unknown" />
                        <asp:BoundField HeaderText="Description" DataField="description" NullDisplayText="Unknown" />
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:Button ID="ButtonNomMe" runat="server" CommandName='nom_me'
                                    Enabled='<%#!(dbLogic.isUserNominated(user.ID, Eval("position").ToString()) || dbLogic.isUserWTS(user.ID, Eval("position").ToString())) %>'
                                    CommandArgument='<%#Eval("idelection_position") %>' Text="Nominate Me"
                                    CssClass="btn btn-primary btn-small" />
                                <asp:Button ID="ButtonNomOther" runat="server" CommandName='nom_other'
                                    CommandArgument='<%#Eval("idelection_position") %>' Text="Nominate Someone Else"
                                    CssClass="btn btn-small" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:Panel ID="OfficerNominationAccept" Visible="false" runat="server">
        <p>The officer election is currently in the <b>nomination acceptance phase</b>. This period acts as a buffer to give extra time to accept nominations.</p>
        
        <asp:Panel id="functions_accept1" style="margin-top:10px;" visible="false" runat="server">
            <a class="btn" href="/ApproveNominations.aspx">Approve Eligibility</a>
        </asp:Panel>
    </asp:Panel>
    
    <!--Slate Approval -->
    <asp:Panel ID="OfficerSlate" Visible="false" runat="server">
        <p>The officer election is currently in the <b>slate approval phase</b>. The Nominations and Elections Committee is curently reviewing the slate and providing a final approval.</p>

        <!--Admin only-->
        <asp:Panel id="functions_slate" style="margin-top:10px;" visible="false" runat="server">
            <p>This is a special phase where the NEC must approve the slate. The phase will automatically be switched when the certification has taken place.</p>
            <div class="btn-group">
                <a class="btn" href="/ApproveNominations.aspx">Approve Eligibility</a>
                <a class="btn" href="/RemoveFromBallot.aspx">Remove Candidate(s) From Slate</a>
            </div>
        </asp:Panel>

        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
            <fieldset id="nec_approval_form" runat="server" Visible="true">
                <legend>NEC Approval Form</legend>
                <p><asp:Label ID="LabelFeedback2" runat="server" Text="To view the nominated for a position, please select a position from the list below"></asp:Label></p>
            
                <p>
                    When you have reviewed the slate, you may approve the slate with this button. <asp:Button CssClass="btn btn-primary" ID="btnApprove" Visible="false" Text="Approve Slate" OnClick="btnApprove_OnClick" runat="server" />
                </p>
                <asp:Panel ID="PanelSlateWrapper2" runat="server" CssClass="well">
                    <div class="row-fluid">
                        <!-- Holds the positions in the election -->
                        <asp:Panel ID="PanelPositions2" CssClass="span4" runat="server">
                            <asp:ListView ID="ListViewPositions2"  OnItemCommand="ListViewPositions2_ItemCommand" runat="server">
                                <LayoutTemplate>
                                    <ul class="nav nav-tabs nav-stacked">
                                    <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                                    </ul>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <li><asp:LinkButton ID="LinkButtonPostions" CssClass="bold" runat="server" CommandName="position" CommandArgument='<%#Eval("position")%>' Text='<%#Eval("position")%>' /></li>
                                    <asp:HiddenField ID="HiddenFieldVotedId2" runat="server" />
                                    <asp:HiddenField ID="HiddenFieldVoteNumber" Value='<%#Eval("votes_allowed")%>' runat="server" />
                                    <asp:HiddenField ID="HiddenFieldAllCandidates" Value="" runat="server" />
                                </ItemTemplate>
                            </asp:ListView>
                        </asp:Panel>
                        <!-- Holds the people nominated for each position -->
                        <asp:Panel ID="PanelPeople2" CssClass="span4" Visible="false" runat="server">
                            <asp:ListView ID="ListViewPeople2" OnItemCommand="ListViewPeople2_ItemCommand" runat="server">
                                <LayoutTemplate>
                                    <ul class="nav nav-tabs nav-stacked">
                                    <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                                    </ul>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <li><asp:LinkButton ID="LinkButtonPostions" runat="server" CssClass="bold" CommandName="id" CommandArgument='<%#Eval("idunion_members")%>' Text='<%#GetName(int.Parse(Eval("idunion_members").ToString())) %>' /></li>
                                </ItemTemplate>
                            </asp:ListView>
                        </asp:Panel>
                        <!-- Holds submit button and the info that belows to that persion -->
                        <asp:Panel ID="PanelSelect2" CssClass="span4" Visible="false" runat="server">
                            <span style="text-decoration: underline; font-weight: bolder">
                                Their Personal Statement:
                            </span>
                            <br />
                            <asp:Label ID="LabelStatement2" runat="server" Text="" />
                            <asp:HiddenField ID="HiddenFieldName2" runat="server" />
                            <asp:HiddenField ID="HiddenFieldId2" runat="server" />
                        </asp:Panel>
                    </div>
                    <asp:HiddenField ID="HiddenFieldCurrentPosition2" runat="server" />
                </asp:Panel>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <!--End Slate Approval-->

    <!--Petition-->
    <asp:Panel ID="OfficerPetition" Visible="false" runat="server">
        <p>The election is in the <b>petition phase</b>. You can petition yourself or other faculty members for a position.</p>

        <asp:Panel id="functions_petition" style="margin-top:10px;" visible="false" runat="server">
            <div class="btn-group">
                <a class="btn" href="/ApproveNominations.aspx">Approve Eligibility</a>
                <a class="btn" href="/Slate.aspx">View current Slate</a>
                <a class="btn" href="/RemoveFromBallot.aspx">Remove Candidate(s) From Slate</a>
            </div>
        </asp:Panel>
        <div class="clear"></div>
    
        <!--The petition functionality will be active during this phase.-->
        <fieldset id="special">
            <legend>Petition Application</legend>
        
            <asp:UpdatePanel ID="UpdatePanel" runat="server" UpdateMode="Conditional">
                <Triggers>
                <asp:PostBackTrigger ControlID="ButtonSubmit" />
                </Triggers>
            <ContentTemplate>
                <div class="well form-search">
                    <p>Search for the individual you would like to submit a petition for:</p>
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="input-medium search-query"></asp:TextBox> 
                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="search" CssClass="btn" />
                    <asp:LinkButton ID="btnViewAll" runat="server" Text="Clear Search" OnClick="clear" Visible="false" CssClass="btn btn-warning" />
                </div>

                <p><asp:Label ID="LabelFeedback" runat="server" Text="" /></p>
                <table class="table table-bordered">
                    <tr>
                        <th>Full Name</th>
                        <th>Department</th>
                        <th>Actions</th>
                    </tr>
                    <asp:ListView ID="ListViewUsers" Visible="false" OnItemCommand="ListViewUsers_ItemCommand" runat="server">
                        <LayoutTemplate>
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Label ID="LabelName" text='<%#Eval("LastNAme") + ", " + Eval("FirstName") %>' runat="server" />
                                </td>
                                <td>
                                    <asp:Label ID="Label1" text='<%#Eval("Department") %>' runat="server" />
                                </td>
                                <td>
                                    <asp:Button ID="ButtonNominate" 
                                        CssClass="btn btn-mini"
                                        commandname="nominate"
                                        commandargument='<%#Eval("ID") %>' 
                                        text="Submit Petition" runat="server" />                
                                </td>
                            </tr>
                         </ItemTemplate>
                    </asp:ListView>
                </table>
            </ContentTemplate>
            </asp:UpdatePanel>

            <asp:Panel ID="PanelChoosePosition" CssClass="modal" runat="server">
                <div class="modal-header">
                    <h3>Submit Petition</h3>
                </div>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:HiddenField ID="HiddenFieldName" runat="server" />
                        <asp:HiddenField ID="HiddenField1" runat="server" />
                        <div class="modal-body">
                            <div class="control-group">
                                <p class="control-label"><asp:Label ID="LabelChoosPosition" runat="server" Text="" /></p>
                                <div class="controls">
                                    <asp:DropDownList ID="DropDownListPostions" runat="server"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button CssClass="btn btn-primary" ID="ButtonSubmit" runat="server" OnClick="ButtonSubmit_Clicked" Text="Submit Your Petition" />
                            <asp:Button CssClass="btn" ID="ButtonCancel" runat="server" OnClick="ButtonCancel_Clicked" Text="Cancel" />
                        </div>
                     </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>

            <asp:Button ID="Button1" runat="server" Text="" style="display: none" />

            <asp:ModalPopupExtender ID="PopupControlExtender1" runat="server"
                TargetControlID="Button1"
                PopupControlID="PanelChoosePosition"
                CancelControlID="ButtonCancel"
                BackgroundCssClass="modalBackground"
                PopupDragHandleControlID="PanelChoosePosition" />
        </fieldset>
    </asp:Panel>
    <!--End Petition-->

    <!--Acceptance 2 phase-->
    <asp:Panel ID="OfficerPetitionAccept" Visible="false" runat="server">
        <p>The election is in a <b>petition acceptance period</b>. This period acts as a buffer to give extra time to accept petition-based nominations.</p>

        <!--Admin only-->
        <asp:Panel id="functions_accept2" style="margin-top:10px;" visible="false" runat="server">
            <div class="btn-group">
                <a class="btn" href="/ApproveNominations.aspx">Approve Eligibility</a>
                <a class="btn" href="/Slate.aspx">View current Slate</a>
            </div>
        </asp:Panel>
    </asp:Panel>

    <!--end acceptance 2-->

    <!--Approval-->
        <!--Only admin will see this phase-->
    <asp:Label ID="OfficerApproval" runat="server" Visible="false">
        <p>This phase that will end as soon as all eligibility forms have been approved or disapproved.</p>

        <!--Admin only-->
        <asp:Panel id="functions_approval" visible="false" runat="server">
            <div class="btn-group">
                <a class="btn" href="/ApproveNominations.aspx">Approve Eligibility</a>
                <a class="btn" href="/Slate.aspx">View current Slate</a>
                <a class="btn" href="/RemoveFromBallot.aspx">Remove Candidate(s) From Slate</a>
            </div>
        </asp:Panel>
    </asp:Label>
    <!--End Approval-->

    <!--Voting-->
    <asp:Panel ID="OfficerVoting" visible="false" runat="server">
        <p>The election is in the <b>voting phase</b>. You must vote for the candidate you feel will best serve in each position.</p>

        <asp:Panel id="functions_voting" style="margin-top:10px;" visible="false" runat="server">
            <a class="btn" href="/RemoveFromBallot.aspx">Remove Candidate(s) From Slate</a>
        </asp:Panel>
    
        <!--The voting functionality will be available for this phase-->
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                
                
                <p><asp:Literal ID="LabelFeedbackVote2" runat="server" /></p>
                <div>
                    <asp:Button ID="ButtonSubmitVotes" Visible="false" runat="server" OnClick="ButtonSubmitVotes_Clicked" Text="Submit Final Ballot" CssClass="btn btn-primary" />
                </div>
                <asp:Panel ID="PanelSlateWrapper" runat="server" CssClass="well">
                    <p>
                        Select each of the officer positions below. It will show
                        you a list of candidates for each position. Clicking each
                        candidate will show his or her statement. When you have
                        made your choice, press the <b>Vote</b> button under the
                        candidate's name. Repeat this process for each officer
                        position.
                    </p>
                    <p>When you have finished voting, press the Submit button.</p>
                <div class="row-fluid">
                    <!-- Holds the positions in the election -->
                    <asp:Panel ID="PanelPositions" CssClass="span4" runat="server">
                        <asp:ListView ID="ListViewPositions"  OnItemCommand="ListViewPositions_ItemCommand" runat="server">
                            <LayoutTemplate>
                                <ul class="nav nav-list">
                                    <li class="nav-header">Positions</li>
                                    <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                                </ul>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <li>
                                    <asp:LinkButton ID="LinkButtonPostions" runat="server" CommandName="position" CommandArgument='<%#Eval("position")%>' Text='<%#Eval("position")%>' />
                                    <asp:Label ID="LabelVotedExtra" runat="server" Text=""></asp:Label>
                                    <asp:Label ID="LabelVoted" runat="server" Text=""></asp:Label>
                                </li>
                                <asp:HiddenField ID="HiddenFieldVotedId" runat="server" />
                                <asp:HiddenField ID="HiddenFieldVoteNumber" Value='<%#Eval("votes_allowed")%>' runat="server" />
                                <asp:HiddenField ID="HiddenFieldAllCandidates" Value="" runat="server" />
                            </ItemTemplate>
                        </asp:ListView>
                    </asp:Panel>
                    <!-- Holds the people nominated for each position -->
                    <asp:Panel ID="PanelPeople" CssClass="span4" Visible="false" runat="server">
                        <asp:ListView ID="ListViewPeople" OnItemCommand="ListViewPeople_ItemCommand" runat="server">
                            <LayoutTemplate>
                                <ul class="nav nav-list">
                                    <li class="nav-header">Candidates for Position</li>
                                    <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                                </ul>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <li>
                                    <asp:LinkButton ID="LinkButtonPostions" runat="server" CommandName="id" CommandArgument='<%#Eval("idunion_members")%>' Text='<%#GetName(int.Parse(Eval("idunion_members").ToString())) %>' />
                                </li>
                            </ItemTemplate>
                        </asp:ListView>
                    </asp:Panel>
                    <!-- Holds submit button and the info that belows to that persion -->
                    <asp:Panel ID="PanelSelect" CssClass="span4 form form-horizontal" Visible="false" runat="server">
                        <h4>Candidate's Statement</h4>
                        <p><asp:Literal ID="LabelStatement" runat="server" Text="" /></p>
                        <p>
                            <asp:Button ID="ButtonVote" OnClick="ButtonVote_Clicked" runat="server" Text="Vote for This Person" CssClass="btn btn-primary" />
                        </p>
                        
                        <asp:HiddenField ID="HiddenField2" runat="server" />
                        <asp:HiddenField ID="HiddenField3" runat="server" />
                    </asp:Panel>
                    <asp:HiddenField ID="HiddenFieldCurrentPosition" runat="server" />
                </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel><!---->
    </asp:Panel>   
    <!--End Voting-->

    <!--Results-->
    <asp:Panel ID="OfficerResults" Visible="false" style="text-align: center;" runat="server">
        <h1>Results of the Election</h1><br />
        
        <asp:GridView ID="resultList" CssClass="simpleGrid" OnRowCommand="resultList_RowCommand" style="margin-left: auto; margin-right: auto;" AutoGenerateColumns="false" runat="server">
        <Columns>
            <asp:BoundField HeaderText="Position" DataField="position" />
            <asp:BoundField HeaderText="Winner" DataField="idunion_members" />
            <asp:TemplateField HeaderText="" >
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButtonPositionDetail" commandname="position"  commandargument='<%#Eval("idelection_position") %>' text="View Position Result Data" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        </asp:GridView>

        <center><br />
        <asp:Label ID="necApprove" runat="server" Text="<b>Please approve the results above from this past election.</b>" Enabled="false" Visible="false" /><br />
        <asp:Button ID="necButton" runat="server" Text="Approve" Visible="false" OnClick="necButton_OnClick" />
        <asp:Label ID="adminEnd" runat="server" Text="<b>The NEC must approve the election results before you can end the current election.</b>" Enabled="false" Visible="false" /><br />
        <asp:Button ID="adminButton" runat="server" Text="Offically End This Election" Visible="false" Enabled="false" OnClick="adminButton_OnClick" />
        </center>
    
    </asp:Panel>
    <!--End Results-->
</asp:Content>


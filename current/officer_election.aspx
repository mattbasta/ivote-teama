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
                <a href="/approvenominations.aspx" class="btn btn-warning">View Forms</a>
            </asp:Panel>
        </div>
    </asp:Panel>
    
    <div class="page-header">
        <h1>Officer Election</h1>
    </div>
    
    <asp:Panel runat="server" ID="JulioButtonPanel" CssClass="well">
        <asp:Hyperlink ID="CancelElection" Visible="true" Text="Cancel Election"
                NavigateUrl="/terminate.aspx" runat="server" CssClass="btn btn-danger pull-right" />
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
            <a class="btn" href="/approvenominations.aspx">Approve Eligibility</a>
        </asp:Panel>
    
        <!-- TODO: For users with can_vote=False, do not show this section. -->
        <p>Click <b>Select</b> next to a position below to see more information or nominate yourself for that position.</p>
        <asp:UpdatePanel ID="NominateUpdatePanel" runat="server">
            <ContentTemplate>
                <!--Display form/data for selected position -->
                <asp:Panel ID="PanelSelected" CssClass="well" Visible="false" runat="server">
                    <asp:Label ID="LabelSelected" runat="server" Text="" />
                    <p class="buttons"> 
                        <asp:Button CssClass="btn" ID="ButtonNominate" runat="server" OnClick="nominate" Text="" />
                        <asp:Button CssClass="btn" ID="ButtonWTS" runat="server" Text="" OnClick="next" /> 
                    </p>
                    <asp:HiddenField ID="HiddenFieldID" runat="server" />
                </asp:Panel>
                
                <!-- List of positions -->
                <asp:GridView ID="GridViewPositions" AutoGenerateColumns="false" OnRowCommand="GridViewPositions_RowCommand" CssClass="table table-bordered" runat="server">
                     <Columns>
                        <asp:BoundField HeaderText="Postion Name" DataField="position" NullDisplayText="Unknown" />
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:Button ID="ButtonSelect" runat="server" CommandName='<%#Eval("position") %>' Text="Select" CssClass="btn btn-small" />
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
        <div class="buttonrow">
            <a class="btn" href="/approvenominations.aspx">Approve Eligibility</a>
        </div>
        </asp:Panel>
    </asp:Panel>
    
    <!--Slate Approval -->
    <asp:Panel ID="OfficerSlate" Visible="false" runat="server">
        <div id="bodyCopy">
            <h1>Slate Approval</h1>
            NEC Members:  Please review and approve the slate below.  If there is a strong reason not to approve the slate, 
            contact the administrator with an explaination and have them cancel the election.
        </div>

        <!--Admin only-->
        <asp:Panel id="functions_slate" style="margin-top:10px;" visible="false" runat="server">
            <b>This is a special phase where the NEC must approve the slate.</b><br /><br />
            <div class="clear"></div>
            <div class="column">
                <b>Election Management</b><br />
                <a href="approvenominations.aspx">Approve Eligibility</a><br />
                <a href="removeFromBallot.aspx">Remove Candidate(s) From Slate</a>
                <br />
            </div>
        </asp:Panel>

        <div class="clear"></div>

        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <div style="color: Blue; padding-bottom: 6px;"><asp:Label ID="LabelFeedback2" runat="server" Text="To view the nominated for a position, please select a position from the list below"></asp:Label></div>
            
                <asp:Panel ID="PanelSlateWrapper2" runat="server">
                    <div id="slateWrapper2" style="width: 710px; height: 385px; background-color: #FF9999;">
                        <!-- Holds the positions in the election -->
                        <asp:Panel ID="PanelPositions2" CssClass="slateListPositions" runat="server">
                            <asp:ListView ID="ListViewPositions2"  OnItemCommand="ListViewPositions2_ItemCommand" runat="server">
                                <LayoutTemplate>
                                    <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                                </LayoutTemplate>
                                <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonPostions" CssClass="bold" runat="server" CommandName="position" CommandArgument='<%#Eval("position")%>' Text='<%#Eval("position")%>' /><br />
                                    <span style="font-size: 12px; font-weight: bold; color: #808080">
                                        <asp:Label ID="LabelVotedExtra2" runat="server" Text=""></asp:Label>
                                        <asp:Label ID="LabelVoted2" runat="server" Text=""></asp:Label>
                                        <asp:HiddenField ID="HiddenFieldVotedId2" runat="server" />
                                        <asp:HiddenField ID="HiddenFieldVoteNumber" Value='<%#Eval("votes_allowed")%>' runat="server" />
                                        <asp:HiddenField ID="HiddenFieldAllCandidates" Value="" runat="server" />
                                    </span>
                                    <hr />
                                </ItemTemplate>
                            </asp:ListView>
                        </asp:Panel>
                        <!-- Holds the people nominated for each position -->
                        <asp:Panel ID="PanelPeople2" CssClass="slateListPeople" Visible="false" runat="server">
                            <asp:ListView ID="ListViewPeople2" OnItemCommand="ListViewPeople2_ItemCommand" runat="server">
                                 <LayoutTemplate>
                                    <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButtonPostions" runat="server" CssClass="bold" CommandName="id" CommandArgument='<%#Eval("idunion_members")%>' Text='<%#Eval("fullname")%>' /><br /><hr />
                                </ItemTemplate>
                            </asp:ListView>
                        </asp:Panel>
                        <!-- Holds submit button and the info that belows to that persion -->
                        <asp:Panel ID="PanelSelect2" CssClass="slateDetailPeople" Visible="false" runat="server">
                            <span style="text-decoration: underline; font-weight: bolder">
                                Their Personal Statement:
                            </span>
                            <br />
                            <div style="overflow: auto; width: 280px; height: 290px;">
                                <asp:Label ID="LabelStatement2" runat="server" Text="" />
                            </div>
                            <asp:HiddenField ID="HiddenFieldName2" runat="server" />
                            <asp:HiddenField ID="HiddenFieldId2" runat="server" />
                        </asp:Panel>
                    </div>
                    <br />
                    <br />
                    <asp:HiddenField ID="HiddenFieldCurrentPosition2" runat="server" />
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
           <asp:Button ID="btnApprove" Enabled="false" Visible="false" Text="Approve Slate" OnClick="btnApprove_OnClick" runat="server" />
    </asp:Panel>

    <!--End Slate Approval-->

    <!--Petition-->
    <asp:Panel ID="OfficerPetition" Visible="false" runat="server">
        <div id="bodyCopy">
            <h1>Petition Period</h1>
            The current election is in the petition phase.  You can petition yourself or other faculty 
            members for a position.
        </div>
        <asp:Panel id="functions_petition" style="margin-top:10px;" visible="false" runat="server">
            <div class="column">
                <b>Election Management</b><br />
                <a href="approvenominations.aspx">Approve Eligibility</a><br />
                <a href="slate.aspx">View current Slate</a><br />
                <a href="removeFromBallot.aspx">Remove Candidate(s) From Slate</a>
                <br />
            </div>
        </asp:Panel>
        <div class="clear"></div>
    
        <!--The petition functionality will be active during this phase.-->
        <div id="special">
        
            <asp:UpdatePanel ID="UpdatePanel" runat="server" UpdateMode="Conditional">
            <Triggers>
            <asp:PostBackTrigger ControlID="ButtonSubmit" />
            </Triggers>
            <ContentTemplate>

            <br />
            Search for the individual you would like to submit a petition for:<br />
            <asp:TextBox ID="txtSearch" runat="server" Width=300></asp:TextBox> 
            <asp:Button ID="btnSearch"  runat="server" Text="Search" OnClick="search" /> 
            <asp:LinkButton ID="btnViewAll"   runat="server" Text="Clear" OnClick="clear" Visible="false" /> <br /><br />
            <span style="color: Blue"><asp:Label ID="LabelFeedback" runat="server" Text="" /></span><br />

                <table class="simpleGrid" style="width: 60%">
                    <tr>
                        <th>Full Name</th>
                        <th>Department</th>
                        <th></th>
                    </tr>
                    <asp:ListView ID="ListViewUsers" Visible="false" OnItemCommand="ListViewUsers_ItemCommand" runat="server">
                        <LayoutTemplate>
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td >
                                    <asp:Label ID="LabelName" text='<%#Eval("last_name") + ", " + Eval("first_name") %>' runat="server" />
                                </td>
                                <td >
                                    <asp:Label ID="Label1" text='<%#Eval("department") %>' runat="server" />
                                </td>
                                <td >
                                   <asp:Button ID="ButtonNominate" 
                                       commandname="nominate"
                                       commandargument='<%#Eval("idunion_members") %>' 
                                       text="Submit Petition" runat="server" />                
                                </td>
                            </tr>
                         </ItemTemplate>
                    </asp:ListView>
                </table>
            </ContentTemplate>
                    </asp:UpdatePanel>

                <asp:Panel ID="PanelChoosePosition" CssClass="modalPopup" runat="server">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="LabelChoosPosition" runat="server" Text="" /><br /><br />
                            <asp:HiddenField ID="HiddenFieldName" runat="server" />
                            <asp:HiddenField ID="HiddenField1" runat="server" />
                            <asp:DropDownList ID="DropDownListPostions" runat="server">
                            </asp:DropDownList><br /> <br />
                            <asp:Button ID="ButtonSubmit" runat="server" OnClick="ButtonSubmit_Clicked" Text="Submit Your Petition" />
                            <asp:Button ID="ButtonCancel" runat="server" OnClick="ButtonCancel_Clicked" Text="Cancel" />
                         </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>

                <asp:Button ID="Button1" runat="server" Text="" style="display: none" />

                <asp:ModalPopupExtender ID="PopupControlExtender1" runat="server"
                    TargetControlID="Button1"
                    PopupControlID="PanelChoosePosition"
                    CancelControlID="ButtonCancel"
                    BackgroundCssClass="modalBackground"
                    PopupDragHandleControlID="PanelChoosePosition"
                />
        </div>
    </asp:Panel>
    <!--End Petition-->

    <!--Acceptance 2 phase-->
    <asp:Panel ID="OfficerPetitionAccept" Visible="false" runat="server">
        <div id="bodyCopy">
            <h1>Petition Acceptance Period</h1>
            The current election is in a petition acceptance period.  This period acts as a buffer to give extra time to accept petition-based nominations.
        </div>

        <!--Admin only-->
        <asp:Panel id="functions_accept2" style="margin-top:10px;" visible="false" runat="server">
            <div class="column">
                <b>Election Management</b><br />
                <a href="approvenominations.aspx">Approve Eligibility</a><br />
                <br />
            </div>
        </asp:Panel>
    </asp:Panel>

    <!--end acceptance 2-->

    <!--Approval-->
        <!--Only admin will see this phase-->
    <asp:Label ID="OfficerApproval" runat="server" Visible="false">
        <div id="bodyCopy">
            <h1>Eligibility Approval Period</h1>
            This is a special phase that will end as soon as all eligibility forms have been approved or disapproved.
        </div>

        <!--Admin only-->
        <asp:Panel id="functions_approval" style="margin-top:10px;" visible="false" runat="server">
            <a href="approvenominations.aspx">Approve Eligibility</a><br />
            <a href="removeFromBallot.aspx">Remove Candidate(s) From Slate</a>
        </asp:Panel>
    </asp:Label>
    <!--End Approval-->

    <!--Voting-->
    <asp:Panel ID="OfficerVoting" visible="false" runat="server">

        <div id="bodyCopy">
            <h1>Voting Period</h1>
            The current election is in the voting phase.  You must vote for the candidate you feel will best serve in each position.
        </div>
        <asp:Panel id="functions_voting" style="margin-top:10px;" visible="false" runat="server">
            <a href="removeFromBallot.aspx">Remove Candidate(s) From Slate</a>
        </asp:Panel>
        <div class="clear"></div>
    
        <!--The voting functionality will be available for this phase-->
        <div id="special">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <div style="color: Blue; padding-bottom: 6px;"><asp:Label ID="LabelFeedbackVote2" runat="server" Text="To begin the voting process, please select a position from the list below"></asp:Label></div>
            
                <asp:Panel ID="PanelSlateWrapper" runat="server">
                    <div id="slateWrapper" style="width: 710px; height: 385px; background-color: #FF9999;">
                        <!-- Holds the positions in the election -->
                        <asp:Panel ID="PanelPositions" CssClass="slateListPositions" runat="server">
                            <asp:ListView ID="ListViewPositions"  OnItemCommand="ListViewPositions_ItemCommand" runat="server">
                                <LayoutTemplate>
                                    <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                                </LayoutTemplate>
                                <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonPostions" CssClass="bold" runat="server" CommandName="position" CommandArgument='<%#Eval("position")%>' Text='<%#Eval("position")%>' /><br />
                                    <span style="font-size: 12px; font-weight: bold; color: #808080">
                                        <asp:Label ID="LabelVotedExtra" runat="server" Text=""></asp:Label>
                                        <asp:Label ID="LabelVoted" runat="server" Text=""></asp:Label>
                                        <asp:HiddenField ID="HiddenFieldVotedId" runat="server" />
                                        <asp:HiddenField ID="HiddenFieldVoteNumber" Value='<%#Eval("votes_allowed")%>' runat="server" />
                                        <asp:HiddenField ID="HiddenFieldAllCandidates" Value="" runat="server" />
                                    </span>
                                    <hr />
                                </ItemTemplate>
                            </asp:ListView>
                        </asp:Panel>
                        <!-- Holds the people nominated for each position -->
                        <asp:Panel ID="PanelPeople" CssClass="slateListPeople" Visible="false" runat="server">
                            <asp:ListView ID="ListViewPeople" OnItemCommand="ListViewPeople_ItemCommand" runat="server">
                                 <LayoutTemplate>
                                    <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButtonPostions" runat="server" CssClass="bold" CommandName="id" CommandArgument='<%#Eval("idunion_members")%>' Text='<%#Eval("fullname")%>' /><br /><hr />
                                </ItemTemplate>
                            </asp:ListView>
                        </asp:Panel>
                        <!-- Holds submit button and the info that belows to that persion -->
                        <asp:Panel ID="PanelSelect" CssClass="slateDetailPeople" Visible="false" runat="server">
                            <br />
                            <div style="margin-right: auto; margin-left: auto; width: 290px; text-align: center;"><asp:Button ID="ButtonVote" OnClick="ButtonVote_Clicked" runat="server" Text="Vote for This Person" /></div>
                            <br />
                            <span style="text-decoration: underline; font-weight: bolder">
                                Their Personal Statement:
                            </span>
                            <br />
                            <div style="overflow: auto; width: 280px; height: 290px;">
                                <asp:Label ID="LabelStatement" runat="server" Text="" />
                            </div>
                            <asp:HiddenField ID="HiddenField2" runat="server" />
                            <asp:HiddenField ID="HiddenField3" runat="server" />
                        </asp:Panel>
                    </div>
                    <br />
                    <div style="width: 100%; text-align:center;">
                        <asp:Button ID="ButtonSubmitVotes" Visible="false" runat="server" OnClick="ButtonSubmitVotes_Clicked" Text="Submit Your Completed Ballot For Processing" />
                    </div>
                    <br />
                    <asp:HiddenField ID="HiddenFieldCurrentPosition" runat="server" />
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        </div>
    </asp:Panel>   
    <!--End Voting-->

    <!--Results-->
    <asp:Panel ID="OfficerResults" Visible="false" style="text-align: center;" runat="server">
            <h1>Results of the Election</h1><br />
            
            <asp:GridView ID="resultList" CssClass="simpleGrid" OnRowCommand="resultList_RowCommand" style="margin-left: auto; margin-right: auto;" AutoGenerateColumns="false" runat="server">
            <Columns>
                <asp:BoundField HeaderText="Position" DataField="position" />
                <asp:BoundField HeaderText="Winner" DataField="fullname" />
                <asp:TemplateField HeaderText="" >
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButtonPositionDetail" commandname="position"  commandargument='<%#Eval("position") %>' text="View Position Result Data" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            </asp:GridView>

            <center><br />
            <asp:Label ID="necApprove" runat="server" Text="<b>Please approve the results above from this past election.</b>" Enabled="false" Visible="false" /><br />
            <asp:Button ID="necButton" runat="server" Text="Approve" Visible="false" Enabled="false" OnClick="necButton_OnClick" />
            <asp:Label ID="adminEnd" runat="server" Text="<b>The NEC must approve the election results before you can end the current election.</b>" Enabled="false" Visible="false" /><br />
            <asp:Button ID="adminButton" runat="server" Text="Offically End This Election" Visible="false" Enabled="false" OnClick="adminButton_OnClick" />
            </center>
    
    </asp:Panel>
    <!--End Results-->
</asp:Content>


<%@ Page Title="Committee Elections" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="True" Inherits="committee_election" CodeFile="committee_election.aspx.cs" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit"%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <ul class="breadcrumb">
        <li><a href="/home.aspx">Home</a> <span class="divider">/</span></li>
        <li class="active">Committee Election</li>
    </ul>
    
    <div class="page-header">
        <h1><asp:Literal ID="CommitteeNameLiteral" runat="server" /> Election</h1>
    </div>
    
    <asp:Panel runat="server" ID="JulioButtonPanel" CssClass="well" Visible="false">
        <asp:LinkButton ID="CancelElection" Visible="true" Text="Cancel Election"
                NavigateUrl="#" runat="server" CssClass="btn btn-danger pull-right" />
        <p><big><strong>Current Phase:</strong> <asp:Literal ID="PhaseLiteral" Text="Inactive" runat="server" /></big></p>
        <p><asp:Literal ID="DaysRemaining" Text="No election is currently in progress." runat="server" /></p>
        <asp:Panel runat="server" ID="JulioButtonHider" CssClass="form form-inline juliobuttonbox">
            <asp:Button runat="server" ID="JulioButton" Text="Switch to Next Phase"
                    CssClass="btn btn-primary btn-small" OnClick="JulioButton_Clicked" />
            or switch to
            <asp:DropDownList runat="server" ID="JulioButtonPhase">
                <asp:ListItem Value="WTSPhase">WTS</asp:ListItem>
                <asp:ListItem Value="NominationPhase">Primary Election</asp:ListItem>
                <asp:ListItem Value="VotePhase">Voting</asp:ListItem>
                <asp:ListItem Value="CertificationPhase">Certification</asp:ListItem>
                <asp:ListItem Value="ConflictPhase">Conclift Resolution</asp:ListItem>
            </asp:DropDownList>
            <asp:Button runat="server" ID="JulioButtonCustom" Text="Switch"
                    CssClass="btn" OnClick="JulioButtonCustom_Clicked" />
        </asp:Panel>

        <asp:Literal ID="DeltaLabel" Text="Adjust this phase's deadline by" runat="server" />
        <asp:TextBox ID="DeltaText" runat="server" TextMode="SingleLine" MaxLength="2"  />
        <asp:NumericUpDownExtender ID="Delta" runat="server" TargetControlID="DeltaText" Width="55" Maximum="14" Minimum="-14" />
        <asp:Literal ID="DayLabel" Text="days past the original deadline." runat="server" />
        <asp:Button ID="DeltaSubmit" Text="Delay/Rush" runat="server" 
         CssClass="btn btn-primary btn-small" OnClick="DeltaSubmit_Click" />

    </asp:Panel>
    <asp:ToolkitScriptManager ID="AJAXManager" runat="Server" />

    <asp:Panel ID="AdminTabs" runat="server" Visible="false">
        <div class="tabbable">
            <ul class="nav nav-tabs">
                <li runat="server" ID="wts_tab" Visible="false">
                    <asp:LinkButton Text="Willingness to Serve" runat="server"
                            OnClick="Tab_Clicked" CommandName="WTSPhase" />
                </li>
                <li runat="server" ID="nominations_tab" Visible="false">
                    <asp:LinkButton Text="Primary Nominations" runat="server"
                            OnClick="Tab_Clicked" CommandName="NominationPhase" />
                </li>
                <li runat="server" ID="votes_tab" Visible="false">
                    <asp:LinkButton Text="Votes" runat="server"
                            OnClick="Tab_Clicked" CommandName="VotePhase" />
                </li>
                <li runat="server" ID="certifications_tab" Visible="false">
                    <asp:LinkButton Text="NEC Certifications" runat="server"
                            OnClick="Tab_Clicked" CommandName="CertificationPhase"
                            ID="certifications_tab_link" />
                </li>
                <li runat="server" ID="conflicts_tab" Visible="false">
                    <asp:LinkButton Text="Election Conflicts" runat="server"
                            OnClick="Tab_Clicked" CommandName="ConflictPhase" />
                </li>
                <li runat="server" ID="closed_tab" Visible="false">
                    <asp:LinkButton Text="Final Results" runat="server"
                            OnClick="Tab_Clicked" CommandName="ClosedPhase" />
                </li>
            </ul>
        </div>
    </asp:Panel>

    <asp:Panel ID="AdminWTSPanel" runat="server" Visible="false">
        <fieldset>
            <legend>Revoke Willingness to Serve</legend>
            <asp:Panel ID="wtsAdminConfirm" runat="server" Visible="false">
                <strong>WTS Revoked</strong>
                User's willingness to serve has been revoked.
            </asp:Panel>
            <asp:Panel ID="wtsAdminList" runat="server">
                <asp:Table ID="wtsAdminTable" runat="server" CssClass="table table-bordered">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell>Name</asp:TableHeaderCell>
                        <asp:TableHeaderCell>Department</asp:TableHeaderCell>
                        <asp:TableHeaderCell>Actions</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </asp:Panel>
        </fieldset>
    </asp:Panel>

    <asp:Panel ID="AdminNominationsPanel" runat="server" Visible="false">
        <fieldset>
            <legend>Review Primary Election Nominations</legend>
            <asp:Table ID="AdminNominationsTable" CssClass="table table-bordered" runat="server">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell>Name</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Votes</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Will Be Candidate?</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </fieldset>
    </asp:Panel>

    <asp:Panel ID="AdminVotingPanel" runat="server" Visible="false">
        <fieldset>
            <legend>Review Vote Tally</legend>
            <asp:Table ID="AdminVotingTable" CssClass="table table-bordered" runat="server">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell>User Name</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Votes</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </fieldset>
    </asp:Panel>

    <asp:Panel ID="NECCertificationPanel" runat="server" Visible="false" CssClass="form form-horizontal">
        <fieldset>
            <legend>Review Vote Tally</legend>
            <asp:Label ID="NECCertificationComplete" runat="server" Visible="false" CssClass="alert alert-success"
                Text="Thank you for certifying the election results!"/>
            <asp:Table ID="NECVotingTable" CssClass="table table-bordered" runat="server">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell>User Name</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Votes</asp:TableHeaderCell>
                </asp:TableHeaderRow>            
            </asp:Table>
            <div id="NECCertifyAgreement" runat="server">
                <p>As a member of the NEC, I certify that these election results are valid and have been properly collected.</p>
                <div class="control-group">
                    <label class="control-label">Confirm</label>
                    <div class="controls">
                        <label class="checkbox">
                            <asp:CheckBox ID="CertifyCheckBox" Checked="false" runat="server" />
                            I hereby certify this election's results.
                        </label>
                        <asp:Label ID="CertifyWarning" Text="Please tick the certification check box before submitting your certification!"
                            CssClass="help-block error" runat="server" Visible="false" />
                    </div>
                </div>
                <div class="form-actions">
                    <asp:Button ID="CertifyButton" Text="Submit Certification"
                        runat="server" OnClick="Certify_Click" CssClass="btn btn-primary" />
                </div>
            </div>
        </fieldset>
    </asp:Panel>


    <asp:Panel ID="AdminCertificationPanel" runat="server" Visible="false">
        <fieldset>
            <legend>Review NEC Certification Activity</legend>
            <p><asp:Label ID="AdminCertCount" Visible="true" runat="server" /></p>
            <div class="progress progress-success">
                <div class="bar" ID="necprogressbar" style="width:0;" runat="server"></div>
            </div>
        </fieldset>
    </asp:Panel>

    <asp:Panel ID="AdminConflictPanel" runat="server" Visible="false">
        <asp:Label ID="AdminNoConflicts" Visible="false" Text="There are no conflicts to resolve." runat="server" />
    </asp:Panel>

    <asp:Panel ID="AdminClosedPanel" runat="server" Visible="false" CssClass="alert">
        <strong>Election Closed</strong>
        This election is currently closed. You can review the vote counts on the <b>Votes</b> tab.
    </asp:Panel>
    
    
    <asp:Panel ID="FacultyClosed" runat="server" Visible="false" CssClass="alert">
        <strong>Election Closed</strong>
        This election is currently closed. Thank you for participating.
    </asp:Panel>

    <asp:Panel ID="FacultyWTS" runat="server" Visible="false" CssClass="form form-horizontal">
        <fieldset>
            <legend>Willingness to Serve</legend>
            <asp:Panel ID="wtsPanelNew" runat="server">
                <div class="control-group">
                    <label class="control-label">Statement</label>
                    <div class="controls">
                        <asp:TextBox ID="wtsStatement" runat="server" TextMode="MultiLine" CssClass="input-xlarge" />
                        <p class="help-block">Give a brief statement about your willingness to serve in this committee.</p>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label">Confirm</label>
                    <div class="controls">
                        <label class="checkbox">
                            <asp:CheckBox ID="wtsConfirm" runat="server" ValidationGroup="wts" />
                            I confirm that I am willing to serve as an APSCUF Official.
                        </label>
                        <asp:CustomValidator ID="wtsAcceptValidator" runat="server" 
                            ErrorMessage="Please confirm your willingness to server by checking the box above." 
                            onservervalidate="wtsAcceptValidator_ServerValidate"
                            CssClass="help-block error"/>
                    </div>
                </div>
                <div class="form-actions">
                    <asp:Button ID="wtsSubmit" runat="server" Text="Submit" 
                        onclick="wtsSubmit_Click" CssClass="btn btn-primary" />
                </div>
            </asp:Panel>
            <asp:Panel ID="wtsPanelExisting" runat="server" Visible="false" CssClass="alert alert-info">
                <strong>Willingness to Serve Submitted</strong>
                You have already submitted a willing to server form for this election.
            </asp:Panel>
            <asp:Panel ID="wtsPanelDone" runat="server" Visible="false" CssClass="alert alert-success">
                <strong>Willingness to Serve Submitted</strong>
                Your willingness to server has been successfully registered.
            </asp:Panel>
        </fieldset>
    </asp:Panel>

    <asp:Panel ID="FacultyNomination" runat="server" Visible="false" CssClass="form form-horizontal">
        <fieldset>
            <legend>Primary Election</legend>
            <p>Please cast your vote in the primary election for one of the following candidates.</p>
            <div class="control-group">
                <label class="control-label">Candidates</label>
                <div class="controls" ID="FacultyNominationList" runat="server"></div>
            </div>
            <div class="form-actions">
                <asp:Button ID="FacultyCastNomination" Text="Cast Vote" runat="server"
                    OnClick="FacultyCastNomination_Click" CssClass="btn btn-primary" />
            </div>
        </fieldset>
    </asp:Panel>

    <asp:Panel ID="FacultyNominationComplete" runat="server" Visible="false">
        <div class="alert alert-success">
            Thank you for placing your vote in the primary election!
        </div>
    </asp:Panel>

    <asp:Panel ID="FacultyVote" runat="server" Visible="false" CssClass="form form-horizontal">
        <fieldset>
            <legend>General Election</legend>
            <p>Please cast your vote in the final election for one of the following nominees:</p>
            <div class="control-group">
                <label class="control-label">Nominees</label>
                <div class="controls">
                    <asp:RadioButtonList ID="FacultyVoteList" runat="server" ></asp:RadioButtonList>
                </div>
            </div>
            <div class="form-actions">
                <asp:Button ID="FacultyCastVote" Text="Cast Vote" runat="server" OnClick="FacultyCastVote_Click"
                    CssClass="btn btn-primary"/>
            </div>
            
        </fieldset>
    </asp:Panel>

    <asp:Panel ID="FacultyVoteComplete" runat="server" Visible="false">
        <div class="alert alert-success">
            Thank you for placing your vote in the general election!
        </div>
    </asp:Panel>

</asp:Content>
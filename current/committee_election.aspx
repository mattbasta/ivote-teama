<%@ Page Title="Committee Elections" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="True" Inherits="committee_election" CodeFile="committee_election.aspx.cs" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit"%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <ul class="breadcrumb">
        <li><a href="/home.aspx">Home</a> <span class="divider">/</span></li>
        <li class="active">Committee Elections</li>
    </ul>
    
    <div class="page-header">
        <h1>Committee Elections</h1>
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
                            OnClick="Tab_Clicked" CommandName="CertificationPhase" />
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
        Admin WTS Panel
        <asp:Panel ID="wtsAdminList" runat="server">
                <asp:Table ID="wtsAdminTable" runat="server">
                </asp:Table>
        </asp:Panel>
        <asp:Panel ID="wtsAdminConfirm" runat="server" Visible="false">
            Users willingness to server has been revoked.
        </asp:Panel>
    </asp:Panel>

    <asp:Panel ID="AdminNominationsPanel" runat="server" Visible="false">
        <asp:Table ID="AdminNominationsTable" CssClass="table table-bordered" runat="server">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell>User Name</asp:TableHeaderCell>
                <asp:TableHeaderCell>Votes</asp:TableHeaderCell>
                <asp:TableHeaderCell>Will Be Candidate</asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
    </asp:Panel>

    <asp:Panel ID="AdminVotingPanel" runat="server" Visible="false">
        <asp:Table ID="AdminVotingTable" CssClass="table table-bordered" runat="server">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell>User Name</asp:TableHeaderCell>
                <asp:TableHeaderCell>Votes</asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
    </asp:Panel>

    <asp:Panel ID="NECCertificationPanel" runat="server" Visible="false">
        <asp:Table ID="NECVotingTable" CssClass="table table-bordered" runat="server">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell>User Name</asp:TableHeaderCell>
                <asp:TableHeaderCell>Votes</asp:TableHeaderCell>
            </asp:TableHeaderRow>            
        </asp:Table>
        <asp:Label ID="NECCertifyAgreement" Text="As a member of the NEC, I certify that these election results are valid and have been properly collected.<br>" runat="server" />
        <asp:CheckBox ID="CertifyCheckBox" Text="I hereby certify this election's results." TextAlign="Right" Checked="false" runat="server" />
        <asp:Button ID="CertifyButton" Text="Submit Certification" runat="server" OnClick="Certify_Click" />
        <asp:Label ID="CertifyWarning" Text="<br>Please tick the certification check box before submitting your certification!" runat="server" Visible="false" />
        <asp:Label ID="NECCertificationComplete" runat="server" Visible="false" Text="Thank you for certifying the election results!"/>
    </asp:Panel>


    <asp:Panel ID="AdminCertificationPanel" runat="server" Visible="false">
        <asp:Label ID="AdminCertCount" Visible="true" runat="server" />
    </asp:Panel>

    <asp:Panel ID="AdminConflictPanel" runat="server" Visible="false">
        <asp:Label ID="AdminNoConflicts" Visible="false" Text="There are no conflicts to resolve." runat="server" />
    </asp:Panel>

    <asp:Panel ID="AdminClosedPanel" runat="server" Visible="false">
        Admin Closed Panel
    </asp:Panel>
    
    <asp:Panel ID="FacultyClosed" runat="server" Visible="false">
    Faculty Closed
    </asp:Panel>

    <asp:Panel ID="FacultyWTS" runat="server" Visible="false">
    Faculty WTS
            <asp:Panel ID="wtsPanelNew" runat="server">
                Statement: <asp:TextBox ID="wtsStatement" runat="server" Height="226px" 
                    TextMode="MultiLine" Width="213px"></asp:TextBox><br />
                </p>
                <p>
                    <asp:CheckBox ID="wtsConfirm" runat="server" 
                        Text="I confirm that I am willing to serve as an APSCUF Official. " 
                        ValidationGroup="wts" />
                </p>
                <p>
                    <asp:CustomValidator ID="wtsAcceptValidator" runat="server" 
                        ErrorMessage="Please confirm your willingness to server by checking the box above." 
                        onservervalidate="wtsAcceptValidator_ServerValidate"></asp:CustomValidator>
                </p>
                <p>
                    <asp:Button ID="wtsSubmit" runat="server" Text="Submit" 
                        onclick="wtsSubmit_Click" />
                </p>
                <asp:HiddenField ID="wtsEmail" runat="server" />
            </asp:Panel>
            <asp:Panel ID="wtsPanelExisting" runat="server" Visible="false">
                You have already submitted a willing to server form for this election.
            </asp:Panel>
            <asp:Panel ID="wtsPanelDone" runat="server" Visible="false">
                Your willingness to server has been successfully registered.
            </asp:Panel>
    </asp:Panel>

    <asp:Panel ID="FacultyNomination" runat="server" Visible="false">
        Please cast your vote in the primary election for one of the following nominees:
        <asp:RadioButtonList ID="FacultyNominationList" runat="server" >
        </asp:RadioButtonList>
        <asp:Button ID="FacultyCastNomination" Text="Cast Vote" runat="server" OnClick="FacultyCastNomination_Click" />
    </asp:Panel>

    <asp:Panel ID="FacultyNominationComplete" runat="server" Visible="false">
        Thank you for placing your vote in the primary election!
    </asp:Panel>

    <asp:Panel ID="FacultyVote" runat="server" Visible="false">
        Please cast your vote in the final election for one of the following nominees:
        <asp:RadioButtonList ID="FacultyVoteList" runat="server" >
        </asp:RadioButtonList>
        <asp:Button ID="FacultyCastVote" Text="Cast Vote" runat="server" OnClick="FacultyCastVote_Click" />
    </asp:Panel>

    <asp:Panel ID="FacultyVoteComplete" runat="server" Visible="false">
        Thank you for placing your vote in the final election!
    </asp:Panel>

</asp:Content>


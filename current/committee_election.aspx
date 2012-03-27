<%@ Page Title="Committee Elections" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="committee_election.aspx.cs" Inherits="committee_election" %>
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
    </asp:Panel>

    <asp:Panel ID="AdminNominationsPanel" runat="server" Visible="false">
        Admin Nominate Panel
    </asp:Panel>

    <asp:Panel ID="AdminVotingPanel" runat="server" Visible="false">
        Admin Vote Panel
    </asp:Panel>

    <asp:Panel ID="AdminCertificationPanel" runat="server" Visible="false">
        Admin Cert Panel
    </asp:Panel>

    <asp:Panel ID="AdminConflictPanel" runat="server" Visible="false">
        Admin Conflict Panel
    </asp:Panel>

    <asp:Panel ID="AdminClosedPanel" runat="server" Visible="false">
        Admin Closed Panel
    </asp:Panel>
    
    <asp:Panel ID="FacultyClosed" runat="server" Visible="false">
    Faculty Closed
    </asp:Panel>

    <asp:Panel ID="FacultyWTS" runat="server" Visible="false">
    Faculty WTS
    </asp:Panel>

    <asp:Panel ID="FacultyNomination" runat="server" Visible="false">
    Faculty Primary
    </asp:Panel>

    <asp:Panel ID="FacultyVote" runat="server" Visible="false">
    Faculty vote
    </asp:Panel>

</asp:Content>


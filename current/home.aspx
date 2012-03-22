<%@ Page Title="iVote" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="home.aspx.cs" Inherits="home" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit"  TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <ul class="breadcrumb">
        <li class="active">Home</li>
    </ul>

    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    
    <!--Notifications-->
    <asp:Panel ID="PanelNominationPending" Visible="false" Enabled="false" runat="server">
        <div id="notifications">
            <asp:Panel id="nom_pending" class="alert alert-info" Visible="false" Enabled="false" runat="server">
                <strong>Notification</strong>
                <p>You have a nomination pending!</p>
                <a href="/Nominations.aspx" class="btn btn-primary">View Nomination</a>
            </asp:Panel>
            <asp:Panel ID="elig_pending" class="alert" Visible="false" Enabled="false" runat="server">
                <strong>Notification</strong>
                <p>There are eligibility forms that must be approved.</p>
                <a href="/approvenominations.aspx" class="btn btn-warning">View Forms</a>
            </asp:Panel>
        </div>
    </asp:Panel>
    
    <div class="page-header">
        <h1>Dashboard</h1>
    </div>
    
    <asp:Panel ID="AdminTabs" Visible="false" runat="server">
        <div class="tabbable">
            <ul class="nav nav-tabs">
                <li class="active"><a href="#">Active Elections</a></li>
                <li><a href="/committees.aspx">Committees <asp:Label runat="server" ID="WaitingCommittees" CssClass="badge badge-info" Visible="False" Text="0" /></a></li>
                <li><a href="/users.aspx">Users</a></li>
            </ul>
        </div>
    </asp:Panel>
    
    
    <!-- Officer Elections -->
    <div id="officer_elections">
        <h2>Officer Election</h2>
        
        <asp:Panel ID="OfficerStateless" Visible="false" runat="server" CssClass="alert alert-info">
            <strong>No Active Officer Election</strong>
            <p>There are currently no active election phases. This could mean that there is no election running, or that there is no action required on your part at this time.</p>
            <a class="btn" href="/officer_election.aspx">Visit Election Dashboard</a>
        </asp:Panel>
        
        <asp:Panel ID="OfficerNominate" Visible="false" runat="server" CssClass="alert alert-info">
            <strong>Nomination Phase</strong>
            <p>The officer election is currently in the <b>nomination phase</b>. During this period, you may nominate yourself or other faculty members.</p>
            <a class="btn" href="/officer_election.aspx">Visit Election Dashboard</a>
        </asp:Panel>
        <asp:Panel ID="OfficerNominationAccept" Visible="false" runat="server" CssClass="alert alert-info">
            <strong>Nomination Acceptance Phase</strong>
            <p>The officer election is currently in the <b>nomination acceptance phase</b>. This period acts as a buffer to give extra time to accept nominations.</p>
            <a class="btn" href="/officer_election.aspx">Visit Election Dashboard</a>
        </asp:Panel>
        <asp:Panel ID="OfficerSlate" Visible="false" runat="server" CssClass="alert alert-info">
            <strong>Slate Phase</strong>
            <p>The officer election is currently in the <b>nomination acceptance phase</b>. This period acts as a buffer to give extra time to accept nominations.</p>
            <a class="btn" href="/officer_election.aspx">Visit Election Dashboard</a>
        </asp:Panel>
        <asp:Panel ID="OfficerPetition" Visible="false" runat="server" CssClass="alert alert-info">
            <strong>Petition Phase</strong>
            <p>The current election is in the petition phase.  You can petition yourself or other faculty members for a position.</p>
            <a class="btn" href="/officer_election.aspx">Visit Election Dashboard</a>
        </asp:Panel>
        <asp:Panel ID="OfficerPetitionAccept" Visible="false" runat="server" CssClass="alert alert-info">
            <strong>Petition Acceptance Phase</strong>
            <p>The current election is in a petition acceptance period. This period acts as a buffer to give extra time to accept petition-based nominations.</p>
            <a class="btn" href="/officer_election.aspx">Visit Election Dashboard</a>
        </asp:Panel>
        <asp:Panel ID="OfficerApproval" Visible="false" runat="server" CssClass="alert alert-info">
            <strong>Approval Phase</strong>
            <p>This is a special phase that will end as soon as all eligibility forms have been approved or disapproved.</p>
            <a class="btn" href="/officer_election.aspx">Visit Election Dashboard</a>
        </asp:Panel>
        <asp:Panel ID="OfficerVoting" Visible="false" runat="server" CssClass="alert alert-info">
            <strong>Voting Phase</strong>
            <p>The current election is in the voting phase.  You must vote for the candidate you feel will best serve in each position.</p>
            <a class="btn" href="/officer_election.aspx">Visit Election Dashboard</a>
        </asp:Panel>
        <asp:Panel ID="OfficerResults" Visible="false" runat="server" CssClass="alert alert-info">
            <strong>Results Phase</strong>
            <p>The election results are now available.</p>
            <a href="/officer_election.aspx" class="btn btn-primary">View Results</a>
        </asp:Panel>
    </div>
    <div id="committee_elections">
        <asp:Panel ID="CommitteeElectionList" runat="server">
            <h2>Committee Elections</h2>
            <asp:Label ID="CommitteeElectionStatus" Text="No active committee election." runat="server" Visible="False" />
            <asp:Repeater ID="CommitteeElectionRepeater" runat="server">
                <ItemTemplate>
                    <div class="election">
                        <strong><a href="/committee_election.aspx?id=<%#DataBinder.Eval(Container.DataItem, "ID")%>">
                            <%#GetName(DataBinder.Eval(Container.DataItem, "PertinentCommittee"))%>
                        </a></strong>
                        <p><%#GetCommitteePhaseMessage((DatabaseEntities.CommitteeElection)Container.DataItem)%></p>
                        <% if(IsAdmin()) { %>
                        <div class="<%#GetCommitteeProgressStatus((DatabaseEntities.CommitteeElection)Container.DataItem)%>">
                            <div class="bar" style="width:<%#GetCommitteePhaseProgress((DatabaseEntities.CommitteeElection)Container.DataItem)%>%;"></div>
                        </div>
                        <% } %>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </asp:Panel>
    </div>
</asp:Content>


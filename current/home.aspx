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
    <asp:Panel ID="PanelNominationPending" Visible="false" runat="server">
        <div id="notifications">
            <asp:Panel id="nom_pending" class="alert alert-info" Visible="false" Enabled="false" runat="server">
                <strong>Notification</strong>
                <p>You have a nomination pending!</p>
                <a href="/Nominations.aspx" class="btn btn-primary">View Nomination</a>
            </asp:Panel>
            <asp:Panel ID="elig_pending" class="alert" Visible="false" Enabled="false" runat="server">
                <strong>Notification</strong>
                <p>There are eligibility forms that must be approved.</p>
                <a href="/ApproveNominations.aspx" class="btn btn-warning">View Forms</a>
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
        
        <asp:Panel ID="OfficerStateless" Visible="false" runat="server" CssClass="election">
            <strong>No Active Officer Election</strong>
            <p>There are currently no active election phases. This could mean that there is no election running, or that there is no action required on your part at this time.</p>
            <p><a class="btn" href="/initiate.aspx" id="initiate_new_officer_election" runat="server" Visible="false">Initiate New Election</a></p>
        </asp:Panel>
        
        <asp:Panel ID="OfficerNominate" Visible="false" runat="server" CssClass="election">
            <strong><a href="/officer_election.aspx">Nomination Phase</a></strong>
            <p>The officer election is currently in the <b>nomination phase</b>. During this period, you may nominate yourself or other faculty members.</p>
        </asp:Panel>
        <asp:Panel ID="OfficerNominationAccept" Visible="false" runat="server" CssClass="election">
            <strong><a href="/officer_election.aspx">Nomination Acceptance Phase</a></strong>
            <p>The officer election is currently in the <b>nomination acceptance phase</b>. This period acts as a buffer to give extra time to accept nominations.</p>
        </asp:Panel>
        <asp:Panel ID="OfficerSlate" Visible="false" runat="server" CssClass="election">
            <strong><a href="/officer_election.aspx">Slate Phase</a></strong>
            <p>The officer election is currently in the <b>nomination acceptance phase</b>. This period acts as a buffer to give extra time to accept nominations.</p>
        </asp:Panel>
        <asp:Panel ID="OfficerPetition" Visible="false" runat="server" CssClass="election">
            <strong><a href="/officer_election.aspx">Petition Phase</a></strong>
            <p>The current election is in the petition phase.  You can petition yourself or other faculty members for a position.</p>
        </asp:Panel>
        <asp:Panel ID="OfficerPetitionAccept" Visible="false" runat="server" CssClass="election">
            <strong><a href="/officer_election.aspx">Petition Acceptance Phase</a></strong>
            <p>The current election is in a petition acceptance period. This period acts as a buffer to give extra time to accept petition-based nominations.</p>
        </asp:Panel>
        <asp:Panel ID="OfficerApproval" Visible="false" runat="server" CssClass="election">
            <strong><a href="/officer_election.aspx">Approval Phase</a></strong>
            <p>This is a special phase that will end as soon as all eligibility forms have been approved or disapproved.</p>
        </asp:Panel>
        <asp:Panel ID="OfficerVoting" Visible="false" runat="server" CssClass="election">
            <strong><a href="/officer_election.aspx">Voting Phase</a></strong>
            <p>The current election is in the voting phase.  You must vote for the candidate you feel will best serve in each position.</p>
        </asp:Panel>
        <asp:Panel ID="OfficerResults" Visible="false" runat="server" CssClass="election">
            <strong><a href="/officer_election.aspx">Election Results Available</a></strong>
            <p>The election results are now available.</p>
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
                        <p><%#GetDaysReamining((DatabaseEntities.CommitteeElection)Container.DataItem)%></p>
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


<%@ Page Title="Committee Elections" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="committee_election.aspx.cs" Inherits="current.committee_election" %>
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

    <asp:ToolkitScriptManager ID="AJAXManager" runat="Server" />

    <asp:TabContainer ID="AdminTabs" runat="server" ActiveTabIndex="0" Visible="false">
        <asp:TabPanel ID="WTSPanel" HeaderText="Users Willing To Serve" runat="server">
            <ContentTemplate>One</ContentTemplate>
        </asp:TabPanel>

        <asp:TabPanel ID="NominationPanel" HeaderText="Nominations" runat="server">
            <ContentTemplate>Two</ContentTemplate>
        </asp:TabPanel>
    
        <asp:TabPanel ID="VotingPanel" HeaderText="Votes" runat="server">
            <ContentTemplate>Three</ContentTemplate>
        </asp:TabPanel>
    
        <asp:TabPanel ID="CertificationPanel" HeaderText="Certification" runat="server">
            <ContentTemplate>Four</ContentTemplate>
        </asp:TabPanel>
    
        <asp:TabPanel ID="ConflictResolutionPanel" HeaderText="Conflict Reconliation" runat="server">
            <ContentTemplate>Five</ContentTemplate>
        </asp:TabPanel>
    </asp:TabContainer>

    <asp:Panel ID="ClosePanel" HorizontalAlign="Center" runat="server" Visible="false">
    One
    </asp:Panel>

    <asp:Panel ID="FacultyWTS" HorizontalAlign="Center" runat="server" Visible="false">
    Two
    </asp:Panel>

    <asp:Panel ID="FacultyNomination" HorizontalAlign="Center" runat="server" Visible="false">
    Three
    </asp:Panel>

    <asp:Panel ID="FacultyVote" HorizontalAlign="Center" runat="server" Visible="false">
    Four
    </asp:Panel>

</asp:Content>


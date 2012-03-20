<%@ Page Title="Confirm New Election" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="committee_election_confirminit.aspx.cs" Inherits="wwwroot_phase1aSite_committee_election_confirminit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <ul class="breadcrumb">
        <li><a href="/home.aspx">Home</a> <span class="divider">/</span></li>
        <li><a href="/committees.aspx">Committees</a> <span class="divider">/</span></li>
        <li class="active">New Committee Election</li>
    </ul>
    
    <div class="page-header">
        <h1>Initialize Committee Election</h1>
    </div>
    
    <p>By pressing "Initialize" below, you'll officially kick off a new committee election. This will allow </p>
    
    <p>Initializing the election will perform the following actions:</p>
    
    <ol>
        <li>Open election for Willingness to Serve applications</li>
        <li>Distribute Willingness to Serve applications via email</li>
        <li>Start a timer for the next election phase, which will alert you on the election dashboard when the next phase should be started</li>
    </ol>
    
    <asp:Button ID="StartElection" CssClass="btn btn-primary"
            Text="Initiate Election"
            OnClick="StartElection_Clicked" runat="server" />
</asp:Content>


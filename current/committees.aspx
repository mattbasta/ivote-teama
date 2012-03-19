<%@ Page Title="Committee Management" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="committees.aspx.cs" Inherits="wwwroot_phase1aSite_users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <ul class="breadcrumb">
        <li><a href="/home.aspx">Home</a> <span class="divider">/</span></li>
        <li class="active">Committees</li>
    </ul>
    
    <div class="page-header">
        <h1>Committee Manager</h1>
    </div>
    
    <div class="tabbable">
        <ul class="nav nav-tabs">
            <li><a href="/home.aspx">Active Elections</a></li>
            <li class="active"><a href="#">Committees</a></li>
            <li><a href="/users.aspx">Users</a></li>
        </ul>
    </div>
    
    <asp:Table ID="CommitteeTable" CssClass="table table-bordered" runat="server">
        <asp:TableHeaderRow>
            <asp:TableHeaderCell>Committee Name</asp:TableHeaderCell>
            <asp:TableHeaderCell>Positions</asp:TableHeaderCell>
            <asp:TableHeaderCell>Vacancy Count</asp:TableHeaderCell>
            <asp:TableHeaderCell>Actions/Status</asp:TableHeaderCell>
        </asp:TableHeaderRow>
    </asp:Table>
</asp:Content>


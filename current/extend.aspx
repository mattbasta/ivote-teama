<%@ Page Title="All Users" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="extend.aspx.cs" Inherits="wwwroot_phase1aSite_users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:Label runat="server" ID="end" />
Hi.  Extend the timeline: <br />
<asp:Button ID="extend" OnClick="extend_OnClick" runat="server" Text="Extend." />
</asp:Content>


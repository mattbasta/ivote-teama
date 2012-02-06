<%@ Page Title="" Language="C#" MasterPageFile="../../Site.master" AutoEventWireup="true" CodeFile="stateless.aspx.cs" Inherits="Admin_stateless" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link  href="../Views.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:Label ID="lblForm" runat="server">
    <div id="notifications">
        <span class="notification">
            Notification!
        </span>
    </div>
    <div id="bodyCopy">
        <h1></h1>
        There are currently no active election phases.
    </div>
    <div id="functions">
        <div class="column">
            <b>User Management</b><br />
            <asp:LinkButton ID="LinkButton2"  commandname="id" CommandArgument="../../Register.aspx"  OnCommand="transfer"
                        text="Create User" runat="server" /> <br />
            <asp:LinkButton ID="LinkButton4"  commandname="id" CommandArgument="../../users.aspx"  OnCommand="transfer"
                        text="Edit User" runat="server" /><br />
        </div>
        <div class="column">
            <b>Election Management</b><br />
            Create Election
        </div>
        <div class="column">
            <b>Account Management</b><br />
            <asp:LinkButton ID="LinkButton1"  commandname="id" CommandArgument="../../cpw.aspx"  OnCommand="transfer"
                        text="Change Password" runat="server" />
        </div>
    </div>
    <div class="clear"></div>
    
</asp:Label>
</asp:Content>


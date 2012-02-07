<%@ Page Title="" Language="C#" MasterPageFile="../../Site.master" AutoEventWireup="true" CodeFile="stateless.aspx.cs" Inherits="Faculty_stateless" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link  href="../Views.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:Label ID="lblForm" runat="server">
    <asp:Panel ID="PanelNominationPending" Visible="false" Enabled="false" runat="server">
            <div id="notifications">
               <!--No notifications during a stateless period-->
            </div>
        </asp:Panel>
    <div id="bodyCopy">
        <h1>No Current Election</h1>
        There are currently no active election phases.
    </div>
    <div id="functions">
        
        <div class="column">
            <b>Election Participation</b><br />
            There are no active elections.
        </div>
        <div class="column">
            <b>Account Management</b><br />
            <asp:LinkButton ID="LinkButton1"  commandname="id" CommandArgument="../../cpw.aspx"  OnCommand="transfer"
                        text="Change Password" runat="server" />
        </div>
        <div class="column">
            <!--No third column.-->
        </div>
    </div>
    <div class="clear"></div>
    
</asp:Label>
</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="../../Site.master" AutoEventWireup="true" CodeFile="nom_accept.aspx.cs" Inherits="Faculty_nom_accept" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link  href="../Views.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:Label ID="lblForm" runat="server">
    <asp:ScriptManager runat="server" />
    <!--Notifications for this phase include pending nomination.--> 

        <asp:Panel ID="PanelNominationPending" Visible="false" Enabled="false" runat="server">
            <div id="notifications">
                <asp:Panel id="nom_pending" class="notification" Visible="false" Enabled="false" runat="server">
                    <b>You have a nomination pending! </b>
                    <asp:LinkButton ID="LinkButton5"  commandname="id" CommandArgument="../../Nominations.aspx"  OnCommand="transfer"
                        text="View Nominations." runat="server" />
                </asp:Panel>
            </div>
        </asp:Panel>
        
    

    <div id="bodyCopy">
        <h1>Nomination Acceptance Period</h1>
        If you have a pending nomination, you have until [date] to accept or deny, otherwise the nomination will automatically be declined.
    </div>
    <div id="functions">
        <div class="column">
            <b>Account Management</b><br />
            <asp:LinkButton ID="LinkButton1"  commandname="id" CommandArgument="../../cpw.aspx"  OnCommand="transfer"
                        text="Change Password" runat="server" />
        </div>
    </div>
    <div class="clear"></div>
    
    <!--None for this phase.-->
    <div id="special">
        
    </div>
    
</asp:Label>
</asp:Content>


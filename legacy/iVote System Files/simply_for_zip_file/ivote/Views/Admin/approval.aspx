<%@ Page Title="" Language="C#" MasterPageFile="../../Site.master" AutoEventWireup="true" CodeFile="approval.aspx.cs" Inherits="Admin_approval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link  href="../Views.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:Label ID="lblForm" runat="server">
    <asp:ScriptManager runat="server" />
    <!--This phase is eligibility dependent.  A notification will be forced for eligibility. --> 
        
        

    
        <asp:Panel ID="PanelNominationPending" Visible="false" Enabled="false" runat="server">
            <div id="notifications">
                <asp:Panel ID="elig_pending" class="notification" Visible="false" Enabled="false" runat="server">
                    <b>There are eligibility forms that MUST be approved for the election to progress. </b>
                    <asp:LinkButton ID="LinkButton6"  commandname="id" CommandArgument="../../approvenominations.aspx"  OnCommand="transfer"
                        text="Approve Eligibility" runat="server" />
                </asp:Panel>
            </div>
        </asp:Panel>
        
    

    <div id="bodyCopy">
        <h1>Eligibility Approval Period</h1>
        The current election is in the eligibility approval phase.  This phase will pass as soon as all eligibility forms are approved
         or denied.  Faculty members will not be able to access any election functionality until this phase passes.
    </div>
    <div id="functions">
        <div class="column">
            <b>User Management</b><br />
            <asp:LinkButton ID="LinkButton2"  commandname="id" CommandArgument="../../Register.aspx"  OnCommand="transfer"
                        text="Create User" runat="server" /><br />
            <asp:LinkButton ID="LinkButton4"  commandname="id" CommandArgument="../../users.aspx"  OnCommand="transfer"
                        text="Edit User" runat="server" /><br />
            
        </div>
        <div class="column">
            <b>Election Management</b><br />
              <asp:LinkButton ID="LinkButton3"  commandname="id" CommandArgument="../../approvenominations.aspx"  OnCommand="transfer"
                        text="Approve Eligibility" runat="server" /><br />
            <br />
        </div>
        <div class="column">
            <b>Account Management</b><br />
            <asp:LinkButton ID="LinkButton1"  commandname="id" CommandArgument="../../cpw.aspx"  OnCommand="transfer"
                        text="Change Password" runat="server" />
        </div>
    </div>
    <div class="clear"></div>
    
    <!--Nothing special for this phase.-->
    <div id="special">

    </div>
    
</asp:Label>
</asp:Content>


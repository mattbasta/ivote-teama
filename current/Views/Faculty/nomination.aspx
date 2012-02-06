<%@ Page Title="" Language="C#" MasterPageFile="../../Site.master" AutoEventWireup="true" CodeFile="nomination.aspx.cs" Inherits="Faculty_nomination" %>

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
        <h1>Nomination Period</h1>
        The current election is in a nomination period.  During this period, you must may nominate yourself or other faculty members.
    </div>
    <div id="functions">
        <div class="column">
            <b>Account Management</b><br />
            <asp:LinkButton ID="LinkButton1"  commandname="id" CommandArgument="../../cpw.aspx"  OnCommand="transfer"
                        text="Change Password" runat="server" />
        </div>
    </div>
    <div class="clear"></div>
    
    <!--For this phase, we'll show the nomination functionality.-->
    <div id="special">
        <asp:Label ID="LabelResponse" runat="server" Text="" />
        <br />
        <br />
        

        <span style="color: Blue">Click <b>Select</b>, next to each position below, to nominate yourself or view information for that position.</span><br />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <!-- List of positions -->
                <asp:GridView ID="GridViewPositions" AutoGenerateColumns="false" OnRowCommand="GridViewPositions_RowCommand" CssClass="simpleGrid" runat="server">
                     <Columns>        
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:Button ID="ButtonSelect" runat="server" commandname="positions" commandargument='<%#Eval("position") %>' Text="Select" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Postion Name" DataField="position" NullDisplayText="Error"/> 

                    </Columns>
                </asp:GridView>

                <!--Display form/data for selected position -->
                <asp:Panel ID="PanelSelected" CssClass="simpleBorder" Visible="false" runat="server">
                    <br />
                    <asp:Label ID="LabelSelected" runat="server" Text="" />
                    <p class="submitButton"> 
                    <asp:Button ID="ButtonNominate" Width="255" runat="server" OnClick="nominate" Text="" /><br />
                    <asp:Button ID="ButtonWTS" Width="255" runat="server" Text="" OnClick="next" /> 
                    </p>
                    <asp:HiddenField ID="HiddenFieldID" runat="server" />
                </asp:Panel>
            


            </ContentTemplate>
    </asp:UpdatePanel>
    </div>
    
</asp:Label>
</asp:Content>


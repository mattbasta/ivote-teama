<%@ Page Title="" Language="C#" MasterPageFile="../../Site.master" AutoEventWireup="true" CodeFile="petition.aspx.cs" Inherits="Faculty_petition" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link  href="../Views.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:Label ID="lblForm" runat="server">
    <asp:ToolkitScriptManager runat="server" />
   
    <!--This phase has a nomination notification.--> 
        
        

    
        <asp:Panel ID="PanelNominationPending" Visible="false" Enabled="false" runat="server">
            <div id="notifications">
                <asp:Panel id="nom_pending" class="notification" Visible="false" Enabled="false" runat="server">
                    <b>You have been granted a nomination by petition of your peers. </b>
                    <asp:LinkButton ID="LinkButton5"  commandname="id" CommandArgument="../../Nominations.aspx"  OnCommand="transfer"
                        text="View Nominations." runat="server" />
                </asp:Panel>
            </div>
        </asp:Panel>
        
    

    <div id="bodyCopy">
        <h1>Petition Period</h1>
        The current election is in the petition phase.  You may search and petition for your peers below.
        
    </div>
    <div id="functions">
        
        <div class="column">
            <b>Account Management</b><br />
            <asp:LinkButton ID="LinkButton1"  commandname="id" CommandArgument="../../cpw.aspx"  OnCommand="transfer"
                        text="Change Password" runat="server" />
        </div>
        <div class="column">
            <!--None-->
        </div>
        <div class="column">
            <!--None-->
        </div>
        
    </div>
    <div class="clear"></div>
    
    <!--The petition functionality will be active during this phase.-->
    <div id="special">
        
        <asp:UpdatePanel ID="UpdatePanel" runat="server" UpdateMode="Conditional">
        <Triggers>
        <asp:PostBackTrigger ControlID="ButtonSubmit" />
        </Triggers>
        <ContentTemplate>

        <br />
        Search for the individual you would like to submit a petition for:<br />
        <asp:TextBox ID="txtSearch" runat="server" Width=300></asp:TextBox> 
        <asp:Button ID="btnSearch"  runat="server" Text="Search" OnClick="search" /> 
        <asp:LinkButton ID="btnViewAll"   runat="server" Text="Clear" OnClick="clear" Visible="false" /> <br /><br />
        <span style="color: Blue"><asp:Label ID="LabelFeedback" runat="server" Text="" /></span><br />

            <table class="simpleGrid" style="width: 60%">
                <tr>
                    <th>Full Name</th>
                    <th>Department</th>
                    <th></th>
                </tr>
                <asp:ListView ID="ListViewUsers" Visible="false" OnItemCommand="ListViewUsers_ItemCommand" runat="server">
                    <LayoutTemplate>
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td >
                                <asp:Label ID="LabelName" text='<%#Eval("last_name") + ", " + Eval("first_name") %>' runat="server" />
                            </td>
                            <td >
                                <asp:Label ID="Label1" text='<%#Eval("department") %>' runat="server" />
                            </td>
                            <td >
                               <asp:Button ID="ButtonNominate" 
                                   commandname="nominate"
                                   commandargument='<%#Eval("idunion_members") %>' 
                                   text="Submit Petition" runat="server" />                
                            </td>
                        </tr>
                     </ItemTemplate>
                </asp:ListView>
            </table>
        </ContentTemplate>
                </asp:UpdatePanel>

            <asp:Panel ID="PanelChoosePosition" CssClass="modalPopup" runat="server">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="LabelChoosPosition" runat="server" Text="" /><br /><br />
                        <asp:HiddenField ID="HiddenFieldName" runat="server" />
                        <asp:HiddenField ID="HiddenFieldId" runat="server" />
                        <asp:DropDownList ID="DropDownListPostions" runat="server">
                        </asp:DropDownList><br /> <br />
                        <asp:Button ID="ButtonSubmit" runat="server" OnClick="ButtonSubmit_Clicked" Text="Submit Your Petition" />
                        <asp:Button ID="ButtonCancel" runat="server" OnClick="ButtonCancel_Clicked" Text="Cancel" />
                     </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>

            <asp:Button ID="Button1" runat="server" Text="" style="display: none" />

            <asp:ModalPopupExtender ID="PopupControlExtender1" runat="server"
                TargetControlID="Button1"
                PopupControlID="PanelChoosePosition"
                CancelControlID="ButtonCancel"
                BackgroundCssClass="modalBackground"
                PopupDragHandleControlID="PanelChoosePosition"
            />
    </div>
    
</asp:Label>
</asp:Content>


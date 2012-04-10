<%@ Page Title="Petition" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Petition.aspx.cs" Inherits="wwwroot_experimental_petition" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

To start or sign a petition for a faculty member to be on the upcoming slate, please search for them below and then click "Start Petition".

<asp:ToolkitScriptManager runat="server" />
<Triggers>
<asp:PostBackTrigger ControlID="ButtonSubmit" />
</Triggers>
<ContentTemplate>

<br />
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
                        <asp:Label ID="LabelName" text='<%#Eval("LastName") + ", " + Eval("FirstName") %>' runat="server" />
                    </td>
                    <td >
                        <asp:Label ID="Label1" text='<%#Eval("Department") %>' runat="server" />
                    </td>
                    <td >
                       <asp:Button ID="ButtonNominate" 
                           commandname="nominate"
                           commandargument='<%#Eval("ID") %>' 
                           text="Submit Petition" runat="server" />                
                    </td>
                </tr>
             </ItemTemplate>
        </asp:ListView>
    </table>
</ContentTemplate>

    <asp:Panel ID="PanelChoosePosition" CssClass="modalPopup" runat="server">

            <ContentTemplate>
                <asp:Label ID="LabelChoosPosition" runat="server" Text="" /><br /><br />
                <asp:HiddenField ID="HiddenFieldName" runat="server" />
                <asp:HiddenField ID="HiddenFieldId" runat="server" />
                <asp:DropDownList ID="DropDownListPostions" runat="server">
                </asp:DropDownList><br /> <br />
                <asp:Button ID="ButtonSubmit" runat="server" OnClick="ButtonSubmit_Clicked" Text="Submit Your Petition" />
                <asp:Button ID="ButtonCancel" runat="server" OnClick="ButtonCancel_Clicked" Text="Cancel" />
             </ContentTemplate>

    </asp:Panel>

    <asp:Button ID="Button1" runat="server" Text="" style="display: none" />

    <asp:ModalPopupExtender ID="PopupControlExtender1" runat="server"
        TargetControlID="Button1"
        PopupControlID="PanelChoosePosition"
        CancelControlID="ButtonCancel"
        BackgroundCssClass="modalBackground"
        PopupDragHandleControlID="PanelChoosePosition"
    />

</asp:Content>


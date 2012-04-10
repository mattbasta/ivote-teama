<%@ Page Title="Nominate" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Nominate.aspx.cs" Inherits="wwwroot_phase1aSite_nominate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<h1><asp:Label ID="LabelHeader" runat="server" Text=""></asp:Label></h1><br />


<asp:ScriptManager ID="ScriptManager1" runat="server" />
<asp:UpdatePanel ID="UpdatePanel" runat="server" UpdateMode="Conditional">
<ContentTemplate>

<asp:Panel ID="PanelSearch" runat="server">
<asp:Label ID="LabelExplain" runat="server" Text=""></asp:Label><br /><br />
<asp:TextBox ID="txtSearch" runat="server" Width=300></asp:TextBox> 
<asp:Button ID="btnSearch"  runat="server" Text="Search" OnClick="search" /> 
<asp:LinkButton ID="btnViewAll"   runat="server" Text="Clear" OnClick="clear" Visible="false" /> <br /><br />
<asp:Label ID="LabelFeedback" runat="server" Text="" /><br />

<asp:HiddenField ID="HiddenFieldPosition" runat="server" />

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
                       OnClientClick='<%# Eval("FirstName", "return confirm(\"Are you sure you want to nominate {0} for this poistion?\")") %>'
                       commandargument='<%#Eval("ID") %>' 
                       text="Nominate" runat="server" />                
                </td>
            </tr>
         </ItemTemplate>
    </asp:ListView>
</table>
</asp:Panel>
<asp:Panel ID="PanelComplete" Visible="false" runat="server">
    <asp:Label ID="LabelComplete" runat="server" Text="" /><br />
    <!--must be ../ as the page adds the /3 at the end.-->
    <a href="../home.aspx">Click here to return to the homepage.</a>
</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>

</asp:Content>


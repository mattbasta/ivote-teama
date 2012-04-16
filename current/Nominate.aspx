<%@ Page Title="Nominate" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Nominate.aspx.cs" Inherits="wwwroot_phase1aSite_nominate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <ul class="breadcrumb">
        <li><a href="/home.aspx">Home</a> <span class="divider">/</span></li>
        <li><a href="/officer_election.aspx">Officer Election</a> <span class="divider">/</span></li>
        <li class="active">Nominations</li>
    </ul>
    
    <div class="page-header">
        <h1><asp:Label ID="LabelHeader" runat="server" Text=""></asp:Label></h1>
    </div>

<asp:ScriptManager ID="ScriptManager1" runat="server" />
<asp:UpdatePanel ID="UpdatePanel" runat="server" UpdateMode="Conditional">
<ContentTemplate>

<asp:Panel ID="PanelSearch" runat="server">
    <div class="well form-search">
        <p>Search for the individual you would like to submit a petition for:</p>
        <asp:TextBox ID="txtSearch" runat="server" CssClass="input-medium search-query"></asp:TextBox> 
        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="search" CssClass="btn" />
        <asp:LinkButton ID="btnViewAll" runat="server" Text="Clear Search" OnClick="clear" Visible="false" CssClass="btn btn-warning" />
    </div>

<p><asp:Literal ID="LabelFeedback" runat="server" Text="" /></p>

<asp:HiddenField ID="HiddenFieldPosition" runat="server" />

<table class="table table-bordered">
    <tr>
        <th>Full Name</th>
        <th>Department</th>
        <th>Actions</th>
    </tr>
    <asp:ListView ID="ListViewUsers" Visible="false" OnItemCommand="ListViewUsers_ItemCommand" runat="server">
        <LayoutTemplate>
            <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <asp:Label ID="LabelName" text='<%#Eval("LastName") + ", " + Eval("FirstName") %>' runat="server" />
                </td>
                <td>
                    <asp:Label ID="Label1" text='<%#Eval("Department") %>' runat="server" />
                </td>
                <td>
                   <asp:Button CssClass="btn btn-small" ID="ButtonNominate" 
                       commandname="nominate"
                       OnClientClick='<%# Eval("FirstName", "return confirm(\"Are you sure you want to nominate {0} for this position?\")") %>'
                       commandargument='<%#Eval("ID") %>' 
                       text="Nominate" runat="server" />                
                </td>
            </tr>
         </ItemTemplate>
    </asp:ListView>
</table>
</asp:Panel>
<asp:Panel ID="PanelComplete" Visible="false" runat="server">
    <asp:Label ID="LabelComplete" runat="server" Text="" />
    <a href="/officer_election.aspx" class="btn">Back</a>
</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>

</asp:Content>


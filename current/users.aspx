<%@ Page Title="All Users" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="users.aspx.cs" Inherits="wwwroot_phase1aSite_users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<!--Adam Code-->

<asp:ScriptManager ID="ScriptManager1" runat="server" />
<asp:UpdatePanel ID="UpdatePanel" runat="server" UpdateMode="Conditional">
<ContentTemplate>

<asp:Panel ID="PanelSearch" runat="server">
<asp:Label ID="LabelExplain" runat="server" Text=""></asp:Label><br /><br />
<asp:TextBox ID="txtSearch" runat="server" Width=300></asp:TextBox> 
<asp:Button ID="btnSearch"  runat="server" Text="Search" OnClick="search_adam" /> 
<asp:LinkButton ID="btnViewAll"   runat="server" Text="Clear" OnClick="allUsers" Visible="false" /> <br /><br />
<asp:Label ID="Label1" runat="server" Text="" /><br />

<asp:HiddenField ID="HiddenFieldPosition" runat="server" />

<asp:Label ID="LabelFeedback" runat="server" Text="" /><br />
<div style="margin-left: auto; width:95%; margin-right: auto">
<asp:GridView ID="GridViewUsers" runat="server" OnRowCommand="GridViewUsers_RowCommand" CssClass="usersGrid" AutoGenerateColumns="False" AllowSorting="true" OnSorting="sorting" >
    <Columns>        
        <asp:TemplateField HeaderText="Full Name" SortExpression="last_name">
                <ItemTemplate>
                    <asp:Label ID="LabelName" text='<%#Eval("last_name") + ", " + Eval("first_name") %>' runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        <asp:TemplateField HeaderText="Email Address" SortExpression="email">
                <ItemTemplate>
                        <asp:HyperLink ID="HyperLink1" text='<%#Eval("email")%>' NavigateUrl='<%#DataBinder.Eval(Container, "DataItem.email","MAILTO:{0}")%>' runat="server">HyperLink</asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
        <asp:BoundField HeaderText="Phone" DataField="phone" NullDisplayText="None Provided" SortExpression="phone"/> 
        <asp:BoundField HeaderText="Department" DataField="department" NullDisplayText="None" SortExpression="department"/>
        <asp:TemplateField HeaderText="" >
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButton1" commandname="id"  commandargument='<%#Eval("idunion_members") %>'
                        text="Edit User" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
    </Columns>
</asp:GridView>
</div>
</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>


<%@ Page Title="All Users" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="users.aspx.cs" Inherits="wwwroot_phase1aSite_users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <ul class="breadcrumb">
        <li><a href="/home.aspx">Home</a> <span class="divider">/</span></li>
        <li class="active">Users</li>
    </ul>
    
    <div class="page-header">
        <h1>User Manager</h1>
    </div>
    
    <div style="float:right;">
        <a href="/Register.aspx" class="btn btn-success">New User</a>
    </div>
    <div class="tabbable">
        <ul class="nav nav-tabs">
            <li><a href="/home.aspx">Active Elections</a></li>
            <li><a href="/committees.aspx">Committees</a></li>
            <li class="active"><a href="#">Users</a></li>
        </ul>
    </div>
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="PanelSearch" runat="server">
                <div class="well form-search">
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="input-medium search-query"></asp:TextBox> 
                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="search_adam" CssClass="btn" />
                    <asp:LinkButton ID="btnViewAll" runat="server" Text="Clear Search" OnClick="allUsers" Visible="false" CssClass="btn btn-warning" />
                </div>
                
                <asp:HiddenField ID="Query" runat="server" />
                <asp:HiddenField ID="Sort" runat="server" />
                
                <div style="margin-left: auto; width:95%; margin-right: auto">
                <asp:GridView GridLines="none" ID="GridViewUsers" runat="server" OnRowCommand="GridViewUsers_RowCommand" CssClass="table table-bordered" AutoGenerateColumns="False" AllowSorting="true" OnSorting="sorting">
                    <Columns>        
                        <asp:TemplateField HeaderText="Full Name" SortExpression="LastName">
                            <ItemTemplate>
                                <asp:Label ID="LabelName" text='<%#Eval("LastName") + ", " + Eval("FirstName") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Email Address" SortExpression="Email">
                            <ItemTemplate>
                                <asp:HyperLink ID="HyperLink1" text='<%#Eval("Email")%>' NavigateUrl='<%#DataBinder.Eval(Container, "DataItem.Email","MAILTO:{0}")%>' runat="server">HyperLink</asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Union" SortExpression="IsUnion">
                            <ItemTemplate>
                                <asp:Image ID="IsUnion" Visible='<%#Eval("IsUnion")%>' ImageUrl="/images/check.png" runat="server" CssClass="make_block" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tenured" SortExpression="IsTenured">
                            <ItemTemplate>
                                <asp:Image ID="IsTenured" Visible='<%#Eval("IsTenured")%>' ImageUrl="/images/check.png" runat="server" CssClass="make_block" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="NEC" SortExpression="IsNEC">
                            <ItemTemplate>
                                <asp:Image ID="IsNEC" Visible='<%#Eval("IsNEC")%>' ImageUrl="/images/check.png" runat="server" CssClass="make_block" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Department" DataField="Department" NullDisplayText="None" SortExpression="Department"/>
                        <asp:TemplateField HeaderText="" >
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" commandname="id" commandargument='<%#Eval("ID") %>'
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


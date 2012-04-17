<%@ Page Title="Confirm New Election" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="committee_election_manage.aspx.cs" Inherits="wwwroot_phase1aSite_committee_election_manage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <ul class="breadcrumb">
        <li><a href="/home.aspx">Home</a> <span class="divider">/</span></li>
        <li><a href="/committees.aspx">Committees</a> <span class="divider">/</span></li>
        <li class="active">New Committee Election</li>
    </ul>

    <div class="page-header">
        <h1>Manage <asp:Literal runat="server" id="CommName1" /></h1>
    </div>

    <p>
        The table below contains the current list of members in the
        <asp:Literal runat="server" id="CommName2" />. To create vacancies in
        the committee, check the boxes next to committee members' names <b>in
        order to remove those members from the committee</b>. When you are
        finished, click the Create Vacancies button.
    </p>
    
    <asp:Panel runat="server" ID="failure_panel" CssClass="alert" Visible="false">
        <strong>No New Vacancies</strong>
        You didn't select any users to remove from the committee.
    </asp:Panel>

    <asp:GridView ID="GridViewUsers" GridLines="none" runat="server" CssClass="table" AutoGenerateColumns="False" AllowSorting="true" OnSorting="sorting">
        <Columns>        
            <asp:TemplateField HeaderText="Full Name" SortExpression="LastName">
                <ItemTemplate>
                    <asp:Label ID="LabelName" text='<%#Eval("LastName") + ", " + Eval("FirstName") %>' runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Email" DataField="Email" NullDisplayText="None" SortExpression="Email"/>
            <asp:BoundField HeaderText="Department" DataField="Department" NullDisplayText="None" SortExpression="Department"/>
            <asp:TemplateField HeaderText="Tenured" SortExpression="IsTenured">
                <ItemTemplate>
                    <asp:Image ID="IsTenured" Visible='<%#Eval("IsTenured")%>' ImageUrl="/images/check.png" runat="server" CssClass="make_block" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Remove">
                <ItemTemplate>
                    <asp:Checkbox ID="CBRemove" runat="server" />
                    <asp:HiddenField runat="server" ID="UserID" Value='<%#Eval("ID")%>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <asp:Button ID="CreateVacancies" CssClass="btn btn-primary pull-right"
            Text="Create Vacancies"
            OnClick="CreateVacancies_Clicked" runat="server" />
</asp:Content>


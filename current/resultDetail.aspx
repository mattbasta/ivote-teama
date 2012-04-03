<%@ Page Title="Result Details" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="resultDetail.aspx.cs" Inherits="wwwroot_finalsite_resultDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<!--Adam Blank Code, added 12/5/2011-->
<div style="text-align: center">
    <h1><asp:Label ID="LabelPosition" runat="server" Text="" /></h1>
    <br />
    <asp:GridView ID="GridViewData" CssClass="simpleGrid" style="margin-left: auto; margin-right: auto;" AutoGenerateColumns="false" runat="server">
        <Columns>
            <asp:BoundField DataField="fullname" HeaderText="Person" />
            <asp:BoundField DataField="count" HeaderText="Votes Recieved" />
        </Columns>
    </asp:GridView><br />
</div>
</asp:Content>



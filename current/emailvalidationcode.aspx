<%@ Page Title="Email Link Validation Test" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="emailvalidationcode.aspx.cs" Inherits="emailvalidationcode" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <b>Shows proof of concept for generating validation code, storing code in db, and retrieving code from db to verifiy user.<br />
            In actual implemention, url generated will be sent via email.</b><br /><br />
            <asp:Button ID="Button1" runat="server" Text="Generate code" 
                onclick="Button1_Click" /><br /><br />
            <asp:Label ID="Label1" runat="server" Text="" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


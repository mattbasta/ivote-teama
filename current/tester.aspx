<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tester.aspx.cs" Inherits="tester" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            height: 26px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:button ID="Button1" runat="server" text="Create Tables" 
            onclick="Button1_Click" />
    &nbsp;*Warning: May break the database. Don&#39;t click unless you know what you are 
        doing. *<br />
        <br />
        <asp:Button ID="Button2" runat="server" onclick="Button2_Click" 
            Text="SMTP Test" />
        <asp:TextBox ID="email" runat="server"></asp:TextBox>
        <br />
        <br />
        <asp:Button ID="Button3" runat="server" onclick="Button3_Click" 
            Text="Create Test Admin" />
        <asp:Button ID="Button5" runat="server" onclick="Button5_Click" 
            Text="Delete Test User" />
        <br />
        <br />
        <asp:Button ID="Button4" runat="server" onclick="Button4_Click" 
            Text="Dump Users" />
        <br />
        <asp:Label ID="Label1" runat="server" Enabled="False" EnableViewState="False"></asp:Label>
        <br />
        <br />
        <br />
        <br />
        <asp:Button ID="Button7" runat="server" onclick="Button7_Click" 
            Text="Hash Password" />
        <asp:TextBox ID="preHash" runat="server"></asp:TextBox>
        <br />
        <asp:Label ID="Label2" runat="server"></asp:Label>
        <br />
        <br />
        <asp:Button ID="Button8" runat="server" onclick="Button8_Click" 
            Text="Test Auth" />
        <asp:TextBox ID="authEmail" runat="server"></asp:TextBox>
        <asp:TextBox ID="authPassword" runat="server"></asp:TextBox>
        <br />
        <asp:Label ID="Label3" runat="server"></asp:Label>
        <br />
        <br />
        <asp:Button ID="Button9" runat="server" onclick="Button9_Click" 
            Text="Start Election" />
        <br />
        <asp:Button ID="Button10" runat="server" CssClass="style1" 
            onclick="Button10_Click" Text="Set WTS" />
    </div>
    </form>
</body>

</html>

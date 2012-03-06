<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tester.aspx.cs" Inherits="tester" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
            Text="Create Test User" />
        <asp:Button ID="Button5" runat="server" onclick="Button5_Click" 
            Text="Delete Test User" />
        <br />
        <br />
        <asp:Button ID="Button4" runat="server" onclick="Button4_Click" 
            Text="Dump Users" />
        <br />
        <asp:Label ID="Label1" runat="server" Enabled="False" EnableViewState="False"></asp:Label>
    </div>
    </form>
</body>

</html>

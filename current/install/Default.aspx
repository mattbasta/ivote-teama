<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="install_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>iVote System Installation</title>
    
    <style type="text/css">
          .good
          {
              color: Green;
          }
          
          .bad
          {
              color: Red;
          }
          
          .neutral
          {
              color: Orange;
          }
        .style1
        {
            font-size: x-large;
        }
        .style2
        {
            font-size: large;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <strong>
    
        <span class="style1">iVote System Installation<br /></span>
        <br />
        <span class="style2">Delete or protect the <em>install</em> folder after you have setup the system!</span></strong><br />
        <br />
        <strong>Settings.config check: </strong><br />
        baseUrl: <asp:Label ID="baseUrlStatus" runat="server"></asp:Label><br />
        <br />
        mysqlHost: <asp:Label ID="mysqlHostStatus" runat="server"></asp:Label><br />
        mysqlUser: <asp:Label ID="mysqlUserStatus" runat="server"></asp:Label><br />
        mysqlPassword: <asp:Label ID="mysqlPasswordStatus" runat="server"></asp:Label><br />
        mysqlDB: <asp:Label ID="mysqlDBStatus" runat="server"></asp:Label><br />
        <br />
        fromAddress: <asp:Label ID="fromAddressStatus" runat="server"></asp:Label><br />
        smtpHost: <asp:Label ID="smtpHostStatus" runat="server"></asp:Label><br />
        smtpPort: <asp:Label ID="smtpPortStatus" runat="server"></asp:Label><br />
        smtpUser: <asp:Label ID="smtpUserStatus" runat="server"></asp:Label><br />
        smtpPassword: <asp:Label ID="smtpPasswordStatus" runat="server"></asp:Label><br />
        smtpEnableSSL: <asp:Label ID="smtpEnableSSLStatus" runat="server"></asp:Label>
        <br />
        <br />
        <asp:Button ID="checkSettings" runat="server" onclick="checkSettings_Click" 
            Text="Recheck Settings" CausesValidation="False" />
        <br />
        <hr />
        <br />
        <strong>Database:</strong><br />
        This option will generate the initial database schema. This will completely 
        clear and reset the database.<br /><br />
        <asp:Button ID="createScheme" runat="server" onclick="createScheme_Click" 
            Text="Create Schema" CausesValidation="False" />
        <asp:Label ID="createSchemaStatus" runat="server" Text="Done" CssClass="good" Visible="False"></asp:Label>
        <br />
        <hr />
        <br />
        <strong>Admin User:</strong><br />
        This option will generate an initial user in the system with the ADMIN role. 
        After it is create, you should login and create additional user accounts and 
        delete this one.<br /><br />
        Email Address: 
        <asp:TextBox ID="email" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
            ControlToValidate="email" CssClass="bad" 
            ErrorMessage="Email address is required"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
            ControlToValidate="email" CssClass="bad" ErrorMessage="Invalid email address" 
            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
        <br />
        Password: 
        <asp:TextBox ID="password" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
            ControlToValidate="password" CssClass="bad" ErrorMessage="Password is required"></asp:RequiredFieldValidator>
        <br />
        <br />
        <asp:Button ID="createUser" runat="server" onclick="createUser_Click" 
            Text="Create User" />
        <asp:Label ID="createUserStatus" runat="server" 
            Text="Initial admin user created. You may now use the system." CssClass="good" 
            Visible="False"></asp:Label>
        <br />
        <hr />
        <br />
        <strong>Import users:</strong><br />
        This option will import users from the outlook database and insert them into 
        the user table with all the information provided.  All accounts will have the 
        default password 'ivoteteamaisawesome' and users will be required to change 
        this password upon their initial login.  <br /><br />
        <asp:Button ID="importUsers" runat="server" OnClick="importUsers_Click"
            Text="Import Users" />
            <asp:Label ID="importUserStatus" runat="server" Text="Done" CssClass="good" Visible="false"></asp:Label>
        </div>
    </form>
</body>
</html>
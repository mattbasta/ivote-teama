<%@ Page Title="" Language="C#" MasterPageFile="Site.master" AutoEventWireup="true" CodeFile="CPW.aspx.cs" Inherits="CPW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<div id="container">
    <asp:Label ID="lblForm" runat="server">
    <table>
    <tr>
    <td>New Password: </td>
    <td> <asp:TextBox TextMode="Password" ID="newPW" runat="server" /><asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="newPW" Text="Required." runat="server" /><br /></td>
    
    </tr>
    <tr>
    <td>
    Confirm Password:
    </td>
    <td>
    <asp:TextBox TextMode="Password" ID="confirmPW" runat="server" /><asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="confirmPW" Text="Required." runat="server" /><br />
    <asp:CompareValidator ID="compval" ControlToValidate="newPW" ControlToCompare="confirmPW" Type="String" ForeColor="Red" Text="The passwords do not match." runat="server" /><br /> 
    </td>
    </tr>
    <tr>
    <td>Old Password:  </td>
    <td><asp:TextBox TextMode="Password" ID="oldPW" runat="server" /><asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="oldPW" Text="Required." runat="server"  /><br /></td>
    </tr>
    <tr>
    <td><asp:Button ID="submit" Text="Submit" OnClick="submission" runat="server" /></td>
    <td><asp:Button ID="reset" Text="Reset" runat="server"  /></td>
    </tr>
    </table>
         
    </asp:Label>
    <asp:Label ID="lblConfirm" runat="server" />

</div>


</asp:Content>


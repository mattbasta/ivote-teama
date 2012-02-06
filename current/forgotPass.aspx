<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="forgotPass.aspx.cs" Inherits="phase1aSite_forgotPass" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <h2>
        Forgot Password
    </h2>

    <asp:Label ID="lblForgot" runat="server">
        <p>
            Please enter the following information and you will receive an email with instructions to retrieve your password.
        </p>

        <asp:ValidationSummary ID="ValidationSummary" CssClass="failureNotification" 
                               ValidationGroup="RegisterUserValidationGroup" runat="server" />
                    
        <div class="accountInfo">
                <p>
                    Email Address: <asp:TextBox ID="email" runat="server"></asp:TextBox> 
                    <asp:RequiredFieldValidator ControlToValidate="email" runat="server" ForeColor="Red" Text=" Required." />
                </p>
        </div>
        <asp:Button ID="forgotSubmit" runat="server" Text="Submit" OnClick="submit" />
    </asp:Label> 

    <asp:Label ID="lblConfirm" runat="server" Visible="false">
                
                You should receive an email within 10 minutes in regards to your password.<br /><br />
                <a href="Default.aspx">Return to log in page.</a>
    </asp:Label>

    <asp:Label ID="lblError" runat="server" Visible="false">
                
                The email address you have entered does not exist in our system.<br /><br />
                <a href="forgotPass.aspx">Please enter it again.</a>
    </asp:Label>


</asp:Content>


<%@ Page Title="Forgot Password" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ForgotPass.aspx.cs" Inherits="phase1aSite_ForgotPass" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <div class="page-header">
        <h1>Forgot Password</h1>
    </div>

    <asp:Panel ID="lblConfirm" runat="server" Visible="false" CssClass="alert alert-success">
        <strong>Password Reset</strong>
        You should receive an email within 10 minutes in regards to your password. <a href="/" class="btn btn-success">Return to log in page.</a>
    </asp:Panel>

    <asp:Panel ID="lblError" runat="server" Visible="false" CssClass="alert alert">
        <strong>Invalid Email Address</strong>
        The email address you have entered does not exist in our system.
    </asp:Panel>

    <asp:ValidationSummary ID="ValidationSummary" CssClass="failureNotification" ValidationGroup="RegisterUserValidationGroup" runat="server" />
    <asp:Panel ID="lblForgot" runat="server" CssClass="form form-horizontal">
        <p>
            Please enter the following information and you will receive an email with instructions to retrieve your password.
        </p>
        <div class="control-group">
            <label class="control-label">Email Address</label>
            <div class="controls">
                <asp:TextBox ID="email" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="email" runat="server" ForeColor="Red" Text=" Required." CssClass="error help-block" />
            </div>
        </div>
        <div class="form-actions">
            <asp:Button ID="forgotSubmit" runat="server" Text="Submit" OnClick="submit" CssClass="btn btn-primary" />
        </div>
    </asp:Panel>
    
</asp:Content>


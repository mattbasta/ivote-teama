<%@ Page Title="Confirmation" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Confirm.aspx.cs" Inherits="Confirm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="page-header">
        <h1>Confirmation</h1>
    </div>
    
    <asp:Panel ID="PanelError" runat="server" Visible="false" CssClass="alert">
    <strong>Error</strong> A problem has occurred while attempting to verify your account. Please
    contact the system administrator so they can re-send the email verification
    link to you.
    </asp:Panel>

    <p><asp:Label ID="LabelFeedback" runat="server" Text=""></asp:Label></p>
    <asp:Panel ID="PanelHide"  runat="server">
        <fieldset>
            <legend>Activate Your Account</legend>
            <div class="control-group">
                <label class="control-label">New Password</label>
                <div class="controls">
                    <asp:TextBox ID="TextBoxPassword" TextMode="Password" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                        ControlToValidate="TextBoxPassword" CssClass="red" Display="Dynamic" 
                        ErrorMessage="Please enter your new password."></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="control-group">
                <label class="control-label">Type it Again</label>
                <div class="controls">
                    <asp:TextBox ID="TextBoxPassword2" TextMode="Password" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                        ControlToValidate="TextBoxPassword2" CssClass="red" Display="Dynamic" 
                        ErrorMessage="Please confirm your new password."></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="TextBoxPassword2" CssClass="red" Display="Dynamic" ControlToCompare="TextBoxPassword" ErrorMessage="Your passwords must match." />
                </div>
            </div>
            <asp:HiddenField ID="HiddenFieldPassword" runat="server" />
            <div class="form-actions">
                <asp:Button ID="ButtonSave" runat="server" Text="Activate Account" onclick="ButtonSave_Clicked" CssClass="btn btn-primary" />
            </div>
            

        </fieldset>
    </asp:Panel>

    <asp:Panel ID="lblConfirm" Visible="false" runat="server" CssClass="alert alert-success">
        <strong>Password Set</strong>
        Your new account password has been successfully saved. You can now log into the system.
        <a href="login.aspx" class="btn btn-success">Log In Now<a />
    </asp:Panel>



</asp:Content>


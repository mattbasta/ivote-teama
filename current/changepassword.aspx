<%@ Page Title="Change Password - iVote" Language="C#" MasterPageFile="Site.master" AutoEventWireup="true" CodeFile="changepassword.aspx.cs" Inherits="CPW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<div class="page-header">
    <h1>Change Password</h1>
</div>

<asp:Panel ID="SuccessPanel" runat="server" Visible="False">
    <div class="alert alert-success">
        <strong>Password Updated Successfully</strong><br />
        Your password was updated and the changes were saved. Your new password is effective immediately.
    </div>
</asp:Panel>
<asp:Panel ID="UpdatePanel" runat="server" CssClass="form-horizontal">
    <asp:Panel ID="FailurePanel" runat="server" Visible="False">
        <div class="alert alert-error">
            <strong>Error</strong><br />
            <asp:Label ID="FailureMessage" runat="server" Text=""></asp:Label>
        </div>
    </asp:Panel>
    <div class="control-group">
        <asp:Label ID="NewPasswordLabel" runat="server" Text="New Password:" CssClass="control-label"></asp:Label>
        <div class="controls">
            <asp:TextBox TextMode="Password" ID="newPW" runat="server" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" 
                ControlToValidate="newPW" Text="Required." runat="server" 
                CssClass="help-inline" EnableClientScript="False" />
        </div>
    </div>
    <div class="control-group">
        <asp:Label ID="ConfirmPasswordLabel" runat="server" Text="Type it Again:" CssClass="control-label"></asp:Label>
        <div class="controls">
            <asp:TextBox TextMode="Password" ID="confirmPW" runat="server" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" 
                ControlToValidate="confirmPW" Text="Required." runat="server" 
                CssClass="help-inline" EnableClientScript="False" />
            <asp:CompareValidator ID="CompareValidator1" ControlToValidate="newPW" 
                ControlToCompare="confirmPW" Type="String" ForeColor="Red" 
                Text="The passwords do not match." runat="server" CssClass="help-inline" 
                EnableClientScript="True" />
        </div>
    </div>
    <div class="control-group">
        <asp:Label ID="OldPasswordLabel" runat="server" Text="Old Password:" CssClass="control-label"></asp:Label>
        <div class="controls">
            <asp:TextBox TextMode="Password" ID="oldPW" runat="server" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" 
                ControlToValidate="oldPW" Text="Required." runat="server" 
                CssClass="help-inline" EnableClientScript="False"  />
            <p class="help-block">For security, we need you to enter your old password.</p>
        </div>
    </div>
    <div class="form-actions">
        <asp:Button ID="submit" Text="Submit" OnClick="submission" runat="server" CssClass="btn btn-primary" />
    </div>
</asp:Panel>

</asp:Content>


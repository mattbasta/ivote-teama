<%@ Page Title="Login - iVote" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="page-header">
        <h1>Log In</h1>
    </div>
    
    <asp:Login ID="LoginUser" runat="server" EnableViewState="false" RenderOuterTable="false" FailureText='<div class="alert alert-error">Your login attempt was not successful. Please try again.</div>'>
        <LayoutTemplate>
            <div class="row"><div class="offset3 span6">
                <asp:Literal ID="FailureText" runat="server"></asp:Literal>
            </div></div>
            <asp:ValidationSummary ID="LoginUserValidationSummary" runat="server" CssClass="failureNotification" 
                 ValidationGroup="LoginUserValidationGroup" DisplayMode="List"/>
            <div class="row">
                <div class="offset3 span6 form-horizontal">
                    <p>Please sign in using your username and password.</p>
                    <div class="control-group">
                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" CssClass="control-label">Username:</asp:Label>
                        <div class="controls">
                            <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" 
                                CssClass="failureNotification" ErrorMessage="User Name is required." ToolTip="User Name is required." 
                                ValidationGroup="LoginUserValidationGroup" Visible="False">*</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="control-group">
                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" CssClass="control-label">Password:</asp:Label>
                        <div class="controls">
                            <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" 
                                    ErrorMessage="Password is required." ToolTip="Password is required." 
                                    ValidationGroup="LoginUserValidationGroup" Visible="False">*</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-actions">
                        <asp:Button ID="Button1" runat="server" CommandName="Login" Width="100" 
                            Text="Log In" ValidationGroup="LoginUserValidationGroup" CssClass="btn btn-primary"/>
                        <a href="ForgotPass.aspx" class="btn">Forgot Password?</a>
                    </div>
                </div>
            </div>
        </LayoutTemplate>
    </asp:Login>
    <div class="row">
        <div class="offset3 span6">
            <p class="well">
                Please visit APSCUF's website for more information <a href="http://www.apscuf.com">here</a>.
                For the Kutztown University chapter homepage, click <a href="http://www.apscuf.com/kutztown/index.html">here</a>.
            </p>
        </div>
    </div>
</asp:Content>


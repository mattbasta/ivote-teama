﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="userinfo.aspx.cs" Inherits="wwwroot_phase1aSite_userinfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <ul class="breadcrumb">
        <li><a href="/home.aspx">Home</a> <span class="divider">/</span></li>
        <li><a href="/users.aspx">Users</a> <span class="divider">/</span></li>
        <li class="active">User Info</li>
    </ul>
    
    <asp:ScriptManager runat="server" />
    <asp:HiddenField ID="HiddenFieldID" runat="server" />
    
    <div class="page-header">
        <h1><asp:Label ID="LabelFullname" runat="server" Text="" /></h1>
    </div>
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <p>
                <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email">E-mail:</asp:Label>
                <asp:TextBox ID="Email" runat="server" Enabled="false" CssClass="textEntry"></asp:TextBox>
                <asp:LinkButton ID="LinkButtonChangeEmail" Text="Change" OnClick="LinkButtonChangeEmail_Clicked" runat="server" />
                <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email" 
                        CssClass="failureNotification" ErrorMessage="E-mail is required." ToolTip="E-mail is required." 
                        ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
            </p>
        </ContentTemplate>
    </asp:UpdatePanel>
    <p>
        <asp:Label ID="FirstNameLabel" runat="server" AssociatedControlID="FirstName">First Name:</asp:Label>
        <asp:TextBox ID="FirstName" runat="server" CssClass="textEntry" ></asp:TextBox>
        <asp:RequiredFieldValidator ID="FirstNameRequired" runat="server" ControlToValidate="FirstName" 
                CssClass="failureNotification" ErrorMessage="First Name is required." ToolTip="First Name is required." 
                ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
        <br /><asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="FirstName" ValidationExpression="^[a-zA-Z0-9\s.\-'!?]+$" Display="Dynamic" CssClass="failureNotification" runat="server" ErrorMessage="Please only use alphanumeric characters in your name." />
    </p>
    <p>
        <asp:Label ID="LastNameLabel" runat="server" AssociatedControlID="LastName">Last Name:</asp:Label>
        <asp:TextBox ID="LastName" runat="server" CssClass="textEntry" ></asp:TextBox>
        <asp:RequiredFieldValidator ID="LastNameRequired" runat="server" ControlToValidate="LastName" 
                CssClass="failureNotification" ErrorMessage="Last Name is required." ToolTip="Last Name is required." 
                ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
        <br /><asp:RegularExpressionValidator ID="RegularExpressionValidator3" ControlToValidate="LastName" ValidationExpression="^[a-zA-Z0-9\s.\-'!?]+$" Display="Dynamic" CssClass="failureNotification" runat="server" ErrorMessage="Please only use alphanumeric characters in your name." />
    </p>
    <p>
        <asp:Label ID="PhoneLabel" runat="server" AssociatedControlID="Phone">Phone:</asp:Label>
        <asp:TextBox ID="Phone" runat="server" CssClass="textEntry" ></asp:TextBox>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationExpression="^(\d{3}-\d{3}-\d{4})*$" 
                                ControlToValidate="Phone" CssClass="failureNotification"  runat="server" Display="Dynamic" 
                                ErrorMessage="Please use the format ###-###-####" />
    </p>
    <p>
        <asp:Label ID="Dept" runat="server" AssociatedControlID="DeptDropDown">Department:</asp:Label>
        <asp:DropDownList ID="DeptDropDown" runat="server" >
            <asp:ListItem></asp:ListItem>
        </asp:DropDownList>                            
        <asp:RequiredFieldValidator ID="DeptRequired" runat="server" ControlToValidate="DeptDropDown" 
                CssClass="failureNotification" ErrorMessage="Department is required." ToolTip="Department is required." 
                ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
    </p>
    <asp:RadioButtonList ID="RadioButtonListRoles" runat="server">
                                <asp:ListItem Value="faculty" Text="This is a FACULTY account" />
                                <asp:ListItem Value="nec" Text="This is a NEC account" />
                                <asp:ListItem Value="admin" Text="This is a PEER ADMIN account" />
                                </asp:RadioButtonList>
    <br />
    <asp:Button ID="ButtonSave" OnClick="ButtonSave_Clicked" runat="server" Text="Save Changes" />
    <asp:Button ID="ButtonDelete" OnClick="ButtonDelete_Clicked" runat="server" Text="Delete Account" />
    <asp:Button ID="ButtonCancel" runat="server" OnClick="returnToUsersPage" Text="Cancel" />
    <asp:HiddenField ID="HiddenFieldUsername" runat="server" />
</asp:Content>


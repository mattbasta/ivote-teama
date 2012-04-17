<%@ Page Title="User Inspector" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="wwwroot_phase1aSite_UserInfo" CodeFile="UserInfo.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server"></asp:Content>
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

    <asp:Panel ID="SuccessPanel" runat="server" Visible="false" CssClass="alert alert-success">
        <strong>User updated successfully.</strong>
        The user has been successfully updated.
    </asp:Panel>

    <asp:Panel ID="FailurePanel" runat="server" Visible="false" CssClass="alert alert-error">
        <strong>User was not updated.</strong>
        The user is either ineligible for this committee, or the committee is full.
    </asp:Panel>

    <asp:Panel ID="InElectionPanel" runat="server" Visible="false" CssClass="alert alert-error">
        <strong>User was not updated.</strong>
        You cannot add users to a committee that is currently in an election cycle.
    </asp:Panel>
    
    <fieldset class="form form-horizontal">
        <legend>Account Information</legend>
        <div class="control-group">
            <asp:Label CssClass="control-label" ID="EmailLabel" runat="server" AssociatedControlID="Email">E-mail:</asp:Label>
            <div class="controls">
                <asp:TextBox ID="Email" runat="server" CssClass="textEntry" Enabled="false"></asp:TextBox>
            </div>
        </div>
        <div class="control-group">
            <asp:Label CssClass="control-label" ID="FirstNameLabel" runat="server" AssociatedControlID="FirstName">First Name:</asp:Label>
            <div class="controls">
                <asp:TextBox ID="FirstName" runat="server" CssClass="textEntry" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="FirstNameRequired" runat="server" ControlToValidate="FirstName"
                    CssClass="red" ErrorMessage="Please enter the new user's first name." ToolTip="First Name is required."
                    ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="control-group">
            <asp:Label CssClass="control-label" ID="LastNameLabel" runat="server" AssociatedControlID="LastName">Last Name:</asp:Label>
            <div class="controls">
                <asp:TextBox ID="LastName" runat="server" CssClass="textEntry" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="LastNameRequired" runat="server" ControlToValidate="LastName"
                    CssClass="red" ErrorMessage="Please enter the new user's last name." ToolTip="Last Name is required."
                    ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="control-group">
            <asp:Label CssClass="control-label" ID="Dept" runat="server" AssociatedControlID="DeptDropDown">Department:</asp:Label>
            <div class="controls">
                <asp:DropDownList ID="DeptDropDown" runat="server" CssClass="input-small">
                    <asp:ListItem>Select...</asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="DeptRequired" runat="server" ControlToValidate="DeptDropDown"
                    CssClass="red" ErrorMessage="Please select a department for the new user." InitialValue="Select..."
                    ToolTip="Department is required."
                    ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="control-group">
            <asp:Label CssClass="control-label" ID="CurrComm" runat="server" AssociatedControlID="CurrentCommittee">Current Committee:</asp:Label>
            <div class="controls">
                <asp:DropDownList ID="CurrentCommittee" runat="server">
                    <asp:ListItem Value="-1">None</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="control-group">
            <label class="control-label">User Roles</label>
            <div class="controls">
                <label class="checkbox"><asp:CheckBox ID="IsAdmin" runat="server" /> Administrator</label>
                <label class="checkbox"><asp:CheckBox ID="IsNEC" runat="server" /> NEC</label>
                <label class="checkbox"><asp:CheckBox ID="IsFaculty" runat="server" /> Faculty</label>
                <label class="checkbox"><asp:CheckBox ID="IsTenured" runat="server" /> Tenured Faculty</label>
                <label class="checkbox"><asp:CheckBox ID="IsUnion" runat="server" /> Union Member</label>
                <label class="checkbox"><asp:CheckBox ID="IsBU" runat="server" /> Bargaining Unit</label>
            </div>
        </div>
        <div class="control-group">
            <label class="control-label">User Permissions</label>
            <div class="controls">
                <label class="checkbox"><asp:CheckBox ID="CanVote" runat="server" /> Allowed to Vote</label>
            </div>
        </div>
        <div class="form-actions">
            <asp:Button ID="ButtonDelete" OnClick="ButtonDelete_Clicked" runat="server" Text="Delete Account" CssClass="btn btn-danger pull-right" />
            <asp:Button ID="ButtonSave" OnClick="ButtonSave_Clicked" runat="server" Text="Save Changes" CssClass="btn btn-primary" />
            <a href="/users.aspx" class="btn">Back</a>
        </div>
    </fieldset>
    
</asp:Content>


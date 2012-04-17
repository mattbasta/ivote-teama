<%@ Page Title="Register - iVote" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="Account_Register" CodeFile="Register.aspx.cs" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
<script type="text/javascript">
<!--
$(document).ready(function() {
    $("input[type=radio]").each(function() {
        var _this = $(this),
            L = _this.next("label");
        _this.remove();
        L.addClass("radio");
        L.prepend(_this);
    });
});
-->
</script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <ul class="breadcrumb">
        <li><a href="/home.aspx">Home</a> <span class="divider">/</span></li>
        <li><a href="/users.aspx">Users</a> <span class="divider">/</span></li>
        <li class="active">User Info</li>
    </ul>

    <div class="page-header">
        <h1>Create New User</h1>
    </div>

    <asp:Panel ID="SuccessPanel" runat="server" Visible="false" CssClass="alert alert-success">
        <strong>User Created Successfully</strong>
        The user should recieve a validation email within the next 10 minutes.
    </asp:Panel>
    <asp:Panel ID="ConflictPanel" runat="server" Visible="false" CssClass="alert">
        <strong>Conflict</strong>
        Adding this user to the specified committee would cause a conflict within that committee.
    </asp:Panel>

    <p>Use the form below to create a new account.</p>

    <!--Feedback label for admin if this user is already in the database -->
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Label ID="LabelFeedback" CssClass="red" runat="server" Text="" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="accountInfo">
        <fieldset class="form form-horizontal">
            <legend>Account Information</legend>
            <div class="control-group">
                <asp:Label CssClass="control-label" ID="EmailLabel" runat="server" AssociatedControlID="Email">E-mail:</asp:Label>
                <div class="controls">
                    <asp:TextBox ID="Email" runat="server" CssClass="textEntry"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email"
                         CssClass="red" Display="Dynamic" ErrorMessage="Please enter the new user's e-mail address." ToolTip="E-mail is required."
                         ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator runat="server"
                        ValidationExpression="^([\w\-\.]+)@((\[([0-9]{1,3}\.){3}[0-9]{1,3}\])|(([\w\-]+\.)+)([a-zA-Z]{2,4}))$"
                        CssClass="red" ControlToValidate="Email" ValidationGroup="RegisterUserValidationGroup" Display="Dynamic"
                        ErrorMessage="Please enter a valid e-mail address.">*</asp:RegularExpressionValidator>
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
                    <label class="checkbox"><asp:CheckBox ID="IsFaculty" runat="server" Checked="true" /> Faculty</label>
                    <label class="checkbox"><asp:CheckBox ID="IsTenured" runat="server" /> Tenured Faculty</label>
                    <label class="checkbox"><asp:CheckBox ID="IsUnion" runat="server" Checked="true" /> Union Member</label>
                    <label class="checkbox"><asp:CheckBox ID="IsBU" runat="server" Checked="true" /> Bargaining Unit</label>
                </div>
            </div>
            <div class="control-group">
                <label class="control-label">User Permissions</label>
                <div class="controls">
                    <label class="checkbox"><asp:CheckBox ID="CanVote" runat="server" Checked="true" /> Allowed to Vote</label>
                </div>
            </div>
            <div class="form-actions">
                <asp:Button ID="CreateUserButton" runat="server" CommandName="MoveNext" Text="Create User" OnClick="submit"
                     ValidationGroup="RegisterUserValidationGroup" CssClass="btn btn-primary" />
            </div>
        </fieldset>
    </div>
</asp:Content>

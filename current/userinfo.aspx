<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="userinfo.aspx.cs" Inherits="wwwroot_phase1aSite_userinfo" %>

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
        <p>The user has been successfully updated.</p>
    </asp:Panel>
    
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
            <label class="control-label">User Roles</label>
            <div class="controls">
                <label class="checkbox"><asp:CheckBox ID="IsAdmin" runat="server" /> Administrator</label>
                <label class="checkbox"><asp:CheckBox ID="IsNEC" runat="server" /> NEC</label>
                <!-- <label class="checkbox"><asp:CheckBox ID="IsFaculty" runat="server" /> Faculty</label> TODO: BUG 31 -->
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


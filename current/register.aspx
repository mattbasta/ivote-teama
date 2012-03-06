<%@ Page Title="Register - iVote" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="register.aspx.cs" Inherits="Account_Register" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
                   
                    <h2>
                        Create a New Account
                    </h2>

        <asp:Label ID="lblForm" runat="server">
                    <p>
                        Use the form below to create a new account.
                    </p>
                    
               
               <asp:ValidationSummary ID="ValidationSummary" CssClass="failureNotification" 
                                      ValidationGroup="RegisterUserValidationGroup" runat="server" />

              <!--Feedback label for admin if this user is already in the database -->
            <asp:ScriptManager runat="server" />
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:Label ID="LabelFeedback" CssClass="red" runat="server" Text="" />
                </ContentTemplate>
            </asp:UpdatePanel>
               
                    <div class="accountInfo"><fieldset class="register" runat="server">
                        <legend>Account Information</legend>
                            <p>
                               <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email">E-mail:</asp:Label> 
                                <asp:TextBox ID="Email" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email" 
                                     CssClass="red" Display="Dynamic" ErrorMessage="Please enter the new user's e-mail address." ToolTip="E-mail is required." 
                                     ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator runat="server" 
                                ValidationExpression="^([\w\-\.]+)@((\[([0-9]{1,3}\.){3}[0-9]{1,3}\])|(([\w\-]+\.)+)([a-zA-Z]{2,4}))$" 
                                CssClass="red" ControlToValidate="Email" ValidationGroup="RegisterUserValidationGroup" Display="Dynamic"
                                ErrorMessage="Please enter a valid e-mail address.">*</asp:RegularExpressionValidator>
                            </p>
                            <p>
                                <asp:Label ID="FirstNameLabel" runat="server" AssociatedControlID="FirstName">First Name:</asp:Label>
                                <asp:TextBox ID="FirstName" runat="server" CssClass="textEntry" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="FirstNameRequired" runat="server" ControlToValidate="FirstName" 
                                     CssClass="red" ErrorMessage="Please enter the new user's first name." ToolTip="First Name is required." 
                                     ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                            </p>
                            <p>
                                <asp:Label ID="LastNameLabel" runat="server" AssociatedControlID="LastName">Last Name:</asp:Label>
                                <asp:TextBox ID="LastName" runat="server" CssClass="textEntry" ></asp:TextBox>  
                                <asp:RequiredFieldValidator ID="LastNameRequired" runat="server" ControlToValidate="LastName" 
                                     CssClass="red" ErrorMessage="Please enter the new user's last name." ToolTip="Last Name is required." 
                                     ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                             </p>
                             <!--<p>
                                <asp:Label ID="PhoneLabel" runat="server" AssociatedControlID="Phone">Phone (Ex: 555-123-3456):</asp:Label>
                                <asp:TextBox ID="Phone" runat="server" ></asp:TextBox>
                                <asp:RegularExpressionValidator ValidationExpression="^(\d{3}-\d{3}-\d{4})*$" 
                                ControlToValidate="Phone" runat="server" Display="Dynamic" 
                                ErrorMessage="Please use the format ###-###-####" />
                             </p>-->
                            <p>
                                <asp:Label ID="Dept" runat="server" AssociatedControlID="DeptDropDown">Department:</asp:Label>
                                <asp:DropDownList ID="DeptDropDown" runat="server" >
                                    <asp:ListItem>Select...</asp:ListItem>                 
                                </asp:DropDownList>                            
                                <asp:RequiredFieldValidator ID="DeptRequired" runat="server" ControlToValidate="DeptDropDown" 
                                     CssClass="red" ErrorMessage="Please select a department for the new user." InitialValue="Select..."
                                     ToolTip="Department is required." 
                                     ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                            </p>

                   </fieldset>   
                        <asp:RadioButtonList ID="RadioButtonListRoles" runat="server">
                                <asp:ListItem Value="faculty" Selected="True" Text="This is a FACULTY account" />
                                <asp:ListItem Value="nec" Text="This is an NEC account" />
                                <asp:ListItem Value="admin" Text="This is a PEER ADMIN account" />
                                </asp:RadioButtonList>
                        <p class="submitButton">
                            <asp:Button ID="CreateUserButton" runat="server" CommandName="MoveNext" Text="Create User" OnClick="submit" 
                                 ValidationGroup="RegisterUserValidationGroup"/>
                           <asp:Button ID="ClearFieldButton" runat="server" Text="Clear" OnClientClick="this.form.reset();return false" /> 
                        </p>
                    
                    </div>
            </asp:Label>
            <asp:Label ID="lblConfirm" runat="server" Visible="false">
                
                The account has been submitted successfully.<br /><br />
                The user should recieve a validation email within the next 10 minutes.
            
            </asp:Label>
                               
</asp:Content>

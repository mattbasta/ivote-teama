<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Register.aspx.cs" Inherits="Account_Register" %>

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
                             <p>
                                <asp:Label ID="PhoneLabel" runat="server" AssociatedControlID="Phone">Phone (Ex: 555-123-3456):</asp:Label>
                                <asp:TextBox ID="Phone" runat="server" ></asp:TextBox>
                                <asp:RegularExpressionValidator ValidationExpression="^(\d{3}-\d{3}-\d{4})*$" 
                                ControlToValidate="Phone" runat="server" Display="Dynamic" 
                                ErrorMessage="Please use the format ###-###-####" />
                             </p>
                            <p>
                                <asp:Label ID="Dept" runat="server" AssociatedControlID="DeptDropDown">Department:</asp:Label>
                                <asp:DropDownList ID="DeptDropDown" runat="server" >
                                    <asp:ListItem>Select...</asp:ListItem>
                                    <asp:ListItem>ACC</asp:ListItem>
                                    <asp:ListItem>AER</asp:ListItem>
                                    <asp:ListItem>AMS</asp:ListItem>
                                    <asp:ListItem>ANT</asp:ListItem>
                                    <asp:ListItem>ARA</asp:ListItem>
                                    <asp:ListItem>ART</asp:ListItem>
                                    <asp:ListItem>ARC</asp:ListItem>
                                    <asp:ListItem>ARU</asp:ListItem>
                                    <asp:ListItem>ARH</asp:ListItem>
                                    <asp:ListItem>ASE</asp:ListItem>
                                    <asp:ListItem>AST</asp:ListItem>
                                    <asp:ListItem>AVC</asp:ListItem>
                                    <asp:ListItem>BTE</asp:ListItem>
                                    <asp:ListItem>BIO</asp:ListItem>
                                    <asp:ListItem>BUS</asp:ListItem>
                                    <asp:ListItem>CHM</asp:ListItem>
                                    <asp:ListItem>CHI</asp:ListItem>
                                    <asp:ListItem>CMI</asp:ListItem>
                                    <asp:ListItem>CDE</asp:ListItem>
                                    <asp:ListItem>CSC</asp:ListItem>
                                    <asp:ListItem>CPY</asp:ListItem>
                                    <asp:ListItem>CFT</asp:ListItem>
                                    <asp:ListItem>CRJ</asp:ListItem>
                                    <asp:ListItem>DAN</asp:ListItem>
                                    <asp:ListItem>DVE</asp:ListItem>
                                    <asp:ListItem>DVR</asp:ListItem>
                                    <asp:ListItem>ECO</asp:ListItem>
                                    <asp:ListItem>EDU</asp:ListItem>
                                    <asp:ListItem>EDW</asp:ListItem>
                                    <asp:ListItem>ELU</asp:ListItem>
                                    <asp:ListItem>EGR</asp:ListItem>
                                    <asp:ListItem>ENU</asp:ListItem>
                                    <asp:ListItem>ENV</asp:ListItem>
                                    <asp:ListItem>FIN</asp:ListItem>
                                    <asp:ListItem>FAR</asp:ListItem>
                                    <asp:ListItem>FAS</asp:ListItem>
                                    <asp:ListItem>FLA</asp:ListItem>
                                    <asp:ListItem>FRE</asp:ListItem>
                                    <asp:ListItem>FRS</asp:ListItem>
                                    <asp:ListItem>GEG</asp:ListItem>
                                    <asp:ListItem>GEl</asp:ListItem>
                                    <asp:ListItem>GER</asp:ListItem>
                                    <asp:ListItem>HEA</asp:ListItem>
                                    <asp:ListItem>HIS</asp:ListItem>
                                    <asp:ListItem>HPD</asp:ListItem>
                                    <asp:ListItem>HUM</asp:ListItem>
                                    <asp:ListItem>ITC</asp:ListItem>
                                    <asp:ListItem>INT</asp:ListItem>
                                    <asp:ListItem>IST</asp:ListItem>
                                    <asp:ListItem>LIB</asp:ListItem>
                                    <asp:ListItem>MGM</asp:ListItem>
                                    <asp:ListItem>MAR</asp:ListItem>
                                    <asp:ListItem>MAT</asp:ListItem>
                                    <asp:ListItem>MKT</asp:ListItem>
                                    <asp:ListItem>MAU</asp:ListItem>
                                    <asp:ListItem>MED</asp:ListItem>
                                    <asp:ListItem>MIC</asp:ListItem>
                                    <asp:ListItem>MIL</asp:ListItem>
                                    <asp:ListItem>MLS</asp:ListItem>
                                    <asp:ListItem>MCS</asp:ListItem>
                                    <asp:ListItem>MUS</asp:ListItem>
                                    <asp:ListItem>MUU</asp:ListItem>
                                    <asp:ListItem>MUP</asp:ListItem>
                                    <asp:ListItem>NSE</asp:ListItem>
                                    <asp:ListItem>NUR</asp:ListItem>
                                    <asp:ListItem>Other</asp:ListItem>
                                    <asp:ListItem>PLG</asp:ListItem>
                                    <asp:ListItem>PHI</asp:ListItem>
                                    <asp:ListItem>PED</asp:ListItem>
                                    <asp:ListItem>PHY</asp:ListItem>
                                    <asp:ListItem>POL</asp:ListItem>
                                    <asp:ListItem>PRO</asp:ListItem>
                                    <asp:ListItem>PSY</asp:ListItem>
                                    <asp:ListItem>RAR</asp:ListItem>
                                    <asp:ListItem>RSS</asp:ListItem>
                                    <asp:ListItem>RUS</asp:ListItem>
                                    <asp:ListItem>SCI</asp:ListItem>
                                    <asp:ListItem>SCU</asp:ListItem>
                                    <asp:ListItem>SEU</asp:ListItem>
                                    <asp:ListItem>SSC</asp:ListItem>
                                    <asp:ListItem>SSE</asp:ListItem>
                                    <asp:ListItem>SSU</asp:ListItem>
                                    <asp:ListItem>SWK</asp:ListItem>
                                    <asp:ListItem>SOC</asp:ListItem>
                                    <asp:ListItem>SPA</asp:ListItem>
                                    <asp:ListItem>SPU</asp:ListItem>
                                    <asp:ListItem>SPE</asp:ListItem>
                                    <asp:ListItem>Staff</asp:ListItem>
                                    <asp:ListItem>THE</asp:ListItem>
                                    <asp:ListItem>TVR</asp:ListItem>
                                    <asp:ListItem>UST</asp:ListItem>
                                    <asp:ListItem>WRI</asp:ListItem>
                                    <asp:ListItem>WST</asp:ListItem>                                  
                                </asp:DropDownList>                            
                                <asp:RequiredFieldValidator ID="DeptRequired" runat="server" ControlToValidate="DeptDropDown" 
                                     CssClass="red" ErrorMessage="Please select a department for the new user." InitialValue="Select..."
                                     ToolTip="Department is required." 
                                     ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                            </p>

                   </fieldset>   
                        <asp:RadioButtonList ID="RadioButtonListRoles" runat="server">
                                <asp:ListItem Value="faculty" Selected="True" Text="This is a FACULTY account" />
                                <asp:ListItem Value="pres" Text="This is an NEC account" />
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
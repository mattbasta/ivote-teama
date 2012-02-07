<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="userinfo.aspx.cs" Inherits="wwwroot_phase1aSite_userinfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ScriptManager runat="server" />

    <asp:HiddenField ID="HiddenFieldID" runat="server" />
    <h2><asp:Label ID="LabelFullname" runat="server" Text="" /></h2>
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
                                    <asp:ListItem>Other</asp:ListItem>
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


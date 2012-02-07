
<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="WTS.aspx.cs" Inherits="experimental_WTS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <h2><asp:Label ID="LabelHeader" runat="server" Text="" /></h2>
    <p><asp:Label ID="LabelExplain" runat="server" Text="" /></p>

    <asp:ValidationSummary ID="ValidationSummary" CssClass="failureNotification" 
                                      ValidationGroup="WTSValidationGroup" runat="server" />

              <!--Feedback label for admin if this user is already in the database -->
            <asp:ScriptManager ID="ScriptManager1" runat="server" />
            <asp:UpdatePanel  ID="UpdatePanel1" runat="server" >
            <Triggers>
                <asp:PostBackTrigger ControlID="Submit" />
            </Triggers>
                <ContentTemplate>
                    <asp:Label ID="LabelFeedback" CssClass="red" runat="server" Text="" />
                    
                    <div class="accountInfo"><fieldset id="Fieldset2" class="register" runat="server">
                        <legend>Willingness To Serve Information</legend>
                            <p>
                               <asp:Label ID="NameLabel" runat="server" AssociatedControlID="Name">Name:</asp:Label> 
                               <asp:TextBox ID="Name" runat="server" CssClass="textEntry" Enabled="false"></asp:TextBox>
                            </p>
                            <p>
                                <asp:Label ID="DeptLabel" runat="server" AssociatedControlID="Dept">Department:</asp:Label>
                                <asp:TextBox ID="Dept" runat="server" CssClass="textEntry" Enabled="false"></asp:TextBox>
                            </p><br />
                            <p>
                                <asp:Label ID="StatementLabel" runat="server" AssociatedControlID="Statement">Statement:</asp:Label>
                                <asp:TextBox ID="Statement" runat="server" TextMode="MultiLine" Width="320px" Height="100px"></asp:TextBox>                                  
                            </p>
                    </div>
                    </fieldset>

                    <asp:Label ID="AcceptLabel" runat="server" AssociatedControlID="Accept"></asp:Label>
                    <asp:CheckBox ID="Accept" runat="server" /> <b>I confirm that I am willing to serve as an APSCUF Official. </b><br />
                    <asp:Label ID="AcceptError" runat="server" Visible="false" CssClass="red">*Please confirm your willingness to serve.</asp:Label><br />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationGroup="check" ControlToValidate="Statement" ValidationExpression="^[a-zA-Z0-9\s.\-'!?]+$" Display="Dynamic" CssClass="red" runat="server" ErrorMessage="Please only use alphanumeric characters and normal punctuation in your statement." />

                    <p class="submitButton">         
                        <asp:Button ID="Submit" runat="server" ValidationGroup="check" Text="Submit" OnClick="submit"/>
                    </p>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:HiddenField ID="HiddenFieldPosition" runat="server" />
            <asp:Label ID="Confirm" runat="server" Text="Your willingness to serve has been completed!" Visible="false"></asp:Label>
</asp:Content>


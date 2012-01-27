<%@ Page Title="Terminate Current Election - iVote" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="terminate.aspx.cs" Inherits="wwwroot_finalsite_terminate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <br />
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="PanelTerminate" runat="server">
                To <b>terminate</b> the current election, please type a short message to send to all users explaining why the current election is being terminated. This message will be sent to everyone's email.
                <br /><br />
                By terminating the election, you will cause all data associated with this election to be deleted, including:
                The timeline, the positions, the people who have been nominated for those positions, any petition data, etc. (This will not however, delete any user account information.)
                <br /><br />
                Once the election is completely terminated, the system is considered reset and you may begin a new election at any time.
                <br /><br />
                <span class="blue">Enter message below:</span><br />
                <asp:TextBox ID="TextBoxMessage" runat="server" TextMode="MultiLine" Width="600" MaxLength="2000" Height="200" />
                <asp:Button ID="ButtonTerminate" OnClick="ButtonTerminate_clicked" runat="server" Text="Terminate The Current Election" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="red" ControlToValidate="TextBoxMessage" runat="server" ErrorMessage="You must type a message to send to all users." />
            </asp:Panel>
            <asp:Panel ID="PanelComplete" Visible="false" Enabled="false" runat="server">
                The election has been offically terminated.<br />
                All records associated with this election have been deleted.<br />
                Email to all users successfully sent.<br /> <br />
                <b>The system is considered reset and you may begin a new election at any time.</b><br /><br />
                <a href="home.aspx">Return to the homepage</a>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


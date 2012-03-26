<%@ Page Title="Terminate Current Election - iVote" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="terminate.aspx.cs" Inherits="wwwroot_finalsite_terminate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <ul class="breadcrumb">
        <li><a href="/home.aspx">Home</a> <span class="divider">/</span></li>
        <li class="active">Terminate Officer Election</li>
    </ul>
    
    <div class="page-header">
        <h1>Terminate Officer Election</h1>
    </div>
    
    <!-- No tab bar here because we don't want this to be in the user's recent history (i.e.: so they can't use Back to get to it.) -->

    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="PanelTerminate" runat="server">
                <p>To <b>terminate</b> the current election, please type a short message to send to all users explaining why the current election is being terminated. This message will be sent to everyone's email.</p>
                <p>By terminating the election, you will cause all data associated with this election to be deleted, including:</p>
                <ul>
                    <li>Officer election timeline</li>
                    <li>Officer positions</li>
                    <li>Officer nominations</li>
                    <li>Petition data for officers</li>
                </ul>
                <p>Conversely, the following information will <b>not</b> be deleted:</p>
                <ul>
                    <li>Committee election information</li>
                    <li>User information</li>
                </ul>
                <p>Once the election is completely terminated, the system is considered reset and you may begin a new election at any time.</p>
                <div class="well">
                    <label>Enter a message about the reason behind the termination below:</label>
                    <asp:TextBox ID="TextBoxMessage" runat="server" TextMode="MultiLine" Width="350" Height="100" MaxLength="2000" />
                    <div>
                        <asp:Button ID="ButtonTerminate" OnClick="ButtonTerminate_clicked" runat="server" Text="Terminate Current Officer Election" CssClass="btn btn-danger" />
                    </div>
                </div>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="red" ControlToValidate="TextBoxMessage" runat="server" ErrorMessage="You must type a message to send to all users." />
            </asp:Panel>
            <asp:Panel ID="PanelComplete" Visible="false" Enabled="false" runat="server">
                <p>The election has been offically terminated. All associated records have been successfully deleted and termination emails have been distributed to users.</p>
                <a class="btn btn-primary" href="/home.aspx">Return to the homepage</a>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


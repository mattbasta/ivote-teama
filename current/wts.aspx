<%@ Page Title="Willingness to Serve Submission" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="wts.aspx.cs" Inherits="experimental_WTS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <ul class="breadcrumb">
        <li><a href="/home.aspx">Home</a> <span class="divider">/</span></li>
        <li><a href="/officer_election.aspx">Officer Election</a> <span class="divider">/</span></li>
        <li class="active">Willingness to Serve Application</li>
    </ul>
    <div class="page-header">
        <h1>
            Willingness to Serve
            <small><asp:Label ID="LabelHeader" runat="server" Text="" /></small>
        </h1>
    </div>

    <p>If you are willing to hold this officer position, please fill out the following form:</p>

    <asp:Panel CssClass="alert alert-success" ID="Confirm" runat="server" Visible="false">
        Your Willingness to Serve application has been completed!
    </asp:Panel>
    <asp:Panel ID="wtsPanelLength" runat="server" Visible="false" CssClass="alert alert-warning">
        <strong>Not Submitted</strong>
        Your statement is too long. You are limited to 10,000 characters.
    </asp:Panel>
    <fieldset id="Fieldset2" class="form form-horizontal" runat="server">
        <legend>Willingness To Serve Application</legend>
        <div class="control-group">
            <label class="control-label">Statement</label>
            <div class="controls">
                <asp:TextBox ID="Statement" runat="server" TextMode="MultiLine" CssClass="input-xlarge"></asp:TextBox>
            </div>
        </div>
        <div class="control-group">
            <label class="control-label">Acceptance</label>
            <div class="controls">
                <label class="checkbox">
                    <asp:CheckBox ID="Accept" runat="server" />
                    I confirm that I am willing to serve as an APSCUF Official.
                </label>
                <asp:Label ID="AcceptError" runat="server" Visible="false" CssClass="help-block error">*Please confirm your willingness to serve.</asp:Label>
            </div>
        </div>

        <div class="form-actions">
            <asp:Button ID="Submit" runat="server" ValidationGroup="check" Text="Submit" OnClick="submit" Enabled="false" CssClass="btn btn-primary" />
        </div>
    </fieldset>

    <asp:HiddenField ID="HiddenFieldPosition" runat="server" />
</asp:Content>


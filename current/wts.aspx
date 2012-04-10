﻿<%@ Page Title="Willingness to Serve Submission" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="wts.aspx.cs" Inherits="experimental_WTS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
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
    
    <asp:ValidationSummary ID="ValidationSummary" CssClass="failureNotification" 
                                      ValidationGroup="WTSValidationGroup" runat="server" />

    <fieldset id="Fieldset2" class="form form-horizontal" runat="server">
        <legend>Willingness To Serve Application</legend>
        <div class="control-group">
            <label class="control-label">Statement</label>
            <div class="controls">
                <asp:TextBox ID="Statement" runat="server" TextMode="MultiLine" CssClass="input-xlarge"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationGroup="check" ControlToValidate="Statement" ValidationExpression="^[a-zA-Z0-9\s.\-'!?]+$" Display="Dynamic" CssClass="error help-block" runat="server" ErrorMessage="Please only use alphanumeric characters and normal punctuation in your statement." />
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


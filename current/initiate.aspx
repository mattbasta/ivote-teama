<%@ Page Title="Initiate Election" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="initiate.aspx.cs" Inherits="initiate" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <ul class="breadcrumb">
        <li><a href="/home.aspx">Home</a> <span class="divider">/</span></li>
        <li><a href="/officer_election.aspx">Officer Election</a> <span class="divider">/</span></li>
        <li class="active">New Officer Election</li>
    </ul>
    
    <div class="page-header">
        <h1>New Officer Election</h1>
    </div>
    
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <div class="row">
            <fieldset class="form span6">
                <legend><b>Step 1:</b> Add Officer Positions</legend>
                
                <div class="form form-horizontal control-group">
                    <div class="control-group">
                        <label class="control-label">Position Name</label>
                        <div class="controls">
                            <asp:TextBox ID="positionText" runat="server" CausesValidation="false" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                                        ValidationExpression="^[a-zA-Z0-9\s.\-'!?]+$" 
                                                        CssClass="red" ControlToValidate="positionText" Display="Dynamic"
                                                        ErrorMessage="Please enter a position using only alpha-numeric characters.">*</asp:RegularExpressionValidator>
                            <asp:Label id="lblPosError" Visible="false" runat="server" CssClass="help-block" />
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label">Vote Type</label>
                        <div class="controls">
                            <asp:DropDownList ID="voteMethodList" runat="server">
                                <asp:ListItem Text="Simple" Value="classic" />
                                <asp:ListItem Text="Majority" Value="majority" />
                                <asp:ListItem Text="Plurality" Value="plural" />
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label">Description</label>
                        <div class="controls">
                            <asp:TextBox ID="Description" runat="server" TextMode="MultiLine" Height="150px" /> <br />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                                        ValidationExpression="^[a-zA-Z0-9\s.\-'!?]+$" 
                                                        CssClass="red" ControlToValidate="Description" Display="Dynamic"
                                                        ErrorMessage="Please enter a description using only alpha-numeric characters.">*</asp:RegularExpressionValidator>
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label">Save Position</label>
                        <div class="controls">
                            <asp:Button ID="addPosition" runat="server" CausesValidation="false" Text="Add Position" OnClick="addPosition_clicked" CssClass="btn btn-primary" />
                            <asp:Label ID="LabelFeedback" runat="server" Text="" CssClass="" />
                        </div>
                    </div>
                </div>
            </fieldset>
            <fieldset class="form span6 position_list">
                <legend><b>Step 2:</b> Review Positions</legend>
                <asp:Panel ID="positionsList" runat="server" Visible="false">
                    <asp:Table ID="PositionTable" CssClass="table table-bordered" runat="server">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell>Position Name</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Vote Type</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                </asp:Panel>
                <asp:Panel ID="EmptyPositions" runat="server" Visible="true">
                    <p class="well">You have not yet added any positions to the election.</p>
                </asp:Panel>
            </fieldset>
        </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="form-actions">
        <div class="pull-right">
            <asp:Button ID="ButtonSave" runat="server" ValidationGroup="timeline" Text="Create Election" OnClick="ButtonSave_Clicked" CssClass="btn btn-primary" />
            <a href="/officer_election.aspx" class="btn">Cancel</a>
        </div>
        <div class="clear"></div>
    </div>
    <asp:Button ID="Button1" runat="server" style="display:none" Text="Button" />
    <asp:Panel ID="PanelPlurality" CssClass="modal" runat="server">
        <div class="modal-header">
            <h3>Describe Plurality</h3>
        </div>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" class="form form-horizontal">
            <ContentTemplate>
                <div class="modal-body">
                    <div class="control-group">
                        <p class="control-label">Number of available positions for this position:</p>
                        <div class="controls">
                            <asp:DropDownList ID="DropDownListPlurality" runat="server">
                                <asp:ListItem>2</asp:ListItem>
                                <asp:ListItem>3</asp:ListItem>
                                <asp:ListItem>4</asp:ListItem>
                                <asp:ListItem>5</asp:ListItem>
                                <asp:ListItem>6</asp:ListItem>
                                <asp:ListItem>7</asp:ListItem>
                                <asp:ListItem>8</asp:ListItem>
                                <asp:ListItem>9</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                            </asp:DropDownList>
                            <p class="help-block">If there is only one available position then please choose a different tally method instead of plurality.</p>
                            <asp:Label ID="popupError" runat="server" class="help-block" />
                        </div>
                    </div>
                    <div class="control-group">
                        <p class="control-label">Votes per user:</p>
                        <div class="controls">
                            <asp:DropDownList ID="DropDownListVoting" runat="server">
                                <asp:ListItem>1</asp:ListItem>
                                <asp:ListItem>2</asp:ListItem>
                                <asp:ListItem>3</asp:ListItem>
                                <asp:ListItem>4</asp:ListItem>
                                <asp:ListItem>5</asp:ListItem>
                                <asp:ListItem>6</asp:ListItem>
                                <asp:ListItem>7</asp:ListItem>
                                <asp:ListItem>8</asp:ListItem>
                                <asp:ListItem>9</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                            </asp:DropDownList>
                            <p class="help-block">This is the number of times a user can vote for this position.</p>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="ButtonPluralityComplete" runat="server" CssClass="btn btn-primary" OnClick="ButtonPluralityComplete_clicked" Text="Complete Plurality" />
                    <asp:Button ID="ButtonPluralityCancel" runat="server" Text="Cancel" CssClass="btn" OnClick="ButtonPluralityCancel_clicked" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
        TargetControlID="Button1"
        PopupControlID="PanelPlurality"
        BackgroundCssClass="modal-backdrop"
        CancelControlID="ButtonPluralityCancel" />
    
</asp:Content>


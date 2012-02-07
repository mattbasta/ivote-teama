<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="controlRoom.aspx.cs" Inherits="controlRoom" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:ToolkitScriptManager runat="server" />
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

    <asp:Label ID="LabelFeedback" runat="server" CssClass="blue" Text="" />
    <asp:Panel ID="PanelCreateElection" runat="server">
   
<b><h1>Start A New Election</h1></b><br />
<asp:Label ID="lblForm" runat="server" Visible="true">
<b>Add Positions</b><br />
<asp:Label ID="label1" runat="server" Text="Please type in the name of the position(s) you would like to add to the election." ForeColor="Blue" /><br /><br />
<asp:Panel ID="positionsAdd" runat="server">
    <asp:TextBox ID="positionText" runat="server" CausesValidation="false" />
     <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                ValidationExpression="^[a-zA-Z0-9\s.\-'!?]+$" 
                                CssClass="red" ControlToValidate="positionText" Display="Dynamic"
                                ErrorMessage="Please enter a position using only alpha-numeric characters.">*</asp:RegularExpressionValidator>
    <asp:DropDownList ID="voteMethodList" runat="server">
        <asp:ListItem Text="Simple" Value="classic" />
        <asp:ListItem Text="Majority" Value="majority" />
        <asp:ListItem Text="Plurality" Value="plural" />
    </asp:DropDownList><br />
    <asp:TextBox ID="Description" runat="server" TextMode="MultiLine" Height="150px" Width="430px"  MaxLength="5000" /> <br />
    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                ValidationExpression="^[a-zA-Z0-9\s.\-'!?]+$" 
                                CssClass="red" ControlToValidate="Description" Display="Dynamic"
                                ErrorMessage="Please enter a description using only alpha-numeric characters.">*</asp:RegularExpressionValidator>
    <asp:Label id="lblPosError" Visible="false" runat="server" />
    <asp:Button ID="addPosition" runat="server" CausesValidation="false" Text="Add" OnClick="addPosition_clicked" />
    <asp:LinkButton ID="LinkButtonClear" OnClick="LinkButtonClear_Clicked" runat="server" Visible="false" Text="Clear Positions" />
</asp:Panel>
<asp:Label ID="lblPositionsError" runat="server" />
<br />
<asp:Panel ID="positionsList" runat="server" Visible="false">
    <asp:Label ID="posLeadText" runat="server" Text="The following are the positions you added to this election:" Font-Bold="true" />
    <asp:Label ID="list" runat="server" Text="" /><br />
    <asp:Label ID="LabelEndTable" runat="server" Visible="false" Text="</table>"></asp:Label>
</asp:Panel>
<br />
<b>Create the Timeline</b>
<br />

            <br />
            Nomination phase START date:<br />

            Date:&nbsp;&nbsp;<asp:TextBox ID="TextBoxDateNomination" OnTextChanged="updateSpanNominate" AutoPostBack="true" MaxLength="8" Width="67" runat="server"></asp:TextBox><span style="color:Red"><asp:RequiredFieldValidator ControlToValidate="TextBoxDateNomination" Text=" Required." Display="Dynamic" ValidationGroup="timeline" runat="server" /></span>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <b><asp:Label ID="LabelWhenNominate" CssClass="blue" runat="server" Text="Enter the date and time for this phase first."></asp:Label></b>
            <br />
            Time:&nbsp;&nbsp;<asp:TextBox ID="TextBoxTimeHourNomination" MaxLength="2" Width="18" Text="00" runat="server"></asp:TextBox> :
            <asp:TextBox ID="TextBoxTimeMinNomination" MaxLength="2"  Width="18" Text="00" runat="server"></asp:TextBox>
            (24-hour Clock Format)
            <br />
            <br />

            <!--Test area-->

            Nomination Acceptance and Approval phase START date:<br />

            Date:&nbsp;&nbsp;<asp:TextBox ID="TextBoxDateAccept1" Enabled="false" OnTextChanged="updateSpanAccept1" AutoPostBack="true" MaxLength="8" Width="67" runat="server"></asp:TextBox> <span style="color:Red"><asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="TextBoxDatePetition" Display="Dynamic" ValidationGroup="timeline" Text=" Required." runat="server" /></span>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <b><asp:Label ID="LabelWhenAccept1" CssClass="blue" runat="server" Text=""></asp:Label></b>
            <br />
            Time:&nbsp;&nbsp;<asp:TextBox ID="TextBoxTimeHourAccept1" Enabled="false" MaxLength="2" Width="18" Text="00" runat="server"></asp:TextBox> :
            <asp:TextBox ID="TextBoxTimeMinAccept1" Enabled="false" MaxLength="2" Width="18" Text="00" runat="server"></asp:TextBox>
            (24-hour Clock Format)
            <br />
            <br />

            <!--end test area-->

            Petition phase START date:<br />

            Date:&nbsp;&nbsp;<asp:TextBox ID="TextBoxDatePetition" Enabled="false" OnTextChanged="updateSpanPetition" AutoPostBack="true" MaxLength="8" Width="67" runat="server"></asp:TextBox> <span style="color:Red"><asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="TextBoxDatePetition" Display="Dynamic" ValidationGroup="timeline" Text=" Required." runat="server" /></span>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <b><asp:Label ID="LabelWhenPetition" CssClass="blue" runat="server" Text=""></asp:Label></b>
            <br />
            Time:&nbsp;&nbsp;<asp:TextBox ID="TextBoxTimeHourPetition" Enabled="false" MaxLength="2" Width="18" Text="00" runat="server"></asp:TextBox> :
            <asp:TextBox ID="TextBoxTimeMinPetition" Enabled="false" MaxLength="2" Width="18" Text="00" runat="server"></asp:TextBox>
            (24-hour Clock Format)
            <br />
            <br />

            <!--TEST AREA-->
            Petition Acceptance and Approval phase START date:<br />

            Date:&nbsp;&nbsp;<asp:TextBox ID="TextBoxDateAccept2" Enabled="false" OnTextChanged="updateSpanAccept2" AutoPostBack="true" MaxLength="8" Width="67" runat="server"></asp:TextBox> <span style="color:Red"><asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="TextBoxDatePetition" Display="Dynamic" ValidationGroup="timeline" Text=" Required." runat="server" /></span>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <b><asp:Label ID="LabelWhenAccept2" CssClass="blue" runat="server" Text=""></asp:Label></b>
            <br />
            Time:&nbsp;&nbsp;<asp:TextBox ID="TextBoxTimeHourAccept2" Enabled="false" MaxLength="2" Width="18" Text="00" runat="server"></asp:TextBox> :
            <asp:TextBox ID="TextBoxTimeMinAccept2" Enabled="false" MaxLength="2" Width="18" Text="00" runat="server"></asp:TextBox>
            (24-hour Clock Format)
            <br />
            <br />


            <!--END TEST AREA-->
            Voting phase START date:<br />

            Date:&nbsp;&nbsp;<asp:TextBox ID="TextBoxDateVote" Enabled="false" OnTextChanged="updateSpanVote" AutoPostBack="true" MaxLength="8" Width="67" runat="server"></asp:TextBox> <span style="color:Red"><asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="TextBoxDateVote" Text=" Required." Display="Dynamic" ValidationGroup="timeline" runat="server" /></span>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <b><asp:Label ID="LabelWhenVote" CssClass="blue" runat="server" Text=""></asp:Label></b>
            <br />
            Time:&nbsp;&nbsp;<asp:TextBox ID="TextBoxTimeHourVote" Enabled="false" MaxLength="2" Width="18" Text="00" runat="server"></asp:TextBox> :
            <asp:TextBox ID="TextBoxTimeMinVote" Enabled="false" MaxLength="2" Width="18" Text="00" runat="server"></asp:TextBox>
            (24-hour Clock Format)
            <br />
            <br />

            

            Voting phase END date:<br />

            Date:&nbsp;&nbsp;<asp:TextBox ID="TextBoxDateVoteEnd" Enabled="false" OnTextChanged="updateSpanVoteEnd" AutoPostBack="true" MaxLength="8" Width="67" runat="server"></asp:TextBox><span style="color:Red"><asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="TextBoxDateVoteEnd" Text=" Required." Display="Dynamic" ValidationGroup="timeline" runat="server" /></span>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <b><asp:Label ID="LabelWhenVoteEnd" CssClass="blue" runat="server" Text=""></asp:Label></b>
            <br />
            Time:&nbsp;&nbsp;<asp:TextBox ID="TextBoxTimeHourVoteEnd" Enabled="false" MaxLength="2" Width="18" Text="00" runat="server"></asp:TextBox> :
            <asp:TextBox ID="TextBoxTimeMinVoteEnd" Enabled="false" MaxLength="2" Width="18" Text="00" runat="server"></asp:TextBox>
            (24-hour Clock Format)
            <br />
            <br />

            <asp:Button ID="ButtonSave" runat="server" ValidationGroup="timeline" Text="Create Election" OnClick="ButtonSave_Clicked" />

        
            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TextBoxDateNomination" />
            <asp:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="TextBoxDateAccept1" />
            <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="TextBoxDatePetition" />
            <asp:CalendarExtender ID="CalendarExtender6" runat="server" TargetControlID="TextBoxDateAccept2" />
            <asp:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="TextBoxDateVote" />
            <asp:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="TextBoxDateVoteEnd" />

        
    </asp:Label>
    </asp:Panel>
    <asp:Panel ID="PanelComplete" Visible="false" runat="server">
    <br />
    Although the system is <i>highly</i> automated, it is very important that someone (most likely you, the admin) login at the very beginning of each new phase.
    When someone logs into the site for the first time for any given phase, a mass email will be sent out to all users telling them that the specified phase has begin.
    No user actions are required, simply sign in and the system will send the appropriate mass email message.<br /><br />
    Use the timeline below to set reminders for yourself to login to the system.
    <br /><br />
    <div style="font-size: 15px; font-weight: bold; text-align:center; width:100%">Timeline For The Election:</div>
    <div style="padding: 15px; margin-left: auto; margin-right:auto; width: 80%; height: 100%; border: medium double #666666; background-color: #E2E0FE;">
        <asp:Label ID="LabelFinalTimeline" runat="server" Text="" /><br />
        <div style="text-align:center; width:100%"><asp:Button ID="ButtonSendEmail" OnClick="ButtonSendEmail_clicked" runat="server" Text="Email This Timeline To Me" /></div>
    </div>
    <br />
    <div style="text-align:center; width:100%"><a href="home.aspx">Return To Homepage</a></div>
    </asp:Panel>

    </ContentTemplate>
    </asp:UpdatePanel>

     <asp:Button ID="Button1" runat="server" style="display:none" Text="Button" />
    <asp:Panel ID="PanelPlurality" CssClass="modalPopup" style="width: 400px; padding: 10, 10, 10, 10" runat="server">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
            
        <asp:Label ID="LabelPlurality" runat="server" Text="Label"></asp:Label><br /><br />
        <asp:DropDownList ID="DropDownListPlurality" runat="server">
            <asp:ListItem Selected="True">Select a Value...</asp:ListItem>
            <asp:ListItem>2</asp:ListItem>
            <asp:ListItem>3</asp:ListItem>
            <asp:ListItem>4</asp:ListItem>
            <asp:ListItem>5</asp:ListItem>
            <asp:ListItem>6</asp:ListItem>
            <asp:ListItem>7</asp:ListItem>
            <asp:ListItem>8</asp:ListItem>
            <asp:ListItem>9</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
        </asp:DropDownList><br /><br />
        Select how many times a user can vote for this position:<br />
        <asp:DropDownList ID="DropDownListVoting" runat="server">
            <asp:ListItem Selected="True">Select a Value...</asp:ListItem>
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
        </asp:DropDownList><br /><br />
        <asp:Label ID="popupError" runat="server" />
        <asp:Button ID="ButtonPluralityComplete" runat="server" OnClick="ButtonPluralityComplete_clicked" Text="Complete Plurality" />
        <asp:Button ID="ButtonPluralityCancel" runat="server" Text="Cancel" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
        PopupControlID="PanelPlurality"
        TargetControlID="Button1"
        BackgroundCssClass="modalBackground"
        PopupDragHandleControlID="PanelPlurality"
        CancelControlID="ButtonPluralityCancel"
    />
    
</asp:Content>


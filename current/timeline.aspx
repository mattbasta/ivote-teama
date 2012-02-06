<%@ Page Title="Set Timeline" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="timeline.aspx.cs" Inherits="timeline" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:ToolkitScriptManager runat="server" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        
            Create custome timeline
            <br /><br />
            Nomination phase START date:<br />

            Date:&nbsp;&nbsp;<asp:TextBox ID="TextBoxDateNomination" OnTextChanged="updateSpanNominate" AutoPostBack="true" MaxLength="8" Width="67" runat="server"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <b><asp:Label ID="LabelWhenNominate" CssClass="blue" runat="server" Text=""></asp:Label></b>
            <br />
            Time:&nbsp;&nbsp;<asp:TextBox ID="TextBoxTimeNomination" OnTextChanged="updateSpanNominate" AutoPostBack="true" Width="40" Text="00:00" runat="server"></asp:TextBox> (24-hour Clock Format)
            <br />
            <br />

            Petition phase START date:<br />

            Date:&nbsp;&nbsp;<asp:TextBox ID="TextBoxDatePetition" OnTextChanged="updateSpanPetition" AutoPostBack="true" MaxLength="8" Width="67" runat="server"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <b><asp:Label ID="LabelWhenPetition" CssClass="blue" runat="server" Text=""></asp:Label></b>
            <br />
            Time:&nbsp;&nbsp;<asp:TextBox ID="TextBoxTimePetition" OnTextChanged="updateSpanPetition" AutoPostBack="true" Width="40" Text="00:00" runat="server"></asp:TextBox> (24-hour Clock Format)
            <br />
            <br />

            Voting phase START date:<br />

            Date:&nbsp;&nbsp;<asp:TextBox ID="TextBoxDateVote" OnTextChanged="updateSpanVote" AutoPostBack="true" MaxLength="8" Width="67" runat="server"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <b><asp:Label ID="LabelWhenVote" CssClass="blue" runat="server" Text=""></asp:Label></b>
            <br />
            Time:&nbsp;&nbsp;<asp:TextBox ID="TextBoxTimeVote" OnTextChanged="updateSpanVote" AutoPostBack="true" Width="40" Text="00:00" runat="server"></asp:TextBox> (24-hour Clock Format)
            <br />
            <br />

            Voting phase END date:<br />

            Date:&nbsp;&nbsp;<asp:TextBox ID="TextBoxDateVoteEnd" OnTextChanged="updateSpanVoteEnd" AutoPostBack="true" MaxLength="8" Width="67" runat="server"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <b><asp:Label ID="LabelWhenVoteEnd" CssClass="blue" runat="server" Text=""></asp:Label></b>
            <br />
            Time:&nbsp;&nbsp;<asp:TextBox ID="TextBoxTimeVoteEnd" OnTextChanged="updateSpanVoteEnd" AutoPostBack="true" Width="40" Text="00:00" runat="server"></asp:TextBox> (24-hour Clock Format)
            <br />
            <br />

            <asp:Button ID="ButtonSave" runat="server" Text="Save" OnClick="ButtonSave_Clicked" />

        
    <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TextBoxDateNomination" />
    <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="TextBoxDatePetition" />
    <asp:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="TextBoxDateVote" />
    <asp:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="TextBoxDateVoteEnd" />

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>


<%@ Page Title="Slate" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="slate.aspx.cs" Inherits="slate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<style type="text/css">
   .slateListPositions
   {
       width: 200px;
       height: 375px;
       background-color: White;
       position: relative;
       top: 5px;
       left: 5px;
       float:left;
   }
   .slateListPeople
   {
       width: 195px;
       height: 375px;
       background-color: White;
       position: relative;
       top: 5px;
       padding-left: 5px;
       margin-left: 210px;
       margin-right: auto;
   }
   
   .slateDetailPeople
   {
       width: 280px;
       height: 365px;
       background-color: White;
       position: relative;
       top: -370px;
       margin-left: auto;
       margin-right: 5px;
       padding: 5px;
   }
   
   .bold
   {
       font-weight: bold;
   }
   
 </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="color: Blue; padding-bottom: 6px;"><asp:Label ID="LabelFeedback" runat="server" Text="To begin the voting process, please select a position from the list below"></asp:Label></div>
            
            <asp:Panel ID="PanelSlateWrapper" runat="server">
                <div id="slateWrapper" style="width: 710px; height: 385px; background-color: #FF9999;">
                    <!-- Holds the positions in the election -->
                    <asp:Panel ID="PanelPositions" CssClass="slateListPositions" runat="server">
                        <asp:ListView ID="ListViewPositions"  OnItemCommand="ListViewPositions_ItemCommand" runat="server">
                            <LayoutTemplate>
                                <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                            </LayoutTemplate>
                            <ItemTemplate>
                                    <asp:LinkButton ID="LinkButtonPostions" CssClass="bold" runat="server" CommandName="position" CommandArgument='<%#Eval("position")%>' Text='<%#Eval("position")%>' /><br />
                                <span style="font-size: 12px; font-weight: bold; color: #808080">
                                    <asp:Label ID="LabelVotedExtra" runat="server" Text=""></asp:Label>
                                    <asp:Label ID="LabelVoted" runat="server" Text=""></asp:Label>
                                    <asp:HiddenField ID="HiddenFieldVotedId" runat="server" />
                                    <asp:HiddenField ID="HiddenFieldVoteNumber" Value='<%#Eval("slots_plurality")%>' runat="server" />
                                    <asp:HiddenField ID="HiddenFieldAllCandidates" Value="" runat="server" />
                                </span>
                                <hr />
                            </ItemTemplate>
                        </asp:ListView>
                    </asp:Panel>
                    <!-- Holds the people nominated for each position -->
                    <asp:Panel ID="PanelPeople" CssClass="slateListPeople" Visible="false" runat="server">
                        <asp:ListView ID="ListViewPeople" OnItemCommand="ListViewPeople_ItemCommand" runat="server">
                             <LayoutTemplate>
                                <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButtonPostions" runat="server" CssClass="bold" CommandName="id" CommandArgument='<%#Eval("idunion_members")%>' Text='<%#Eval("fullname")%>' /><br /><hr />
                            </ItemTemplate>
                        </asp:ListView>
                    </asp:Panel>
                    <!-- Holds submit button and the info that belows to that persion -->
                    <asp:Panel ID="PanelSelect" CssClass="slateDetailPeople" Visible="false" runat="server">
                        <br />
                        <div style="margin-right: auto; margin-left: auto; width: 290px; text-align: center;"><asp:Button ID="ButtonVote" OnClick="ButtonVote_Clicked" Visible="false" runat="server" Text="Vote for This Person" /></div>
                        <br />
                        <span style="text-decoration: underline; font-weight: bolder">
                            Their Personal Statement:
                        </span>
                        <br />
                        <div style="overflow: auto; width: 280px; height: 290px;">
                            <asp:Label ID="LabelStatement" runat="server" Text="" />
                        </div>
                        <asp:HiddenField ID="HiddenFieldName" runat="server" />
                        <asp:HiddenField ID="HiddenFieldId" runat="server" />
                    </asp:Panel>
                </div>
                <br />
                <div style="width: 100%; text-align:center;">
                    <asp:Button ID="ButtonSubmitVotes" Visible="false" runat="server" OnClick="ButtonSubmitVotes_Clicked" Text="Submit Your Completed Ballot For Processing" />
                </div>
                <br />
                <asp:HiddenField ID="HiddenFieldCurrentPosition" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>


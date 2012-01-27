<%@ Page Title="" Language="C#" MasterPageFile="../../Site.master" AutoEventWireup="true" CodeFile="voting.aspx.cs" Inherits="NEC_voting" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link  href="../Views.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:Label ID="lblForm" runat="server">
    <asp:ToolkitScriptManager runat="server" />
   
    <!--This phase is eligibility dependent.--> 
        
        

    
        <asp:Panel ID="PanelNominationPending" Visible="false" Enabled="false" runat="server">
            <div id="notifications">
                
            </div>
        </asp:Panel>
        
    

    <div id="bodyCopy">
        <h1>Voting Period</h1>
        The current election is in the voting phase.  This phase is scheduled to end on [date].  
    </div>
    <div id="functions">
        <div class="column">
            <b>Account Management</b><br />
            <asp:LinkButton ID="LinkButton1"  commandname="id" CommandArgument="../../cpw.aspx"  OnCommand="transfer"
                        text="Change Password" runat="server" />
        </div>
        <div class="column">
            <!--none-->            
        </div>
        <div class="column">
            <!--none-->
        </div>
        
    </div>
    <div class="clear"></div>
    
    <!--The voting functionality will be available for this phase-->
    <div id="special">
    <br />
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
                        <div style="margin-right: auto; margin-left: auto; width: 290px; text-align: center;"><asp:Button ID="ButtonVote" OnClick="ButtonVote_Clicked" runat="server" Text="Vote for This Person" /></div>
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
    </div>
    
</asp:Label>
</asp:Content>


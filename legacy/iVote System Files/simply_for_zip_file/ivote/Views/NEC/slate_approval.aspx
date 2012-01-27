<%@ Page Title="" Language="C#" MasterPageFile="../../Site.master" AutoEventWireup="true" CodeFile="slate_approval.aspx.cs" Inherits="NEC_slate_approval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link  href="../Views.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:Label ID="lblForm" runat="server">
    <asp:Panel ID="PanelNominationPending" Visible="false" Enabled="false" runat="server">
            <div id="notifications">
               <!--No notifications during a stateless period-->
            </div>
        </asp:Panel>
    <div id="bodyCopy">
        <h1>Slate Approval</h1>
        Please review and approve the slate below.
    </div>
    <div id="functions">
        <div class="column">
            <b>Account Management</b><br />
            <asp:LinkButton ID="LinkButton1"  commandname="id" CommandArgument="../../cpw.aspx"  OnCommand="transfer"
                        text="Change Password" runat="server" />
        </div> 
        <div class="column">
            <!--None-->
        </div>
        
        <div class="column">
            <!--No third column.-->
        </div>
    </div>
    <div class="clear"></div>

    <!--Slate Approval-->
    <div id="special">
        <br />
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div style="color: Blue; padding-bottom: 6px;"><asp:Label ID="LabelFeedback" runat="server" Text="To view the nominated for a position, please select a position from the list below"></asp:Label></div>
            
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
           <asp:Button ID="btnApprove" Text="Approve Slate" runat="server" />
    </div>
    
</asp:Label>
</asp:Content>


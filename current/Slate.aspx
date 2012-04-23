<%@ Page Title="Slate" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Slate.aspx.cs" Inherits="slate" %>

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
    <ul class="breadcrumb">
        <li><a href="/home.aspx">Home</a> <span class="divider">/</span></li>
        <li><a href="/officer_election.aspx">Officer Election</a> <span class="divider">/</span></li>
        <li class="active">Slate Confirmation</li>
    </ul>
    
    <div class="page-header">
        <h1>Slate Confirmation</h1>
    </div>
    
    <asp:ScriptManager runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="PanelSlateWrapper" runat="server">
                <div id="slateWrapper" class="row">
                    <!-- Holds the positions in the election -->
                    <asp:Panel ID="PanelPositions" CssClass="span3" runat="server">
                        <asp:ListView ID="ListViewPositions"  OnItemCommand="ListViewPositions_ItemCommand" runat="server">
                            <LayoutTemplate>
                                <ul class="nav nav-tabs nav-stacked">
                                <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                                </ul>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <li>
                                    <asp:LinkButton ID="LinkButtonPostions" runat="server" CommandName="position" CommandArgument='<%#Eval("position")%>' Text='<%#Eval("position")%>' />
                                </li>
                            </ItemTemplate>
                        </asp:ListView>
                    </asp:Panel>
                    <!-- Holds the people nominated for each position -->
                    <asp:Panel ID="PanelPeople" CssClass="span3" Visible="false" runat="server">
                        <asp:ListView ID="ListViewPeople" OnItemCommand="ListViewPeople_ItemCommand" runat="server">
                            <LayoutTemplate>
                                <ul class="nav nav-tabs nav-stacked">
                                <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                                </ul>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <li>
                                <asp:LinkButton ID="LinkButtonPostions" runat="server" CommandName="id" CommandArgument='<%#Eval("idunion_members")%>' Text='<%#GetName((int)Eval("idunion_members"))%>' />
                                </li>
                            </ItemTemplate>
                        </asp:ListView>
                    </asp:Panel>
                    <!-- Holds submit button and the info that belows to that persion -->
                    <asp:Panel ID="PanelSelect" CssClass="span6" Visible="false" runat="server">
                        <div class="well">
                            <strong>Candidate's Personal Statement:</strong>
                            <p><asp:Label ID="LabelStatement" runat="server" Text="" /></p>
                        </div>
                    </asp:Panel>
                </div>
                <br />
                <asp:HiddenField ID="HiddenFieldCurrentPosition" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>


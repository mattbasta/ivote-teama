<%@ Page Title="Remove from Ballot" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="RemoveFromBallot.aspx.cs" Inherits="wwwroot_finalsite_RemoveFromBallot" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <ul class="breadcrumb">
        <li><a href="/home.aspx">Home</a> <span class="divider">/</span></li>
        <li><a href="/officer_election.aspx">Officer Election</a> <span class="divider">/</span></li>
        <li class="active">Remove Entrants From Ballot</li>
    </ul>
    
    <div class="page-header">
        <h1>Remove Entrants From Ballot</h1>
    </div>
    
    <asp:ScriptManager runat="server" />


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <p><asp:Label ID="LabelFeedback" runat="server" Text="Approve or deny the eligibility of each Willingness-to-serve below." /></p>
            <table class="table table-bordered" style="width: 100%">
                <tr>
                    <th>Name</th>
                    <th>Position</th>
                    <th></th>
                </tr>
                <asp:ListView ID="ListViewApproval" OnItemCommand="showFullStatement" runat="server">
                    <LayoutTemplate>
                        <tr>
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                        </tr>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <td>
                            <asp:Label ID="LabelFullname" runat="server" Text='<%#GetName(int.Parse(Eval("idunion_members").ToString())) %>' />
                            <asp:HiddenField ID="HiddenFieldID" Value='<%#Eval("wts_id")%>' runat="server" />
                            <asp:HiddenField ID="HiddenFieldEligible" Value='<%#Eval("eligible")%>' runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="LabelPosition" runat="server" Text='<%#Eval("position") %>' />
                        </td>
                        <td>
                            <asp:Button ID="ButtonRemove" 
                            runat="server" CommandName="remove" CommandArgument='<%#Eval("wts_id") + "%" + Eval("idunion_members")%>'
                            OnClientClick='Are you sure you want to remove this person from this election? An email will be sent out alerting ALL users.'
                            Text="Remove From Ballot" CssClass="btn btn-danger btn-mini" />
                        </td>
                    </ItemTemplate>
                </asp:ListView>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>


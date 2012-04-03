<%@ Page Title="Results" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="results.aspx.cs" Inherits="finalsite_results" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <ul class="breadcrumb">
        <li><a href="/home.aspx">Home</a> <span class="divider">/</span></li>
        <li class="active">Officer Election Results</li>
    </ul>
    
    <div class="page-header">
        <h1>Officer Election Results</h1>
    </div>
    
    <div class="row">
        <div class="offset3 span6">
            <asp:Repeater ID="resultList" runat="server" >
                <HeaderTemplate>
                    <table class="table table-bordered">
                        <tr>
                            <th>Position</th>
                            <th>Winner</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>            
                        <td> <%#Eval("position") %> </td>
                        <td> <asp:Label ID="person" runat="server" Text='<%#Eval("fullname") %>' /> </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table></center>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
   
    <div class="form-horizontal">
        <div class="form-actions form-inline">
            <asp:Label ID="necApproved" runat="server" Text="<b>Thank you for certifying the results.</b>" Visible="false" />
            <asp:Label ID="necApprove" runat="server" Text="<b>Please approve the results above from this past election.</b>" Visible="false" />
            <asp:Button ID="necButton" runat="server" Text="Approve" Visible="false" OnClick="necButton_OnClick" CssClass="btn btn-success" />
        </div>
    </div>
</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="results.aspx.cs" Inherits="finalsite_results" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <h1>Results of the Election</h1><br />
    <asp:Repeater ID="resultList" runat="server" >
    <HeaderTemplate>
        <center><table class="simpleGrid" style="width: 60%">
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
    <center><br /><br />
    <asp:Label ID="necApprove" runat="server" Text="<b>Please approve the results above from this past election.</b>" Visible="false" /><br />
    <asp:Button ID="necButton" runat="server" Text="Approve" Visible="false" OnClick="necButton_OnClick" />
    </center>
</asp:Content>


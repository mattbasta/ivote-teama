<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Nominations.aspx.cs" Inherits="wwwroot_finalsite_Nominations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    

    <!-- use a GridView to display all nominations with an approve/reject button for the user -->
    <asp:Label ID="ConfirmLabel" runat="server" Visible="false" Text="" ></asp:Label>
    <asp:UpdatePanel ID="NominationsPanel" runat="server">            
        <ContentTemplate>
            <!--<asp:ScriptManager ID="ScriptManager1" runat="server" />--> 
            <!-- List of nomintations, if any -->
            <asp:GridView ID="GridViewNominations" AutoGenerateColumns="false" OnRowCommand="GridViewNominations_ItemCommand" CssClass="simpleGrid" runat="server">
                <Columns>
                    <asp:BoundField HeaderText="Postion" DataField="position" NullDisplayText="Error"/>                                       
                    <asp:TemplateField HeaderText="" >
                        <ItemTemplate>
                            <asp:Button ID="ButtonAccept" runat="server" CommandName="accept" CommandArgument='<%#Eval("position") %>' Text="Accept" BackColor="Green" />
                            <asp:Button ID="ButtonReject" runat="server" CommandName="reject" CommandArgument='<%#Eval("position") %>' Text="Reject" BackColor="Red" />
                        </ItemTemplate> 
                    </asp:TemplateField>                                                              
                </Columns>
            </asp:GridView>
            <asp:Label ID="temp" runat="server" />
            <!-- displays comment to enter (if any) for the nomination -->
            <asp:Panel ID="PanelNomination" CssClass="simpleBorder" Visible="false" runat="server">
                <br />
                <asp:Label ID="LabelSelected" runat="server" Text="" />
                <asp:TextBox ID="StatementBox" runat="server" TextMode="MultiLine"></asp:TextBox>
                <asp:HiddenField ID="HiddenFieldID" runat="server"  />
            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>
     

</asp:Content>


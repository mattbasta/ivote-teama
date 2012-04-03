<%@ Page Title="User Nomination Acceptance" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="user_acceptance.aspx.cs" Inherits="finalsite_user_acceptance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <asp:ScriptManager runat="server" />
    <!-- use a GridView to display all nominations with an approve/reject button for the user -->
    <asp:Label ID="ConfirmLabel" runat="server" Visible="false" Text="" ></asp:Label>
    <asp:UpdatePanel ID="NominationsPanel" runat="server">            
        <ContentTemplate>           
            <!-- List of nomintations, if any -->
            <asp:GridView ID="GridViewNominations" AutoGenerateColumns="false" OnRowCommand="GridViewNominations_RowCommand" CssClass="simpleGrid" runat="server">
                 <Columns>        
                    <asp:BoundField HeaderText="Position" DataField="position" NullDisplayText="Error!" />
                    <asp:TemplateField HeaderText="" >                       
                        <ItemTemplate>
                            <asp:Button ID="btnAccept" runat="server" commandname="accept" commandargument='<%#Eval("position") %>' Text="Accept" BackColor="Green" />
                            <asp:Button ID="btnReject" runat="server" commandname="reject" commandargument='<%#Eval("position") %>' Text="Reject" backColor="Red" />
                        </ItemTemplate>
                    </asp:TemplateField>                   
                </Columns>
            </asp:GridView>
            <!-- displays comment to enter (if any) for the nomination -->
            <asp:Panel ID="PanelNomination" CssClass="simpleBorder" Visible="false" runat="server">
                <br />
                <asp:Label ID="LabelSelected" runat="server" Text="" />
                <asp:TextBox ID="StatementBox" runat="server" TextMode="MultiLine"></asp:TextBox>
                <p class="submitButton"> <asp:Button ID="ButtonAccept" runat="server" Text="Accept" BackColor="Green" /> <asp:Button ID="ButtonReject" runat="server" Text="Reject" BackColor="Red" /> </p>
                <asp:HiddenField ID="HiddenFieldID" runat="server" />
            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>


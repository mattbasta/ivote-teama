<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="approvenominations.aspx.cs" Inherits="wwwroot_experimental_approvenominations" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolKit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <ajaxToolKit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <span style="color:blue;"><asp:Label ID="LabelFeedback" runat="server" Text="Approve or deny the eligibility of each Willingness-to-serve below." /></span>
    <br />
    <table class="simpleGrid" style="width: 100%">
    <tr>
            <th>Name</th>
            <th>Position</th>
            <th>Statement</th>
            <th>Approve</th>
            <th>Deny</th>
        </tr>
        <asp:ListView ID="ListViewApproval" OnItemCommand="showFullStatement" runat="server">
            <LayoutTemplate>
                <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <td >
                        <asp:Label ID="LabelFullname" runat="server" Text='<%#Eval("fullname") %>' />
                        <asp:HiddenField ID="HiddenFieldID" Value='<%#Eval("wts_id")%>' runat="server" />
                        <asp:HiddenField ID="HiddenFieldEligible" Value='<%#Eval("eligible")%>' runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="LabelPosition" runat="server" Text='<%#Eval("position") %>' />
                    </td>
                    <td>
                        <asp:Label ID="LabelStatement" runat="server" Text=' <%# "\"" + Eval("statement").ToString().Substring(0,Math.Min(60,Eval("statement").ToString().Length))+"..." + "\"" %>' />
                        <asp:LinkButton ID="LinkButtonStatement" CommandName="statement" CommandArgument='<%#Eval("statement") %>' runat="server">Continue</asp:LinkButton>
                    </td>
                    <td>
                        <asp:RadioButton ID="RadioButton1" GroupName="nomination" runat="server" />
                    </td>
                    <td>
                        <asp:RadioButton ID="RadioButton2" GroupName="nomination" runat="server" />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
        </table>

        
        <br /><br />
        <asp:Button ID="ButtonSave" runat="server" Text="Save Changes" Width="150" OnClick="Click_ButtonSave" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:Panel ID="PanelStatement" CssClass="modalPopup" Width="300" runat="server">
       <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <br />
                <asp:Label ID="LabelFullStatment" runat="server" Text=""></asp:Label>
                <br /><br />
                <div style="margin-left: auto; margin-right:auto; width: 50px">
                    <asp:Button ID="ButtonDone" OnClick="ButtonDone_Click" runat="server" Text="Done" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    
    <asp:Button ID="Button1" runat="server" style="display: none" Text="" />

    <ajaxToolKit:ModalPopupExtender ID="ModalPopupExtender1" runat="server"
        TargetControlID="Button1"
        PopupControlID="PanelStatement"
        CancelControlID="ButtonDone"
        BackgroundCssClass="modalBackground"
        PopupDragHandleControlID="PanelStatement"
    />

</asp:Content>


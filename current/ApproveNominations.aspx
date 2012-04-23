<%@ Page Title="Approve Nominations" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ApproveNominations.aspx.cs" Inherits="wwwroot_experimental_ApproveNominations" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolKit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <ajaxToolKit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
    
    <ul class="breadcrumb">
        <li><a href="/home.aspx">Home</a> <span class="divider">/</span></li>
        <li><a href="/officer_election.aspx">Officer Election</a> <span class="divider">/</span></li>
        <li class="active">Approve Officer Nominations</li>
    </ul>
    
    <div class="page-header">
        <h1>Approve Officer Nominations</h1>
    </div>
    
    <p>Approve or deny the eligibility of each Willingness-to-Serve form below.</p>
    
    <asp:Panel ID="LabelFeedbackAlert" runat="server" CssClass="alert" Visible="false">
        <asp:Literal ID="LabelFeedback" runat="server" Text="" />
    </asp:Panel>
    
    <asp:Panel ID="SavedConfirmation" runat="server" CssClass="alert alert-success" Visible="false">
        <strong>Saved</strong>
        Approvals and rejections have been saved successfully.
    </asp:Panel>
    
    <table class="table table-bordered">
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
                        <asp:Literal ID="LabelFullname" runat="server" Text='<%#GetName(int.Parse(Eval("idunion_members").ToString())) %>' />
                        <asp:HiddenField ID="HiddenFieldID" Value='<%#Eval("wts_id")%>' runat="server" />
                        <asp:HiddenField ID="HiddenUserID" Value='<%#Eval("idunion_members")%>' runat="server" />
                        <asp:HiddenField ID="HiddenFieldEligible" Value='<%#Eval("eligible")%>' runat="server" />
                    </td>
                    <td>
                        <asp:Literal ID="LabelPosition" runat="server" Text='<%#Eval("position") %>' />
                    </td>
                    <td>
                        <asp:Literal ID="LabelStatement" runat="server" Text='<%#GetSummary(Eval("statement").ToString()) %>' />
                        <asp:LinkButton ID="LinkButtonStatement" CommandName="statement" CommandArgument='<%#Eval("statement") %>' Visible='<%#Eval("statement").ToString().Length > 140 %>' runat="server">Continue</asp:LinkButton>
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

    <asp:Button ID="ButtonSave" runat="server" Text="Save Changes" CssClass="btn btn-primary" OnClick="Click_ButtonSave" />
    <a href="/officer_election.aspx" class="btn">Go Back</a>

    <asp:Panel ID="PanelStatement" CssClass="modal" runat="server">
        <div class="modal-header">
            <h3>View Full Statement</h3>
        </div>
       <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div class="modal-body">
                    <p><asp:Literal ID="LabelFullStatment" runat="server" Text="" /></p>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="ButtonDone" OnClick="ButtonDone_Click" runat="server" Text="Done" CssClass="btn btn-primary" />
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


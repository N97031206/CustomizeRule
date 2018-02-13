<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddNewItem.ascx.cs"
    Inherits=" ciMes.Web.Common.UserControl.AddNewItem" %>
<ajaxToolkit:ModalPopupExtender ID="ModalPopupAddItem" runat="server" BackgroundCssClass="modalBackground"
    DropShadow="True" PopupControlID="divAddNewItem" PopupDragHandleControlID="divAddNewItem"
    RepositionMode="RepositionOnWindowScroll" TargetControlID="">
</ajaxToolkit:ModalPopupExtender>
<div id="divAddNewItem" runat="server" style="width: 300px; height: 75px; display: none">
    <center>
        <asp:Label ID="lblAddNewItemTitle" runat="server" CssClass="messageTitle" Text="<%$ Resources:Auto|RuleFace, Add %>"
            Width="98%"></asp:Label></center>
    <asp:TextBox ID="ttbNewClassName" runat="server" Width="98%"></asp:TextBox>
    <asp:Panel ID="Panel1" runat="server" Height="2px">
    </asp:Panel>
    <cimesui:cimesbutton id="btnOK" runat="server" cssclass="CSOKButton" onclick="btnOK_Click"
        text="<%$ Resources:Auto|RuleFace, OK %>" />
    <cimesui:cimesbutton id="btnCancel" runat="server" cssclass="CSCancelButton" onclick="btnCancel_Click"
        text="<%$ Resources:Auto|RuleFace, Cancel %>" />
</div>

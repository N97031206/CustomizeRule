<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DefectActionSetupBlock.ascx.cs" Inherits="ciMes.Web.Common.UserControl.DefectActionSetupBlock" %>
<table align="left" width="99%">
    <tr>
        <td align="left" width="50%">
            <asp:RadioButtonList ID="rblDefectActionMode" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblDefectActionMode_SelectedIndexChanged">
                <asp:ListItem Text="<%$ Resources:Auto|RuleFace, NoDefect %>" Value="NoDefect"></asp:ListItem>
                <asp:ListItem Text="<%$ Resources:Auto|RuleFace, SingleAction %>" Value="SingleAction"></asp:ListItem>
                <asp:ListItem Text="<%$ Resources:Auto|RuleFace, ReasonControl %>" Value="ReasonControl"></asp:ListItem>
                <asp:ListItem Text="<%$ Resources:Auto|RuleFace, MultiAction %>" Value="MultiAction"></asp:ListItem>
            </asp:RadioButtonList>
        </td>
        <td align="left" width="50%">
            <asp:Label ID="lblActionModeDescr" runat="server" Width="100%"></asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="2" align="left">
            <asp:MultiView ID="mvDefectActionMode" runat="server">
                <asp:View ID="viewNoDefect" runat="server">
                </asp:View>
                <asp:View ID="viewSingleAction" runat="server">
                    <asp:DropDownList ID="ddlDefectAction" runat="server" OnSelectedIndexChanged="ddlDefectAction_SelectedIndexChanged">
                    </asp:DropDownList>
                </asp:View>
                <asp:View ID="viewReasonControl" runat="server">
                </asp:View>
                <asp:View ID="viewMultiAction" runat="server">
                    <asp:CheckBoxList ID="cblDefectActions" runat="server" RepeatDirection="Vertical"></asp:CheckBoxList>
                </asp:View>
            </asp:MultiView>
        </td>
    </tr>
</table>

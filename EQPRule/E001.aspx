<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="E001.aspx.cs" Inherits="CustomizeRule.EQPRule.E001" %>

<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock"
    TagPrefix="uc4" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>ChangeState</title>
</head>
<body>
    <form id="ChangeState" method="post" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc4:ProgramInformationBlock ID="ProgramInformationBlock1" runat="server" />
            <table style="width: 800px; margin-left: auto; margin-right: auto">
                <tr>
                    <td style="width: 15%;" class="CSTDMust">
                        <asp:Label ID="lblEquipment" runat="server" Text="<%$ Resources:Auto|RuleFace, Equipment %>"></asp:Label>
                    </td>
                    <td>
                        <CimesUI:CimesEquipmentInput ID="ciEquipment" runat="server" TabIndex="1" TextWidth="100%"
                            CssClass="CSInputMust">
                        </CimesUI:CimesEquipmentInput>
                    </td>
                </tr>
                <tr>
                    <td class="CSTD" style="width: 15%;">
                        <asp:Label ID="lblOldState" runat="server" Text="<%$ Resources:Auto|RuleFace, OldState %>"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="ttbOldState" runat="server" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%;" class="CSTDMust">
                        <asp:Label ID="lblNewState" runat="server" Text="<%$ Resources:Auto|RuleFace, NewState %>"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlNewState" runat="server" CssClass="CSDropDownListMust" TabIndex="1">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%;" class="CSTDMust">
                        <asp:Label ID="lblReasonCode" runat="server" Text="<%$ Resources:Auto|RuleFace, ReasonCode %>"></asp:Label>
                    </td>
                    <td>
                        <CimesUI:CSReason LabelVisible="false" ID="csReason" runat="server" AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%;" class="CSTD">
                        <asp:Label ID="lblDescription" runat="server" Text="<%$ Resources:Auto|RuleFace, Description %>"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="ttbDesc" runat="server" Height="155px" TextMode="MultiLine" TabIndex="1"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <CimesUI:CimesButton ID="btnOK" TabIndex="2" runat="server" CssClass="CSOKButton"
                            Enabled="False" Text="<%$ Resources:Auto|RuleFace, OK %>" OnClick="btnOK_Click">
                        </CimesUI:CimesButton>
                        <CimesUI:CimesButton ID="btnCancel" runat="server" CssClass="CSCancelButton" OnClick="btnCancel_Click"
                            Text="<%$ Resources:Auto|RuleFace, Cancel %>" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>

    <script>
        document.getElementById("ttbEqpID").focus();
    </script>

</body>
</html>

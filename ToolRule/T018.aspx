<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="T018.aspx.cs" Inherits="CustomizeRule.ToolRule.T018" %>

<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>T018</title>
    <script type="text/javascript">
        function ShowLoading() {
            var img = document.getElementById('imgProgress');
            img.style.visibility = 'visible';
            return true;
        }
    </script>
</head>
<body>
    <form id="T018" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <asp:Updatepanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:ProgramInformationBlock ID="ProgramInformationBlock1" runat="server" />
                <div style="margin: 0 auto; width: 500px;">
                    <fieldset style="width: 100%;">
                        <legend>
                           <asp:Label ID="lblToolInfo" runat="server" Text="<%$ Resources:Auto|RuleFace, ToolPartsInfo %>"></asp:Label>
                        </legend>
                        <table style="width: 97%;">
                            <tr>
                                <td class="CSTDMust" style="width: 20%;">
                                    <asp:Label ID="lblToolName" Text="<%$ Resources:Auto|RuleFace, ToolParts %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 80%;" colspan="2">
                                    <asp:TextBox ID="ttbToolName" runat="server" AutoPostBack="true" OnTextChanged="ttbToolName_TextChanged"></asp:TextBox>
                                </td>
                            </tr>
                             <tr>
                                <td class="CSTD" style="width: 20%;">
                                    <asp:Label ID="lblToolType" Text="<%$ Resources:Auto|RuleFace, ToolTypes %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 80%;" colspan="2">
                                    <asp:TextBox ID="ttbToolType" runat="server" Readonly ="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                               <td class="CSTD" style="width: 20%;">
                                <asp:Label ID="lblInspectionReport" runat="server" Text="<%$ Resources:Auto|RuleFace, InspectionReport %>"></asp:Label>
                                </td>
                                <td style="width: 40%;">
                                    <asp:FileUpload ID="FileUpload1" runat="server" Width="100%" Style="float: left;" />
                                </td>
                                <td align="right">
                                    <CimesUI:CimesButton ID="btnUpload" runat="server" CssClass="CSAddButton" OnClientClick="return ShowLoading();" Text="<%$ Resources:Auto|RuleFace, UploadFile %>"
                                        OnClick="btnUpload_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td class="CSTD" style="width: 20%;">
                                    <asp:Label ID="lblPreview" Text="<%$ Resources:Auto|RuleFace, Preview %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 80%;" colspan="2">
                                    <asp:HyperLink ID="hlShowFile" runat="server" Target="_blank"></asp:HyperLink>
                                    <asp:Image runat="server" Style="float: right;visibility: hidden" ID="imgProgress" ImageUrl="~/Images/Loading.gif" Height="16px" Width="16px" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <table style="width: 100%;">
                        <tr>
                            <td align="center">
                                <CimesUI:CimesButton ID="btnOK" runat="server" CssClass="CSOKButton"
                                        Text="<%$ Resources:Auto|RuleFace, OK %>" OnClick="btnOK_Click"></CimesUI:CimesButton>
                                <CimesUI:CimesButton ID="btnCancel" runat="server" CssClass="CSCancelButton" OnClick="btnCancel_Click"
                                        Text="<%$ Resources:Auto|RuleFace, Exit %>" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnUpload" />
            </Triggers>
        </asp:UpdatePanel>
    </form>
</body>
</html>
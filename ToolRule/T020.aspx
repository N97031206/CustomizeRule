<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="T020.aspx.cs" Inherits="CustomizeRule.ToolRule.T020" %>

<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>T020</title>
    <script type="text/javascript">
        function Change(SCheckBox) {

            //找到頁面所有 input
            var objs = document.getElementsByTagName("input");

            for (var i = 0; i < objs.length; i++) {
                //找到input中的checkbox
                if (objs[i].type.toLowerCase() == "checkbox") {
                    //所有checkbox為false
                    objs[i].checked = false;
                }
            }

            //找到選中checkbox
            var SelectCheckBoxID = SCheckBox.id;

            //選中checkbox為true
            document.getElementById(SelectCheckBoxID).checked = true;
        }
    </script>
</head>
<body>
    <form id="T020" runat="server">
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
                                <td style="width: 79%;">
                                    <asp:TextBox ID="ttbToolName" runat="server" AutoPostBack="true" OnTextChanged="ttbToolName_TextChanged"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="CSTDMust" style="width: 20%;">
                                    <asp:Label ID="lblMainHead" Text="<%$ Resources:Auto|RuleFace, MainHead %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 79%;">
                                    <asp:TextBox ID="ttbMainHead" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <table style="width: 100%;">
                            <tr>
                                <td align="center">
                                    <div style="height: 200px; overflow: auto;">
                                        <CimesUI:CimesGridView ID="gvToolHead" runat="server" Width="95%" AutoGenerateColumns="false" OnRowDataBound="gvToolHead_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" HeaderStyle-Wrap="false">
                                                    <ItemTemplate>
                                                          <asp:CheckBox ID="ckbSelect" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Head" HeaderText="<%$ Resources:Auto|RuleFace, Head %>" HeaderStyle-Width="30%" />
                                                <asp:BoundField DataField="Life" HeaderText="<%$ Resources:Auto|RuleFace, Life %>" HeaderStyle-Width="30%"/>
                                                <asp:BoundField DataField="UseCount" HeaderText="<%$ Resources:Auto|RuleFace, UseCount %>" HeaderStyle-Width="30%"/>
                                            </Columns>
                                        </CimesUI:CimesGridView>
                                    </div>
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
        </asp:UpdatePanel>
    </form>
</body>
</html>

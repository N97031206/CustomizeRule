<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="W007.aspx.cs" Inherits="CustomizeRule.WIPRule.W007" %>

<%@ Register Assembly="Ares.Cimes.IntelliService.Web" Namespace="Ares.Cimes.IntelliService.Web.UI" TagPrefix="CimesUI" %>
<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock" TagPrefix="uc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>W007</title>
    <style type="text/css">
        .outUpSpec {
            color: White;
            background-color: Blue;
        }

        .outLowSpec {
            color: White;
            background-color: Maroon;
        }

        .normal {
            color: black;
            background-color: white;
        }

        input:hover {
            color: black;
            border-color: orange;
        }

        .align {
            text-align: center;
        }
    </style>

    <script type="text/javascript">

        function ClearSpecError(txtObj) {
            txtObj.className = "normal";
            txtObj.setAttribute("SpecError", "false");
            txtObj.removeAttribute("FormatError");
        }

        function ValidateInputNumber(txtObj, LowSpec, UpSpec) {
            ClearSpecError(txtObj);
            if (txtObj.value != '') {
                exp = new RegExp("^\\s*([-\\+])?(\\d+)?(\\.(\\d+))?\\s*$");
                m = txtObj.value.match(exp);
                if (m == null) {
                    txtObj.setAttribute("FormatError", true);
                    txtObj.focus();
                    return false;
                }
                //cleanInput = m[1] + (m[2].length > 0 ? m[2] : "0") + "." + m[4];
                num = parseFloat(txtObj.value);
                if (isNaN(num)) {
                    txtObj.setAttribute("FormatError", true);
                    txtObj.focus();
                    return false;
                }
                if (num > UpSpec) {
                    txtObj.className = "outUpSpec";
                    txtObj.setAttribute("SpecError", "true");
                    txtObj.removeAttribute("FormatError");

                    return true;
                }

                if (num < LowSpec) {
                    txtObj.className = "outLowSpec";
                    txtObj.setAttribute("SpecError", "true");
                    txtObj.removeAttribute("FormatError");
                    return true;
                }

                var hasError = false;
                $("input[EDCFlag='Y']").each(function (index) {
                    //console.log(index + ":" + $(this).val());
                    if ($(this).attr('SpecError') == 'true' || $(this).val() == '') {
                        hasError = true;
                    }
                });

                if (!hasError) {
                    $('#btnOK').click();
                }
            }
            ClearSpecError(txtObj);
            return true;
        }
    </script>
</head>
<body>
    <form id="W007" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <uc2:programinformationblock id="ProgramInformationBlock1" runat="server" />
                <table style="margin-left: auto; margin-right: auto; width: 700px;">
                    <tr>
                        <td>
                            <fieldset class="CSFieldset">
                                <legend>
                                    <asp:Label ID="lblWorkpieceInfo" runat="server" Text="<%$ Resources:Auto|RuleFace, WorkpieceInfo %>"></asp:Label>
                                </legend>
                                <table style="width: 100%; padding: 10px">
                                    <tr>
                                        <td class="CSTDMust">
                                            <asp:Label ID="lblWOLot" runat="server" Text="<%$ Resources:Auto|RuleFace, RuncardLot %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ttbWOLot" runat="server" AutoPostBack="true" OnTextChanged="ttbWOLot_TextChanged"></asp:TextBox>
                                        </td>
                                        <td class="CSTDMust">
                                            <asp:Label ID="lblWorkpiece" runat="server" Text="DMC"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ttbWorkpiece" runat="server" OnTextChanged="ttbWorkpiece_TextChanged" AutoPostBack="true" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="CSTD">
                                            <asp:Label ID="lblMaterialLot" runat="server" Text="<%$ Resources:Auto|RuleFace, MaterialLot %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ttbMaterialLot" runat="server" ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td class="CSTD">
                                            <asp:Label ID="lblWorkpieceSerialNumber" runat="server" Text="<%$ Resources:Auto|RuleFace, WorkpieceSerialNumber %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ttbWorkpieceSerialNumber" runat="server" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="CSTD">
                                            <asp:Label ID="lblDeviceName" runat="server" Text="<%$ Resources:Auto|RuleFace, DeviceName %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ttbDeviceName" runat="server" ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td class="CSTD">
                                            <asp:Label ID="lblDeviceDescr" runat="server" Text="<%$ Resources:Auto|RuleFace, DeviceDescription %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ttbDeviceDescr" runat="server" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="CSTD">
                                            <asp:Label ID="lblRouteName" runat="server" Text="<%$ Resources:Auto|RuleFace, RouteName %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ttbRouteName" runat="server" ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td class="CSTD">
                                            <asp:Label ID="lblOperation" runat="server" Text="<%$ Resources:Auto|RuleFace, Operation %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ttbOperation" runat="server" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="CSTD">
                                            <asp:Label ID="lblTemperature" Text="<%$ Resources:Auto|RuleFace, Temperature %>" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ttbTemperature" runat="server" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <fieldset class="CSFieldset" style="padding: 10px;">
                                <legend>
                                    <asp:Label ID="lblEDCInfo" runat="server" Text="<%$ Resources:Auto|RuleFace, EDCInfo %>"></asp:Label>
                                </legend>
                                <cimesui:cimesgridview id="gvComponentEDC" runat="server" autogeneratecolumns="False"
                                    onrowdatabound="gvComponentEDC_RowDataBound" useaccessibleheader="False" width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, Item %>">
                                            <HeaderStyle Width="10%" Wrap="false" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblItem" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle Wrap="false" />  
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:TextBox ID="ttbEDC" runat="server" Width="150px"></asp:TextBox>
                                            </ItemTemplate>                                           
                                        </asp:TemplateField>
                                    </Columns>
                                </cimesui:cimesgridview>
                                <table style="width: 100%;">                                    
                                    <tr>
                                        <td colspan="2" style="text-align: center">
                                            <cimesui:cimesbutton id="btnOK" runat="server" cssclass="CSOKButton"
                                                text="<%$ Resources:Auto|RuleFace, OK %>" onclick="btnOK_Click"></cimesui:cimesbutton>

                                            <cimesui:cimesbutton id="btnCancel" runat="server" cssclass="CSCancelButton" onclick="btnCancel_Click"
                                                text="<%$ Resources:Auto|RuleFace, Exit %>" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>

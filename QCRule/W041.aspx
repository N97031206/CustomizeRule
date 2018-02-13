<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="W041.aspx.cs" Inherits="CustomizeRule.QCRule.W041" %>

<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>W041</title>
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
    <form id="W041" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <asp:Updatepanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:ProgramInformationBlock ID="ProgramInformationBlock1" runat="server" />
                <div style="margin: 0 auto; width: 940px;">
                    <fieldset style="width: 99%;">
                        <legend>
                           <asp:Label ID="lblProductionInfo" runat="server" Text="<%$ Resources:Auto|RuleFace, ProductionInfo %>"></asp:Label>
                        </legend>
                        <table style="width: 98%;">
                            <tr>
                                <td class="CSTDMust" style="width: 20%;">
                                    <asp:Label ID="lblLot" Text="<%$ Resources:Auto|RuleFace, eqp_complot_lot %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 30%;">
                                     <asp:TextBox ID="ttbLot" runat="server" AutoPostBack="true" OnTextChanged="ttbLot_TextChanged"></asp:TextBox>
                                </td>
                                <td class="CSTD" style="width: 20%;"> </td>
                                <td style="width: 30%;"></td>
                            </tr>
                            <tr>
                                <td class="CSTD" style="width: 20%;">
                                    <asp:Label ID="lblEquip" Text="<%$ Resources:Auto|RuleFace, Equip %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 30%;">
                                    <asp:DropDownList ID="ddlEquip" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEquip_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                 <td class="CSTD" style="width: 20%;">
                                    <asp:Label ID="lblSN" Text="<%$ Resources:Auto|RuleFace, SN %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 30%;">
                                    <asp:DropDownList ID="ddlSN" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSN_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="CSTD" style="width: 20%;">
                                    <asp:Label ID="lblWorkOrderLot" Text="<%$ Resources:Auto|RuleFace, WorkOrderLot %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 30%;">
                                     <asp:TextBox ID="ttbWorkOrderLot" runat="server" ReadOnly="true" ></asp:TextBox>
                                </td>
                                <td class="CSTD" style="width: 20%;">
                                    <asp:Label ID="lblMaterialLot" Text="<%$ Resources:Auto|RuleFace, MaterialLot %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 30%;">
                                     <asp:TextBox ID="ttbMaterialLot" runat="server" ReadOnly="true" ></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>    
                    <table style="width: 99%; margin-left: auto; margin-right: auto;" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 30%;">
                                <CimesUI:CimesTab ID="rtsDetail" runat="server" MultiPageID="rmpDetail" selectedindex="0"
                                    Width="100%" CssClass="tabStrip">
                                    <Tabs>
                                        <CimesUI:CimesTabItem Text="<%$ Resources:Auto|RuleFace, DecisionMethod %>" Target="rpvDecisionMethod"
                                            Value="DecisionMethod" Selected="true" Enabled="true"></CimesUI:CimesTabItem>
                                        <CimesUI:CimesTabItem Text="<%$ Resources:Auto|RuleFace, IndependentInspection %>" Target="rpvInspectionData"
                                            Value="InspectionData" Selected="false" Enabled="true"></CimesUI:CimesTabItem>
                                    </Tabs>
                                </CimesUI:CimesTab>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <CimesUI:CimesMultiPage ID="rmpDetail" runat="server" selectedindex="0" scrollbars="Auto" CssClass="RadPageView" Width="99%" Height="250px">
                                    <CimesUI:CimesPageView ID="rpvDecisionMethod" runat="server" Width="99%" Enabled="true">
                                        <table width="90%">
                                            <tr>
                                                <td class="CSTD" style="width: 15%;">
                                                    <asp:Label ID="lblPQCResult" Text="<%$ Resources:Auto|RuleFace, PQCResult %>" runat="server"></asp:Label>
                                                </td>
                                                <td style="width: 35%;">
                                                    <asp:RadioButton ID="rdbOK" runat="server" Text="OK" GroupName="RepairResult" />
                                                    <asp:RadioButton ID="rdbNG" runat="server" Text="NG" GroupName="RepairResult" />
                                                </td>
                                                <td class="CSTD" style="width: 15%;"> </td>
                                                <td style="width: 35%;"></td>
                                            </tr>
                                            <tr>
                                                <td class="CSTD" style="width: 15%;">
                                                    <asp:Label ID="lblPQCReasonCode" Text="<%$ Resources:Auto|RuleFace, ReasonCode %>" runat="server"></asp:Label>
                                                </td>
                                                <td style="width: 35%;">
                                                    <asp:DropDownList ID="ddlPQCReasonCode" runat="server"></asp:DropDownList>
                                                </td>
                                                <td class="CSTD" style="width: 20%;"> </td>
                                                <td style="width: 30%;"></td>
                                            </tr>
                                            <tr>
                                                <td class="CSTD" style="width: 15%;">
                                                    <asp:Label ID="lblDescr" Text="<%$ Resources:Auto|RuleFace, Description %>" runat="server"></asp:Label>
                                                </td>
                                                <td style="width: 35%;">
                                                        <asp:TextBox ID="ttbDescr" runat="server" Height="60px" TextMode="MultiLine" ></asp:TextBox>
                                                </td>
                                                <td class="CSTD" style="width: 20%;"> </td>
                                                <td style="width: 30%;"></td>
                                            </tr>
                                        </table>
                                    </CimesUI:CimesPageView>
                                    <CimesUI:CimesPageView ID="rpvInspectionData" runat="server" Width="99%" Enabled="true">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td align="center">
                                                    <div style="height: 200px; overflow: auto;">
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
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </CimesUI:CimesPageView>
                                </CimesUI:CimesMultiPage>
                            </td>
                        </tr>
                    </table>
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

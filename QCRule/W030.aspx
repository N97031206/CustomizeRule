<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="W030.aspx.cs" Inherits="CustomizeRule.QCRule.W030" %>

<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>W030</title>
</head>
<body>
    <form id="W030" runat="server">
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
                                <td class="CSTDMust" style="width: 22%;">
                                    <asp:Label ID="lblLot" Text="<%$ Resources:Auto|RuleFace, eqp_complot_lot %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 28%;">
                                     <asp:TextBox ID="ttbLot" runat="server" AutoPostBack="true" OnTextChanged="ttbLot_TextChanged"></asp:TextBox>
                                </td>
                                <td class="CSTD" style="width: 20%;"> </td>
                                <td style="width: 30%;"></td>
                            </tr>
                            <tr>
                                <td class="CSTD" style="width: 22%;">
                                    <asp:Label ID="lblEquip" Text="<%$ Resources:Auto|RuleFace, Equip %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 28%;">
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
                                <td class="CSTD" style="width: 22%;">
                                    <asp:Label ID="lblWorkOrderLot" Text="<%$ Resources:Auto|RuleFace, WorkOrderLot %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 28%;">
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
                                        <CimesUI:CimesTabItem Text="<%$ Resources:Auto|RuleFace, InspectionData %>" Target="rpvInspectionData"
                                            Value="InspectionData" Selected="false" Enabled="true"></CimesUI:CimesTabItem>
                                       <%-- <CimesUI:CimesTabItem Text="<%$ Resources:Auto|RuleFace, InspectionDataFaceP %>" Target="rpvInspectionDataFaceP"
                                            Value="InspectionDataFaceP" Selected="false" Enabled="true"></CimesUI:CimesTabItem>
                                        <CimesUI:CimesTabItem Text="<%$ Resources:Auto|RuleFace, InspectionDataFaceN %>" Target="rpvInspectionDataFaceN"
                                            Value="InspectionDataFaceN" Selected="false" Enabled="true"></CimesUI:CimesTabItem>--%>
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
                                                    <asp:Label ID="lblFAIResult" Text="<%$ Resources:Auto|RuleFace, FAIResult %>" runat="server"></asp:Label>
                                                </td>
                                                <td style="width: 35%;">
                                                    <asp:RadioButton ID="rdbOK" runat="server" Text="OK" GroupName="RepairResult" />
                                                    <asp:RadioButton ID="rdbNG" runat="server" Text="NG" GroupName="RepairResult" />
                                                    <asp:RadioButton ID="rdbPASS" runat="server" Text="SKIP" GroupName="RepairResult" />
                                                    <asp:RadioButton ID="rdbCLOSE" runat="server" Text="CLOSE" GroupName="RepairResult" />
                                                </td>
                                                <td class="CSTD" style="width: 15%;"> </td>
                                                <td style="width: 35%;"></td>
                                            </tr>
                                            <tr>
                                                <td class="CSTD" style="width: 15%;">
                                                    <asp:Label ID="lblFAIReasonCode" Text="<%$ Resources:Auto|RuleFace, ReasonCode %>" runat="server"></asp:Label>
                                                </td>
                                                <td style="width: 35%;">
                                                    <asp:DropDownList ID="ddlFAIReasonCode" runat="server"></asp:DropDownList>
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
                                        <table style="width: 80%;">
                                            <tr>
                                                 <td class="CSTD" style="width: 20%;">
                                                    <asp:Label ID="lblFileName" Text="<%$ Resources:Auto|RuleFace, FileName %>" runat="server"></asp:Label>
                                                </td>
                                                <td style="width: 80%;">
                                                     <asp:DropDownList ID="ddlFileName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFileName_SelectedIndexChanged"></asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td align="center">
                                                    <div style="height: 200px; overflow: auto;">
                                                        <CimesUI:CimesGridView ID="gvInspectionData" runat="server" Width="95%" AutoGenerateColumns="false">
                                                            <Columns>
                                                                <asp:BoundField DataField="Characteristic" HeaderText="<%$ Resources:Auto|RuleFace, Characteristic %>" HeaderStyle-Width="15%" />
                                                                <asp:BoundField DataField="Actual" HeaderText="<%$ Resources:Auto|RuleFace, Actual %>" HeaderStyle-Width="17%"/>
                                                                <asp:BoundField DataField="Nominal" HeaderText="<%$ Resources:Auto|RuleFace, Nominal %>" HeaderStyle-Width="17%"/>
                                                                <asp:BoundField DataField="Upper" HeaderText="<%$ Resources:Auto|RuleFace, Upper %>" HeaderStyle-Width="17%"/>
                                                                <asp:BoundField DataField="Lower" HeaderText="<%$ Resources:Auto|RuleFace, Lower %>" HeaderStyle-Width="17%"/>
                                                                <asp:BoundField DataField="Deviation" HeaderText="<%$ Resources:Auto|RuleFace, Deviation %>" HeaderStyle-Width="17%"/>
                                                            </Columns>
                                                        </CimesUI:CimesGridView>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </CimesUI:CimesPageView>
                                    <%--<CimesUI:CimesPageView ID="rpvInspectionDataFaceP" runat="server" Width="99%" Enabled="true">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td align="center">
                                                    <div style="height: 200px; overflow: auto;">
                                                        <CimesUI:CimesGridView ID="gvInspectionDataFaceP" runat="server" Width="95%" AutoGenerateColumns="false">
                                                            <Columns>
                                                                <asp:BoundField DataField="Characteristic" HeaderText="<%$ Resources:Auto|RuleFace, Characteristic %>" HeaderStyle-Width="15%" />
                                                                <asp:BoundField DataField="Actual" HeaderText="<%$ Resources:Auto|RuleFace, Actual %>" HeaderStyle-Width="17%"/>
                                                                <asp:BoundField DataField="Nominal" HeaderText="<%$ Resources:Auto|RuleFace, Nominal %>" HeaderStyle-Width="17%"/>
                                                                <asp:BoundField DataField="Upper" HeaderText="<%$ Resources:Auto|RuleFace, Upper %>" HeaderStyle-Width="17%"/>
                                                                <asp:BoundField DataField="Lower" HeaderText="<%$ Resources:Auto|RuleFace, Lower %>" HeaderStyle-Width="17%"/>
                                                                <asp:BoundField DataField="Deviation" HeaderText="<%$ Resources:Auto|RuleFace, Deviation %>" HeaderStyle-Width="17%"/>
                                                            </Columns>
                                                        </CimesUI:CimesGridView>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </CimesUI:CimesPageView>
                                    <CimesUI:CimesPageView ID="rpvInspectionDataFaceN" runat="server" Width="99%" Enabled="true">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td align="center">
                                                    <div style="height: 200px; overflow: auto;">
                                                        <CimesUI:CimesGridView ID="gvInspectionDataFaceN" runat="server" Width="95%" AutoGenerateColumns="false">
                                                            <Columns>
                                                                <asp:BoundField DataField="Characteristic" HeaderText="<%$ Resources:Auto|RuleFace, Characteristic %>" HeaderStyle-Width="15%" />
                                                                <asp:BoundField DataField="Actual" HeaderText="<%$ Resources:Auto|RuleFace, Actual %>" HeaderStyle-Width="17%"/>
                                                                <asp:BoundField DataField="Nominal" HeaderText="<%$ Resources:Auto|RuleFace, Nominal %>" HeaderStyle-Width="17%"/>
                                                                <asp:BoundField DataField="Upper" HeaderText="<%$ Resources:Auto|RuleFace, Upper %>" HeaderStyle-Width="17%"/>
                                                                <asp:BoundField DataField="Lower" HeaderText="<%$ Resources:Auto|RuleFace, Lower %>" HeaderStyle-Width="17%"/>
                                                                <asp:BoundField DataField="Deviation" HeaderText="<%$ Resources:Auto|RuleFace, Deviation %>" HeaderStyle-Width="17%"/>
                                                            </Columns>
                                                        </CimesUI:CimesGridView>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </CimesUI:CimesPageView>--%>
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

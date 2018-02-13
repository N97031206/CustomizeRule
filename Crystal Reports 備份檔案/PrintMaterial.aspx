<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintMaterial.aspx.cs" Inherits="CustomizeRule.MMSRule.PrintMaterial" %>

<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock" TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>PrintMaterial</title>
</head>
<body>
    <form id="PrintMaterial" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:programinformationblock id="ProgramInformationBlock1" runat="server" />
                <table class="csTable" style="width: 920px; text-align: left; vertical-align: top; margin-left: auto; margin-right: auto;" border="0">
                    <tr>
                        <td class="CSTD" style="width: 15%">
                            <asp:Label ID="lblMaterialFormNo" runat="server" Text="<%$ Resources:Auto|RuleFace, MaterialFormNo %>"></asp:Label></td>
                        <td style="width: 20%">
                            <asp:TextBox ID="ttbMaterialFormNo" runat="server"></asp:TextBox></td>
                        <td class="CSTD" style="width: 15%">
                            <asp:Label ID="lblYear" runat="server" Text="<%$ Resources:Auto|RuleFace, Year %>"></asp:Label></td>
                        <td style="width: 20%">
                            <asp:TextBox ID="ttbYear" runat="server"></asp:TextBox></td>
                        <td class="CSTD" style="width: 30%">
                        </td>
                    </tr>
                    <tr>
                        <td class="CSTD" style="width: 15%">
                            <asp:Label ID="lblSapNo" runat="server" Text="<%$ Resources:Auto|RuleFace, SapNo %>"></asp:Label></td>
                        <td style="width: 20%">
                            <asp:TextBox ID="ttbSapNo" runat="server"></asp:TextBox></td>
                        <td class="CSTD" style="width: 15%" colspan="2">
                            <cimesui:cimesbutton id="btnQuery" runat="server" cssclass="CSQueryButton" text="<%$ Resources:Auto|RuleFace, Query %>" onclick="btnQuery_Click" />
                        </td>
                        <td class="CSTD" style="width: 30%"></td>
                    </tr>
                </table>
                <div style="width: 1160px; height: 400px; text-align: left; vertical-align: top; margin-left: auto; margin-right: auto;">
                    <fieldset>
                        <div style="width: 100%; height: 400px; overflow: auto;">
                            <cimesui:cimesgridview id="gvMaterialLotList" runat="server" onrowcommand="gvMaterialLotList_RowCommand" autogeneratecolumns="false" width="100%">
                                    <Columns>
                                    <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, Print %>">
                                        <HeaderStyle Wrap="False" Width="8%"></HeaderStyle>
                                        <ItemTemplate>
                                            <CimesUI:CimesButton ID="btnPrintByMaterialLot" runat="server" Enabled="false" CssClass="CSPrintButton" Text="<%$ Resources:Auto|RuleFace, Print %>" CommandName="Print" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, MBLNR %>" Visible="False">
                                            <HeaderStyle Wrap="false"></HeaderStyle>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtMBLNR" runat="server" Text='<%# Bind("MBLNR") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblMBLNR" runat="server" Text='<%# Bind("MBLNR") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, MJAHR %>" Visible="False">
                                            <HeaderStyle Wrap="true"></HeaderStyle>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtMJAHR" runat="server" Text='<%# Bind("MJAHR") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblMJAHR" runat="server" Text='<%# Bind("MJAHR") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, ZEILE %>" Visible="False">
                                            <HeaderStyle Wrap="False"></HeaderStyle>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtZEILE" runat="server" Text='<%# Bind("ZEILE") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblZEILE" runat="server" Text='<%# Bind("ZEILE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, MATNR %>">
                                            <HeaderStyle Wrap="False"></HeaderStyle>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtMATNR" runat="server" Text='<%# Bind("MATNR") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblMATNR" runat="server" Text='<%# Bind("MATNR") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, MAKTX %>" >
                                            <HeaderStyle Wrap="False"></HeaderStyle>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtMAKTX" runat="server" Text='<%# Bind("MAKTX") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblMAKTX" runat="server" Text='<%# Bind("MAKTX") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, LGORT %>">
                                            <HeaderStyle Wrap="False"></HeaderStyle>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtLGORT" runat="server" Text='<%# Bind("LGORT") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblLGORT" runat="server" Text='<%# Bind("LGORT") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, MENGE %>">
                                            <HeaderStyle Wrap="true"></HeaderStyle>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtMENGE" runat="server" Text='<%# Bind("MENGE") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblMENGE" runat="server" Text='<%# Bind("MENGE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, MEINS %>">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtMEINS" runat="server" Text='<%# Bind("MEINS") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblMEINS" runat="server" Text='<%# Bind("MEINS") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, EBELN %>" Visible="False">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtEBELN" runat="server" Text='<%# Bind("EBELN") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblEBELN" runat="server" Text='<%# Bind("EBELN") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, EBELP %>" Visible="False">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtEBELP" runat="server" Text='<%# Bind("EBELP") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblEBELP" runat="server" Text='<%# Bind("EBELP") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, LIFNR %>" Visible="False">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtLIFNR" runat="server" Text='<%# Bind("LIFNR") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblLIFNR" runat="server" Text='<%# Bind("LIFNR") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, LIFNRTXT %>" Visible="False">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtLIFNRTXT" runat="server" Text='<%# Bind("LIFNRTXT") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblLIFNRTXT" runat="server" Text='<%# Bind("LIFNRTXT") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, CHARG %>">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtCHARG" runat="server" Text='<%# Bind("CHARG") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCHARG" runat="server" Text='<%# Bind("CHARG") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, VENDORBATCH %>">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtVENDORBATCH" runat="server" Text='<%# Bind("VENDORBATCH") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblVENDORBATCH" runat="server" Text='<%# Bind("VENDORBATCH") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, HSDAT %>" Visible="False">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtHSDAT" runat="server" Text='<%# Bind("HSDAT") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblHSDAT" runat="server" Text='<%# Bind("HSDAT") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, MAKER %>" Visible="False">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtMAKER" runat="server" Text='<%# Bind("MAKER") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblMAKER" runat="server" Text='<%# Bind("MAKER") %>' ></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, MAKERTXT %>" Visible="False">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtMAKERTXT" runat="server" Text='<%# Bind("MAKERTXT") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblMAKERTXT" runat="server" Text='<%# Bind("MAKERTXT") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, SORTL %>" Visible="False">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtSORTL" runat="server" Text='<%# Bind("SORTL") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSORTL" runat="server" Text='<%# Bind("SORTL") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, SORTLMK %>" Visible="False">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtSORTLMK" runat="server" Text='<%# Bind("SORTLMK") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSORTLMK" runat="server" Text='<%# Bind("SORTLMK") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, PRINTNUM %>">
                                            <HeaderStyle Wrap="False" Width="5%"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPRINTNUM" runat="server" Text='<%# Bind("ROLLNUM") %>' style="width:90%"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        </Columns>
                                </cimesui:cimesgridview>
                        </div>
                    </fieldset>
                </div>

                <div align="center">
                    <cimesui:cimesbutton id="btnSave" runat="server" enabled="false" cssclass="CSSaveButton" text="<%$ Resources:Auto|RuleFace, Save %>" onclick="btnSave_Click" />
                    <cimesui:cimesbutton id="btnPrint" runat="server" enabled="false" cssclass="CSPrintButton" text="<%$ Resources:Auto|RuleFace, Print %>" onclick="btnPrint_Click" />
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>

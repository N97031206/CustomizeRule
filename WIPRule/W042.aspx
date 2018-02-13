<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="W042.aspx.cs" Inherits="CustomizeRule.WIPRule.W042" %>

<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>W042</title>
</head>
<body>
    <form id="W042" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:ProgramInformationBlock ID="ProgramInformationBlock1" runat="server" />
                <div style="margin: 0 auto; width: 600px;">
                    <fieldset style="width: 100%;">
                        <legend>
                            <asp:Label ID="lblWOInfo" runat="server" Text="<%$ Resources:Auto|RuleFace, WOInfo %>"></asp:Label>
                        </legend>
                        <table style="width: 97%;">
                            <tr>
                                <td class="CSTDMust" style="width: 20%;">
                                    <asp:Label ID="lblWorkOrderLot" Text="<%$ Resources:Auto|RuleFace, WorkOrderLot %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 80%;">
                                    <asp:TextBox ID="ttbWorkOrderLot" runat="server" OnTextChanged="ttbWorkOrderLot_TextChanged" AutoPostBack="true"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset style="width: 100%;">
                        <legend>
                            <asp:Label ID="lblLotList" runat="server" Text="<%$ Resources:Auto|RuleFace, WorkpieceList %>"></asp:Label>
                        </legend>
                        <table style="width: 100%;">
                            <tr>
                                <td align="center">
                                    <div style="width: 100%; height: 300px; overflow: auto;">
                                        <cimesui:cimesgridview id="gvWorkpiece" runat="server" width="95%" autogeneratecolumns="false" OnRowDataBound="gvWorkpiece_RowDataBound">
                                            <Columns>                                                
                                                <asp:BoundField DataField="ComponentLot" HeaderText="<%$ Resources:Auto|RuleFace, WorkpieceLot %>" HeaderStyle-Width="40%" />
                                                <asp:BoundField DataField="OperationName" HeaderText="<%$ Resources:Auto|RuleFace, Operation %>" HeaderStyle-Width="40%" />
                                                <asp:BoundField DataField="Status" HeaderText="<%$ Resources:Auto|RuleFace, Status %>" HeaderStyle-Width="20%"/>
                                            </Columns>
                                        </cimesui:cimesgridview>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <table style="width: 100%;">
                        <tr>
                            <td align="center">
                                <cimesui:cimesbutton id="btnOK" runat="server" cssclass="CSOKButton"
                                    text="<%$ Resources:Auto|RuleFace, OK %>" onclick="btnOK_Click"></cimesui:cimesbutton>
                                <cimesui:cimesbutton id="btnCancel" runat="server" cssclass="CSCancelButton" onclick="btnCancel_Click"
                                    text="<%$ Resources:Auto|RuleFace, Exit %>" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>

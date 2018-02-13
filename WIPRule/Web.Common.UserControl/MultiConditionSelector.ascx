<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MultiConditionSelector.ascx.cs"
    Inherits="ciMes.Web.Common.UserControl.MultiConditionSelector" %>
<asp:Panel ID="pnlConditionSelector" runat="server" GroupingText="Class Name" Width="100%"
    Height="100%">
    <div id="divConditionSelector" style="height: auto; border-width: 0px; overflow: auto;">
        <table cellspacing="1" cellpadding="1" style="border: 0px solid #C0C0C0; width: 100%"
            id="tblSelector" runat="server">
            <tr>
                <td>
                    <asp:Label ID="pnlREMARK01" runat="server" Text="Class Name" Width="98%"></asp:Label>
                    <asp:TextBox ID="ttbFilterREMARK01" runat="server" AutoPostBack="True" OnTextChanged="ttbRemarkFilter_TextChanged"
                        Style="width: 0px; visibility: hidden"></asp:TextBox><asp:DropDownList ID="ddlREMARK01"
                            runat="server" Style="width: 100%">
                        </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="pnlREMARK02" runat="server" Text="Class Name" Width="98%"></asp:Label>
                    <asp:TextBox ID="ttbFilterREMARK02" runat="server" AutoPostBack="True" OnTextChanged="ttbRemarkFilter_TextChanged"
                        Style="width: 0px; visibility: hidden"></asp:TextBox><asp:DropDownList ID="ddlREMARK02"
                            runat="server" Style="width: 100%">
                        </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="pnlREMARK03" runat="server" Text="Class Name" Width="98%"></asp:Label>
                    <asp:TextBox ID="ttbFilterREMARK03" runat="server" AutoPostBack="True" OnTextChanged="ttbRemarkFilter_TextChanged"
                        Style="width: 0px; visibility: hidden"></asp:TextBox><asp:DropDownList ID="ddlREMARK03"
                            runat="server" Style="width: 100%">
                        </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="pnlREMARK04" runat="server" Text="Class Name" Width="98%"></asp:Label>
                    <asp:TextBox ID="ttbFilterREMARK04" runat="server" AutoPostBack="True" OnTextChanged="ttbRemarkFilter_TextChanged"
                        Style="width: 0px; visibility: hidden"></asp:TextBox><asp:DropDownList ID="ddlREMARK04"
                            runat="server" Style="width: 100%">
                        </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="pnlREMARK05" runat="server" Text="Class Name" Width="98%"></asp:Label>
                    <asp:TextBox ID="ttbFilterREMARK05" runat="server" AutoPostBack="True" OnTextChanged="ttbRemarkFilter_TextChanged"
                        Style="width: 0px; visibility: hidden"></asp:TextBox><asp:DropDownList ID="ddlREMARK05"
                            runat="server" Style="width: 100%">
                        </asp:DropDownList>
                </td>
                <td style="width: 16px" rowspan="2" align="left" valign="top">
                    <cimesui:cimesbutton id="btnQuery" runat="server" onclick="btnQuery_Click" CssClass="CSQueryButton" Text=""
                        width="40px"></cimesui:cimesbutton>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="pnlREMARK06" runat="server" Text="Class Name" Width="98%"></asp:Label>
                    <asp:TextBox ID="ttbFilterREMARK06" runat="server" AutoPostBack="True" OnTextChanged="ttbRemarkFilter_TextChanged"
                        Style="width: 0px; visibility: hidden"></asp:TextBox><asp:DropDownList ID="ddlREMARK06"
                            runat="server" Style="width: 100%">
                        </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="pnlREMARK07" runat="server" Text="Class Name" Width="98%"></asp:Label>
                    <asp:TextBox ID="ttbFilterREMARK07" runat="server" AutoPostBack="True" OnTextChanged="ttbRemarkFilter_TextChanged"
                        Style="width: 0px; visibility: hidden"></asp:TextBox><asp:DropDownList ID="ddlREMARK07"
                            runat="server" Style="width: 100%">
                        </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="pnlREMARK08" runat="server" Text="Class Name" Width="98%"></asp:Label>
                    <asp:TextBox ID="ttbFilterREMARK08" runat="server" AutoPostBack="True" OnTextChanged="ttbRemarkFilter_TextChanged"
                        Style="width: 0px; visibility: hidden"></asp:TextBox><asp:DropDownList ID="ddlREMARK08"
                            runat="server" Style="width: 100%">
                        </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="pnlREMARK09" runat="server" Text="Class Name" Width="98%"></asp:Label>
                    <asp:TextBox ID="ttbFilterREMARK09" runat="server" AutoPostBack="True" OnTextChanged="ttbRemarkFilter_TextChanged"
                        Style="width: 0px; visibility: hidden"></asp:TextBox><asp:DropDownList ID="ddlREMARK09"
                            runat="server" Style="width: 100%">
                        </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="pnlREMARK10" runat="server" Text="Class Name" Width="98%"></asp:Label>
                    <asp:TextBox ID="ttbFilterREMARK10" runat="server" AutoPostBack="True" OnTextChanged="ttbRemarkFilter_TextChanged"
                        Style="width: 0px; visibility: hidden"></asp:TextBox><asp:DropDownList ID="ddlREMARK10"
                            runat="server" Style="width: 100%">
                        </asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
</asp:Panel>

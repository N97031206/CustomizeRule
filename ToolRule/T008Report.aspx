<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="T008Report.aspx.cs" Inherits="CustomizeRule.ToolRule.T008Report" %>

<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock" TagPrefix="uc2" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>T008Report</title>
    <style type="text/css">
        #Cimesdatetimepicker1 {
            display: none;
        }
    </style>
</head>
<body class="Model">
    <form id="T008Report" method="post" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:ProgramInformationBlock ID="ProgramInformationBlock1" runat="server" />
                <CimesUI:CimesDateTimePicker ID="Cimesdatetimepicker1" runat="server" />
                <table id="table1" style="width: 1000px; margin-left: auto; margin-right: auto;">
                     <tr>
                        <td colspan="6" style="text-align: right">
                            <CimesUI:CimesButton ID="btnExit" runat="server" CssClass="CSExitButton" OnClick="btnExit_Click"
                                Text="<%$ Resources:Auto|RuleFace, Exit %>" />
                        </td>
                    </tr>
                   
                </table>
                <div style="overflow: auto; margin-left: auto; margin-right: auto; width: 500px; height: 300px;">
                    <CimesUI:CimesGridView ID="gvToolReport" runat="server" Width="98%" AutoGenerateColumns="false" 
                        OnRowDataBound="gvToolReport_RowDataBound" OnRowDeleting="gvToolReport_RowDeleting" >
                        <Columns>
                            <CimesUI:CimesCommandField ShowDeleteButton="true" HeaderStyle-Width="10%"></CimesUI:CimesCommandField>
                            <%--<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" HeaderStyle-Wrap="false">
                                <ItemTemplate>
                                     <CimesUI:CimesButton ID="btnOpen" runat="server" OnClick="btnOpen_Click" 
                                         Text="<%$ Resources:Auto|RuleFace, OpenFile %>" align="center" />
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <%--<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Auto|RuleFace, FileName %>" HeaderStyle-Width="40%" HeaderStyle-Wrap="false">
                                <ItemTemplate>
                                     <asp:Label ID="lblFileName" runat="server" Width="100%"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                             <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Auto|RuleFace, FileName %>" HeaderStyle-Width="60%" HeaderStyle-Wrap="false">
                                <ItemTemplate>
                                     <asp:LinkButton ID="btnOpenFile" runat="server" Width="100%" OnClick="btnOpenFile_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Auto|RuleFace, UpdateTime %>" HeaderStyle-Width="30%" HeaderStyle-Wrap="false">
                                <ItemTemplate>
                                     <asp:Label ID="lblUpdateTime" runat="server" Width="100%"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </CimesUI:CimesGridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebApp.aspx.cs" Inherits="CustomizeRule.WIPRule.WebApp" %>

<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="WebApp" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <asp:Updatepanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:ProgramInformationBlock ID="ProgramInformationBlock1" runat="server" />
                <div style="text-align:center">
                    <iframe src="http://192.168.100.160/SAIWebApp/#/login" width="400" height="520"></iframe>
                </div>             
            </ContentTemplate>
        </asp:Updatepanel>
    </form>
</body>
</html>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProgramInformationBlock.ascx.cs"
    Inherits="ciMes.Web.Common.UserControl.ProgramInformationBlock" %>
<div id="content-title">
    <asp:Label ID="lblCaption" runat="server"></asp:Label>
    <asp:Label ID="lblTime" runat="server" CssClass="date"></asp:Label>
    <asp:UpdateProgress ID="updateProgress" runat="server" DisplayAfter="1">
        <ProgressTemplate>
            <asp:Image runat="server" Style="float: right" ID="imgProgress" ImageUrl="~/Images/Loading.gif"
                Height="16px" Width="16px" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div id="title-message-content" style="float: right; max-width: 400px; overflow: hidden; position: relative; margin-right: 10px;">
        <span id="title-message" style="display: block; white-space: nowrap;"></span>
        <span id="title-message-extend" style="display: none; position: absolute; right: 0; top: 0; z-index: +1; color: #fff; background-color: #666766;">...</span>
    </div>
    <div id="message_icon" class="icon" style="width: 25px; height: 25px; display: none;" onclick="javascript:ShowPIBDetail(this);"></div>
</div>
<div id="pib-dialog" style="display: none; font-size: medium !important;">
    <div id="pnlMessagePopup" style="overflow: hidden; width: 700px; max-height: 320px;">
        <div id="divMessageContent" style="width: 100%" class="messageTitle" runat="server">
            <asp:Label ID="lblPopupMessageTitle" runat="server" Width="100%" TabIndex="-1"></asp:Label>
        </div>
        <table style="width: 100%; height: 100%" cellpadding="1" cellspacing="0" border="0">
            <tr>
                <td>
                    <table style="width: 100%;" cellpadding="1" cellspacing="0" border="1px" class="innerTable">
                        <tr>
                            <td colspan="4" align="center">
                                <input id="btnDetail" type="button" class="pib-button pib-down" runat="server" visible="false"
                                    value="<%$ Resources:Auto|RuleFace, Expand %>" onclick="javascript: SwitchDetail();" />
                                <input id="btnClose" type="button" runat="server" class="pib-button pib-cancel" value="<%$ Resources:Auto|RuleFace, Close %>" />
                                <input id="btnCopy" type="button" value="<%$ Resources:Auto|RuleFace, CopyMessage %>"
                                    class="pib-button pib-CopyPost" runat="server" visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tbla CSTD" style="width: 20%">
                                <asp:Label ID="lblUserLabel" Text="<%$ Resources:Auto|RuleFace, UserID %>" runat="server"></asp:Label>
                            </td>
                            <td style="width: 30%">
                                <asp:Label ID="lblUserID" runat="server" ForeColor="Blue"></asp:Label>
                            </td>
                            <td class="tbla CSTD" style="width: 20%">
                                <asp:Label ID="lblServerInfo" runat="server" Text="<%$ Resources:Auto|RuleFace, Server %>"></asp:Label>
                            </td>
                            <td style="width: 30%">
                                <asp:Label ID="lblServer" runat="server" ForeColor="Blue"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tbla CSTD" style="width: 20%">
                                <asp:Label ID="lblClientInfo" runat="server" Text="<%$ Resources:Auto|RuleFace, Client %>"></asp:Label>
                            </td>
                            <td style="width: 30%">
                                <asp:Label ID="lblClient" runat="server" ForeColor="Blue"></asp:Label>
                            </td>
                            <td class="tbla CSTD" style="width: 20%">
                                <asp:Label ID="lblVersionInfo" runat="server" Text="<%$ Resources:Auto|RuleFace, Version %>"></asp:Label>
                            </td>
                            <td style="width: 30%">
                                <asp:Label ID="lblVersion" runat="server" ForeColor="Blue"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <asp:Label ID="lblMessageForCopy" runat="server" Style="display: none"></asp:Label>
                    <div id="divExceptionDetail" style="overflow: auto; border: none; width: 100%; height: 200px; display: none;">
                        <CimesUI:CimesGridView ID="gvException" runat="server" AutoGenerateColumns="False" Width="100%" EnableViewState="false">
                            <columns>
                                <asp:BoundField DataField="Index" HeaderText="<%$ Resources:Auto|RuleFace, Index %>">
                                    <ItemStyle Width="50px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Message" HeaderText="<%$ Resources:Auto|RuleFace, Message %>"></asp:BoundField>
                            </columns>
                        </CimesUI:CimesGridView>
                        <CimesUI:CimesGridView ID="gvInnerException" runat="server" AutoGenerateColumns="False" Width="96%" EnableViewState="false">
                            <columns>
                                <asp:BoundField DataField="Index" HeaderText="<%$ Resources:Auto|RuleFace, Index %>">
                                    <ItemStyle Width="50px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Message" HeaderText="<%$ Resources:Auto|RuleFace, Message %>"></asp:BoundField>
                            </columns>
                        </CimesUI:CimesGridView>
                    </div>
                </td>
            </tr>
        </table>
        <asp:Label ID="pibMessageFooter" runat="server" EnableViewState="false" Style="float: right; font-size: smaller !important;"></asp:Label>
    </div>
</div>
<script type="text/javascript">var pibDialog = null; function ClosePopup(a, b) { if (null == pibDialog) return !1; EnableEventKeyOfTab(); try { pibDialog.dialog("close"); if (null != document.getElementById(a)) ajaxFocus(a, b); else if (null != document.getElementById("hdnPostBackControl")) { var c = document.getElementById("hdnPostBackControl").value; null != document.getElementById(c) && document.getElementById(c).focus() } try { MsgHintAndRedirect() } catch (d) { } } catch (e) { alert(e) } return !1 }
    function pibToCenter() { var a = pibDialog.parent(); a.css("top", Math.max(0, ($(window).height() - a.outerHeight()) / 2 + $(window).scrollTop()) + "px"); a.css("left", Math.max(0, ($(window).width() - a.outerWidth()) / 2 + $(window).scrollLeft()) + "px") }
    function SwitchDetail() { var a = $(event.srcElement); a.removeClass("pib-up").removeClass("pib-down"); if (null == pibDialog) return !1; var b = $("#divExceptionDetail"); "none" == b.css("display") ? (a.addClass("pib-up"), b.show()) : (a.addClass("pib-down"), b.hide()); pibToCenter(); return !1 } function CopyToPaste(a) { window.clipboardData.setData("Text", document.getElementById(a).innerHTML.replace(/<BR>/g, "\r\n").replace(/<br>/g, "\r\n")) }
    function ShowMessageOnPIBTitle(a, b) { $("#title-message-content").data("message", a); $("#content-title>#message_icon").addClass(b).show(); var c = $("#title-message-content"); c.find("#title-message").html(a); var d = $("<div></div>").html(a).css({ top: -1E4, left: -1E4, position: "absolute" }); d.appendTo("form"); 400 < d.width() && c.find("#title-message-extend").show(); d.remove() } function ShowCimesMessage(a) { $($().CimesMessage("showNoticeMessage", a)).parent().css({ width: "600px" }) }
    function ShowPIBDetail(a) {
        if (!a) return !0; if ($(a).hasClass("hint")) return ShowCimesMessage($("#title-message-content").data("message")), !0; a = $("#pib-dialog .exceptionTitle,.messageTitle").hide().find(">span").first().text(); pibDialog = $("#pib-dialog").dialog({ draggable: !1, modal: !0, autoOpen: !0, resizable: !1, width: "auto", title: a, close: function () { $(this).dialog("destroy"); pibDialog = null } }); a = pibDialog.parent().find(".ui-dialog-titlebar"); a.css({
            width: "700px", "word-wrap": "break-word", "word-break": "break-all", "max-height": "40px",
            overflow: "auto"
        }).removeClass("ui-widget-header").addClass("exceptionTitle"); a.find("button").remove(); var b = a.find("span"); 0 < b.length && a.html(b.first().html()); pibToCenter()
    };
</script>


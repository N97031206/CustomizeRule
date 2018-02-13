using Ares.Cimes.IntelliService;
using Ares.Cimes.IntelliService.Info;
using ciMes.Web.Common;
using ciMes.Web.Common.UserControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomizeRule.ToolRule
{
    public partial class T008Picture : CimesBasePage
    {
        private ProgramInformationBlock _ProgramInformationBlock
        {
            get { return ProgramInformationBlock1 as ProgramInformationBlock; }
        }

        /// <summary>
        /// 儲存路徑資料
        /// </summary>
        private List<WpcExClassItemInfo> _SavePicturedData
        {
            get { return this["_SavePicturedData"] as List<WpcExClassItemInfo>; }
            set { this["_SavePicturedData"] = value; }
        }

        /// <summary>
        /// 刀具類型圖檔清單
        /// </summary>
        private List<CSTToolTypePictureInfo> _ToolTypePictures
        {
            get { return this["_ToolTypePictures"] as List<CSTToolTypePictureInfo>; }
            set { this["_ToolTypePictures"] = value; }
        }

        /// <summary>
        /// 傳入的ToolType
        /// </summary>
        private string _CurrentToolType
        {
            get
            {
                if (Request["ToolType"].IsNullOrTrimEmpty())
                {
                    throw new Exception(TextMessage.Error.T00045(GetUIResource("ToolTypes")));
                }

                return Request["ToolType"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    gvToolTypeImage.Initialize();

                    //取得路徑對應清單
                    _SavePicturedData = GetExtendItemListByClass("SAIToolTypeImage");

                    GetToolTypeImages(_CurrentToolType);
                }
                else
                {

                }
            }
            catch (Exception E)
            {
                HandleError(E);
            }
        }

        /// <summary>
        /// //取得刀具圖檔
        /// </summary>
        /// <param name="toolType"></param>
        private void GetToolTypeImages(string toolType)
        {
            //清除資料
            _ToolTypePictures = new List<CSTToolTypePictureInfo>();

            //取得刀具圖檔清單
            _ToolTypePictures = CSTToolTypePictureInfo.GetDataListByToolType(toolType);

            gvToolTypeImage.SetDataSource(_ToolTypePictures, true);
        }

        protected void gvToolTypeImage_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType != DataControlRowType.DataRow)
                    return;
                
                var btnOpenFile = e.Row.FindControl("btnOpenFile") as LinkButton;

                string filePath = _ToolTypePictures[e.Row.DataItemIndex].FileName;
                var text = filePath.Replace(_SavePicturedData[0].Remark01 + "\\" + _ToolTypePictures[e.Row.DataItemIndex].ToolType + "\\", "");
                btnOpenFile.Text = text;
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        ///// <summary>
        ///// 點擊圖檔
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void ibtnToolType_Click(object sender, ImageClickEventArgs e)
        //{ 
        //    var ibtnToolType = ((ImageButton)sender);

        //    //打開文件
        //    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(),
        //        Session.SessionID, "window.open('" + ibtnToolType.ImageUrl + "','_blank','resizable,scrollbars=no,menubar=no,toolbar=no,location=no,status=no',false);", true);
        //}

        /// <summary>
        /// 開啟檔案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOpenFile_Click(object sender, EventArgs e)
        {
            var row = ((LinkButton)sender).Parent.Parent as GridViewRow;

            string url = "";
            string filePath = _ToolTypePictures[row.DataItemIndex].FileName;
            string sHost = Request.Url.Host;
            string sApplicationPath = Request.ApplicationPath;
            string picturePath = "http://" + sHost + sApplicationPath + "/ToolImage";
            url = filePath.Replace(_SavePicturedData[0].Remark01, picturePath).Replace(@"\", "/");

            //打開文件
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(),
                Session.SessionID, "window.open('" + url + "','_blank','resizable,scrollbars=no,menubar=no,toolbar=no,location=no,status=no',false);", true);
        }

        /// <summary>
        /// 取得系統控制設定的類別資料
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        private List<WpcExClassItemInfo> GetExtendItemListByClass(string className)
        {
            var dataList = WpcExClassItemInfo.GetExtendItemListByClassAndRemarks(className);

            if (dataList.Count == 0)
            {
                // [00466]系統控制設定，類別：[{0}]，項目：[{1}]未設定！
                throw new Exception(TextMessage.Error.T00466(className, ""));
            }

            return dataList;
        }

        /// <summary>
        /// 離開此畫面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "OpenDialog", "javascript:window.parent.closeMasterWindow();", true);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }
    }
}
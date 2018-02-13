using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ciMes.Web.Common;
using Ares.Cimes.IntelliService.Function;
using System.Web.UI.HtmlControls;
using CustomizeRule;

namespace CustomizeRule.RuleUtility
{
    public class CustomizeRuleBasePage : CimesRuleBasePage
    {
        protected override void VCFailure(object sender, EventArgs e)
        {
        }

        protected override void VCSuccess(object sender, EventArgs e)
        {
        }

        public new CustomizeExtendManager RuleExtendManager
        {
            get
            {
                return (CustomizeExtendManager)this["CustomizeRuleExtendManager"];
            }
            private set
            {
                this["CustomizeRuleExtendManager"] = value;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                RuleExtendManager = new CustomizeExtendManager(ProgramRight);
            }

            base.OnLoad(e);
        }
    }
}

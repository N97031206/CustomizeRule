using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ciMes;

namespace CustomizeRule
{
    [Serializable]
    public partial class CustomizeExtendManager : CimesExtendManager
    {
        public CustomizeExtendManager(string sProgramRight)
            : base(sProgramRight)
        {
        }

        public CustomizeExtendManager(string sProgramRight, string sRuleFunctionName):base(sProgramRight, sRuleFunctionName)
        {
        }
    }
}

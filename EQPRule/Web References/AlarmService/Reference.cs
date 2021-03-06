﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.42000
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 原始程式碼已由 Microsoft.VSDesigner 自動產生，版本 4.0.30319.42000。
// 
#pragma warning disable 1591

namespace CustomizeRule.EQPRule {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="AlarmServiceSoap", Namespace="http://tempuri.org/")]
    public partial class AlarmService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback LaunchAlarmNowMailToSpecifyUserSIDOperationCompleted;
        
        private System.Threading.SendOrPostCallback LaunchAlarmNowOperationCompleted;
        
        private System.Threading.SendOrPostCallback LaunchAlarmOperationCompleted;
        
        private System.Threading.SendOrPostCallback CheckAlarmOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public AlarmService() {
            this.Url = global::EQPRule.Properties.Settings.Default.EQPRule_AlarmService_AlarmService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event LaunchAlarmNowMailToSpecifyUserSIDCompletedEventHandler LaunchAlarmNowMailToSpecifyUserSIDCompleted;
        
        /// <remarks/>
        public event LaunchAlarmNowCompletedEventHandler LaunchAlarmNowCompleted;
        
        /// <remarks/>
        public event LaunchAlarmCompletedEventHandler LaunchAlarmCompleted;
        
        /// <remarks/>
        public event CheckAlarmCompletedEventHandler CheckAlarmCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/LaunchAlarmNowMailToSpecifyUserSID", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string LaunchAlarmNowMailToSpecifyUserSID(string module, string type, string msg, string createtype, string userid, string mailto) {
            object[] results = this.Invoke("LaunchAlarmNowMailToSpecifyUserSID", new object[] {
                        module,
                        type,
                        msg,
                        createtype,
                        userid,
                        mailto});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void LaunchAlarmNowMailToSpecifyUserSIDAsync(string module, string type, string msg, string createtype, string userid, string mailto) {
            this.LaunchAlarmNowMailToSpecifyUserSIDAsync(module, type, msg, createtype, userid, mailto, null);
        }
        
        /// <remarks/>
        public void LaunchAlarmNowMailToSpecifyUserSIDAsync(string module, string type, string msg, string createtype, string userid, string mailto, object userState) {
            if ((this.LaunchAlarmNowMailToSpecifyUserSIDOperationCompleted == null)) {
                this.LaunchAlarmNowMailToSpecifyUserSIDOperationCompleted = new System.Threading.SendOrPostCallback(this.OnLaunchAlarmNowMailToSpecifyUserSIDOperationCompleted);
            }
            this.InvokeAsync("LaunchAlarmNowMailToSpecifyUserSID", new object[] {
                        module,
                        type,
                        msg,
                        createtype,
                        userid,
                        mailto}, this.LaunchAlarmNowMailToSpecifyUserSIDOperationCompleted, userState);
        }
        
        private void OnLaunchAlarmNowMailToSpecifyUserSIDOperationCompleted(object arg) {
            if ((this.LaunchAlarmNowMailToSpecifyUserSIDCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.LaunchAlarmNowMailToSpecifyUserSIDCompleted(this, new LaunchAlarmNowMailToSpecifyUserSIDCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/LaunchAlarmNow", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string LaunchAlarmNow(string module, string type, string msg, string createtype, string userid) {
            object[] results = this.Invoke("LaunchAlarmNow", new object[] {
                        module,
                        type,
                        msg,
                        createtype,
                        userid});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void LaunchAlarmNowAsync(string module, string type, string msg, string createtype, string userid) {
            this.LaunchAlarmNowAsync(module, type, msg, createtype, userid, null);
        }
        
        /// <remarks/>
        public void LaunchAlarmNowAsync(string module, string type, string msg, string createtype, string userid, object userState) {
            if ((this.LaunchAlarmNowOperationCompleted == null)) {
                this.LaunchAlarmNowOperationCompleted = new System.Threading.SendOrPostCallback(this.OnLaunchAlarmNowOperationCompleted);
            }
            this.InvokeAsync("LaunchAlarmNow", new object[] {
                        module,
                        type,
                        msg,
                        createtype,
                        userid}, this.LaunchAlarmNowOperationCompleted, userState);
        }
        
        private void OnLaunchAlarmNowOperationCompleted(object arg) {
            if ((this.LaunchAlarmNowCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.LaunchAlarmNowCompleted(this, new LaunchAlarmNowCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/LaunchAlarm", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string LaunchAlarm(string module, string type, string msg, string alarmtime, string createtype, string userid) {
            object[] results = this.Invoke("LaunchAlarm", new object[] {
                        module,
                        type,
                        msg,
                        alarmtime,
                        createtype,
                        userid});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void LaunchAlarmAsync(string module, string type, string msg, string alarmtime, string createtype, string userid) {
            this.LaunchAlarmAsync(module, type, msg, alarmtime, createtype, userid, null);
        }
        
        /// <remarks/>
        public void LaunchAlarmAsync(string module, string type, string msg, string alarmtime, string createtype, string userid, object userState) {
            if ((this.LaunchAlarmOperationCompleted == null)) {
                this.LaunchAlarmOperationCompleted = new System.Threading.SendOrPostCallback(this.OnLaunchAlarmOperationCompleted);
            }
            this.InvokeAsync("LaunchAlarm", new object[] {
                        module,
                        type,
                        msg,
                        alarmtime,
                        createtype,
                        userid}, this.LaunchAlarmOperationCompleted, userState);
        }
        
        private void OnLaunchAlarmOperationCompleted(object arg) {
            if ((this.LaunchAlarmCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.LaunchAlarmCompleted(this, new LaunchAlarmCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/CheckAlarm", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string CheckAlarm() {
            object[] results = this.Invoke("CheckAlarm", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void CheckAlarmAsync() {
            this.CheckAlarmAsync(null);
        }
        
        /// <remarks/>
        public void CheckAlarmAsync(object userState) {
            if ((this.CheckAlarmOperationCompleted == null)) {
                this.CheckAlarmOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCheckAlarmOperationCompleted);
            }
            this.InvokeAsync("CheckAlarm", new object[0], this.CheckAlarmOperationCompleted, userState);
        }
        
        private void OnCheckAlarmOperationCompleted(object arg) {
            if ((this.CheckAlarmCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CheckAlarmCompleted(this, new CheckAlarmCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    public delegate void LaunchAlarmNowMailToSpecifyUserSIDCompletedEventHandler(object sender, LaunchAlarmNowMailToSpecifyUserSIDCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class LaunchAlarmNowMailToSpecifyUserSIDCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal LaunchAlarmNowMailToSpecifyUserSIDCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    public delegate void LaunchAlarmNowCompletedEventHandler(object sender, LaunchAlarmNowCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class LaunchAlarmNowCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal LaunchAlarmNowCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    public delegate void LaunchAlarmCompletedEventHandler(object sender, LaunchAlarmCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class LaunchAlarmCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal LaunchAlarmCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    public delegate void CheckAlarmCompletedEventHandler(object sender, CheckAlarmCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1038.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CheckAlarmCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CheckAlarmCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591
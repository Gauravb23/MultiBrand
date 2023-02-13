﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace HSRP.SMLService {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="RssplServiceSoap", Namespace="http://tempuri.org/")]
    public partial class RssplService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback VinEngineNoValidationOperationCompleted;
        
        private System.Threading.SendOrPostCallback RegNoVinEngineNoHSRPOrderOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public RssplService() {
            this.Url = global::HSRP.Properties.Settings.Default.HSRP_SMLService_RssplService;
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
        public event VinEngineNoValidationCompletedEventHandler VinEngineNoValidationCompleted;
        
        /// <remarks/>
        public event RegNoVinEngineNoHSRPOrderCompletedEventHandler RegNoVinEngineNoHSRPOrderCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/VinEngineNoValidation", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Xml.XmlElement VinEngineNoValidation(string userid, string password, string VinNumber, string ENGINENO) {
            object[] results = this.Invoke("VinEngineNoValidation", new object[] {
                        userid,
                        password,
                        VinNumber,
                        ENGINENO});
            return ((System.Xml.XmlElement)(results[0]));
        }
        
        /// <remarks/>
        public void VinEngineNoValidationAsync(string userid, string password, string VinNumber, string ENGINENO) {
            this.VinEngineNoValidationAsync(userid, password, VinNumber, ENGINENO, null);
        }
        
        /// <remarks/>
        public void VinEngineNoValidationAsync(string userid, string password, string VinNumber, string ENGINENO, object userState) {
            if ((this.VinEngineNoValidationOperationCompleted == null)) {
                this.VinEngineNoValidationOperationCompleted = new System.Threading.SendOrPostCallback(this.OnVinEngineNoValidationOperationCompleted);
            }
            this.InvokeAsync("VinEngineNoValidation", new object[] {
                        userid,
                        password,
                        VinNumber,
                        ENGINENO}, this.VinEngineNoValidationOperationCompleted, userState);
        }
        
        private void OnVinEngineNoValidationOperationCompleted(object arg) {
            if ((this.VinEngineNoValidationCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.VinEngineNoValidationCompleted(this, new VinEngineNoValidationCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/RegNoVinEngineNoHSRPOrder", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Xml.XmlElement RegNoVinEngineNoHSRPOrder(string userid, string password, string RegNo, string VinNumber, string ENGINENO, string OrderStatus, string OrderDate, string DeliverDate) {
            object[] results = this.Invoke("RegNoVinEngineNoHSRPOrder", new object[] {
                        userid,
                        password,
                        RegNo,
                        VinNumber,
                        ENGINENO,
                        OrderStatus,
                        OrderDate,
                        DeliverDate});
            return ((System.Xml.XmlElement)(results[0]));
        }
        
        /// <remarks/>
        public void RegNoVinEngineNoHSRPOrderAsync(string userid, string password, string RegNo, string VinNumber, string ENGINENO, string OrderStatus, string OrderDate, string DeliverDate) {
            this.RegNoVinEngineNoHSRPOrderAsync(userid, password, RegNo, VinNumber, ENGINENO, OrderStatus, OrderDate, DeliverDate, null);
        }
        
        /// <remarks/>
        public void RegNoVinEngineNoHSRPOrderAsync(string userid, string password, string RegNo, string VinNumber, string ENGINENO, string OrderStatus, string OrderDate, string DeliverDate, object userState) {
            if ((this.RegNoVinEngineNoHSRPOrderOperationCompleted == null)) {
                this.RegNoVinEngineNoHSRPOrderOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRegNoVinEngineNoHSRPOrderOperationCompleted);
            }
            this.InvokeAsync("RegNoVinEngineNoHSRPOrder", new object[] {
                        userid,
                        password,
                        RegNo,
                        VinNumber,
                        ENGINENO,
                        OrderStatus,
                        OrderDate,
                        DeliverDate}, this.RegNoVinEngineNoHSRPOrderOperationCompleted, userState);
        }
        
        private void OnRegNoVinEngineNoHSRPOrderOperationCompleted(object arg) {
            if ((this.RegNoVinEngineNoHSRPOrderCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RegNoVinEngineNoHSRPOrderCompleted(this, new RegNoVinEngineNoHSRPOrderCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    public delegate void VinEngineNoValidationCompletedEventHandler(object sender, VinEngineNoValidationCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class VinEngineNoValidationCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal VinEngineNoValidationCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Xml.XmlElement Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlElement)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    public delegate void RegNoVinEngineNoHSRPOrderCompletedEventHandler(object sender, RegNoVinEngineNoHSRPOrderCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RegNoVinEngineNoHSRPOrderCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal RegNoVinEngineNoHSRPOrderCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Xml.XmlElement Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlElement)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591
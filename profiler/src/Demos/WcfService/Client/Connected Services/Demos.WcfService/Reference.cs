﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Datadog.Demos.WcfService.Client.Demos.WcfService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="StringInfo", Namespace="http://schemas.datacontract.org/2004/07/Datadog.Demos.WcfService.Library")]
    [System.SerializableAttribute()]
    public partial class StringInfo : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int HashCodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StringValueField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int HashCode {
            get {
                return this.HashCodeField;
            }
            set {
                if ((this.HashCodeField.Equals(value) != true)) {
                    this.HashCodeField = value;
                    this.RaisePropertyChanged("HashCode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StringValue {
            get {
                return this.StringValueField;
            }
            set {
                if ((object.ReferenceEquals(this.StringValueField, value) != true)) {
                    this.StringValueField = value;
                    this.RaisePropertyChanged("StringValue");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="Datadog.Demos.WcfService", ConfigurationName="Demos.WcfService.IStringProvider")]
    public interface IStringProvider {
        
        [System.ServiceModel.OperationContractAttribute(Action="Datadog.Demos.WcfService/IStringProvider/GenerateRandomAsciiString", ReplyAction="Datadog.Demos.WcfService/IStringProvider/GenerateRandomAsciiStringResponse")]
        string GenerateRandomAsciiString(int length);
        
        [System.ServiceModel.OperationContractAttribute(Action="Datadog.Demos.WcfService/IStringProvider/GenerateRandomAsciiString", ReplyAction="Datadog.Demos.WcfService/IStringProvider/GenerateRandomAsciiStringResponse")]
        System.Threading.Tasks.Task<string> GenerateRandomAsciiStringAsync(int length);
        
        [System.ServiceModel.OperationContractAttribute(Action="Datadog.Demos.WcfService/IStringProvider/ComputeStableHash", ReplyAction="Datadog.Demos.WcfService/IStringProvider/ComputeStableHashResponse")]
        int ComputeStableHash(string str);
        
        [System.ServiceModel.OperationContractAttribute(Action="Datadog.Demos.WcfService/IStringProvider/ComputeStableHash", ReplyAction="Datadog.Demos.WcfService/IStringProvider/ComputeStableHashResponse")]
        System.Threading.Tasks.Task<int> ComputeStableHashAsync(string str);
        
        [System.ServiceModel.OperationContractAttribute(Action="Datadog.Demos.WcfService/IStringProvider/GenerateRandomAsciiStringWithHash", ReplyAction="Datadog.Demos.WcfService/IStringProvider/GenerateRandomAsciiStringWithHashRespons" +
            "e")]
        Datadog.Demos.WcfService.Client.Demos.WcfService.StringInfo GenerateRandomAsciiStringWithHash(int length);
        
        [System.ServiceModel.OperationContractAttribute(Action="Datadog.Demos.WcfService/IStringProvider/GenerateRandomAsciiStringWithHash", ReplyAction="Datadog.Demos.WcfService/IStringProvider/GenerateRandomAsciiStringWithHashRespons" +
            "e")]
        System.Threading.Tasks.Task<Datadog.Demos.WcfService.Client.Demos.WcfService.StringInfo> GenerateRandomAsciiStringWithHashAsync(int length);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IStringProviderChannel : Datadog.Demos.WcfService.Client.Demos.WcfService.IStringProvider, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class StringProviderClient : System.ServiceModel.ClientBase<Datadog.Demos.WcfService.Client.Demos.WcfService.IStringProvider>, Datadog.Demos.WcfService.Client.Demos.WcfService.IStringProvider {
        
        public StringProviderClient() {
        }
        
        public StringProviderClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public StringProviderClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public StringProviderClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public StringProviderClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string GenerateRandomAsciiString(int length) {
            return base.Channel.GenerateRandomAsciiString(length);
        }
        
        public System.Threading.Tasks.Task<string> GenerateRandomAsciiStringAsync(int length) {
            return base.Channel.GenerateRandomAsciiStringAsync(length);
        }
        
        public int ComputeStableHash(string str) {
            return base.Channel.ComputeStableHash(str);
        }
        
        public System.Threading.Tasks.Task<int> ComputeStableHashAsync(string str) {
            return base.Channel.ComputeStableHashAsync(str);
        }
        
        public Datadog.Demos.WcfService.Client.Demos.WcfService.StringInfo GenerateRandomAsciiStringWithHash(int length) {
            return base.Channel.GenerateRandomAsciiStringWithHash(length);
        }
        
        public System.Threading.Tasks.Task<Datadog.Demos.WcfService.Client.Demos.WcfService.StringInfo> GenerateRandomAsciiStringWithHashAsync(int length) {
            return base.Channel.GenerateRandomAsciiStringWithHashAsync(length);
        }
    }
}

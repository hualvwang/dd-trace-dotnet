﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Datadog.DynamicDiagnosticSourceBindings.VendoredLibs.DiagnosticSource {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class System_Diagnostics_Resources_Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System_Diagnostics_Resources_Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Datadog.DynamicDiagnosticSourceBindings.VendoredLibs.DiagnosticSource.System.Diagnos" +
                            "tics.Resources.Strings", typeof(System_Diagnostics_Resources_Strings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;Value must be a valid ActivityIdFormat value&quot;.
        /// </summary>
        internal static string ActivityIdFormatInvalid {
            get {
                return ResourceManager.GetString("ActivityIdFormatInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Trying to set an Activity that is not running.
        /// </summary>
        internal static string ActivityNotRunning {
            get {
                return ResourceManager.GetString("ActivityNotRunning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;Can not stop an Activity that was not started&quot;.
        /// </summary>
        internal static string ActivityNotStarted {
            get {
                return ResourceManager.GetString("ActivityNotStarted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;Can not start an Activity that was already started&quot;.
        /// </summary>
        internal static string ActivityStartAlreadyStarted {
            get {
                return ResourceManager.GetString("ActivityStartAlreadyStarted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;EndTime is not UTC&quot;.
        /// </summary>
        internal static string EndTimeNotUtc {
            get {
                return ResourceManager.GetString("EndTimeNotUtc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;Invalid trace parent.&quot;.
        /// </summary>
        internal static string InvalidTraceParent {
            get {
                return ResourceManager.GetString("InvalidTraceParent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;The collection already contains item with same key &apos;{0}&apos;&apos;&quot;.
        /// </summary>
        internal static string KeyAlreadyExist {
            get {
                return ResourceManager.GetString("KeyAlreadyExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;OperationName must not be null or empty&quot;.
        /// </summary>
        internal static string OperationNameInvalid {
            get {
                return ResourceManager.GetString("OperationNameInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;ParentId is already set&quot;.
        /// </summary>
        internal static string ParentIdAlreadySet {
            get {
                return ResourceManager.GetString("ParentIdAlreadySet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;ParentId must not be null or empty&quot;.
        /// </summary>
        internal static string ParentIdInvalid {
            get {
                return ResourceManager.GetString("ParentIdInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;Can not change format for an activity that was already started&quot;.
        /// </summary>
        internal static string SetFormatOnStartedActivity {
            get {
                return ResourceManager.GetString("SetFormatOnStartedActivity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;Can not add link to activity after it has been started&quot;.
        /// </summary>
        internal static string SetLinkInvalid {
            get {
                return ResourceManager.GetString("SetLinkInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;Can not set ParentId on activity which has parent&quot;.
        /// </summary>
        internal static string SetParentIdOnActivityWithParent {
            get {
                return ResourceManager.GetString("SetParentIdOnActivityWithParent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;StartTime is not UTC&quot;.
        /// </summary>
        internal static string StartTimeNotUtc {
            get {
                return ResourceManager.GetString("StartTimeNotUtc", resourceCulture);
            }
        }
    }
}

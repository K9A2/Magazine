﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace test.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    var temp = new global::System.Resources.ResourceManager("test.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Provider=Microsoft.Jet.OLEDB.4.0;Data Source=dbo.mdb;Jet OLEDB:Database Password=admin;.
        /// </summary>
        internal static string string_connection_string {
            get {
                return ResourceManager.GetString("string_connection_string", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unabale to get the database connestion..
        /// </summary>
        internal static string string_no_database_connection {
            get {
                return ResourceManager.GetString("string_no_database_connection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Program terminated..
        /// </summary>
        internal static string string_program_terminated {
            get {
                return ResourceManager.GetString("string_program_terminated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please try again later..
        /// </summary>
        internal static string string_try_again_later {
            get {
                return ResourceManager.GetString("string_try_again_later", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to execute this command..
        /// </summary>
        internal static string string_unable_to_execute_command {
            get {
                return ResourceManager.GetString("string_unable_to_execute_command", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to open the target database connection.
        /// </summary>
        internal static string string_unable_to_open_connection {
            get {
                return ResourceManager.GetString("string_unable_to_open_connection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to read the target result..
        /// </summary>
        internal static string string_unable_to_read_result {
            get {
                return ResourceManager.GetString("string_unable_to_read_result", resourceCulture);
            }
        }
    }
}

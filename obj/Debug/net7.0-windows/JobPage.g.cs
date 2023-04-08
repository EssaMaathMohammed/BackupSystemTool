﻿#pragma checksum "..\..\..\JobPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "CE94E7C9818EB3D3C1523B5D6B101B832AAD8A2F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using BackupSystemTool;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace BackupSystemTool {
    
    
    /// <summary>
    /// JobPage
    /// </summary>
    public partial class JobPage : System.Windows.Window, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 22 "..\..\..\JobPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView jobItemsListView;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\..\JobPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button decryptionPageButton;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\JobPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid jobInformation_Grid;
        
        #line default
        #line hidden
        
        
        #line 79 "..\..\..\JobPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox jobName_TextBox;
        
        #line default
        #line hidden
        
        
        #line 84 "..\..\..\JobPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ConnectionName_TextBox;
        
        #line default
        #line hidden
        
        
        #line 91 "..\..\..\JobPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox relatedDatabases_TextBox;
        
        #line default
        #line hidden
        
        
        #line 95 "..\..\..\JobPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BrowseDatabasesButton;
        
        #line default
        #line hidden
        
        
        #line 104 "..\..\..\JobPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Location_TextBox;
        
        #line default
        #line hidden
        
        
        #line 107 "..\..\..\JobPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button backupLocationOptions;
        
        #line default
        #line hidden
        
        
        #line 124 "..\..\..\JobPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ScheduleBackup_TextBox;
        
        #line default
        #line hidden
        
        
        #line 128 "..\..\..\JobPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button backupNowButton;
        
        #line default
        #line hidden
        
        
        #line 134 "..\..\..\JobPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button scheduleBackupButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.4.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/BackupSystemTool;component/jobpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\JobPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.4.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 8 "..\..\..\JobPage.xaml"
            ((BackupSystemTool.JobPage)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            
            #line 9 "..\..\..\JobPage.xaml"
            ((BackupSystemTool.JobPage)(target)).Closed += new System.EventHandler(this.Window_Closed);
            
            #line default
            #line hidden
            return;
            case 2:
            this.jobItemsListView = ((System.Windows.Controls.ListView)(target));
            
            #line 25 "..\..\..\JobPage.xaml"
            this.jobItemsListView.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.jobItemsListView_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 60 "..\..\..\JobPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.AddJobButton_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.decryptionPageButton = ((System.Windows.Controls.Button)(target));
            
            #line 64 "..\..\..\JobPage.xaml"
            this.decryptionPageButton.Click += new System.Windows.RoutedEventHandler(this.decryptionPageButton_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.jobInformation_Grid = ((System.Windows.Controls.Grid)(target));
            return;
            case 8:
            this.jobName_TextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            this.ConnectionName_TextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 10:
            this.relatedDatabases_TextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 11:
            this.BrowseDatabasesButton = ((System.Windows.Controls.Button)(target));
            
            #line 97 "..\..\..\JobPage.xaml"
            this.BrowseDatabasesButton.Click += new System.Windows.RoutedEventHandler(this.BrowseDatabasesButton_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            this.Location_TextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 13:
            this.backupLocationOptions = ((System.Windows.Controls.Button)(target));
            
            #line 109 "..\..\..\JobPage.xaml"
            this.backupLocationOptions.Click += new System.Windows.RoutedEventHandler(this.backupLocationOptions_Click);
            
            #line default
            #line hidden
            return;
            case 14:
            
            #line 114 "..\..\..\JobPage.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.localLocationMenuOption_Click);
            
            #line default
            #line hidden
            return;
            case 15:
            
            #line 116 "..\..\..\JobPage.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.cloudLocationMenuOption_Click);
            
            #line default
            #line hidden
            return;
            case 16:
            this.ScheduleBackup_TextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 17:
            this.backupNowButton = ((System.Windows.Controls.Button)(target));
            
            #line 130 "..\..\..\JobPage.xaml"
            this.backupNowButton.Click += new System.Windows.RoutedEventHandler(this.backupNowButton_Click);
            
            #line default
            #line hidden
            return;
            case 18:
            this.scheduleBackupButton = ((System.Windows.Controls.Button)(target));
            
            #line 136 "..\..\..\JobPage.xaml"
            this.scheduleBackupButton.Click += new System.Windows.RoutedEventHandler(this.scheduleBackupButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.4.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 3:
            
            #line 46 "..\..\..\JobPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.optionsButton_Click);
            
            #line default
            #line hidden
            break;
            case 4:
            
            #line 49 "..\..\..\JobPage.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.deleteJobMenuItem_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}


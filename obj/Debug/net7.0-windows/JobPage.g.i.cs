﻿#pragma checksum "..\..\..\JobPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "B9D18AAE87790CAA3AF722F687F260D9658DA834"
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
        
        
        #line 20 "..\..\..\JobPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView jobItemsListView;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\JobPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid jobInformation_Grid;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\..\JobPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox jobName_TextBox;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\..\JobPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ConnectionName_TextBox;
        
        #line default
        #line hidden
        
        
        #line 87 "..\..\..\JobPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox relatedDatabases_TextBox;
        
        #line default
        #line hidden
        
        
        #line 91 "..\..\..\JobPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BrowseDatabasesButton;
        
        #line default
        #line hidden
        
        
        #line 100 "..\..\..\JobPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Location_TextBox;
        
        #line default
        #line hidden
        
        
        #line 103 "..\..\..\JobPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button backupLocationOptions;
        
        #line default
        #line hidden
        
        
        #line 120 "..\..\..\JobPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ScheduleBackup_TextBox;
        
        #line default
        #line hidden
        
        
        #line 124 "..\..\..\JobPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button backupNowButton;
        
        #line default
        #line hidden
        
        
        #line 130 "..\..\..\JobPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button scheduleBackupButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.2.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/BackupSystemTool;V1.0.0.0;component/jobpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\JobPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.2.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.jobItemsListView = ((System.Windows.Controls.ListView)(target));
            
            #line 23 "..\..\..\JobPage.xaml"
            this.jobItemsListView.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.jobItemsListView_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 58 "..\..\..\JobPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.AddJobButton_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.jobInformation_Grid = ((System.Windows.Controls.Grid)(target));
            return;
            case 6:
            this.jobName_TextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.ConnectionName_TextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.relatedDatabases_TextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            this.BrowseDatabasesButton = ((System.Windows.Controls.Button)(target));
            
            #line 93 "..\..\..\JobPage.xaml"
            this.BrowseDatabasesButton.Click += new System.Windows.RoutedEventHandler(this.BrowseDatabasesButton_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.Location_TextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 11:
            this.backupLocationOptions = ((System.Windows.Controls.Button)(target));
            
            #line 105 "..\..\..\JobPage.xaml"
            this.backupLocationOptions.Click += new System.Windows.RoutedEventHandler(this.backupLocationOptions_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 110 "..\..\..\JobPage.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.localLocationMenuOption_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 112 "..\..\..\JobPage.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.cloudLocationMenuOption_Click);
            
            #line default
            #line hidden
            return;
            case 14:
            this.ScheduleBackup_TextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 15:
            this.backupNowButton = ((System.Windows.Controls.Button)(target));
            
            #line 126 "..\..\..\JobPage.xaml"
            this.backupNowButton.Click += new System.Windows.RoutedEventHandler(this.backupNowButton_Click);
            
            #line default
            #line hidden
            return;
            case 16:
            this.scheduleBackupButton = ((System.Windows.Controls.Button)(target));
            
            #line 132 "..\..\..\JobPage.xaml"
            this.scheduleBackupButton.Click += new System.Windows.RoutedEventHandler(this.scheduleBackupButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.2.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 2:
            
            #line 44 "..\..\..\JobPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.optionsButton_Click);
            
            #line default
            #line hidden
            break;
            case 3:
            
            #line 47 "..\..\..\JobPage.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.deleteJobMenuItem_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}


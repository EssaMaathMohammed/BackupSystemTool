﻿#pragma checksum "..\..\..\BackupDialog.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9BCFD96FF5DFB3D85B94FAA6CCCF4E2CF8DD3D08"
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
using Gu.Wpf.Adorners;
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
    /// BackupDialog
    /// </summary>
    public partial class BackupDialog : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 21 "..\..\..\BackupDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel backupStackPanel;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\BackupDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel backupOnceStackPanel;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\BackupDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton scheduleBackupRadioButton;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\BackupDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton backupOnceRadioButton;
        
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
            System.Uri resourceLocater = new System.Uri("/BackupSystemTool;component/backupdialog.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\BackupDialog.xaml"
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
            this.backupStackPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 2:
            this.backupOnceStackPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 3:
            this.scheduleBackupRadioButton = ((System.Windows.Controls.RadioButton)(target));
            
            #line 45 "..\..\..\BackupDialog.xaml"
            this.scheduleBackupRadioButton.Checked += new System.Windows.RoutedEventHandler(this.scheduleBackupRadioButton_Checked);
            
            #line default
            #line hidden
            return;
            case 4:
            this.backupOnceRadioButton = ((System.Windows.Controls.RadioButton)(target));
            
            #line 50 "..\..\..\BackupDialog.xaml"
            this.backupOnceRadioButton.Checked += new System.Windows.RoutedEventHandler(this.backupOnceRadioButton_Checked);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}


﻿#pragma checksum "..\..\..\AddConnectionDialog.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "B387679F1CDF4FDA2242CFB0EE16DEA73EFB1AA9"
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
    /// AddConnectionDialog
    /// </summary>
    public partial class AddConnectionDialog : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 21 "..\..\..\AddConnectionDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel localDatabaseStackPanel;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\AddConnectionDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel lanDatabaseStackPanel;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\AddConnectionDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton useConnectionStringRadioButton;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\..\AddConnectionDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton useParametersStringRadioButton;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\..\AddConnectionDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox lanConnectionStringTextBox;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\..\AddConnectionDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox lanConnectionUserTextBox;
        
        #line default
        #line hidden
        
        
        #line 84 "..\..\..\AddConnectionDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox lanConnectionPasswordBox;
        
        #line default
        #line hidden
        
        
        #line 102 "..\..\..\AddConnectionDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel cloudDatabaseStackPanel;
        
        #line default
        #line hidden
        
        
        #line 132 "..\..\..\AddConnectionDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton localDatabaseRadioButton;
        
        #line default
        #line hidden
        
        
        #line 138 "..\..\..\AddConnectionDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton lanDatabaseRadioButton;
        
        #line default
        #line hidden
        
        
        #line 143 "..\..\..\AddConnectionDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton cloudDatabaseRadioButton;
        
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
            System.Uri resourceLocater = new System.Uri("/BackupSystemTool;component/addconnectiondialog.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\AddConnectionDialog.xaml"
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
            this.localDatabaseStackPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 2:
            this.lanDatabaseStackPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 3:
            this.useConnectionStringRadioButton = ((System.Windows.Controls.RadioButton)(target));
            
            #line 63 "..\..\..\AddConnectionDialog.xaml"
            this.useConnectionStringRadioButton.Checked += new System.Windows.RoutedEventHandler(this.useConnectionStringRadioButton_Checked);
            
            #line default
            #line hidden
            return;
            case 4:
            this.useParametersStringRadioButton = ((System.Windows.Controls.RadioButton)(target));
            
            #line 72 "..\..\..\AddConnectionDialog.xaml"
            this.useParametersStringRadioButton.Checked += new System.Windows.RoutedEventHandler(this.useParametersStringRadioButton_Checked);
            
            #line default
            #line hidden
            return;
            case 5:
            this.lanConnectionStringTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.lanConnectionUserTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.lanConnectionPasswordBox = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 8:
            this.cloudDatabaseStackPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 9:
            this.localDatabaseRadioButton = ((System.Windows.Controls.RadioButton)(target));
            
            #line 134 "..\..\..\AddConnectionDialog.xaml"
            this.localDatabaseRadioButton.Checked += new System.Windows.RoutedEventHandler(this.LocalDatabaseRadioButton_Checked);
            
            #line default
            #line hidden
            return;
            case 10:
            this.lanDatabaseRadioButton = ((System.Windows.Controls.RadioButton)(target));
            
            #line 142 "..\..\..\AddConnectionDialog.xaml"
            this.lanDatabaseRadioButton.Checked += new System.Windows.RoutedEventHandler(this.LanDatabaseRadioButton_Checked);
            
            #line default
            #line hidden
            return;
            case 11:
            this.cloudDatabaseRadioButton = ((System.Windows.Controls.RadioButton)(target));
            
            #line 147 "..\..\..\AddConnectionDialog.xaml"
            this.cloudDatabaseRadioButton.Checked += new System.Windows.RoutedEventHandler(this.CloudDatabaseRadioButton_Checked);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}


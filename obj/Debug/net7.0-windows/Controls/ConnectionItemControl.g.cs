﻿#pragma checksum "..\..\..\..\Controls\ConnectionItemControl.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9B769831FAA5708D20E19B5D195D5E50441D5F4C"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using BackupSystemTool.Controls;
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


namespace BackupSystemTool.Controls {
    
    
    /// <summary>
    /// ConnectionItemControl
    /// </summary>
    public partial class ConnectionItemControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 18 "..\..\..\..\Controls\ConnectionItemControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ConnectionNameTextBlock;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\..\Controls\ConnectionItemControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock StatusTextBlock;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\..\Controls\ConnectionItemControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button expandButton;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\..\Controls\ConnectionItemControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel moreInfoStackPanel;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\..\Controls\ConnectionItemControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock connectionLocation_TextBlock;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\..\Controls\ConnectionItemControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock connectionString_TextBlock;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\..\Controls\ConnectionItemControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock connectionPort_TextBlock;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\..\Controls\ConnectionItemControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock connectionUsername_TextBlock;
        
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
            System.Uri resourceLocater = new System.Uri("/BackupSystemTool;component/controls/connectionitemcontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Controls\ConnectionItemControl.xaml"
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
            this.ConnectionNameTextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 2:
            this.StatusTextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.expandButton = ((System.Windows.Controls.Button)(target));
            
            #line 23 "..\..\..\..\Controls\ConnectionItemControl.xaml"
            this.expandButton.Click += new System.Windows.RoutedEventHandler(this.expandButton_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.moreInfoStackPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 5:
            this.connectionLocation_TextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.connectionString_TextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.connectionPort_TextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            this.connectionUsername_TextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 9:
            
            #line 35 "..\..\..\..\Controls\ConnectionItemControl.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.editButton_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 36 "..\..\..\..\Controls\ConnectionItemControl.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.deleteButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}


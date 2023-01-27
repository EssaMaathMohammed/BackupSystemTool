using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BackupSystemTool.Controls
{
    /// <summary>
    /// Interaction logic for ConnectionItemControl.xaml
    /// </summary>
    public partial class ConnectionItemControl : UserControl
    {
        bool expansionStatus = false;
        /// <summary>
        /// The Dependency Propoerty is the ConnectionItem bind from the xaml side
        /// the Property Metadata(default, custom) works as an event listener, whenever a change 
        /// happens (object bind) a method gets called
        /// </summary> 
        public ConnectionItem ConnectionItem
        {
            get { return (ConnectionItem)GetValue(ConnectionItemProperty); }
            set { SetValue(ConnectionItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConnectionItem.
        // This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConnectionItemProperty =
            DependencyProperty.Register("ConnectionItem", typeof(ConnectionItem),
                typeof(ConnectionItemControl), new PropertyMetadata(new ConnectionItem { Name = "Defualt Item",
                    ConnectionString = "Default Connection String" }, setConnectionDetails));

        // The new item (connection Item) gets bound, we call a custom method to assign
        // the values of the new item to the xaml file text blocks
        private static void setConnectionDetails(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ConnectionItemControl itemControls)
            {
                ConnectionItem newConnectionItem = (ConnectionItem)e.NewValue;
                itemControls.ConnectionNameTextBlock.Text = newConnectionItem.Name;
                itemControls.StatusTextBlock.Text = "Status: UP";
                Debug.WriteLine(e.NewValue.ToString());

            }
        }

        public ConnectionItemControl()
        {
            InitializeComponent();
            this.Height = 30;
        }

        private void expandButton_Click(object sender, RoutedEventArgs e)
        {
            if (!expansionStatus)
            {
                moreInfoStackPanel.Visibility = Visibility.Visible;
                this.Height = 140;
                expansionStatus = true;
            }
            else {
                moreInfoStackPanel.Visibility = Visibility.Hidden;
                this.Height = 30;
                expansionStatus = false;
            }
        }
    }
}

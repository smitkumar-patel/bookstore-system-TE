using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace BookStoreSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginDialog : Window
    {
        public LoginDialog()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            //This will be used to check if value is true or false in Windows.xaml.cs file
            //When user clicks ok, it will will Dialogresult will be true which is then checked in to see if use pressed ok
            this.DialogResult = true;
        }
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            //This will be used to check if value is true or false in Windows.xaml.cs file
            //When user clicks cancel, it will will Dialogresult will be false which is then checked in to see if use pressed cancel
            this.DialogResult = false;
        }
    }
}

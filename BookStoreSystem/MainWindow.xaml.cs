using API;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
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
    public partial class MainWindow : Window
    {
        DataSet dsBookCat;
        BookOrder bookOrder;
        User userObj = new User();
        Book b;
        public MainWindow()
        {
            InitializeComponent();
           
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var thread = new Thread(() =>{ MessageBox.Show("Please wait while we load the data from server");});
            thread.Start();
            b = new Book();
            b.getAllData();
            dsBookCat = b.getDataset();
            this.DataContext = dsBookCat.Tables["Category"];
            categoriesComboBox.SelectedIndex = 0;
            bookOrder = new BookOrder();
            this.orderListView.ItemsSource = bookOrder.OrderItemList;
        }


        private async void loginButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new LoginDialog();
            dlg.Owner = this;
            dlg.ShowDialog();
            if (dlg.DialogResult == true)
            {
                string username = dlg.nameTextBox.Text;
                string password = dlg.passwordTextBox.Password;
                if (!Validations.ValidateUserName(username))
                {
                    MessageBox.Show("Valid username is not blank, starts with letter and may contain letter and number");
                }
                else if (!Validations.ValidatePassword(password))
                {
                    MessageBox.Show("Valid password is not blank,starts with letter and may contain letter,number and @!#,.");
                }
                else
                {
                   await userObj.Login(username, password);
                    if (userObj.didUserUpdate)
                    {
                        MessageBox.Show("User logged in");
                        this.statusTextBlock.Text = "You are logged in as User #" + userObj.UserName;
                    }
                    else
                    {
                        MessageBox.Show("Wrong username or password");
                    }
                }
            }
               
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            OrderItemDialog orderItemDialog = new OrderItemDialog();
            DataRowView selectedRow;
            try
            { //If user did not select any book to add
                if ((DataRowView)this.ProductsDataGrid.SelectedItem == null)
                {
                    MessageBox.Show("Please select a book to add");
                }
                //If user has selected a book
                else
                {
                    selectedRow = (DataRowView)this.ProductsDataGrid.SelectedItems[0];
                    orderItemDialog.isbnTextBox.Text = selectedRow.Row.ItemArray[0].ToString();
                    orderItemDialog.titleTextBox.Text = selectedRow.Row.ItemArray[2].ToString();
                    orderItemDialog.priceTextBox.Text = selectedRow.Row.ItemArray[4].ToString();
                    orderItemDialog.Owner = this;
                    orderItemDialog.ShowDialog();
                    if (orderItemDialog.DialogResult == true)
                    {
                        string isbn = orderItemDialog.isbnTextBox.Text;
                        string title = orderItemDialog.titleTextBox.Text;
                        double unitPrice = double.Parse(orderItemDialog.priceTextBox.Text);
                        int quantity = int.Parse(orderItemDialog.quantityTextBox.Text);
                        if (!Validations.ValidateNumberofBooks(quantity)) MessageBox.Show("Wrong quantity, You are not allowed to purchase more 5 books at a time.");
                        else bookOrder.AddItem(new OrderItem(isbn, title, unitPrice, quantity));
                    }
                }
            }
            catch (Exception bookException)
            {
                MessageBox.Show("Please select another book");
                Debug.WriteLine(bookException.ToString());
            }
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            if (bookOrder.OrderItemList.Count == 0)
            {
                MessageBox.Show("There is no book to remove.");
            }
            else if (this.orderListView.SelectedItem == null)
            {
                MessageBox.Show("Please select a book to remove");
            }
            else
            {
                var selectedOrderItem = this.orderListView.SelectedItem as OrderItem;
                bookOrder.RemoveItem(selectedOrderItem.BookID);
            }
        }

        private async void chechoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (userObj.UserID == 0)
            {
                MessageBox.Show("Please log in first to checkout");
            }
            //If user has not added any books to cart, then he is asked to add book first
            //Empty order can not be placed
            else if (bookOrder.OrderItemList.Count == 0)
            {
                MessageBox.Show("Order is empty. Please add book first. Order is not placed.");
            }
            else
            {
                await bookOrder.PlaceOrder(userObj.UserID);
                if (bookOrder.OrderID == 0)
                {
                    MessageBox.Show("There is some error. Please try again");
                }
                else
                {
                    MessageBox.Show("Order is placed. Order id is : " + bookOrder.OrderID);
                }
            }
        }
    }
}

using System;
using System.Windows;
using ComicX.Auth;
using ComicX.Menu;

namespace ComicX.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AuthManager.InitializeUsersFile();
            MenuManager.InitializeStockFile();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;
            int loginAttempts = 3;

            if (AuthManager.IsLockedOut(out TimeSpan remainingTime))
            {
                MessageBox.Show($"System is locked! Please try again in {Math.Ceiling(remainingTime.TotalMinutes)} minutes.", "Locked", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (AuthManager.ValidateCredentials(username, password))
            {
                pnlLogin.Visibility = Visibility.Collapsed;
                pnlMenu.Visibility = Visibility.Visible;
                txtPassword.Clear();
            }
            else
            {
                loginAttempts--;
                if (loginAttempts > 0)
                {
                    MessageBox.Show($"Wrong username and/or password. Remaining attempts: {loginAttempts}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    AuthManager.SetLockout();
                    MessageBox.Show("You are not authorized to access this service", "Unauthorized", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            pnlMenu.Visibility = Visibility.Collapsed;
            pnlLogin.Visibility = Visibility.Visible;
            txtUsername.Clear();
            txtPassword.Clear();
            txtOutput.Clear();
            txtThemeInput.Clear();
            txtQuantityInput.Text = "1";
        }

        private void BtnStock_Click(object sender, RoutedEventArgs e)
        {
            var stock = MenuManager.GetStock();
            string output = "--- Stock Count for Each Theme ---\n";
            foreach (var item in stock)
            {
                output += $"Theme: {item.Key} - Quantity: {item.Value} pcs\n";
            }
            txtOutput.Text = output;
        }

        private void BtnValues_Click(object sender, RoutedEventArgs e)
        {
            var values = MenuManager.GetTotalValues();
            string output = "--- Total Value of Each Theme Type ---\n";
            foreach (var item in values)
            {
                output += $"Theme: {item.Key} - Total Value: {item.Value} USD\n";
            }
            txtOutput.Text = output;
        }

        private void BtnSell_Click(object sender, RoutedEventArgs e)
        {
            string theme = txtThemeInput.Text?.Trim();

            if (int.TryParse(txtQuantityInput.Text, out int quantity) && quantity > 0)
            {
                if (!string.IsNullOrEmpty(theme))
                {
                    if (MenuManager.RegisterSale(theme, quantity))
                    {
                        txtOutput.Text = $"Sale registered successfully! Sold {quantity} pcs of {theme}.";
                        txtThemeInput.Clear();
                        txtQuantityInput.Text = "1";
                    }
                    else
                    {
                        txtOutput.Text = "Error: Invalid theme name or not enough stock!";
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a theme name!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid positive quantity!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnStatus_Click(object sender, RoutedEventArgs e)
        {
            string status = MenuManager.GetStockStatus();
            txtOutput.Text = $"--- Stock Status ---\nCurrent overall stock status: {status}";
        }
    }
}

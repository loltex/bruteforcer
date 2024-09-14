using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Security.Cryptography;
using System;

namespace Bruteforce
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string resultCrackedPassword;
        private TimeSpan resultTimeSpent;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordBox1.Password.Length == 0)
            {
                MessageBox.Show("Password must not be empty!");
                return;
            }
            Crack(PasswordBox1, ref resultCrackedPassword, ref resultTimeSpent);
            TextBox1.Text = resultCrackedPassword;
            TextBox2.Text = resultTimeSpent.ToString();
        }

        static void Crack(PasswordBox _passwordBox, ref string resultCrackedPassword, ref TimeSpan resultTimeSpent)
        {
            Hasher passwordHasher = new Hasher();
            Cracker passwordCracker = new Cracker(passwordHasher);
            // Max expected password length or fail.
            int maxPasswordLength = _passwordBox.Password.Length;

            string originalPassword = _passwordBox.Password;
            string hashedPassword = passwordHasher.HashPassword(originalPassword);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            string crackedPassword = passwordCracker.CrackPassword(hashedPassword, maxPasswordLength);

            stopwatch.Stop();
            TimeSpan timeTaken = stopwatch.Elapsed;

            resultCrackedPassword = crackedPassword.Trim();
            resultTimeSpent = timeTaken;
        }        
    }
}
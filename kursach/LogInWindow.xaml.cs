using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace kursach
{
    /// <summary>
    /// Логика взаимодействия для LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        public LogInWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var passWord = PassWordInput.Password;
            var logIn = LogInInput.Text;
            using (Гостиночный_комплекмEntities db = new Гостиночный_комплекмEntities())
            {
                var logins = (from Users in db.User
                              where passWord == Users.Password && logIn == Users.Login
                              select new { logIn }).Distinct();
                if (logins.Count() == 0)
                    MessageBox.Show("Uncorrect login or password");
                else
                {
                    if (logins.Single().logIn == "admin")
                    {
                        OrdersWindow w2 = new OrdersWindow();
                        w2.Show();
                        this.Close();
                    }
                    else
                    {
                        UserForm uf = new UserForm();
                        uf.Show();
                        this.Close();
                    }
                }
            }
        }
    }
}

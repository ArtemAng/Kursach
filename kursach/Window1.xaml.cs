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
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var login = RegLoginInput.Text;
            var passWord = RegPassWordInput.Password;

            using (Гостиночный_комплекмEntities db = new Гостиночный_комплекмEntities())
            {
                int id = (from Users in db.User
                          select new { Users.Id }).Max(a => a.Id) + 1;
                var user = new User() { Id = id, Login = login, Password = passWord };
                var users = from Users in db.User
                            where Users.Login == login
                            select new { Users.Id };
                if (users.Count() > 0)
                {
                    MessageBox.Show("This user is registered");
                }
                else
                {
                    db.User.Add(user);
                    db.SaveChanges();
                }
            }
        }
    }
}

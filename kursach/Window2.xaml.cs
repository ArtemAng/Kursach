using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
                var Client = new
                {
                    fName = fName.Text,
                    sName = fName.Text,
                    oName = fName.Text,
                    Phone = Convert.ToInt32(Phone.Text),
                    Passport = Passport.Text,
                    BirthDate = BirthDate.SelectedDate,
                    Gender = GenderPicker11.Text,
                    idNumber = Convert.ToInt32(idNumber.Text)
                };
            using (Гостиночный_комплекмEntities db = new Гостиночный_комплекмEntities())
            {
               
                try
                {
                    db.Клиент.Add(new Клиент() { Код_клиента = db.Клиент.Max(a => a.Код_клиента) + 1, Имя = Client.fName, Фамилия = Client.sName, Отчество = Client.oName, Паспортные_данные = Client.Passport, Дата_рождения = Client.BirthDate, Телефон = Client.Phone, Пол = Client.Gender, Код_состояния_номера= Client.idNumber });
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            MessageBox.Show("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }
                }
                db.Бронь.Add(new Бронь()
                {
                    Фамилия = Client.sName,
                    Код_номера = Client.idNumber,
                    Код_бронь = db.Бронь.Max(a => a.Код_бронь) + 1,
                    Код_состояния = Client.idNumber
                });
                db.SaveChanges();
                MessageBox.Show("Boocking is done");
            }
        }
}
}

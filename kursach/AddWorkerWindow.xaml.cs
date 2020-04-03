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
    /// Логика взаимодействия для AddWorkerWindow.xaml
    /// </summary>
    public partial class AddWorkerWindow : Window
    {
        public AddWorkerWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var worker = new
            {
                fName = fName.Text,
                sName = sNameAdd.Text,
                oName = oName.Text,
                obrazovanie = Obrazovanie.IsChecked,
                idDolzhnost = Convert.ToInt32(Dolzhnost.Text)
            };
            using (Гостиночный_комплекмEntities db = new Гостиночный_комплекмEntities())
            {

                db.Сотрудники.Add(new Сотрудники() { Код_сотрудника = db.Сотрудники.Max(a => a.Код_сотрудника) + 1, Имя = worker.fName, Фамилия = worker.sName, Отчество = worker.oName, Код_должности = worker.idDolzhnost, Образование = worker.obrazovanie });
                db.SaveChanges();
            }
            MessageBox.Show("Worker added");
        }
    }
}

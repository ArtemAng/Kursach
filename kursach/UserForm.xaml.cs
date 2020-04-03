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
    /// Логика взаимодействия для UserForm.xaml
    /// </summary>
    public partial class UserForm : Window
    {
        public UserForm()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DBItems.Items.Clear();
            using (Гостиночный_комплекмEntities db = new Гостиночный_комплекмEntities())
            {
                var Numbers1 =
                    from Numbers in db.Номера
                    join NumberType in db.Тип_номера on Numbers.Код_тип_номера equals NumberType.Код_тип_номера
                    join DopUdobstva in db.Доп__Удобства on Numbers.Код_доп__удобства equals DopUdobstva.Код_доп__удобства
                    select new
                    {
                        NumberType.Стоимость,
                        NumberType.Наименование,
                        NumberType.Количество_комнат,
                        DopUdobstvaName = DopUdobstva.Наименование,
                        DopUdobstva.Цена_доп_удобства
                    };
                foreach (var n in Numbers1)
                    DBItems.Items.Add($" Number type: {n.Наименование}; Number type price: {n.Стоимость}; quantity rooms: {n.Количество_комнат};" +
                        $" Dop udobstva: {n.DopUdobstvaName}; Dop udobstva price: {n.Цена_доп_удобства}; ");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow m = new MainWindow();
            m.Show();
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Window2 w2 = new Window2();
            w2.Show();
        }
    }
}

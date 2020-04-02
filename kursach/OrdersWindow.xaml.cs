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
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class OrdersWindow : Window
    {
        public OrdersWindow()
        {
            InitializeComponent();
            using (Гостиночный_комплекмEntities db = new Гостиночный_комплекмEntities())
            {
                var Numbers1 =
                    from Clients in db.Клиент
                    join StatusNumber in db.Состояние_номера on Clients.Код_состояния_номера equals StatusNumber.Код_состояние_номера
                    join Broni in db.Бронь on StatusNumber.Код_брони equals Broni.Код_бронь
                    join Numbers in db.Номера on StatusNumber.Код_номера equals Numbers.Код_номера
                    join NumberType in db.Тип_номера on Numbers.Код_тип_номера equals NumberType.Код_тип_номера
                    join DopUdobstva in db.Доп__Удобства on Numbers.Код_доп__удобства equals DopUdobstva.Код_доп__удобства
                    join Workers in db.Сотрудники on StatusNumber.Код_сотрудника equals Workers.Код_сотрудника
                    join Doljnost in db.Должность on Workers.Код_должности equals Doljnost.Код_должность
                    select new
                    {
                        Clients.Имя,
                        Clients.Фамилия,
                        Clients.Отчество,
                        Clients.Дата_рождения,
                        Clients.Пол,
                        Clients.Телефон,
                        StatusNumber.Дата_заезд,
                        StatusNumber.Дата_выезд,
                        StatusNumber.Сумма,
                        SecondNameInBronj = Broni.Фамилия,
                        NumberType.Стоимость,
                        NumberType.Наименование,
                        NumberType.Количество_комнат,
                        DopUdobstvaName = DopUdobstva.Наименование,
                        DopUdobstva.Цена_доп_удобства,
                        WorkerName = Workers.Имя + " " + Workers.Фамилия + " " + Workers.Отчество,
                        Workers.Образование,
                        Doljnost.Должность1

                    };
                foreach (var n in Numbers1)
                    DBItems.Items.Add($"Client name: {n.Имя + " " + n.Фамилия + " " + n.Отчество}; " +
                        $"birthday: {n.Дата_рождения}; gender: {n.Пол} phone number: {n.Телефон};" +
                        $" date in: {n.Дата_заезд}; date out: {n.Дата_выезд} price: {n.Сумма}; second name in bronj: {n.SecondNameInBronj}" +
                        $" Number type: {n.Наименование}; Number type price: {n.Стоимость}; quantity rooms: {n.Количество_комнат};" +
                        $" Dop udobstva: {n.DopUdobstvaName}; Dop udobstva price: {n.Цена_доп_удобства}; " +
                        $" Worker name: {n.WorkerName}; is have obrozovanie: {n.Образование}; Doljnost: {n.Должность1}");
            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)// about numbers
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

        private void Button_Click_1(object sender, RoutedEventArgs e)// about clients
        {
            DBItems.Items.Clear();
            using (Гостиночный_комплекмEntities db = new Гостиночный_комплекмEntities())
            {
                var Numbers1 =
                    from Clients in db.Клиент
                    join StatusNumber in db.Состояние_номера on Clients.Код_состояния_номера equals StatusNumber.Код_состояние_номера
                    join Broni in db.Бронь on StatusNumber.Код_брони equals Broni.Код_бронь

                    select new
                    {
                        Clients.Имя,
                        Clients.Фамилия,
                        Clients.Отчество,
                        Clients.Дата_рождения,
                        Clients.Пол,
                        Clients.Телефон,
                        StatusNumber.Дата_заезд,
                        StatusNumber.Дата_выезд,
                        StatusNumber.Сумма,
                        SecondNameInBronj = Broni.Фамилия
                    };
                foreach (var n in Numbers1)
                    DBItems.Items.Add($"Client name: {n.Имя + " " + n.Фамилия + " " + n.Отчество}; " +
                        $"birthday: {n.Дата_рождения}; gender: {n.Пол} phone number: {n.Телефон};" +
                        $" date in: {n.Дата_заезд}; date out: {n.Дата_выезд} price: {n.Сумма}; second name in bronj: {n.SecondNameInBronj}");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)// about workers
        {
            DBItems.Items.Clear();
            using (Гостиночный_комплекмEntities db = new Гостиночный_комплекмEntities())
            {
                var Numbers1 =
                    from Workers in db.Сотрудники
                    join Doljnost in db.Должность on Workers.Код_должности equals Doljnost.Код_должность
                    select new
                    {
                        WorkerName = Workers.Имя + " " + Workers.Фамилия + " " + Workers.Отчество,
                        Workers.Образование,
                        Doljnost.Должность1

                    };
                foreach (var n in Numbers1)
                    DBItems.Items.Add($" Worker name: {n.WorkerName}; is have obrozovanie: {n.Образование}; Doljnost: {n.Должность1}");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            OrdersWithParams owp = new OrdersWithParams();
            owp.Show();
        }
    }
}

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
    /// Логика взаимодействия для OrdersWithParams.xaml
    /// </summary>
    public partial class OrdersWithParams : Window
    {

        public OrdersWithParams()
        {

            InitializeComponent();
        }

        private void Button_Click_13(object sender, RoutedEventArgs e)//order 1
        {
            var ResVol = Convert.ToInt32(ReservedVolume.Text); // Берем значение объема продаж
            DBItems1.Items.Clear();//очистка поля вывода, на случай если там есть данные
            using (Гостиночный_комплекмEntities db = new Гостиночный_комплекмEntities())
            {
                var Numbers1 =//групируем все брони по фамилии
                    from Bronj in db.Бронь
                    group Bronj by Bronj.Фамилия into g//g - это группа, если у нас 3 элемента с именем AAA то будет группа AAA
                    select new { Name = g.Key, Count = g.Count() };//на выходе будут объекты содержащие поля имени и колличества элементов в группе
                var MaxEl = Numbers1.Where(a => a.Count >= ResVol);//Ищем группу с м

                foreach (var n in MaxEl)//Вывод всех имен на экран
                    DBItems1.Items.Add($"Client name: {n.Name};");
                DBItems1.Items.Add($"Quantity clients: {MaxEl.Count()}");//колличество всех выведенных элементов
            }
        }
        private void Button_Click_11(object sender, RoutedEventArgs e)//order 2
        {
            var ClientCharacters = new
            {
                fName = ClientFName2.Text,
                sName = ClientSName2.Text,
                Birthday = BirthdayPicker.SelectedDate,
                Gender = GenderPicker2.Text
            };//Данные о клиенте полученные с полей для ввода
            DateTime fDate = FDate2.SelectedDate.Value;//даты полученые с полей для ввода
            DateTime sDate = SDate2.SelectedDate.Value;
            DBItems2.Items.Clear();
            using (Гостиночный_комплекмEntities db = new Гостиночный_комплекмEntities())
            {
                var Numbers1 =
                    from Clients in db.Клиент
                    join StatusNumber in db.Состояние_номера on Clients.Код_состояния_номера equals StatusNumber.Код_состояние_номера
                    where StatusNumber.Дата_заезд >= fDate &&
                            StatusNumber.Дата_заезд <= sDate &&
                            ClientCharacters.fName == Clients.Имя &&
                            ClientCharacters.sName == Clients.Отчество &&
                            ClientCharacters.Gender == Clients.Пол &&
                            ClientCharacters.Birthday == Clients.Дата_рождения
                    select new
                    {
                        Clients.Имя,
                        Clients.Фамилия,
                        Clients.Отчество,
                        Clients.Пол,
                        Clients.Дата_рождения,
                        Clients.Паспортные_данные,
                        Clients.Телефон,
                        StatusNumber.Сумма,
                        StatusNumber.Дата_выезд,
                        StatusNumber.Дата_заезд
                    };//получаем всех клиентов где поля равны данным которые мы получили

                foreach (var n in Numbers1)
                    DBItems2.Items.Add($"Client name: {n.Имя + " " + n.Фамилия + " " + n.Отчество}; " +
                        $"birthday: {n.Дата_рождения}; gender: {n.Пол} phone number: {n.Телефон};" +
                        $" date in: {n.Дата_заезд}; date out: {n.Дата_выезд} price: {n.Сумма};");//вывод всей информации на экран
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DBItems.Items.Clear();
            using (Гостиночный_комплекмEntities db = new Гостиночный_комплекмEntities())
            {
                var Numbers1 =
                    from StatusNumber in db.Состояние_номера
                    join Numbers in db.Номера on StatusNumber.Код_номера equals Numbers.Код_номера
                    join NumberType in db.Тип_номера on Numbers.Код_тип_номера equals NumberType.Код_тип_номера
                    join DopUdobstva in db.Доп__Удобства on Numbers.Код_доп__удобства equals DopUdobstva.Код_доп__удобства
                    where StatusNumber.Дата_выезд < DateTime.Now//Получаем все номера и всю инфу о них, где дата выезда меньше текущей, то есть номер будет свободен
                    select new
                    {
                        NumberType.Стоимость,
                        NumberType.Наименование,
                        NumberType.Количество_комнат,
                        DopUdobstvaName = DopUdobstva.Наименование,
                        DopUdobstva.Цена_доп_удобства
                    };

                DBItems.Items.Add($"Quantity free numbers: {Numbers1.Count()} ");//вывод колличества наших свободных номеров
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)//order 4
        {
            DBItems.Items.Clear();
            var characters = new
            {
                ReservationDay = ReservationDay4.SelectedDate,//дата бронирования
                ReservationEnd = ReservationEnd4.SelectedDate,//дата выезда
                RoomsQuantity = Convert.ToInt32(RoomsQuantity4.Text),//колличество комнат
                NameOfConvience = NameOfConvience4.Text//Наименование удобства
            };//Получаем данные с полей для ввода 
            using (Гостиночный_комплекмEntities db = new Гостиночный_комплекмEntities())
            {
                var Numbers1 =
                    from StatusNumber in db.Состояние_номера
                    join Numbers in db.Номера on StatusNumber.Код_номера equals Numbers.Код_номера
                    join NumberType in db.Тип_номера on Numbers.Код_тип_номера equals NumberType.Код_тип_номера
                    join DopUdobstva in db.Доп__Удобства on Numbers.Код_доп__удобства equals DopUdobstva.Код_доп__удобства
                    where NumberType.Количество_комнат == characters.RoomsQuantity && StatusNumber.Дата_выезд == characters.ReservationEnd &&
                            StatusNumber.Дата_заезд == characters.ReservationDay//Получаем номера где поля равны нашим, т. е. где колличество комнат равно указаному дата выезда равна указаной и т.д.
                    select new
                    {
                        NumberType.Стоимость,
                        NumberType.Наименование,
                        NumberType.Количество_комнат,
                        DopUdobstvaName = DopUdobstva.Наименование,
                        DopUdobstva.Цена_доп_удобства
                    };//Получим такой объект на выходе

                foreach (var n in Numbers1)//вывод
                    DbItems4.Items.Add($" Number type: {n.Наименование}; Number type price: {n.Стоимость}; quantity rooms: {n.Количество_комнат};" +
                        $" Dop udobstva: {n.DopUdobstvaName}; Dop udobstva price: {n.Цена_доп_удобства}; ");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)//order 5
        {
            int idNumber = Convert.ToInt32(NumberId5.Text);//Получаем id номера
            using (Гостиночный_комплекмEntities db = new Гостиночный_комплекмEntities())
            {
                var Numbers1 =
                    from StatusNumber in db.Состояние_номера
                    join Numbers in db.Номера on StatusNumber.Код_номера equals Numbers.Код_номера
                    join NumberType in db.Тип_номера on Numbers.Код_тип_номера equals NumberType.Код_тип_номера
                    join DopUdobstva in db.Доп__Удобства on Numbers.Код_доп__удобства equals DopUdobstva.Код_доп__удобства
                    where Numbers.Код_номера == idNumber//Ищем записи где наш id равен полученому
                    select new
                    {
                        NumberType.Стоимость,
                        NumberType.Наименование,
                        NumberType.Количество_комнат,
                        DopUdobstvaName = DopUdobstva.Наименование,
                        DopUdobstva.Цена_доп_удобства,
                        StatusNumber.Дата_выезд,
                        StatusNumber.Дата_заезд
                    };

                foreach (var n in Numbers1)//вывод
                    DBItems5.Items.Add($" Number type: {n.Наименование}; Number type price: {n.Стоимость}; quantity rooms: {n.Количество_комнат};" +
                        $" Dop udobstva: {n.DopUdobstvaName}; Dop udobstva price: {n.Цена_доп_удобства}; \nFree until: {n.Дата_заезд.Value.Subtract(DateTime.Now).Days} days");
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)//order 6
        {
            DBItems6.Items.Clear();
            DateTime date = ReservationEnd6.SelectedDate.Value;//Получаем дату которую ввели
            using (Гостиночный_комплекмEntities db = new Гостиночный_комплекмEntities())
            {
                var Numbers1 =
                    from StatusNumber in db.Состояние_номера
                    join Numbers in db.Номера on StatusNumber.Код_номера equals Numbers.Код_номера
                    join NumberType in db.Тип_номера on Numbers.Код_тип_номера equals NumberType.Код_тип_номера
                    join DopUdobstva in db.Доп__Удобства on Numbers.Код_доп__удобства equals DopUdobstva.Код_доп__удобства
                    where StatusNumber.isFree == false && StatusNumber.Дата_выезд.Value == date//Получаем номера которые заняты но освободяться к указаной дате
                    select new
                    {
                        NumberType.Стоимость,
                        NumberType.Наименование,
                        NumberType.Количество_комнат,
                        DopUdobstvaName = DopUdobstva.Наименование,
                        DopUdobstva.Цена_доп_удобства,
                        StatusNumber.Дата_выезд,
                        StatusNumber.Дата_заезд
                    };

                foreach (var n in Numbers1)//вывод
                    DBItems6.Items.Add($" Number type: {n.Наименование}; Number type price: {n.Стоимость}; quantity rooms: {n.Количество_комнат};" +
                        $" Dop udobstva: {n.DopUdobstvaName}; Dop udobstva price: {n.Цена_доп_удобства};");
            }
        }
        private void Button_Click_12(object sender, RoutedEventArgs e)//order 7
        {
            var sName = ClientSName7.Text;//Получаем фамилию
            DateTime fDate = FDate7.SelectedDate.Value;//Получаем первую дату из диапозона
            DateTime sDate = SDate7.SelectedDate.Value;//Получаем вторую дату из диапозона
            DBItems7.Items.Clear();
            using (Гостиночный_комплекмEntities db = new Гостиночный_комплекмEntities())
            {
                var Numbers1 =
                    from Bronj in db.Бронь
                    join StatusNumber in db.Состояние_номера on Bronj.Код_состояния equals StatusNumber.Код_состояние_номера
                    join Numbers in db.Номера on StatusNumber.Код_номера equals Numbers.Код_номера
                    join NumberType in db.Тип_номера on Numbers.Код_тип_номера equals NumberType.Код_тип_номера
                    join DopUdobstva in db.Доп__Удобства on Numbers.Код_доп__удобства equals DopUdobstva.Код_доп__удобства
                    where Bronj.Фамилия == sName && StatusNumber.Дата_заезд >= fDate && StatusNumber.Дата_заезд <= sDate//Получаем список броней где фамилия равна нашей и дата брони в веденном диапозоне
                    select new
                    {
                        NumberType.Стоимость,
                        NumberType.Наименование,
                        NumberType.Количество_комнат,
                        DopUdobstvaName = DopUdobstva.Наименование,
                        DopUdobstva.Цена_доп_удобства,
                        StatusNumber.Дата_выезд,
                        StatusNumber.Дата_заезд
                    };
                var preferedNumber =
                    (from Bronj in db.Бронь
                     where Bronj.Фамилия == sName
                     group Bronj by Bronj.Код_номера into g//групируем по коду номера, так сможем узнать какой номер сколько раз бронился
                     select new { idNumber = g.Key, Count = g.Count() });//получим такой объект с кодами номеров и колличеством их брони
                foreach (var n in Numbers1)
                    DBItems7.Items.Add($" Number type: {n.Наименование}; Number type price: {n.Стоимость}; quantity rooms: {n.Количество_комнат};" +
                        $" Dop udobstva: {n.DopUdobstvaName}; Dop udobstva price: {n.Цена_доп_удобства};");
                DBItems7.Items.Add($"Prefered number: {preferedNumber.Where(a => a.Count == preferedNumber.Max(b => b.Count)).Single().idNumber}");//находим максимальное значение по колличеству броней номеров и выводим
            }
        }
        private void Button_Click_5(object sender, RoutedEventArgs e)//order 10
        {
            int idNumber = Convert.ToInt32(NumberId10.Text);//получаем код номера
            DBItems10.Items.Clear();
            using (Гостиночный_комплекмEntities db = new Гостиночный_комплекмEntities())
            {
                var Numbers1 = //запрос вернет клиентов у которых код номера равен нашему полученому
                    from Clients in db.Клиент
                    join StatusNumber in db.Состояние_номера on Clients.Код_состояния_номера equals StatusNumber.Код_состояние_номера
                    where StatusNumber.Код_номера == idNumber
                    select new
                    {
                        Clients.Имя,
                        Clients.Фамилия,
                        Clients.Отчество,
                        Clients.Пол,
                        Clients.Дата_рождения,
                        Clients.Паспортные_данные,
                        Clients.Телефон,
                        StatusNumber.Сумма,
                        StatusNumber.Дата_выезд,
                        StatusNumber.Дата_заезд
                    };

                foreach (var n in Numbers1)//вывод инфы о клиенте
                    DBItems10.Items.Add($"Client name: {n.Имя + " " + n.Фамилия + " " + n.Отчество}; " +
                        $"birthday: {n.Дата_рождения}; gender: {n.Пол} phone number: {n.Телефон};" +
                        $" date in: {n.Дата_заезд}; date out: {n.Дата_выезд} price: {n.Сумма};");
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)//order 13
        {
            DateTime fDate = FDate13.SelectedDate.Value;//Получим первую дату из диапозона
            DateTime sDate = SDate13.SelectedDate.Value;//Получим вторую дату из диапозона
            DBItems13.Items.Clear();
            using (Гостиночный_комплекмEntities db = new Гостиночный_комплекмEntities())
            {
                var Numbers1 =
                    from Clients in db.Клиент
                    join StatusNumber in db.Состояние_номера on Clients.Код_состояния_номера equals StatusNumber.Код_состояние_номера
                    where StatusNumber.Дата_заезд >= fDate && StatusNumber.Дата_заезд <= sDate//Получаем всех клиентов за указанный период
                    select new
                    {
                        Clients.Имя,
                        Clients.Фамилия,
                        Clients.Отчество,
                        Clients.Пол,
                        Clients.Дата_рождения,
                        Clients.Паспортные_данные,
                        Clients.Телефон,
                        StatusNumber.Сумма,
                        StatusNumber.Дата_выезд,
                        StatusNumber.Дата_заезд
                    };

                foreach (var n in Numbers1)//вывод информации
                    DBItems13.Items.Add($"Client name: {n.Имя + " " + n.Фамилия + " " + n.Отчество}; " +
                        $"birthday: {n.Дата_рождения}; gender: {n.Пол} phone number: {n.Телефон};" +
                        $" date in: {n.Дата_заезд}; date out: {n.Дата_выезд} price: {n.Сумма};");
            }
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)//order 11
        {
            DateTime fDate = FDate11.SelectedDate.Value;//Получим первую дату из диапозона
            DateTime sDate = SDate11.SelectedDate.Value;//Получим вторую дату из диапозона
            using (Гостиночный_комплекмEntities db = new Гостиночный_комплекмEntities())
            {
                var Numbers1 =
                    from Clients in db.Клиент
                    join StatusNumber in db.Состояние_номера on Clients.Код_состояния_номера equals StatusNumber.Код_состояние_номера
                    join Bronj in db.Бронь on StatusNumber.Код_брони equals Bronj.Код_бронь
                    where StatusNumber.Дата_заезд >= fDate && StatusNumber.Дата_заезд <= sDate//вернет список клиентов которые заселились в указаный срок
                    select new
                    {
                        Clients.Имя,
                        Clients.Фамилия,
                        Clients.Отчество,
                        Clients.Пол,
                        Clients.Дата_рождения,
                        Clients.Паспортные_данные,
                        Clients.Телефон,
                        StatusNumber.Сумма,
                        StatusNumber.Дата_выезд,
                        StatusNumber.Дата_заезд
                    };

                foreach (var n in Numbers1)
                    DBItems11.Items.Add($"Client name: {n.Имя + " " + n.Фамилия + " " + n.Отчество}; " +
                        $"birthday: {n.Дата_рождения}; gender: {n.Пол} phone number: {n.Телефон};" +
                        $" date in: {n.Дата_заезд}; date out: {n.Дата_выезд} price: {n.Сумма};");
            }
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)//order 12
        {
            using (Гостиночный_комплекмEntities db = new Гостиночный_комплекмEntities())
            {
                var Numbers1 =
                    from Bronj in db.Бронь
                    group Bronj by Bronj.Фамилия into g
                    select new { Name = g.Key, Count = g.Count() };//Групируем брони по фамилиям
                var MaxEl = Numbers1.Where(a => a.Count == Numbers1.Max(b => b.Count)).Single();//ищем максимальный элемент по полю Count, так сможем найти чаще всего посещающего клиента
                var Finalres = from Clients in db.Клиент//Получаем клиентов у которого фамилия равна фамилии постоянного клиента
                               where Clients.Фамилия == MaxEl.Name
                               select new
                               {
                                   Clients.Имя,
                                   Clients.Фамилия,
                                   Clients.Отчество,
                                   Clients.Пол,
                                   Clients.Дата_рождения,
                                   Clients.Паспортные_данные,
                                   Clients.Телефон
                               };
                foreach (var n in Finalres)//Вывод
                    DBItems12.Items.Add($"Client name: {n.Имя + " " + n.Фамилия + " " + n.Отчество}; " +
                        $"birthday: {n.Дата_рождения}; gender: {n.Пол} phone number: {n.Телефон};");

            }
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)//order 14
        {
            DBItems14.Items.Clear();
            var Name = ClientName14.Text;//Получаем фамилию клиента
            using (Гостиночный_комплекмEntities db = new Гостиночный_комплекмEntities())
            {
                var Numbers1 =
                    from Clients in db.Клиент
                    join StatusNumber in db.Состояние_номера on Clients.Код_состояния_номера equals StatusNumber.Код_состояние_номера
                    join Bronj in db.Бронь on StatusNumber.Код_состояние_номера equals Bronj.Код_состояния
                    join Numbers in db.Номера on StatusNumber.Код_номера equals Numbers.Код_номера
                    join NumberType in db.Тип_номера on Numbers.Код_тип_номера equals NumberType.Код_тип_номера
                    join DopUdobstva in db.Доп__Удобства on Numbers.Код_доп__удобства equals DopUdobstva.Код_доп__удобства
                    where Bronj.Фамилия == Name//Ищем в брони клиента где фамилия равна введенной
                    select new
                    {
                        Clients.Имя,
                        Clients.Фамилия,
                        Clients.Отчество,
                        Clients.Пол,
                        Clients.Дата_рождения,
                        Clients.Паспортные_данные,
                        Clients.Телефон,
                        StatusNumber.Сумма,
                        StatusNumber.Дата_выезд,
                        StatusNumber.Дата_заезд,
                        NumberType.Стоимость,
                        NumberType.Наименование,
                        NumberType.Количество_комнат,
                        DopUdobstvaName = DopUdobstva.Наименование,
                        DopUdobstva.Цена_доп_удобства

                    };//Получим всю инфу о клиенте и где он останавливался

                foreach (var n in Numbers1)
                    DBItems14.Items.Add($"Client name: {n.Имя + " " + n.Фамилия + " " + n.Отчество}; " +
                        $"birthday: {n.Дата_рождения}; gender: {n.Пол} phone number: {n.Телефон};" +
                        $" date in: {n.Дата_заезд}; date out: {n.Дата_выезд} price: {n.Сумма};" +
                        $" Number type: {n.Наименование}; Number type price: {n.Стоимость}; " +
                        $"quantity rooms: {n.Количество_комнат};" +
                        $" Dop udobstva: {n.DopUdobstvaName}; Dop udobstva price: {n.Цена_доп_удобства};");
            }
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)//order 15
        {
            DBItems15.Items.Clear();
            var NumberId = Convert.ToInt32(NumberId15.Text);//Получаем код номера
            using (Гостиночный_комплекмEntities db = new Гостиночный_комплекмEntities())
            {
                var Numbers1 =
                    from Numbers in db.Номера
                    join StatusNumber in db.Состояние_номера on Numbers.Код_номера equals StatusNumber.Код_номера
                    join Clients in db.Клиент on StatusNumber.Код_состояние_номера equals Clients.Код_состояния_номера
                    join Bronj in db.Бронь on StatusNumber.Код_состояние_номера equals Bronj.Код_состояния
                    join NumberType in db.Тип_номера on Numbers.Код_тип_номера equals NumberType.Код_тип_номера
                    join DopUdobstva in db.Доп__Удобства on Numbers.Код_доп__удобства equals DopUdobstva.Код_доп__удобства
                    where Numbers.Код_номера == NumberId//Запрс вернет всю инфу о клиенте, доп услугах и номере код которого равен введенному
                    select new
                    {
                        Clients.Имя,
                        Clients.Фамилия,
                        Clients.Отчество,
                        Clients.Пол,
                        Clients.Дата_рождения,
                        Clients.Паспортные_данные,
                        Clients.Телефон,
                        StatusNumber.Сумма,
                        StatusNumber.Дата_выезд,
                        StatusNumber.Дата_заезд,
                        NumberType.Стоимость,
                        NumberType.Наименование,
                        NumberType.Количество_комнат,
                        DopUdobstvaName = DopUdobstva.Наименование,
                        DopUdobstva.Цена_доп_удобства
                    };//Объект с полями содержащими всю информацию

                foreach (var n in Numbers1)//вывод
                    DBItems15.Items.Add($"Client name: {n.Имя + " " + n.Фамилия + " " + n.Отчество}; " +
                        $"birthday: {n.Дата_рождения}; gender: {n.Пол} phone number: {n.Телефон};" +
                        $" date in: {n.Дата_заезд}; date out: {n.Дата_выезд} price: {n.Сумма};" +
                        $" Number type: {n.Наименование}; Number type price: {n.Стоимость}; " +
                        $"quantity rooms: {n.Количество_комнат};" +
                        $" Dop udobstva: {n.DopUdobstvaName}; Dop udobstva price: {n.Цена_доп_удобства};");
            }
        }

        
    }
}

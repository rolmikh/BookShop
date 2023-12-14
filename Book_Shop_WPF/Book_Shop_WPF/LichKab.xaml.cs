using System;
using Newtonsoft.Json;
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
using System.Net.Http;
using Book_Shop_WPF.Models;
using System.Data;
using System.Reflection;

namespace Book_Shop_WPF
{
    /// <summary>
    /// Логика взаимодействия для LichKab.xaml
    /// </summary>
    public partial class LichKab : Window
    {
        public LichKab()
        {
            InitializeComponent();
            GetUser();
            FillOrder();
        }
        bool auth = false;

        public static DataTable CreateDataTable<T>(IEnumerable<T> list, int index)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            dataTable.TableName = typeof(T).FullName;
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                if (properties[index].GetValue(entity).ToString() != "1")
                {
                    for (int i = 0; i < properties.Length; i++)
                    {
                        values[i] = properties[i].GetValue(entity);
                    }
                    dataTable.Rows.Add(values);
                }

            }


            return dataTable;
        }
        /// <summary>
        /// Создание объекта типа T из DataRow
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        try
                        {
                            pro.SetValue(obj, dr[column.ColumnName], null);
                        }
                        catch { }
                    else
                        continue;
                }
            }
            return obj;
        }
        /// <summary>
        /// Преобразование DataTable в List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private async void FillOrder()
        {
            try
            {
                int ID;
                int ID1;
                List<OrderComposition> orderCompositions = new List<OrderComposition>();
                List<OrderComposition> orderCompositions1 = new List<OrderComposition>();
                List<OrderComposition> orderCompositions2 = new List<OrderComposition>();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(App.ip + "OrderCompositions"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();

                            orderCompositions = JsonConvert.DeserializeObject<List<OrderComposition>>(apiResponse);
                            orderCompositions2 = orderCompositions.Where(n => n.UserId == App.ID).ToList();
                            if (orderCompositions2 != null)
                            {
                                for (int i = 0; i < orderCompositions2.Count; i++)
                                {
                                    ID = orderCompositions2[i].OrderId.Value;
                                    Order order = new Order();
                                    using (var httpClient1 = new HttpClient())
                                    {
                                        using (var response1 = await httpClient1.GetAsync(App.ip + "Orders/" + ID))
                                        {
                                            if (response1.IsSuccessStatusCode)
                                            {
                                                string apiResponse1 = await response1.Content.ReadAsStringAsync();

                                                order = JsonConvert.DeserializeObject<Order>(apiResponse1);
                                                orderCompositions2[i].NameStatusOrder = order.StatusOrderId.ToString();
                                                orderCompositions2[i].NumberOrder = order.NumberOrder;
                                                orderCompositions2[i].DateOrder = DateTime.Parse(order.DateOrder.ToString());
                                                orderCompositions2[i].PriceOrder = decimal.Parse(order.PriceOrder.ToString());
                                                ID1 = int.Parse(orderCompositions2[i].NameStatusOrder);
                                                StatusOrder statusOrder = new StatusOrder();
                                                using (var httpClient2 = new HttpClient())
                                                {
                                                    using (var response2 = await httpClient2.GetAsync(App.ip + "StatusOrders/" + ID1))
                                                    {
                                                        if (response2.IsSuccessStatusCode)
                                                        {
                                                            string apiResponse2 = await response2.Content.ReadAsStringAsync();

                                                            statusOrder = JsonConvert.DeserializeObject<StatusOrder>(apiResponse2);
                                                            orderCompositions2[i].NameStatusOrder = statusOrder.NameStatusOrder;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    DataTable dataTable = new DataTable();
                                    dataTable = CreateDataTable(orderCompositions2, 5);
                                    orderCompositions1 = ConvertDataTable<OrderComposition>(dataTable);

                                }
                                
                                dtOrders.ItemsSource = orderCompositions1;
                            }
                            
                        }

                    }
                }
            }
            catch (Exception ex) { }
        }

        private void btKorzina_Click(object sender, RoutedEventArgs e)
        {
            BasketWindow basket = new BasketWindow();
            basket.Show();
            auth = true;
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!auth)
            {
                switch (MessageBox.Show("Завершить работу приложения?", "Книжная страна", MessageBoxButton.YesNo, MessageBoxImage.Question))
                {
                    //Реакция программы после нажатия кнопки Да
                    case MessageBoxResult.Yes:
                        //Передача в параметр продолжения закрытия приолжения
                        e.Cancel = false;
                        //Завершение работы приложения, как процесса
                        Application.Current.Shutdown();
                        break;
                    //Реакция программеы после нажатия кнопки Нет
                    case MessageBoxResult.No:
                        //Передача в параметр отмены завершения работы приложения
                        e.Cancel = true;
                        break;
                }
            }
        }


        public async Task GetUser()
        {

            try
            {

                User user = new User();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(App.ip + "Users/" + App.ID))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();

                            user = JsonConvert.DeserializeObject<User>(apiResponse);

                            user.FIO = user.SurnameUser + " " + user.NameUser + " " + user.MiddleNameUser;
                            user.date = user.DateBirthUser.ToString().Substring(0,10);

                        }
                        else
                        {

                        }

                    }
                }

                List<User> userList = new List<User> { user };

                UsersListView.ItemsSource = userList;
            }
            catch (Exception ex) { }

        }

        private void btCatalog_Click(object sender, RoutedEventArgs e)
        {
            Catalog catalog = new Catalog();
            catalog.Show();
            auth = true;
            Close();

        }

        private void btNazad_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btLogOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            App.ID = -1;
            auth = true;
            Close();
        }
    }
}

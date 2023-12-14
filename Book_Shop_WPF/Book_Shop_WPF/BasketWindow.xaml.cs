using Book_Shop_WPF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
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

namespace Book_Shop_WPF
{
    /// <summary>
    /// Логика взаимодействия для BasketWindow.xaml
    /// </summary>
    public partial class BasketWindow : Window
    {
        public BasketWindow()
        {
            InitializeComponent();
            GetBasket();

        }
        bool auth = false;
        List<Basket> BasketsList = new List<Basket>();
        

        private async void btToOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Random rnd = new Random();
                int number = rnd.Next(100000, 999999);
                Order order = new Order();
                
                order.NumberOrder = number.ToString();
                order.StatusOrderId = 2;
                order.DateOrder = DateTime.Now;
                order.IsDeleted = 0;
                order.PriceOrder = decimal.Parse(tblTotalPrice.Text);
                Order order1 = new Order();
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync(App.ip + "Orders", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        order1 = JsonConvert.DeserializeObject<Order>(apiResponse);
                    }

                }

                List<Basket> basket = new List<Basket>();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(App.ip + "Baskets/" + "?userId=" + App.ID))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();

                            basket = JsonConvert.DeserializeObject<List<Basket>>(apiResponse);
                            List<Basket> baskets = basket.Where(n => n.IsDeletedBasket == 0).ToList();

                            for (int i = 0; i < baskets.Count; i++)
                            {
                                OrderComposition orderList = new OrderComposition();
                                orderList.OrderId = order1.IdOrder;
                                orderList.BasketId = int.Parse(baskets[i].IdBasket.ToString());
                                orderList.UserId = App.ID;
                                orderList.IsDeleted = 0;
                                int? basId = orderList.BasketId;
                                OrderComposition orderList1 = new OrderComposition();
                                using (var httpClient1 = new HttpClient())
                                {
                                    StringContent content1 = new StringContent(JsonConvert.SerializeObject(orderList), Encoding.UTF8, "application/json");
                                    using (var response1 = await httpClient1.PostAsync(App.ip + "OrderCompositions", content1))
                                    {
                                        string apiResponse1 = await response1.Content.ReadAsStringAsync();
                                        orderList1 = JsonConvert.DeserializeObject<OrderComposition>(apiResponse1);
                                    }

                                }


                                try
                                {
                                    Basket basket1 = new Basket();
                                    using (var httpClient2 = new HttpClient())
                                    {
                                        StringContent content2 = new StringContent(JsonConvert.SerializeObject(basket1), Encoding.UTF8, "application/json");
                                        using (var response2 = await httpClient2.DeleteAsync(App.ip + "Baskets/" + basId))
                                        {
                                            string apiResponse2 = await response2.Content.ReadAsStringAsync();

                                        }
                                    }

                                }
                                catch
                                {
                                    MessageBox.Show("Ошибка!");
                                }
                            }
                        }
                    }
                }
                await GetBasket();
                MessageBox.Show("Заказ оформлен!");
                LichKab lich = new LichKab();
                lich.Show();
            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
        }



        private async void btDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Basket basket = new Basket();
                Button button = e.OriginalSource as Button;
                string product = button.ToolTip as string;
                int? ID = BasketsList.Find(n => n.NameBook == product).IdBasket;
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(basket), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.DeleteAsync(App.ip + "Baskets/" + ID))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                    }
                }
                await GetBasket();


            }
            catch { }
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

        

        public async Task GetBasket()
        {
            try
            {
                int ID;
                double summa = 0;
                List<Basket> basket = new List<Basket>();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(App.ip + "Baskets/" + "?userId=" + App.ID))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();

                            basket = JsonConvert.DeserializeObject<List<Basket>>(apiResponse);
                            List<Basket> baskets = basket.Where(n => n.IsDeletedBasket == 0).ToList();
                            if (baskets != null)
                            {
                                for (int i = 0; i < baskets.Count; i++)
                                {
                                    if (baskets[i].IsDeletedBasket == 0)
                                    {
                                        ID = baskets[i].ProductId.Value;
                                        Product product = new Product();
                                        using (var httpClient1 = new HttpClient())
                                        {
                                            using (var response1 = await httpClient1.GetAsync(App.ip + "Products/" + ID))
                                            {
                                                if (response1.IsSuccessStatusCode)
                                                {
                                                    string apiResponse1 = await response1.Content.ReadAsStringAsync();

                                                    product = JsonConvert.DeserializeObject<Product>(apiResponse1);

                                                    baskets[i].NameBook = product.NameBook.ToString();
                                                    baskets[i].Author = product.Author.ToString();
                                                    baskets[i].PriceBook = product.PriceBook.ToString();
                                                    baskets[i].PhotoBook = product.PhotoBook.ToString();


                                                }
                                            }
                                        }
                                        summa += double.Parse(baskets[i].PriceBook);
                                    }
                                }
                                BasketListView.ItemsSource = baskets;
                                BasketsList = baskets;
                                tblTotalPrice.Text = summa.ToString();
                                //PriceBasketListView.ItemsSource = basket;
                            }
                            else
                            {
                                switch (MessageBox.Show("Ваша корзина пуста. Хотите добавить товар?", "Книжная страна", MessageBoxButton.YesNo, MessageBoxImage.Question))
                                {
                                    //Реакция программы после нажатия кнопки Да
                                    case MessageBoxResult.Yes:
                                        Catalog catalog = new Catalog();
                                        catalog.Show();
                                        Close();
                                        break;
                                    //Реакция программеы после нажатия кнопки Нет
                                    case MessageBoxResult.No:
                                        break;
                                }
                            }
                        }

                    }
                }
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

        private void btLichKab_Click(object sender, RoutedEventArgs e)
        {
            LichKab lichKab = new LichKab();
            lichKab.Show();
            auth = true;
            Close();

        }

        private void btNazad_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

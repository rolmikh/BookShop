using Book_Shop_WPF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для OneProduct.xaml
    /// </summary>
    public partial class OneProduct : Window
    {
        public OneProduct()
        {
            InitializeComponent();
            GetOneProduct();
            

        }

        bool auth = false;
        
        

        public async Task GetOneProduct()
        {

            try
            {
                int ID = App.IDBooks;
                Product product = new Product();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(App.ip + "Products/" + ID))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();

                            product = JsonConvert.DeserializeObject<Product>(apiResponse);

                            using (var httpClient1 = new HttpClient())
                            {
                                using (var response1 = await httpClient1.GetAsync(App.ip + "Categories/" + product.CategoryId))
                                {
                                    if (response.IsSuccessStatusCode)
                                    {
                                        string apiResponse1 = await response1.Content.ReadAsStringAsync();

                                        var category = JsonConvert.DeserializeObject<Category>(apiResponse1);

                                        product.Category = category.NameCategory.ToString();
                                    }

                                }
                            }
                        }

                    }
                }

                List<Product> productList = new List<Product> { product };

                BookListView.ItemsSource = productList;

            }
            catch (Exception ex) { }

        }


        private void btNazad_Click(object sender, RoutedEventArgs e)
        {

            Catalog catalog = new Catalog();
            catalog.Show();
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

        private async void btToBasket_Click(object sender, RoutedEventArgs e)
        {
            
            if (App.ID != -1)
            {
                try
                {
                    Basket basket = new Basket();
                    basket.IdBasket = null;
                    basket.UserId = App.ID;
                    basket.ProductId = App.IDBooks;
                    basket.IsDeletedBasket = 0;
                    Basket basket1 = new Basket();
                    using (var httpClient = new HttpClient())
                    {
                        StringContent content = new StringContent(JsonConvert.SerializeObject(basket), Encoding.UTF8, "application/json");
                        using (var response = await httpClient.PostAsync(App.ip + "Baskets", content))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            basket1 = JsonConvert.DeserializeObject<Basket>(apiResponse);
                            if(response.IsSuccessStatusCode)
                            {
                                switch (MessageBox.Show("Товар добавлен в корзину! Перейти в корзину?", " ", MessageBoxButton.YesNo))
                                {
                                    case MessageBoxResult.Yes:
                                        BasketWindow basketWindow = new BasketWindow();
                                        basketWindow.Show();
                                        auth = true;
                                        Close();
                                        break;
                                    case MessageBoxResult.No:

                                        break;

                                }
                            }
                            else
                            {
                                MessageBox.Show("Ошибка добавления!");
                            }

                        }

                    }






                }
                catch
                {
                    MessageBox.Show("Ошибка!");
                }
            }
            
            else
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                auth = true;
                Close();
            }
        }

        private void btBasket_Click(object sender, RoutedEventArgs e)
        {
            BasketWindow basketWindow = new BasketWindow();
            basketWindow.Show();
            auth = true;
            Close();
        }
    }
}

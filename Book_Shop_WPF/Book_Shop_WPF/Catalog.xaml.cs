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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Book_Shop_WPF
{
    /// <summary>
    /// Логика взаимодействия для Catalog.xaml
    /// </summary>
    public partial class Catalog : Window
    {
        
        public Catalog()
        {
            InitializeComponent();
            GetProduct();

        }
        List<Product> productsList = new List<Product>();
        int i = 0;
        bool auth = false;
        

        private void btKabinet_Click(object sender, RoutedEventArgs e)
        {
            if(App.ID == -1)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                auth = true;
                Close();
            }
            else 
            {
                LichKab lichKab = new LichKab();
                lichKab.Show();
                auth = true;
                Close();
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        public async Task GetProduct()
        {

            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(App.ip + "Products"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();

                            var products = JsonConvert.DeserializeObject<List<Product>>(apiResponse);

                            var productsFilt = products.Where(n => n.IsDeleted == 0).ToList();
                            ProductsListView.ItemsSource = productsFilt;
                            productsList = productsFilt;
                      
                        }
                        else
                        {

                        }

                    }
                }
            }
            catch (Exception ex) { }

        }


        public async Task SortProduct(string typeSort)
        {

            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(App.ip + "Products/" + typeSort))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();

                            var products = JsonConvert.DeserializeObject<List<Product>>(apiResponse);

                            ProductsListView.ItemsSource = products;
                            productsList = products;

                        }
                        else
                        {

                        }

                    }
                }
            }
            catch (Exception ex) { }

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

        private void btBasket_Click(object sender, RoutedEventArgs e)
        {
            if (App.ID == -1)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                auth = true;
                Close();
            }
            else
            {
                BasketWindow basket = new BasketWindow();
                basket.Show();
                auth = true;
                Close();
            }

        }

        

        private void imClick_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Border border = e.OriginalSource as Border;
            string product = border.ToolTip as string;
            App.IDBooks = productsList.Find(n => n.NameBook == product).IdProduct.Value;
            auth = true;
            OneProduct oneProduct = new OneProduct();
            oneProduct.Show();
            Close();
            
            
        }


        private async void btFiltrView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(App.ip + "Products/Filtr?IdCategory=" + int.Parse(cbCategory.SelectedValue.ToString())))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();

                            var products = JsonConvert.DeserializeObject<List<Product>>(apiResponse);
                            if(products.Count == 0)
                            {
                                await GetProduct();
                            }
                            else
                            {
                                ProductsListView.ItemsSource = products;
                                productsList = products;
                                PopupFiltr.IsOpen = false;
                                
                            }
                        }
                        else
                        {

                        }

                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Выберите категорию!");
            }
        }

        private void btFiltr_Click(object sender, RoutedEventArgs e)
        {
            if (i == 0)
            {
                PopupFiltr.IsOpen = true;
                i++;
            }
            else if (i == 1)
            {
                PopupFiltr.IsOpen = false;
                i--;
            }
        }

        private async void cbCategory_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(App.ip + "Categories"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();

                            var categories = JsonConvert.DeserializeObject<List<Category>>(apiResponse);

                            cbCategory.ItemsSource = categories;

                        }
                        else
                        {

                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private async void btSort_Click(object sender, RoutedEventArgs e)
        {
            string typeSort = "";
            if (i == 0)
            {
                typeSort = "SortASC";
                await SortProduct(typeSort);
                i++;
            }
            else if (i == 1)
            {
                typeSort = "SortDesc";
                await SortProduct(typeSort);
                i--;
               
            }
        }

        private void tbSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            //tbSearch.Clear();
        }

        private async void btSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(tbSearch.Text != string.Empty && tbSearch.Text != "Поиск")
                {
                    using (var httpClient = new HttpClient())
                    {
                        using (var response = await httpClient.GetAsync(App.ip + "Products/Search?text=" + tbSearch.Text))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string apiResponse = await response.Content.ReadAsStringAsync();

                                var products = JsonConvert.DeserializeObject<List<Product>>(apiResponse);
                                if(products.Count != null)
                                {
                                    ProductsListView.ItemsSource = products;
                                    productsList = products;
                                }
                                else
                                {
                                    await GetProduct();
                                }

                                


                            }
                            else
                            {

                            }

                        }
                    }
                }
                else
                {
                    await GetProduct();
                }
                
            }
            catch (Exception ex) { }
        }
    }
}

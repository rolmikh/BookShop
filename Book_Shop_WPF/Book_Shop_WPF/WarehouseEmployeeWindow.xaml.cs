using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Reflection;
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
using Book_Shop_WPF.Models;
using System.Text.RegularExpressions;

namespace Book_Shop_WPF
{
    /// <summary>
    /// Логика взаимодействия для WarehouseEmployeeWindow.xaml
    /// </summary>
    public partial class WarehouseEmployeeWindow : Window
    {
        public WarehouseEmployeeWindow()
        {
            InitializeComponent();
        }

        bool auth = false;
        int ID;

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

        private async void btNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(tbCategory.Text))
                {
                    Category category = new Category();
                    category.NameCategory = tbCategory.Text;
                    category.IsDeletedCategory = 0;
                    Category category1 = new Category();
                    using (var httpClient = new HttpClient())
                    {
                        StringContent content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");
                        using (var response = await httpClient.PostAsync(App.ip + "Categories", content))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            category1 = JsonConvert.DeserializeObject<Category>(apiResponse);
                        }

                    }
                    tbCategory.Text = null;

                }
                else
                {
                    MessageBox.Show("Поле 'Название категории' должно быть заполнено!");
                   tbCategory.Focus();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
        }

        private async void btUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(tbCategory.Text))
                {
                    Category category = new Category();
                    category.IdCategory= ID;
                    category.NameCategory = tbCategory.Text;
                    category.IsDeletedCategory = 0;
                    Category category1 = new Category();
                    using (var httpClient = new HttpClient())
                    {
                        StringContent content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");
                        using (var response = await httpClient.PutAsync(App.ip + "Categories/" + ID, content))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            category1 = JsonConvert.DeserializeObject<Category>(apiResponse);


                        }

                    }
                    tbCategory.Text = null;

                }
                else
                {
                    MessageBox.Show("Поле 'Название категории' должно быть заполнено!");
                    tbCategory.Focus();
                }
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
                Category category = new Category();
                Category row = (Category)dtCategory.SelectedItem;
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.DeleteAsync(App.ip + "Categories/" + row.IdCategory))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                    }
                }
                tbCategory.Text = null;

            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
        }

        private async void btFillCategory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Category> categories = new List<Category>();
                List<Category> category1 = new List<Category>();

                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(App.ip + "Categories"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        categories = JsonConvert.DeserializeObject<List<Category>>(apiResponse);


                        DataTable dataTable = new DataTable();
                        dataTable = CreateDataTable(categories, 2);
                        category1 = ConvertDataTable<Category>(dataTable);

                        dtCategory.ItemsSource = category1;

                    }
                }

            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
        }

        

        private void dtCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dtCategory.Items.Count != 0 && dtCategory.SelectedItems.Count != 0)
                {
                    Category dataRow = (Category)dtCategory.SelectedItems[0];
                    ID = (int)dataRow.IdCategory;
                    tbCategory.Text = dataRow.NameCategory;
                }
            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
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

        private void dtContract_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dtContract.Items.Count != 0 && dtContract.SelectedItems.Count != 0)
                {
                    Contract dataRow = (Contract)dtContract.SelectedItems[0];
                    ID = (int)dataRow.IdContract;
                    tbNumberContract.Text = dataRow.NumberContract;
                    tbDateContract.Text = dataRow.DateContract.ToString();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
        }

        private async void btNewContract_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(tbNumberContract.Text))
                {
                    Contract contract = new Contract();
                    contract.NumberContract = tbNumberContract.Text;
                    contract.DateContract = DateTime.Now;
                    contract.IsDeletedContract = 0;
                    Contract contract1 = new Contract();
                    using (var httpClient = new HttpClient())
                    {
                        StringContent content = new StringContent(JsonConvert.SerializeObject(contract), Encoding.UTF8, "application/json");
                        using (var response = await httpClient.PostAsync(App.ip + "Contracts", content))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            contract1 = JsonConvert.DeserializeObject<Contract>(apiResponse);
                        }

                    }
                    tbDateContract.Text = null;
                    tbNumberContract.Text = null;

                }
                else
                {
                    MessageBox.Show("Поле 'Номер контракта' должно быть заполнено!");
                    tbNumberContract.Focus();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
        }

        private async void btUpdateContract_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(tbNumberContract.Text))
                {
                    Contract contract = new Contract();
                    contract.IdContract = ID;
                    contract.NumberContract = tbNumberContract.Text;
                    contract.DateContract = DateTime.Parse(tbDateContract.Text);
                    contract.IsDeletedContract = 0;
                    Contract contract1 = new Contract();
                    using (var httpClient = new HttpClient())
                    {
                        StringContent content = new StringContent(JsonConvert.SerializeObject(contract), Encoding.UTF8, "application/json");
                        using (var response = await httpClient.PutAsync(App.ip + "Contracts/" + ID, content))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            contract1 = JsonConvert.DeserializeObject<Contract>(apiResponse);


                        }

                    }
                    tbDateContract.Text = null;
                    tbNumberContract.Text = null;

                }
                else
                {
                    MessageBox.Show("Поле 'Номер контракта' должно быть заполнено!");
                    tbNumberContract.Focus();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
        }

        private async void btDeleteContract_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Contract contract = new Contract();
                Contract row = (Contract)dtContract.SelectedItem;
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(contract), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.DeleteAsync(App.ip + "Contracts/" + row.IdContract))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                    }
                }
                tbDateContract.Text = null;
                tbNumberContract.Text = null;

            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
        }

        private async void btFillContract_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Contract> contracts = new List<Contract>();
                List<Contract> contract1 = new List<Contract>();

                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(App.ip + "Contracts"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        contracts = JsonConvert.DeserializeObject<List<Contract>>(apiResponse);


                        DataTable dataTable = new DataTable();
                        dataTable = CreateDataTable(contracts, 3);
                        contract1 = ConvertDataTable<Contract>(dataTable);

                        dtContract.ItemsSource = contract1;

                    }
                }

            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
        }

        private void tbNumberContract_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tbNumberDeliveryNote_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private async void btNewDeliveryNote_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(tbNumberDeliveryNote.Text))
                {
                    if (cbContract.SelectedValue != null)
                    {
                        DeliveryNote deliveryNote = new DeliveryNote();
                        deliveryNote.NumberDeliveryNote = tbNumberDeliveryNote.Text;
                        deliveryNote.DateDeliveryNote = DateTime.Now;
                        deliveryNote.ContractId = Int32.Parse(cbContract.SelectedValue.ToString());
                        deliveryNote.IsDeletedNote = 0;
                        DeliveryNote deliveryNote1 = new DeliveryNote();
                        using (var httpClient = new HttpClient())
                        {
                            StringContent content = new StringContent(JsonConvert.SerializeObject(deliveryNote), Encoding.UTF8, "application/json");
                            using (var response = await httpClient.PostAsync(App.ip + "DeliveryNotes", content))
                            {
                                string apiResponse = await response.Content.ReadAsStringAsync();
                                deliveryNote1 = JsonConvert.DeserializeObject<DeliveryNote>(apiResponse);
                            }

                        }
                        tbNumberDeliveryNote.Text = null;
                        tbDateDeliveryNote.Text = null;
                        cbContract.SelectedItem = null;

                    }
                    else
                    {
                        MessageBox.Show("Поле 'Номер договора' должно быть заполнено!");
                        cbContract.Focus();
                    }

                }
                else
                {
                    MessageBox.Show("Поле 'Номер накладной' должно быть заполнено!");
                    tbNumberDeliveryNote.Focus();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
        }

        private async void btUpdateDeliveryNote_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(tbNumberDeliveryNote.Text))
                {
                    if (cbContract.SelectedValue != null)
                    {
                        DeliveryNote deliveryNote = new DeliveryNote();
                        deliveryNote.IdDeliveryNote = ID;
                        deliveryNote.NumberDeliveryNote = tbNumberDeliveryNote.Text;
                        deliveryNote.DateDeliveryNote = DateTime.Parse(tbDateDeliveryNote.Text);
                        deliveryNote.ContractId = Int32.Parse(cbContract.SelectedValue.ToString());
                        deliveryNote.IsDeletedNote = 0;
                        DeliveryNote deliveryNote1 = new DeliveryNote();
                        using (var httpClient = new HttpClient())
                        {
                            StringContent content = new StringContent(JsonConvert.SerializeObject(deliveryNote), Encoding.UTF8, "application/json");
                            using (var response = await httpClient.PutAsync(App.ip + "DeliveryNotes/" + ID, content))
                            {
                                string apiResponse = await response.Content.ReadAsStringAsync();
                                deliveryNote1 = JsonConvert.DeserializeObject<DeliveryNote>(apiResponse);


                            }

                        }
                        tbNumberDeliveryNote.Text = null;
                        tbDateDeliveryNote.Text = null;
                        cbContract.SelectedItem = null;
                    }
                    else
                    {
                        MessageBox.Show("Поле 'Номер договора' должно быть заполнено!");
                        cbContract.Focus();
                    }

                }
                else
                {
                    MessageBox.Show("Поле 'Номер накладной' должно быть заполнено!");
                    tbNumberDeliveryNote.Focus();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
        }

        private async void btDeleteDeliveryNote_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DeliveryNote deliveryNote = new DeliveryNote();
                DeliveryNote row = (DeliveryNote)dtDeliveryNote.SelectedItem;
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(deliveryNote), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.DeleteAsync(App.ip + "DeliveryNotes/" + row.IdDeliveryNote))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                    }
                }
                tbNumberDeliveryNote.Text = null;
                tbDateDeliveryNote.Text = null;
                cbContract.SelectedItem = null;

            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
        }

        private async void btFillDeliveryNote_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<DeliveryNote> deliveryNotes = new List<DeliveryNote>();
                List<DeliveryNote> deliveryNote1 = new List<DeliveryNote>();
                int ID1;
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(App.ip + "DeliveryNotes"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            deliveryNotes = JsonConvert.DeserializeObject<List<DeliveryNote>>(apiResponse);

                            if (deliveryNotes != null)
                            {
                                for (int i = 0; i < deliveryNotes.Count; i++)
                                {
                                    ID1 = deliveryNotes[i].ContractId.Value;
                                    Contract contract = new Contract();
                                    using (var httpClient1 = new HttpClient())
                                    {
                                        using (var response1 = await httpClient1.GetAsync(App.ip + "Contracts/" + ID1))
                                        {
                                            if (response1.IsSuccessStatusCode)
                                            {
                                                string apiResponse1 = await response1.Content.ReadAsStringAsync();
                                                contract = JsonConvert.DeserializeObject<Contract>(apiResponse1);

                                                deliveryNotes[i].NumberContract = contract.NumberContract;
                                                deliveryNotes[i].DateContract = contract.DateContract;
                                            }

                                        }
                                    }
                                    DataTable dataTable = new DataTable();
                                    dataTable = CreateDataTable(deliveryNotes, 4);
                                    deliveryNote1 = ConvertDataTable<DeliveryNote>(dataTable);
                                    dtDeliveryNote.ItemsSource = deliveryNote1;

                                }

                            }
                           
                        }

                    }
                }

            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
        }

        private void dtDeliveryNote_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dtDeliveryNote.Items.Count != 0 && dtDeliveryNote.SelectedItems.Count != 0)
                {
                    DeliveryNote dataRow = (DeliveryNote)dtDeliveryNote.SelectedItems[0];
                    ID = (int)dataRow.IdDeliveryNote;
                    tbNumberDeliveryNote.Text = dataRow.NumberDeliveryNote;
                    tbDateDeliveryNote.Text = dataRow.DateDeliveryNote.ToString();
                    cbContract.SelectedValue = dataRow.ContractId;
                }
            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
        }

        private async void cbContract_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Contract> contracts = new List<Contract>();
                List<Contract> contracts1 = new List<Contract>();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(App.ip + "Contracts"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();

                            contracts = JsonConvert.DeserializeObject<List<Contract>>(apiResponse);

                            DataTable dataTable = new DataTable();
                            dataTable = CreateDataTable(contracts, 2);
                            contracts1 = ConvertDataTable<Contract>(dataTable);
                            cbContract.ItemsSource = contracts1;

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

        private void dtProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dtProduct.Items.Count != 0 && dtProduct.SelectedItems.Count != 0)
                {
                    Product dataRow = (Product)dtProduct.SelectedItems[0];
                    ID = (int)dataRow.IdProduct;
                    tbNameBook.Text = dataRow.NameBook;
                    tbAuthor.Text = dataRow.Author;
                    tbCount.Text = dataRow.Count.ToString();
                    tbArticle.Text = dataRow.ArticleProduct;
                    cbCategory.SelectedValue = dataRow.CategoryId;
                    tbSeries.Text = dataRow.Series;
                    tbType.Text = dataRow.CoverType;
                    tbYearPublish.Text = dataRow.YearOfPublication;
                    tbHousePublish.Text = dataRow.PublishingHouse;
                    tbAge.Text = dataRow.AgeRestriction;
                    tbPhoto.Text = dataRow.PhotoBook;
                    tbCountPage.Text = dataRow.NumberOfPages.ToString();
                    tbPrice.Text = dataRow.PriceBook.ToString();
                    tbAnnotation.Text = dataRow.Annotation;
                }
            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
        }

        private void btNewProduct_Click(object sender, RoutedEventArgs e)
        {
            gridMain.Visibility = Visibility.Hidden;
            gridNewUpdate.Visibility = Visibility.Visible;
        }

        private void btUpdateProduct_Click(object sender, RoutedEventArgs e)
        {
            gridMain.Visibility = Visibility.Hidden;
            gridNewUpdate.Visibility = Visibility.Visible;
        }

        private async void btDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Product product = new Product();
                Product row = (Product)dtProduct.SelectedItem;
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.DeleteAsync(App.ip + "Products/" + row.IdProduct))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                    }
                }
                tbNameBook.Text = null;
                tbAuthor.Text = null;
                tbCount.Text = null;
                cbCategory.SelectedItem = null;
                tbSeries.Text = null;
                tbType.Text = null;
                tbYearPublish.Text = null;
                tbHousePublish.Text = null;
                tbAge.Text = null;
                tbPhoto.Text = null;
                tbCountPage.Text = null;
                tbPrice.Text = null;
                tbAnnotation.Text = null;
            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
        }

        private async void btFillProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Product> products = new List<Product>();
                List<Product> product1 = new List<Product>();
                int ID1;
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(App.ip + "Products"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        products = JsonConvert.DeserializeObject<List<Product>>(apiResponse);

                        if(products != null)
                        {
                            for (int i = 0; i < products.Count; i++)
                            {
                                ID1 = products[i].CategoryId.Value;
                                Category category = new Category();
                                using (var httpClient2 = new HttpClient())
                                {
                                    using (var response2 = await httpClient2.GetAsync(App.ip + "Categories/" + ID1))
                                    {
                                        if (response2.IsSuccessStatusCode)
                                        {
                                            string apiResponse2 = await response2.Content.ReadAsStringAsync();

                                            category = JsonConvert.DeserializeObject<Category>(apiResponse2);
                                            products[i].Category = category.NameCategory;
                                        }
                                    }
                                }
                                DataTable dataTable = new DataTable();
                                dataTable = CreateDataTable(products, 15);
                                product1 = ConvertDataTable<Product>(dataTable);
                            }
                            dtProduct.ItemsSource = product1;

                        }

                    }
                }

            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
        }

        private async void btFillWarehouse_Click(object sender, RoutedEventArgs e)

        {
            try 
            {
                List<Warehouse> warehouses = new List<Warehouse>();
                List<Warehouse> warehouse1 = new List<Warehouse>();

                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(App.ip + "Warehouses"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        warehouses = JsonConvert.DeserializeObject<List<Warehouse>>(apiResponse);


                        DataTable dataTable = new DataTable();
                        dataTable = CreateDataTable(warehouses, 3);
                        warehouse1 = ConvertDataTable<Warehouse>(dataTable);

                        dtWarehouse.ItemsSource = warehouse1;

                    }
                }
            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
        }

        private void dtWarehouse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
        }

        private async void btNewProducts_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(tbNameBook.Text))
                {
                    if(!string.IsNullOrEmpty(tbAuthor.Text))
                    {
                        if(!string.IsNullOrEmpty(tbCount.Text))
                        {
                            if (cbCategory.SelectedValue != null)
                            {
                                if(!string.IsNullOrEmpty(tbSeries.Text))
                                {
                                    if(!string.IsNullOrEmpty(tbType.Text))
                                    {
                                        if(!string.IsNullOrEmpty(tbYearPublish.Text))
                                        {
                                            if(!string.IsNullOrEmpty(tbHousePublish.Text))
                                            {
                                                if(!string.IsNullOrEmpty(tbAge.Text))
                                                {
                                                    if(!string.IsNullOrEmpty(tbPhoto.Text))
                                                    {
                                                        if(!string.IsNullOrEmpty(tbCountPage.Text))
                                                        {
                                                            if(!string.IsNullOrEmpty(tbPrice.Text))
                                                            {
                                                                if(!string.IsNullOrEmpty(tbAnnotation.Text))
                                                                {
                                                                    Random rnd = new Random();
                                                                    int number = rnd.Next(100000, 999999);
                                                                    Product product = new Product();
                                                                    product.NameBook = tbNameBook.Text;
                                                                    product.Author = tbAuthor.Text;
                                                                    product.Count = int.Parse(tbCount.Text);
                                                                    product.CategoryId = Int32.Parse(cbCategory.SelectedValue.ToString());
                                                                    product.ArticleProduct = number.ToString();
                                                                    product.Series = tbSeries.Text;
                                                                    product.CoverType = tbType.Text;
                                                                    product.YearOfPublication = tbYearPublish.Text;
                                                                    product.PublishingHouse = tbHousePublish.Text;
                                                                    product.AgeRestriction = tbAge.Text;
                                                                    product.PhotoBook = tbPhoto.Text;
                                                                    product.NumberOfPages = int.Parse(tbCountPage.Text);
                                                                    product.PriceBook = decimal.Parse(tbPrice.Text);
                                                                    product.Annotation = tbAnnotation.Text;
                                                                    product.IsDeleted = 0;
                                                                    Product product1 = new Product();
                                                                    using (var httpClient = new HttpClient())
                                                                    {
                                                                        StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
                                                                        using (var response = await httpClient.PostAsync(App.ip + "Products", content))
                                                                        {
                                                                            string apiResponse = await response.Content.ReadAsStringAsync();
                                                                            product1 = JsonConvert.DeserializeObject<Product>(apiResponse);
                                                                        }

                                                                    }
                                                                    tbNameBook.Text = null;
                                                                    tbAuthor.Text = null;
                                                                    tbCount.Text = null;
                                                                    cbCategory.SelectedItem = null;
                                                                    tbSeries.Text = null;
                                                                    tbType.Text = null;
                                                                    tbYearPublish.Text = null;
                                                                    tbHousePublish.Text = null;
                                                                    tbAge.Text = null;
                                                                    tbPhoto.Text = null;
                                                                    tbCountPage.Text = null;
                                                                    tbPrice.Text = null;
                                                                    tbAnnotation.Text = null;

                                                                    gridNewUpdate.Visibility = Visibility.Hidden;
                                                                    gridMain.Visibility = Visibility.Visible;
                                                                }
                                                                else
                                                                {
                                                                    MessageBox.Show("Поле 'Аннотация' должно быть заполнено");
                                                                    tbAnnotation.Focus();
                                                                }
                                                            }
                                                            else
                                                            {
                                                                MessageBox.Show("Поле 'Стоимость' должно быть заполнено");
                                                                tbPrice.Focus();
                                                            }
                                                        }
                                                        else
                                                        {
                                                            MessageBox.Show("Поле 'Количество страниц' должно быть заполнено");
                                                            tbCountPage.Focus();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("Поле 'Фото' должно быть заполнено");
                                                        tbPhoto.Focus();
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show("Поле 'Возраст' должно быть заполнено");
                                                    tbAge.Focus();
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show("Поле 'Издательство' должно быть заполнено");
                                                tbHousePublish.Focus();
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Поле 'Год публикации' должно быть заполнено");
                                            tbYearPublish.Focus();
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Поле 'Тип обложки' должно быть заполнено");
                                        tbType.Focus();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Поле 'Серия' должно быть заполнено");
                                    tbSeries.Focus();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Поле 'Категория' должно быть заполнено");
                                cbCategory.Focus();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Поле 'Количество' должно быть заполнено");
                            tbCount.Focus();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Поле 'Автор' должно быть заполнено");
                        tbAuthor.Focus();
                    }
                    
                }
                else
                {
                    MessageBox.Show("Поле 'Название книги' должно быть заполнено");
                    tbNameBook.Focus();
                }

            }
            catch
            {

            }
            
        }

        private async void btUpdateProducts_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(tbNameBook.Text))
                {
                    if (!string.IsNullOrEmpty(tbAuthor.Text))
                    {
                        if (!string.IsNullOrEmpty(tbCount.Text))
                        {
                            if (cbCategory.SelectedValue != null)
                            {
                                if (!string.IsNullOrEmpty(tbSeries.Text))
                                {
                                    if (!string.IsNullOrEmpty(tbType.Text))
                                    {
                                        if (!string.IsNullOrEmpty(tbYearPublish.Text))
                                        {
                                            if (!string.IsNullOrEmpty(tbHousePublish.Text))
                                            {
                                                if (!string.IsNullOrEmpty(tbAge.Text))
                                                {
                                                    if (!string.IsNullOrEmpty(tbPhoto.Text))
                                                    {
                                                        if (!string.IsNullOrEmpty(tbCountPage.Text))
                                                        {
                                                            if (!string.IsNullOrEmpty(tbPrice.Text))
                                                            {
                                                                if (!string.IsNullOrEmpty(tbAnnotation.Text))
                                                                {
                                                                    
                                                                    Product product = new Product();
                                                                    product.IdProduct = ID;
                                                                    product.NameBook = tbNameBook.Text;
                                                                    product.Author = tbAuthor.Text;
                                                                    product.Count = int.Parse(tbCount.Text);
                                                                    product.CategoryId = Int32.Parse(cbCategory.SelectedValue.ToString());
                                                                    product.ArticleProduct = tbArticle.Text;
                                                                    product.Series = tbSeries.Text;
                                                                    product.CoverType = tbType.Text;
                                                                    product.YearOfPublication = tbYearPublish.Text;
                                                                    product.PublishingHouse = tbHousePublish.Text;
                                                                    product.AgeRestriction = tbAge.Text;
                                                                    product.PhotoBook = tbPhoto.Text;
                                                                    product.NumberOfPages = int.Parse(tbCountPage.Text);
                                                                    product.PriceBook = decimal.Parse(tbPrice.Text);
                                                                    product.Annotation = tbAnnotation.Text;
                                                                    product.IsDeleted = 0;
                                                                    Product product1 = new Product();
                                                                    using (var httpClient = new HttpClient())
                                                                    {
                                                                        StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
                                                                        using (var response = await httpClient.PutAsync(App.ip + "Products/" + ID, content))
                                                                        {
                                                                            string apiResponse = await response.Content.ReadAsStringAsync();
                                                                            product1 = JsonConvert.DeserializeObject<Product>(apiResponse);


                                                                        }

                                                                    }
                                                                    tbNameBook.Text = null;
                                                                    tbAuthor.Text = null;
                                                                    tbCount.Text = null;
                                                                    cbCategory.SelectedItem = null;
                                                                    tbSeries.Text = null;
                                                                    tbType.Text = null;
                                                                    tbYearPublish.Text = null;
                                                                    tbHousePublish.Text = null;
                                                                    tbAge.Text = null;
                                                                    tbPhoto.Text = null;
                                                                    tbCountPage.Text = null;
                                                                    tbPrice.Text = null;
                                                                    tbAnnotation.Text = null;

                                                                    gridNewUpdate.Visibility = Visibility.Hidden;
                                                                    gridMain.Visibility = Visibility.Visible;
                                                                }
                                                                else
                                                                {
                                                                    MessageBox.Show("Поле 'Аннотация' должно быть заполнено");
                                                                    tbAnnotation.Focus();
                                                                }
                                                            }
                                                            else
                                                            {
                                                                MessageBox.Show("Поле 'Стоимость' должно быть заполнено");
                                                                tbPrice.Focus();
                                                            }
                                                        }
                                                        else
                                                        {
                                                            MessageBox.Show("Поле 'Количество страниц' должно быть заполнено");
                                                            tbCountPage.Focus();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("Поле 'Фото' должно быть заполнено");
                                                        tbPhoto.Focus();
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show("Поле 'Возраст' должно быть заполнено");
                                                    tbAge.Focus();
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show("Поле 'Издательство' должно быть заполнено");
                                                tbHousePublish.Focus();
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Поле 'Год публикации' должно быть заполнено");
                                            tbYearPublish.Focus();
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Поле 'Тип обложки' должно быть заполнено");
                                        tbType.Focus();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Поле 'Серия' должно быть заполнено");
                                    tbSeries.Focus();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Поле 'Категория' должно быть заполнено");
                                cbCategory.Focus();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Поле 'Количество' должно быть заполнено");
                            tbCount.Focus();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Поле 'Автор' должно быть заполнено");
                        tbAuthor.Focus();
                    }

                }
                else
                {
                    MessageBox.Show("Поле 'Название книги' должно быть заполнено");
                    tbNameBook.Focus();
                }

            }
            catch
            {

            }
            
        }

        private void tbPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9+\.]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tbCountPage_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tbAge_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9+\+]");
            e.Handled = regex.IsMatch(e.Text);
        }


        private void tbYearPublish_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tbCount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private async void cbCategory_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Category> categories = new List<Category>();
                List<Category> categories1 = new List<Category>();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(App.ip + "Categories"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();

                            categories = JsonConvert.DeserializeObject<List<Category>>(apiResponse);

                            DataTable dataTable = new DataTable();
                            dataTable = CreateDataTable(categories, 1);
                            categories1 = ConvertDataTable<Category>(dataTable);
                            cbCategory.ItemsSource = categories1;

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

        private void btNewSupply_Click(object sender, RoutedEventArgs e)
        {
            gridMainSupply.Visibility = Visibility.Hidden;
            gridSupply.Visibility = Visibility.Visible;
        }

        private void btUpdateSupply_Click(object sender, RoutedEventArgs e)
        {
            gridMainSupply.Visibility = Visibility.Hidden;
            gridSupply.Visibility = Visibility.Visible;
        }

        private async void btDeleteSupply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Supply supply = new Supply();
                Supply row = (Supply)dtSupply.SelectedItem;
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(supply), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.DeleteAsync(App.ip + "Supplies/" + row.IdSupply))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                    }
                }
                tbNumberSupply.Text = null;
                dpSuplly.Text = null;
                cbDeliveryNote.SelectedItem = null;
                cbWarehouse.SelectedItem = null;

            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
        }

        private async void btFillSupply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int ID;
                int ID1;
                List<Supply> supplies = new List<Supply>();
                List<Supply> supplies1 = new List<Supply>();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(App.ip + "Supplies"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();

                            supplies = JsonConvert.DeserializeObject<List<Supply>>(apiResponse);
                            if (supplies != null)
                            {
                                for (int i = 0; i < supplies.Count; i++)
                                {
                                    ID = supplies[i].WarehouseId.Value;
                                    Warehouse warehouse = new Warehouse();
                                    using (var httpClient1 = new HttpClient())
                                    {
                                        using (var response1 = await httpClient1.GetAsync(App.ip + "Warehouses/" + ID))
                                        {
                                            if (response1.IsSuccessStatusCode)
                                            {
                                                string apiResponse1 = await response1.Content.ReadAsStringAsync();

                                                warehouse = JsonConvert.DeserializeObject<Warehouse>(apiResponse1);
                                                supplies[i].NumberWarehouse = warehouse.NumberWarehouse;
                                                supplies[i].Address = warehouse.Address;

                                                ID1 = supplies[i].DeliveryNoteId.Value;
                                                DeliveryNote deliveryNote = new DeliveryNote();
                                                using (var httpClient2 = new HttpClient())
                                                {
                                                    using (var response2 = await httpClient2.GetAsync(App.ip + "DeliveryNotes/" + ID1))
                                                    {
                                                        if (response2.IsSuccessStatusCode)
                                                        {
                                                            string apiResponse2 = await response2.Content.ReadAsStringAsync();

                                                            deliveryNote = JsonConvert.DeserializeObject<DeliveryNote>(apiResponse2);
                                                            supplies[i].NumberDeliveryNote = deliveryNote.NumberDeliveryNote;
                                                            supplies[i].DateDeliveryNote = deliveryNote.DateDeliveryNote;

                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    
                                    DataTable dataTable = new DataTable();
                                    dataTable = CreateDataTable(supplies, 5);
                                    supplies1 = ConvertDataTable<Supply>(dataTable);

                                }

                                dtSupply.ItemsSource = supplies1;
                            }

                        }

                    }
                }
            }
            catch (Exception ex) { }
        }

        private void dtSupply_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dtSupply.Items.Count != 0 && dtSupply.SelectedItems.Count != 0)
                {
                    Supply dataRow = (Supply)dtSupply.SelectedItems[0];
                    ID = (int)dataRow.IdSupply;
                    tbNumberSupply.Text = dataRow.NumberSupply;
                    dpSuplly.Text = dataRow.DateSupply.ToString();
                    cbDeliveryNote.SelectedValue = dataRow.DeliveryNoteId;
                    cbWarehouse.SelectedValue = dataRow.WarehouseId;
                }
            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
        }

        private async void btNewSupplies_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(tbNumberSupply.Text))
                {
                    if (cbDeliveryNote.SelectedValue != null)
                    {
                        if(!string.IsNullOrEmpty(dpSuplly.Text))
                        {
                            if(cbWarehouse.SelectedValue != null)
                            {
                                Supply supply = new Supply();
                                supply.NumberSupply = tbNumberSupply.Text;
                                supply.DateSupply = DateTime.Parse(dpSuplly.Text);
                                supply.DeliveryNoteId = Int32.Parse(cbDeliveryNote.SelectedValue.ToString());
                                supply.WarehouseId = Int32.Parse(cbWarehouse.SelectedValue.ToString());
                                supply.IsDeleted = 0;
                                Supply supply1 = new Supply();
                                using (var httpClient = new HttpClient())
                                {
                                    StringContent content = new StringContent(JsonConvert.SerializeObject(supply), Encoding.UTF8, "application/json");
                                    using (var response = await httpClient.PostAsync(App.ip + "Supplies", content))
                                    {
                                        string apiResponse = await response.Content.ReadAsStringAsync();
                                        supply1 = JsonConvert.DeserializeObject<Supply>(apiResponse);
                                    }

                                }
                                tbNumberSupply.Text = null;
                                dpSuplly.Text = null;
                                cbWarehouse.SelectedItem = null;
                                cbDeliveryNote.SelectedItem = null;
                                gridMainSupply.Visibility = Visibility.Visible;
                                gridSupply.Visibility = Visibility.Hidden;
                            }
                            else
                            {
                                MessageBox.Show("Поле 'Номер склада' должно быть заполнено!");
                                cbWarehouse.Focus();
                            }

                        }
                        else
                        {
                            MessageBox.Show("Поле 'Дата поставки' должно быть заполнено!");
                            dpSuplly.Focus();
                        }

                    }
                    else
                    {
                        MessageBox.Show("Поле 'Номер накладной' должно быть заполнено!");
                        cbDeliveryNote.Focus();
                    }

                }
                else
                {
                    MessageBox.Show("Поле 'Номер поставки' должно быть заполнено!");
                    tbNumberSupply.Focus();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
        }

        private async void btUpdateSupplies_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(tbNumberSupply.Text))
                {
                    if (cbDeliveryNote.SelectedValue != null)
                    {
                        if (!string.IsNullOrEmpty(dpSuplly.Text))
                        {
                            if (cbWarehouse.SelectedValue != null)
                            {
                                Supply supply = new Supply();
                                supply.IdSupply = ID;
                                supply.NumberSupply = tbNumberSupply.Text;
                                supply.DateSupply = DateTime.Parse(dpSuplly.Text);
                                supply.DeliveryNoteId = Int32.Parse(cbDeliveryNote.SelectedValue.ToString());
                                supply.WarehouseId = Int32.Parse(cbWarehouse.SelectedValue.ToString());
                                supply.IsDeleted = 0;
                                Supply supply1 = new Supply();
                                using (var httpClient = new HttpClient())
                                {
                                    StringContent content = new StringContent(JsonConvert.SerializeObject(supply), Encoding.UTF8, "application/json");
                                    using (var response = await httpClient.PutAsync(App.ip + "Supplies/" + ID, content))
                                    {
                                        string apiResponse = await response.Content.ReadAsStringAsync();
                                        supply1 = JsonConvert.DeserializeObject<Supply>(apiResponse);


                                    }

                                }
                                tbNumberSupply.Text = null;
                                dpSuplly.Text = null;
                                cbWarehouse.SelectedItem = null;
                                cbDeliveryNote.SelectedItem = null;
                                gridMainSupply.Visibility = Visibility.Visible;
                                gridSupply.Visibility = Visibility.Hidden;
                            }
                            else
                            {
                                MessageBox.Show("Поле 'Номер склада' должно быть заполнено!");
                                cbWarehouse.Focus();
                            }

                        }
                        else
                        {
                            MessageBox.Show("Поле 'Дата поставки' должно быть заполнено!");
                            dpSuplly.Focus();
                        }

                    }
                    else
                    {
                        MessageBox.Show("Поле 'Номер накладной' должно быть заполнено!");
                        cbDeliveryNote.Focus();
                    }

                }
                else
                {
                    MessageBox.Show("Поле 'Номер поставки' должно быть заполнено!");
                    tbNumberSupply.Focus();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
        }

        private async void cbDeliveryNote_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<DeliveryNote> deliverynotes = new List<DeliveryNote>();
                List<DeliveryNote> deliverynote1 = new List<DeliveryNote>();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(App.ip + "DeliveryNotes"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();

                            deliverynotes = JsonConvert.DeserializeObject<List<DeliveryNote>>(apiResponse);

                            DataTable dataTable = new DataTable();
                            dataTable = CreateDataTable(deliverynotes, 2);
                            deliverynote1 = ConvertDataTable<DeliveryNote>(dataTable);
                            cbDeliveryNote.ItemsSource = deliverynote1;

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

        private async void cbWarehouse_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Warehouse> warehouses = new List<Warehouse>();
                List<Warehouse> warehouse1 = new List<Warehouse>();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(App.ip + "Warehouses"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();

                            warehouses = JsonConvert.DeserializeObject<List<Warehouse>>(apiResponse);

                            DataTable dataTable = new DataTable();
                            dataTable = CreateDataTable(warehouses, 2);
                            warehouse1 = ConvertDataTable<Warehouse>(dataTable);
                            cbWarehouse.ItemsSource = warehouse1;

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

        private void tbNumberSupply_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void dtSupplyView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
               
            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
        }

        private async void btFillView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               SupplyView views = new SupplyView();
               SupplyView views1 = new SupplyView();

                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(App.ip + "View/ViewSupply"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        views = JsonConvert.DeserializeObject<SupplyView>(apiResponse);

                        

                    }
                }

                List<SupplyView> viewList = new List<SupplyView> { views };


                dtSupplyView.ItemsSource = viewList;
            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
        }

        private void dtSupplyComposition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dtSupplyComposition.Items.Count != 0 && dtSupplyComposition.SelectedItems.Count != 0)
                {
                    SupplyComposition dataRow = (SupplyComposition)dtSupplyComposition.SelectedItems[0];
                    ID = (int)dataRow.IdSupplyComposition;
                    cbNameProductSupply.SelectedValue = dataRow.ProductId;
                    cbNumberSupply.SelectedValue = dataRow.SupplyId;
                    tbCountSupply.Text = dataRow.CountSupply.ToString();
                    tbPriceSupply.Text = dataRow.PriceSupply.ToString();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }

        }

        private void btNewSupplyComposition_Click(object sender, RoutedEventArgs e)
        {
            gridMainSupplyComposition.Visibility = Visibility.Hidden;
            gridSupplyComposition.Visibility = Visibility.Visible;
        }

        private void btUpdateSupplyComposition_Click(object sender, RoutedEventArgs e)
        {
            gridMainSupplyComposition.Visibility = Visibility.Hidden;
            gridSupplyComposition.Visibility = Visibility.Visible;
        }

        private async void btDeleteSupplyComposition_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SupplyComposition supplycomposition = new SupplyComposition();
                SupplyComposition row = (SupplyComposition)dtSupplyComposition.SelectedItem;
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(supplycomposition), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.DeleteAsync(App.ip + "SupplyCompositions/" + row.IdSupplyComposition))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                    }
                }
                cbNameProductSupply.SelectedItem = null;
                cbNumberSupply.SelectedItem = null;
                tbCountSupply.Text = null;
                tbPriceSupply.Text = null;

            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
        }

        private async void btFillSupplyComposition_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               SupplyCompositionView views = new SupplyCompositionView();
               SupplyCompositionView views1 = new SupplyCompositionView();

                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(App.ip + "SupplyCompositionView/ViewSupplyComposition"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        views = JsonConvert.DeserializeObject<SupplyCompositionView>(apiResponse);
                        

                    }
                }
                
                List<SupplyCompositionView> viewList = new List<SupplyCompositionView> { views };

                dtSupplyComposition.ItemsSource = viewList;
            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
            
        }

        private async void btNewSupplyCompositions_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (cbNameProductSupply.SelectedValue != null)
                {
                    if (cbNumberSupply.SelectedValue != null)
                    {
                        if (!string.IsNullOrEmpty(tbCountSupply.Text))
                        {
                            if (!string.IsNullOrEmpty(tbPriceSupply.Text))
                            {
                                SupplyComposition supplyComposition = new SupplyComposition();
                                supplyComposition.ProductId = Int32.Parse(cbNameProductSupply.SelectedValue.ToString());
                                supplyComposition.SupplyId = Int32.Parse(cbNumberSupply.SelectedValue.ToString());
                                supplyComposition.CountSupply = int.Parse(tbCountSupply.Text);
                                supplyComposition.PriceSupply = int.Parse(tbPriceSupply.Text);
                                supplyComposition.IsDeleted = 0;
                                SupplyComposition supplyComposition1 = new SupplyComposition();
                                using (var httpClient = new HttpClient())
                                {
                                    StringContent content = new StringContent(JsonConvert.SerializeObject(supplyComposition), Encoding.UTF8, "application/json");
                                    using (var response = await httpClient.PostAsync(App.ip + "SupplyCompositions", content))
                                    {
                                        string apiResponse = await response.Content.ReadAsStringAsync();
                                        supplyComposition1 = JsonConvert.DeserializeObject<SupplyComposition>(apiResponse);
                                    }

                                }
                                tbCountSupply.Text = null;
                                tbPriceSupply.Text = null;
                                cbNameProductSupply.SelectedItem = null;
                                cbNumberSupply.SelectedItem = null;
                                gridMainSupplyComposition.Visibility = Visibility.Visible;
                                gridSupplyComposition.Visibility = Visibility.Hidden;
                            }
                            else
                            {
                                MessageBox.Show("Поле 'Стоимость товара' должно быть заполнено!");
                                tbPriceSupply.Focus();
                            }

                        }
                        else
                        {
                            MessageBox.Show("Поле 'Количество товара' должно быть заполнено!");
                            tbCountSupply.Focus();
                        }

                    }
                    else
                    {
                        MessageBox.Show("Поле 'Номер поставки' должно быть заполнено!");
                        cbNumberSupply.Focus();
                    }

                }
                else
                {
                    MessageBox.Show("Поле 'Название товара' должно быть заполнено!");
                    cbNameProductSupply.Focus();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
            
        }

        private async void btUpdateSupplyCompositions_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbNameProductSupply.SelectedValue != null)
                {
                    if (cbNumberSupply.SelectedValue != null)
                    {
                        if (!string.IsNullOrEmpty(tbCountSupply.Text))
                        {
                            if (!string.IsNullOrEmpty(tbPriceSupply.Text))
                            {
                                SupplyComposition supplyComposition = new SupplyComposition();
                                supplyComposition.IdSupplyComposition = ID;
                                supplyComposition.ProductId = Int32.Parse(cbNameProductSupply.SelectedValue.ToString());
                                supplyComposition.SupplyId = Int32.Parse(cbNumberSupply.SelectedValue.ToString());
                                supplyComposition.CountSupply = int.Parse(tbCountSupply.Text);
                                supplyComposition.PriceSupply = int.Parse(tbPriceSupply.Text);
                                supplyComposition.IsDeleted = 0;
                                SupplyComposition supplyComposition1 = new SupplyComposition();
                                using (var httpClient = new HttpClient())
                                {
                                    StringContent content = new StringContent(JsonConvert.SerializeObject(supplyComposition), Encoding.UTF8, "application/json");
                                    using (var response = await httpClient.PutAsync(App.ip + "SupplyCompositions/" + ID, content))
                                    {
                                        string apiResponse = await response.Content.ReadAsStringAsync();
                                        supplyComposition1 = JsonConvert.DeserializeObject<SupplyComposition>(apiResponse);
                                    }

                                }
                                tbCountSupply.Text = null;
                                tbPriceSupply.Text = null;
                                cbNameProductSupply.SelectedItem = null;
                                cbNumberSupply.SelectedItem = null;
                                gridMainSupplyComposition.Visibility = Visibility.Visible;
                                gridSupplyComposition.Visibility = Visibility.Hidden;
                            }
                            else
                            {
                                MessageBox.Show("Поле 'Стоимость товара' должно быть заполнено!");
                                tbPriceSupply.Focus();
                            }

                        }
                        else
                        {
                            MessageBox.Show("Поле 'Количество товара' должно быть заполнено!");
                            tbCountSupply.Focus();
                        }

                    }
                    else
                    {
                        MessageBox.Show("Поле 'Номер поставки' должно быть заполнено!");
                        cbNumberSupply.Focus();
                    }

                }
                else
                {
                    MessageBox.Show("Поле 'Название товара' должно быть заполнено!");
                    cbNameProductSupply.Focus();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка!");
            }
        }

        private async void cbNameProductSupply_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Product> products = new List<Product>();
                List<Product> product1 = new List<Product>();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(App.ip + "Products"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();

                            products = JsonConvert.DeserializeObject<List<Product>>(apiResponse);
                            List<Product> product2 = products.Where(n => n.IsDeleted == 0).ToList();

                            DataTable dataTable = new DataTable();
                            dataTable = CreateDataTable(product2, 14);
                            product1 = ConvertDataTable<Product>(dataTable);
                            cbNameProductSupply.ItemsSource = product1;

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

        private async void cpNumberSupply_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Supply> supplys = new List<Supply>();
                List<Supply> supply1 = new List<Supply>();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(App.ip + "Supplies"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();

                            supplys = JsonConvert.DeserializeObject<List<Supply>>(apiResponse);

                            DataTable dataTable = new DataTable();
                            dataTable = CreateDataTable(supplys, 4);
                            supply1 = ConvertDataTable<Supply>(dataTable);
                            cbNumberSupply.ItemsSource = supply1;

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

        private void tbPriceSupply_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tbCountSupply_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}

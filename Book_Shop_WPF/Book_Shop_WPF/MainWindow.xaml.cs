using Book_Shop_WPF.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Book_Shop_WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        bool auth = false;

        private async void btAutorization_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tbEmail.Text != string.Empty)
                {
                    if (tbPassword.Password != string.Empty)
                    {
                        User user = new User()
                        {
                            EmailUser = tbEmail.Text,
                            PasswordUser = tbPassword.Password.ToString()
                        };
                        using (var httpClient = new HttpClient())
                        {
                            StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                            using (var response = await httpClient.PostAsync(App.ip + "Users/Autorization", content))
                            {
                                string apiResponse = await response.Content.ReadAsStringAsync();
                                user = JsonConvert.DeserializeObject<User>(apiResponse);

                                try
                                {
                                    if (response.IsSuccessStatusCode)
                                    {
                                        string apiRes = await response.Content.ReadAsStringAsync();
                                        Token.token = apiRes;
                                        dynamic data = JObject.Parse(Token.token);
                                        string role = data.role;
                                        App.ID = data.id;
                                        try
                                        {
                                            if (role == "1")
                                            {

                                                AdminWindow admin = new AdminWindow();
                                                admin.Show();
                                                auth = true;
                                                Close();
                                            }
                                            else if (role == "2")
                                            {
                                                LichKab lichKab = new LichKab();
                                                lichKab.Show();
                                                auth = true;
                                                Close();
                                            }
                                            else if (role == "3")
                                            {
                                                WarehouseEmployeeWindow warehouseEmployeeWindow = new WarehouseEmployeeWindow();
                                                warehouseEmployeeWindow.Show();
                                                auth = true;
                                                Close();

                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show("Не верный логин или пароль!");
                                        }

                                    }
                                    else
                                    {
                                        MessageBox.Show("Не верный логин или пароль!");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Отсутствует подключение!!");
                                }
                            }

                        }
                    }
                    else
                    {
                        MessageBox.Show("Заполните данное поле!");
                        tbPassword.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Заполните данное поле!");
                    tbEmail.Focus();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка!!!");
            }
           
        }

        private void btRegistration_Click(object sender, RoutedEventArgs e)
        {
            Registration registration = new Registration();
            registration.Show();
            auth = true;
            Close();
        }

        private void btmay_Click(object sender, RoutedEventArgs e)
        {

            Catalog catalog = new Catalog();
            catalog.Show();
            auth = true;
            Close();
            //OneProduct oneProduct = new OneProduct();   
            //oneProduct.Show();

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
    }
}

using Book_Shop_WPF.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }
        bool auth = false;
        private void btAutorization_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
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


        public bool IsValidPassword(string Password)
        {
            Regex regex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?!.*\\blogin\\b)\\w{5,20}$");
            if (regex.IsMatch(Password))
            {
                return true;
            }
            if (Password.Contains(tbEmail.Text))
            {
                return false;
            }
            else
            {
                return false;
            }
        }

        private async void btRegistration_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbSurname.Text))
            {
                if (!string.IsNullOrEmpty(tbName.Text))
                {
                    if (!string.IsNullOrEmpty(tbMiddleName.Text))
                    {
                        if (!string.IsNullOrEmpty(tbEmail.Text))
                        {
                            if (!string.IsNullOrEmpty(tbPassword.Text))
                            {
                                if (!string.IsNullOrEmpty(dtDateBirth.Text))
                                {
                                    if (IsValidPassword(tbPassword.Text))
                                    {
                                        User user = new User();
                                        user.SurnameUser = tbSurname.Text;
                                        user.NameUser = tbName.Text;
                                        user.MiddleNameUser = tbMiddleName.Text;
                                        user.EmailUser = tbEmail.Text;
                                        user.DateBirthUser = DateTime.Parse(dtDateBirth.Text);
                                        user.PasswordUser = tbPassword.Text;
                                        user.RoleId = 2;
                                        user.IsDeleted = 0;

                                        User user1 = new User();
                                        using (var httpClient = new HttpClient())
                                        {
                                            StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                                            using (var response = await httpClient.PostAsync(App.ip + "Users", content))
                                            {
                                                string apiResponse = await response.Content.ReadAsStringAsync();
                                                user1 = JsonConvert.DeserializeObject<User>(apiResponse);
                                                try
                                                {
                                                    if (response.IsSuccessStatusCode)
                                                    {
                                                        tbSurname.Text = null;
                                                        tbName.Text = null;
                                                        tbMiddleName.Text = null;
                                                        tbEmail.Text = null;
                                                        tbPassword.Text = null;
                                                        dtDateBirth.Text = null;
                                                        MessageBox.Show("Успешная регистрация!");
                                                        MainWindow mainWindow = new MainWindow();
                                                        mainWindow.Show();
                                                        auth = true;
                                                        Close();
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("Не верный логин или пароль!");
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    MessageBox.Show("Отсутствует подключение!");
                                                }

                                            }

                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Пароль не соответствует требованиям.Пароль должен содержать от 5 до 20 символов, и обязательно заглавную и прописную букву");
                                    }
                                    
                                    


                                }
                                else
                                {
                                    MessageBox.Show("Поле 'Дата рождения' должно быть заполнено!");
                                    dtDateBirth.Focus();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Поле 'Пароль' должно быть заполнено!");
                                tbPassword.Focus();
                            }

                        }
                        else
                        {
                            MessageBox.Show("Поле 'Электронная почта' должно быть заполнено!");
                            tbEmail.Focus();
                        }

                    }
                    else
                    {
                        MessageBox.Show("Поле 'Отчество' должно быть заполнено!");
                        tbMiddleName.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Поле 'Имя' должно быть заполнено!");
                    tbName.Focus();
                }
            }
            else
            {
                MessageBox.Show("Поле 'Фамилия' должно быть заполнено!");
                tbSurname.Focus();
            }
        }
    }
}

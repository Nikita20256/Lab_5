using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Lab_5 // Объявление пространства имен для вашего проекта.
{
    public struct MARSH // Определение структуры для хранения данных о маршруте.
    {
        public string StartPoint; // Начальный пункт маршрута.
        public string EndPoint; // Конечный пункт маршрута.
        public string RouteNumber; // Номер маршрута.

        public override string ToString() // Метод для возвращения строкового представления маршрута.
        {
            return $"Маршрут {RouteNumber}: {StartPoint} - {EndPoint}"; // Форматированный вывод данных маршрута.
        }
    }

    public partial class Form1 : Form // Определение класса формы.
    {
        private List<MARSH> marshes = new List<MARSH>(); // Список для хранения маршрутов.
        private List<string> cities = new List<string>
        {
            "Москва", "Санкт-Петербург", "Новосибирск", "Екатеринбург",
            "Казань", "Нижний Новгород", "Челябинск", "Самара",
             "Омск", "Ростов-на-Дону", "Томск", "Кемерово", "Юрга"
        };

        public Form1() // Конструктор класса формы.
        {
            InitializeComponent(); // Инициализация компонентов формы.
        }

        // Метод, вызываемый при нажатии на кнопку "Открыть".
        private void openButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog(); // Создание диалога для открытия файла.
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"; // Установка фильтра для файлов.
            openFileDialog.Title = "Открыть файл маршрутов"; // Установка заголовка диалога.

            if (openFileDialog.ShowDialog() == DialogResult.OK) // Если пользователь выбрал файл и нажал "OK".
            {
                marshes.Clear(); // Очистка списка маршрутов.
                using (StreamReader sr = new StreamReader(openFileDialog.FileName)) // Создание потока чтения из файла.
                {
                    string line; // Переменная для хранения считываемой строки.
                    while ((line = sr.ReadLine()) != null) // Чтение файла построчно.
                    {
                        var parts = line.Split(';'); // Разделение строки на части.
                        if (parts.Length == 3) // Если строка содержит три части.
                        {
                            marshes.Add(new MARSH // Добавление нового маршрута в список.
                            {
                                StartPoint = parts[0],
                                EndPoint = parts[1],
                               RouteNumber = parts[2]
                            });
                        }
                    }
                }

                initTextBox.Text = string.Join(Environment.NewLine, marshes); // Отображение маршрутов в текстовом поле.
            }
        }

        // Метод, вызываемый при нажатии на кнопку "Сгенерировать".
        private void generateButton_Click(object sender, EventArgs e)
        {
            Random random = new Random(); // Создание объекта для генерации случайных чисел.
            marshes.Clear(); // Очистка списка маршрутов.

            for (int i = 0; i < 10; i++) // Генерация десяти маршрутов.
            {
                var startPoint = cities[random.Next(cities.Count)]; // Выбор случайного начального города.
                var endPoint = cities[random.Next(cities.Count)]; // Выбор случайного конечного города.
                while (endPoint == startPoint) // Убедиться, что начальный и конечный города не совпадают.
                {
                    endPoint = cities[random.Next(cities.Count)];
                }

                marshes.Add(new MARSH // Добавление нового маршрута в список.
                {
                    StartPoint = startPoint,
                    EndPoint = endPoint,
                    RouteNumber = (i + 1).ToString()
                });
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog(); // Создание диалога для сохранения файла.
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"; // Установка фильтра для файлов.
            saveFileDialog.Title = "Сохранить сгенерированные данные"; // Установка заголовка диалога.

            if (saveFileDialog.ShowDialog() == DialogResult.OK) // Если пользователь выбрал место для сохранения и нажал "OK".
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName)) // Создание потока записи в файл.
                {
                    foreach (var marsh in marshes) // Перебор всех маршрутов для записи.
                    {
                        sw.WriteLine($"{marsh.StartPoint};{marsh.EndPoint};{marsh.RouteNumber}"); // Запись маршрута в файл.
                    }
                }

                MessageBox.Show("Данные сгенерированы и сохранены."); // Вывод сообщения об успешном сохранении.
            }
        }

        // Метод, вызываемый при нажатии на кнопку "Очистить".
        private void clearButton_Click(object sender, EventArgs e)
        {
            initTextBox.Clear(); // Очистка текстового поля с исходными данными.
            resultTextBox.Clear(); // Очистка текстового поля с результатами.
        }

        // Метод, вызываемый при нажатии на кнопку "Сохранить".
        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog(); // Создание диалога для сохранения файла.
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"; // Установка фильтра для файлов.
            saveFileDialog.Title = "Сохранить результаты"; // Установка заголовка диалога.

            if (saveFileDialog.ShowDialog() == DialogResult.OK) // Если пользователь выбрал место для сохранения и нажал "OK".
            {
                File.WriteAllText(saveFileDialog.FileName, resultTextBox.Text); // Запись содержимого текстового поля в файл.
                MessageBox.Show("Результаты сохранены в файле: " + saveFileDialog.FileName); // Вывод сообщения об успешном сохранении.
            }
        }

        // Метод, вызываемый при нажатии на кнопку "Фильтровать".
        private void filterButton_Click(object sender, EventArgs e)
        {   
            string h1 = filterTextBox.Text; 
            foreach(var marsh in marshes)
            {
                if (h1 == marsh.RouteNumber) { 
                    resultTextBox.Text = marsh.ToString();
                    }
                
                else if (Convert.ToInt32(h1) > 10)
                {
                    string t ="Такого маршрута нет ";
                    resultTextBox.Text = t.ToString();
                }
            }
        }
        private void filterTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void resultTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

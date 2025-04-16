using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using L9_Table;

namespace L9_Table
{
    public class StudentManager
    {
        private List<Student> _students = new List<Student>();
        private string _filePath = "C:\\Users\\ponya\\source\\repos\\WPF_L9\\WPF_L9\\students_list.txt";
        private string save_path = "C:\\Users\\ponya\\source\\repos\\WPF_L9\\WPF_L9\\students_list.txt";
        public StudentManager()
        {
            //CreateTestFile();
            LoadStudents();
        }

        public IEnumerable<object> GetAllStudents() => _students
            .Select(s => new { s.SurName, s.FirstName, s.Patronymic, s.BirthDate, s.Height, s.City, s.BirthYear, s.Age });

        public IEnumerable<object> GetStudentsFromCity(string city) =>
            _students.Where(s => s.City.Equals(city, StringComparison.OrdinalIgnoreCase))
            .Select(s => new { s.SurName, s.FirstName, s.Patronymic, s.BirthDate, s.Height, s.City, s.BirthYear, s.Age });

        public IEnumerable<object> GetAdultStudents() =>
            _students.Where(s => s.Age >= 18)
            .Select(s => new { s.SurName, s.FirstName, s.Patronymic, s.BirthDate, s.Height, s.City, s.BirthYear, s.Age });

        public IEnumerable<object> GetStudentsByBirthYear(int year) =>
            _students.Where(s => s.BirthYear == year)
            .Select(s => new { s.SurName, s.FirstName, s.Patronymic, s.BirthDate, s.Height, s.City, s.BirthYear, s.Age });

        public IEnumerable<object> GetStudentsIntroduction() =>
            _students.Select(s => new { s.SurName, s.FirstName, s.Age, s.City });

        public IEnumerable<object> GetStudentsByDistance()
        {  
            return _students.OrderByDescending(s => s.DistanceFromMoscow).ThenByDescending(s => s.SurName)
                .Select(s => new { s.SurName, s.FirstName, s.Patronymic, s.BirthDate, s.Height, s.City, s.BirthYear, s.Age });
        }

        public void AddStudent(Student student)
        {
            _students.Add(student);
            SaveStudents();
        }

        public void LoadStudents()
        {
            Console.WriteLine($"Ищем файл по пути: {_filePath}"); // Отладка

            if (!File.Exists(_filePath))
            {
                MessageBox.Show($"Файл не найден: {_filePath}");
                return;
            }

            try
            {
                var lines = File.ReadAllLines(_filePath);
                Console.WriteLine($"Найдено строк: {lines.Length}"); // Отладка

                foreach (var line in lines)
                {
                    var data = line.Split(',');
                    if (data.Length != 7) continue;

                    // Парсинг с явным указанием формата даты
                    if (DateTime.TryParseExact(data[3], "MM.dd.yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime birthDate))
                    {
                        _students.Add(new Student
                        {
                            SurName = data[0],
                            FirstName = data[1],
                            Patronymic = data[2],
                            BirthDate = birthDate,
                            Height = double.Parse(data[4]),
                            City = data[5],
                            DistanceFromMoscow = int.Parse(data[6])
                        });
                    }
                    else
                    {
                        Console.WriteLine($"Ошибка формата даты: {data[3]}");
                    }
                }
                Console.WriteLine($"Успешно загружено: {_students.Count} студентов");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}");
            }
        }

        private void SaveStudents()
        {
            var lines = _students.Select(s =>
                $"{s.SurName},{s.FirstName},{s.Patronymic}," +
                $"{s.BirthDate:MM.dd.yyyy},{s.Height},{s.City},{s.DistanceFromMoscow}");
            File.WriteAllLines(save_path, lines);
        }
        //private void CreateTestFile()
        //{
        //    var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "students_list.txt");
        //    if (!File.Exists(path))
        //    {
        //        File.WriteAllText(path,
        //            "Иванов,Иван,Иванович,05.15.2000,1.75,Москва\n" +
        //            "Петров,Петр,Петрович,10.21.1999,1.80,Санкт-Петербург");
        //    }
        //}
    }
}
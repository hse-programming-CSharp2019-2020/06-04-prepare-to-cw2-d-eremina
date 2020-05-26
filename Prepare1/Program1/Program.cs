using System;
using System.IO;
using System.Text;
using Library;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using System.Xml.Serialization;

namespace Program1
{
    class Program
    {
        static  readonly  Random gen = new Random();
        static readonly string alphabetCapital = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        static readonly string alphabet = "abcdefghiklmnopqrstuvwxyz";
        
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            do
            {
                Street[] streetsArray = new Street[0];
                // Для проверки, было ли считывание корректым
                bool flag = false;
                try
                {
                    streetsArray = ReadFile(@"data.txt");
                }
                catch (ArgumentException)
                {
                    flag = true;
                    Console.WriteLine("File has incorrect strings");
                }
                catch (FormatException)
                {
                    flag = true;
                    Console.WriteLine("File has incorrect strings");
                }
                catch (IOException)
                {
                    flag = true;
                    Console.WriteLine("Can't read the file");
                }
                catch (Exception e)
                {
                    flag = true;
                    Console.WriteLine("Error while reading: " + e.Message);
                }

                // Получение значения размера массива объектов
                int N = GetN();

                if (flag)
                {
                    Console.WriteLine("Can't get data from file. Array's objects will be generated");
                    streetsArray = GetStreets(N);
                }
                else
                {
                    // Убрать лишние элементы, если желаемое количество меньше выборки в файле
                    if (streetsArray.Length > N)
                        Array.Resize(ref streetsArray, N);
                }

                // Вывод информации об объектах
                Console.WriteLine("INFO:");
                Console.WriteLine(streetsArray.Select(x => x.ToString()).Aggregate((x, y) => x + "\n" + y));

                // Сериалзиация массива объектов
                try
                {
                    Serialize(streetsArray,
                        Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent + "/out.ser");
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("Can't serialize object: " + e.Message);
                }
                catch (IOException e)
                {
                    Console.WriteLine("File error while serialization: " + e.Message);
                }
                catch (SecurityException)
                {
                    Console.WriteLine("Security exception while serialization");
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine("Error while serialization: can't access. " + e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error while serialization: " + e.Message);
                }

                Console.WriteLine("\tPress ESC to exit. Press any other key to restart.");
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

        }

        static Street[] ReadFile(string path)
        {
            List<Street> streets = new List<Street>();
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs, Encoding.UTF8);
                string line;
                while((line = sr.ReadLine()) != null)
                {
                    string[] input = line.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                    
                    if(input.Length < 2)
                        throw new ArgumentException();

                    string name = input[0];
                    List<string> house = input.ToList();
                    house.RemoveAt(0);
                    int[] houses = house.Select(e => int.Parse(e)).ToArray();
                    streets.Add(new Street(input[0], houses));
                }
            }
            return streets.ToArray();
        }

        /// <summary>
        /// Получает целочисленное неотрицательное значение от пользователя
        /// </summary>
        /// <returns>Введенное значение</returns>
        static int GetN()
        {
            int n;
            Console.WriteLine("Enter N value: ");
            while(!int.TryParse(Console.ReadLine(), out n) || n < 0)
                Console.WriteLine("Wrong input. Enter a positive integer number");
            return n;
        }
        
        /// <summary>
        /// Заполняет масси случайными объектами класса Street
        /// </summary>
        /// <param name="n">Длина массива</param>
        /// <returns>Заполненный массив</returns>
        static Street[] GetStreets(int n)
        {
            Street[] streets = new Street[n];
            for (int p = 0; p < n; ++p)
            {
                // Генерация массива домов
                int size = gen.Next(2, 10);
                int[] houses = new int[size];
                for (int i = 0; i < size; ++i)
                    houses[i] = gen.Next(1, 101);
                
                // Генерация имени
                string name = alphabetCapital[gen.Next(0,  alphabetCapital.Length)].ToString();
                size = gen.Next(2, 22);
                for (int i = 1; i < size; ++i)
                    name += alphabet[gen.Next(0, alphabet.Length)];
                
                streets[p] = new Street(name, houses);
            }
            return streets;
        }

        /// <summary>
        /// Сериализует массив объектов класса Street
        /// </summary>
        /// <param name="objects">Массив объектовв</param>
        /// <param name="path">Путь файла</param>
        static void Serialize(Street[] objects, string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                XmlSerializer sr = new XmlSerializer(typeof(Street[]), new Type[]{typeof(Street)});
                sr.Serialize(fs, objects);
            }
        }
    }
}
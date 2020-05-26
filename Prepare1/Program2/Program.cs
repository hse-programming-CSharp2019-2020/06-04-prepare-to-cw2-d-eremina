using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using System.Xml.Serialization;
using Library;

namespace Program2
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                try
                {
                    Street[] streetsArray =
                        DeseriaLize(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent +
                                    "/out.ser");
                    var selected = from x in streetsArray
                        where x.Houses.Length % 2 == 1 && +x
                        select x;
                    Console.WriteLine("SELECTED INFO:");
                    Console.WriteLine(selected.Select(x => x.ToString()).Aggregate((x, y) => x + "\n" + y));
                }
                catch (IOException e)
                {
                    Console.WriteLine("File error during deserialization: " + e.Message);
                }
                catch (SecurityException e)
                {
                    Console.WriteLine("Security error during deserialization: " + e.Message);
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("Error during deserialization: " + e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error happened: " + e.Message);
                }
                
                Console.WriteLine("\tPress ESC to exit. Press any other key to restart.");
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }

        /// <summary>
        /// Десериализует массив объектов класса Street
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns>Десериализзованный массив</returns>
        static Street[] DeseriaLize(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                XmlSerializer sr = new XmlSerializer(typeof(Street[]),  new Type[]{typeof(Street)});
                return (Street[])sr.Deserialize(fs);
            }
        }
    }
}
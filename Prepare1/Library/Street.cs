using System;
using System.Linq;

namespace Library
{
    [Serializable]
    public class Street
    {
        // Название улицы
        public string name;
        public string Name => name;

        // Номера домов
        public int[] houses;
        public int[] Houses => houses;

        // Для XML сериализации
        public Street() {}
        public Street(string name, int[] houses)
        {
            this.name = name;
            if(!Array.TrueForAll(houses, x => x > 0))
                throw new ArgumentException();
            this.houses = houses;
        }

        // Число домов равно длине массива, содержащего их номера
        public static int operator ~(Street s) => s.Houses.Length;

        // Перегруженный унарный оператор, возвращающий true, если хотя бы один номер дома содержит 7 в названии, иначе false
        public static bool operator +(Street s) =>
            !Array.TrueForAll(s.Houses, i => !i.ToString().ToCharArray().Contains('7'));

        // Строковое представление объектов класса
        public override string ToString() => $"Name: {Name}, Houses: " + houses.Select(t => t.ToString()).
            Aggregate((x, y) => x + " " + y);
    }
}
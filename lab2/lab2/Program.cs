using System;
using lab1classes;


namespace lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            //1st ex
            Edition ex1 = new Edition("BookParidise", new DateTime(2018, 11, 10), 300000);
            Edition ex2 = new Edition("BookParidise", new DateTime(2018, 11, 10), 300000);
            Console.WriteLine($"Инициализируем объекты класса {ex1.GetType()}");
            Console.WriteLine($"Объект 1: {ex1.ToString()}");
            Console.WriteLine($"Объект 2: {ex2.ToString()}");
            unsafe
            {
                ref Edition ref_ex1 = ref ex1;
                ref Edition ref_ex2 = ref ex2;
                Console.WriteLine($"Адрес одинаковый:{ref_ex1 == ref_ex2}");
            }
            Console.WriteLine($"Хэш-коды:\n{ex1.GetHashCode()}\n{ex2.GetHashCode()}");


            //ex2-------------------------------------------------------------
            try
            {
                Console.WriteLine($"Тираж: {ex1.Circulation}");
                ex1.Circulation = 1000;
                Console.WriteLine($"Тираж: {ex1.Circulation}\nПопробуем -20\n\n");
                ex1.Circulation = -20;
            }
            catch (EditionException ex)
            {
                Console.WriteLine("Исключение {0}", ex.Message);
            }


            //ex3-------------------------------------------------------------
            Person p1 = new Person("Святая","Мария", new DateTime(1000,1,1));
            Person p2 = new Person("Антон", "Лавей", new DateTime(666, 6, 6));
            Person p3 = new Person("Гомер","Симспон");
            Person p4 = new Person();

            Article art1 = new Article(p1,"Ешь, молись, лби", 7.3);
            Article art2 = new Article(p2, "Holly Bible 2.0", 9.8);
            Article art3 = new Article(p3, "Пончики, Пончики", 10.0);
            Article art4 = new Article(p1, "Домострой", 3);
            Article art5 = new Article(p4, "no title", 5);

            Magazine magOne = new Magazine("Новое начало", Frequency.Monthly, new DateTime(2018, 11, 11), 1000000);
            magOne.AddArticles(new Article[] { art1, art2, art3, art4, art5 });
            magOne.AddEditors(new Person[] { p1, p2, p3, p4 });
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(magOne.ToString());
            Console.ResetColor();



            //ex4------------------------------------------------------------------
            Console.WriteLine(magOne.EditName);


            //ex5------------------------------------------------------------------
            Magazine magTwo = (Magazine)magOne.DeepCopy();
            magOne.Circulation = 2000000;
            Console.WriteLine("\n\n***Измения в тираже***\n\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(magOne.ToShortString());
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(magTwo.ToShortString());
            Console.ResetColor();

            //ex6-----------------------------------------------------------------
            foreach (Article el in magOne.GetArticles(8.0))
                Console.WriteLine(el.ToString());
            //ex7-----------------------------------------------------------------
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (Article el in magOne.GetArticles("Holly"))
                Console.WriteLine(el.ToString());
            Console.ResetColor();

        }
    }


}

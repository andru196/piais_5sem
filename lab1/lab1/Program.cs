using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    class Program
    {
        public static int stairs(int nbr)
        {
            int i = 0;

            while (nbr >= 0)
            {
                i++;
                nbr -= i;
            }
            return (i);
        }

        public static void adder (Article[][] arts)
        {
            foreach (Article[] elems in arts)
            {
                foreach (Article elem in elems)
                {
                    elem.Author = new Person();
                }
            }
        }
        public static void adder(Article[,] arts)
        {
              foreach (Article elem in arts)
                {
                    elem.Author = new Person();
                }
        }

        static void Main(string[] args)
        {
            // Создаю экземпляр класса и сразу запалняю его статьями методом AddArticles
            Magazine maga1 = new Magazine("КавказСилаWeekly", Frequency.Weekly, new DateTime(1986, 4, 26), 500000);
            maga1.AddArticles(
                new Article(new Person("Мага", "Нага", new DateTime(1900, 1, 31)), "В поисках Аллы", 10.0),
                new Article(new Person(), "Равзал неминуем", 1.1),
                new Article(new Person("Иван", "Иванов"), "Ленина в Грозный!", 8.5),
                new Article()
                );

            Console.WriteLine(maga1.ToShortString()); // Вывод короткого описания со средним рейтингом
            Console.WriteLine($"\n\n\nMonthly: {maga1[Frequency.Monthly]}"); // Вывод Frequency
            Console.WriteLine($"\nWeekly: {maga1[Frequency.Weekly]}");      // Вывод Frequency
            Console.WriteLine($"\nYearly: {maga1[Frequency.Yearly]}\n\n");  // Вывод Frequency
            Console.WriteLine(maga1.ToString()); // Вывод полного описания журнала вместе со всеми статьями
                                                 /*
                                                  * 
                                                  * 
                                                  ЗАМЕРЫ------------------------------------------------
                                                     СКОРОСТИ-------------------------------------------*/
            Console.WriteLine("Введите размерность прямоугольного массива АхB через пробел или икс");
            string str = Console.ReadLine();
            string[] coor = str.Split(new char[] { ' ', 'x', 'х' });
            while (coor.Length < 2)
            {
                Console.WriteLine("\n\n\n\n\nВведите размерность прямоугольного массива АхB через пробел или икс");
                str = Console.ReadLine();
                coor = str.Split(new char[] { ' ', 'x', 'х' });
            }
            int nrow, ncolumn;
            nrow = int.Parse(coor[0]);
            ncolumn = int.Parse(coor[1]);
            Console.WriteLine("Инициализирую массивы.......");
            int len = nrow * ncolumn;
            Article[] newhor1 = new Article[len];
            for (int i = 0; i < len; i++)
                newhor1[i] = new Article();
            // Прямоугольная матрица
            Article[,] newhor2 = new Article[nrow, ncolumn];
            for (int i = 0; i < nrow; i++)
            {
                for (int j = 0; j < ncolumn; j++)
                {
                    newhor2[i, j] = new Article();
                }
            }

            // Зубчатый (ступенчатый) массив
            int k = stairs(len);
            Article[][] newhor3 = new Article[k][];
            for (int i = 0; i < k; i++)
            {
                if (i == (k - 1))
                    newhor3[i] = new Article[len];
                else
                {
                    newhor3[i] = new Article[i + 1];
                    len -= (i + 1);
                }
            }
            for (int i = 0; i < k - 1; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    newhor3[i][j] = new Article();
           
                }
            }
            for (int i = 0; i < len; i++)
            {
                newhor3[k - 1][i] = new Article();
            }
            Console.WriteLine("Готово");
            Article[][] tester = new Article[1][];
            tester[0] = newhor1;
            int clock = Environment.TickCount;
            adder(tester);
            clock = Environment.TickCount - clock;
            Console.WriteLine($"{clock}мкс - массив {nrow * ncolumn}x1");
            clock = Environment.TickCount;
            adder(newhor3);
            clock = Environment.TickCount - clock;
            Console.WriteLine($"{clock}мкс - массив зубчатый из {nrow * ncolumn} элементов");
            clock = Environment.TickCount;
            adder(newhor2);
            clock = Environment.TickCount - clock;
            Console.WriteLine($"{clock}мкс - массив прямоугольный {nrow}x{ncolumn} ");
        }

    }
    enum Frequency
    {
        Weekly,
        Monthly,
        Yearly
    }


    class Person
    {
        private string f_name;
        private string s_name;
        private System.DateTime birthday;

        public string ToShortString()
        {
            return String.Concat("Имя: ", f_name, " Фамилия: ", s_name);
        }

        public override string ToString()
        {
            string bd = String.Concat(birthday.Day,".", birthday.Month, ".", birthday.Year);
            return String.Concat(ToShortString()," Дата рождения: ", bd);
        }
        public int Birthday
        {
            set
            {
                birthday.AddYears(value - birthday.Year);
            }
            get
            {
                return (birthday.Year);
            }
        }

        public Person() : this("Неизвестно")
        { }
        public Person(string f_name) : this(f_name, "Неизвестно")
        { }
        public Person(string f_name, string s_name) : this(f_name, s_name, new DateTime(2008, 5, 1, 8, 30, 52))
        { }
        public Person(string f_name, string s_name, System.DateTime birthday)
        {
            this.f_name = f_name;
            this.s_name = s_name;
            this.birthday = birthday;
        }

    }
    class Article
    {
        public Person Author { get; set; }
        public string Title { get; set; }
        public double Raiting { get; set; }

        public Article(Person au, string ti, double R)
        {
            this.Author = au;
            this.Title = ti;
            this.Raiting = R;
        }

        public Article()
        {
            this.Author = new Person();
            this.Title = "No title";
            this.Raiting = 0.0;
        }

        public override string ToString()
        {
            string auth = String.Concat("Автор:\n", Author.ToString(), "\n");
            string tit = String.Concat("Название статьи:\n", Title, "\n");
            string rai = String.Concat("Рейтинг статьи:\n", Raiting, "\n");
            return (String.Concat(auth, tit, rai));
        }
    }

    class Magazine
    {
        private string name;
        private Frequency howoften;
        private System.DateTime release;
        private int nbr_cpy;
        private Article[] articles;

        public Magazine(string n, Frequency ho, DateTime re, int nc)
        {
            this.name = n;
            this.howoften = ho;
            this.release = re;
            this.nbr_cpy = nc;
        }

        public Magazine()
        {
            this.name = "no name";
            this.howoften = Frequency.Yearly;
            this.release = new DateTime(2008, 5, 1, 8, 30, 52);
            this.nbr_cpy = 1;

        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                this.name = value;
            }
        }

        public Frequency How_Often
        {
            get
            {
                return howoften;
            }
            set
            {
                this.howoften = value;
            }
        }

        public DateTime Release
        {
            get
            {
                return this.release;
            }
            set
            {
                this.release = value;
            }
        }
        public int Number_of_Copy
        {
            get
            {
                return nbr_cpy;
            }
            set
            {
                this.nbr_cpy = value;
            }
        }
        public Article[] Articles
        {
            get
            {
                return this.articles;
            }
            set
            {
                this.articles = value;
            }
        }


        public double Average_Rating
        {
            get
            {
                int i = 0;
                double rez = 0;
                foreach (Article elem in articles)
                {
                    i++;
                    rez += elem.Raiting;
                }
                if (i > 0) rez /= i;
                return (rez);
            }
        }
        public bool this [Frequency index]
        {
            get
            {
                return (index == this.howoften)? true : false ;
            }
        }

        public void AddArticles(params Article[] mass)
        {
            if (this.articles != null)
            {
                int i = this.Articles.Length;
                int k = 0;

                Article[] arts = new Article[i + mass.Length];
                foreach (Article el in this.Articles)
                {
                    arts[k++] = el;
                }
                foreach (Article elem in mass)
                {
                    arts[i++] = elem;
                }
                this.Articles = arts;
            }
            else
            {
                this.articles = mass;
            }
        }

        private string ShortMag()
        {
            string name = String.Concat(" Журнал ", this.name.ToUpper(), "\n");
            string freak = (this.howoften == Frequency.Monthly) ? "Ежемесечный\n" : ((this.howoften == Frequency.Yearly) ? "Ежегодный\n" : "Ежедневный\n");
            string date = String.Concat("Дата выхода издания: ", this.release.Day, ".", this.release.Month, ".", this.release.Day, "\n");
            string tir = String.Concat($"Тираж: {this.nbr_cpy} копий\n");
            return (String.Concat(freak, name, date, tir));
        }

        public override string ToString()
        {
            string inf = ShortMag();
            string arts = "\n";
            foreach (Article elem in this.articles)
            {
                arts = String.Concat(arts, elem.ToString(),"\n\n");
            }
            return (String.Concat(inf, arts));
        }
        public string ToShortString()
        {
            return (String.Concat(ShortMag(), "Средний рейтинг статей: ", Average_Rating));
        }
    }
    }

using System;
using System.Collections.Generic;
using System.Collections;

namespace lab1classes
{
    public enum Frequency
    {
        Weekly,
        Monthly,
        Yearly
    }
    public class EditionException : ArgumentException
    {
        public EditionException(string mess)
            :base(mess)
        { }
    }

    interface IRateAndCopy
    {
        double Rating { get; }
        object DeepCopy();
    }

    public class Edition
    {
        protected string editName;
        protected DateTime dateOfRealese;
        protected int circulation;

        public Edition()
        {
            this.EditName = "";
            this.dateOfRealese = new DateTime(2001,1,1);
            this.circulation = 100;
        }
        public Edition(string name, DateTime date, int tir)
        {
            this.EditName = name;
            this.dateOfRealese = date;
            this.circulation = tir;
        }

        public string EditName
        {
            get
            {
                return (editName);
            }
            set
            {
                this.editName = value;
            }
        }
        public DateTime DateOfRealese
        {
            get
            {
                return (dateOfRealese);
            }
            set
            {
                this.dateOfRealese = value;
            }
        }
        public int Circulation
        {
            get
            {
                return (circulation);
            }
            set
            {
                //this.circulation = (value < 0) ? 0 : value;
                if (value >= 0)
                    this.circulation = value;
                else
                    throw new EditionException($"{value} - не доступное значения для тиража");
            }
        }

        public virtual object DeepCopy()
        {
            Edition cpy = new Edition(this.editName, this.dateOfRealese, this.circulation);
            return (cpy);
        }

        public override bool Equals(object obj)
        {
            var edition = obj as Edition;
            return edition != null &&
                   EditName == edition.EditName &&
                   DateOfRealese == edition.DateOfRealese &&
                   Circulation == edition.Circulation;
        }

        public override int GetHashCode()
        {
            var hashCode = 1580019561;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EditName);
            hashCode = hashCode * -1521134295 + DateOfRealese.GetHashCode();
            hashCode = hashCode * -1521134295 + Circulation.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return ($"Издание: {editName}\nДата издания: {dateOfRealese.Day}.{dateOfRealese.Month}.{dateOfRealese.Year}\nТираж: {circulation}\n");
        }
    }
    public class Person
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
            string bd = String.Concat(birthday.Day, ".", birthday.Month, ".", birthday.Year);
            return String.Concat(ToShortString(), " Дата рождения: ", bd);
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

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType()) return false;

            Person person = (Person)obj;
            return ((this.f_name == person.f_name) && (this.s_name == person.s_name) && (this.birthday == person.birthday));
        }

        public static bool operator ==(Person c1, Object obj)
        {
            return (c1.Equals(obj));
        }
        public static bool operator !=(Person c1, Object obj)
        {
            return (!c1.Equals(obj));
        }

        public virtual Object DeepCopy()
        {
            return (new Person(this.f_name, this.s_name, this.birthday));
        }

        public override int GetHashCode()
        {
            var hashCode = 690581101;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(f_name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(s_name);
            hashCode = hashCode * -1521134295 + birthday.GetHashCode();
            hashCode = hashCode * -1521134295 + Birthday.GetHashCode();
            return hashCode;
        }
    }
    public class Article : IRateAndCopy
    {
        public Person Author { get; set; }
        public string Title { get; set; }
        public double Rating { get; set; }

        public Article(Person au, string ti, double R)
        {
            this.Author = au;
            this.Title = ti;
            this.Rating = R;
        }

        public Article()
        {
            this.Author = new Person();
            this.Title = "No title";
            this.Rating = 0.0;
        }

        public override string ToString()
        {
            string auth = String.Concat("Автор:\n", Author.ToString(), "\n");
            string tit = String.Concat("Название статьи:\n", Title, "\n");
            string rai = String.Concat("Рейтинг статьи:\n", Rating, "\n");
            return (String.Concat(auth, tit, rai));
        }

        public virtual Object DeepCopy()
        {
            return (new Article(this.Author, this.Title, this.Rating));
        }

        public override bool Equals(object obj)
        {
            var article = obj as Article;
            return article != null &&
                   EqualityComparer<Person>.Default.Equals(Author, article.Author) &&
                   Title == article.Title &&
                   Rating == article.Rating;
        }

        public override int GetHashCode()
        {
            var hashCode = 4085079;
            hashCode = hashCode * -1521134295 + EqualityComparer<Person>.Default.GetHashCode(Author);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Title);
            hashCode = hashCode * -1521134295 + Rating.GetHashCode();
            return hashCode;
        }
    }
    public class Magazine : Edition, IRateAndCopy
    {
        private Frequency howoften;
        private ArrayList articles;
        private ArrayList autors;

        public Magazine(string n, Frequency ho, DateTime re, int nc): base(n, re, nc)
        {
            this.howoften = ho;
        }
        public Magazine():base()
        {
            this.howoften = Frequency.Yearly;
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
        public ArrayList Autors
        {
            get
            {
                return this.autors;
            }
            set
            {
                if (value != null)
                    this.autors = value;
            }
        }
        public ArrayList Articles
        {
            get
            {
                return (this.articles);
            }
            set
            {
                if (value != null)
                    this.articles = value;
            }
        }
        public Edition Daddy
        {
            get
            {
                return ((Edition)base.DeepCopy());
            }
            set
            {
                this.EditName = value.EditName;
                this.Circulation = value.Circulation;
                this.DateOfRealese = value.DateOfRealese;
            }
        }


        public double Rating
        {
            get
            {
                int i = 0;
                double rez = 0;
                foreach (Article elem in articles)
                {
                    i++;
                    rez += elem.Rating;
                }
                if (i > 0) rez /= i;
                return (rez);
            }
        }
        public bool this[Frequency index]
        {
            get
            {
                return (index == this.howoften) ? true : false;
            }
        }

        public void AddArticles(params Article[] mass)
        {
            if (mass == null)
                return;
            if (this.articles == null)     
                this.articles = new ArrayList();
            this.articles.AddRange(mass);
        }
        public void AddArticles(ArrayList list)
        {
            if (this.Articles == null)
                this.Articles = new ArrayList();
            foreach (Article elem in list)
            {
                this.Articles.Add(elem);
            }
        }
        public void AddEditors(params Person[] mass)
        {
            if (mass == null)
                return;
            if (this.autors == null)
                this.autors = new ArrayList();
            this.autors.AddRange(mass);
        }
        public void AddEditors(ArrayList list)
        {
            if (this.Autors == null)
                this.Autors = new ArrayList();
            foreach (Person elem in list)
            {
                this.Autors.Add(elem);
            }
        }
        private string ShortMag()
        {
            string info = base.ToString();
            string freak = (this.howoften == Frequency.Monthly) ? "Ежемесечное " : ((this.howoften == Frequency.Yearly) ? "Ежегодное " : "Ежедневное ");
            return (String.Concat(freak, info));
        }

        public override string ToString()
        {
            string inf = ShortMag();
            string arts = "\nСписок статей:\n";
            foreach (Article elem in this.articles)
            {
                arts = String.Concat(arts, elem.ToString(), "\n\n");
            }
            arts = String.Concat(arts, "\nСписок авторов:\n");
            foreach (Person elem in this.autors)
            {
                arts = String.Concat(arts, elem.ToString(), "\n\n");
            }
            return (String.Concat(inf, arts));
        }
        public string ToShortString()
        {
            return (String.Concat(ShortMag(), "Средний рейтинг статей: ", Rating));
        }

        public override object DeepCopy()
        {
            Magazine cpy = new Magazine(this.EditName, this.howoften, this.DateOfRealese, this.Circulation);
            cpy.howoften = this.howoften;
            cpy.AddArticles(this.Articles);
            cpy.AddEditors(this.Autors);
            return (cpy);
        }

        public IEnumerable GetArticles(double r)
        {
            for(int i = 0; i < Articles.Count; i++)
            {
                if (((Article)this.Articles[i]).Rating >= r)
                    yield return (this.Articles[i]);
            }
        }
        public IEnumerable GetArticles(string str)
        {
            for (int i = 0; i < Articles.Count; i++)
            {
                if (((Article)this.Articles[i]).Title.IndexOf(str) != -1)
                    yield return (this.Articles[i]);
            }
        }
    }
}

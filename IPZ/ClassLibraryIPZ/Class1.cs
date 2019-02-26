using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ClassLibraryIPZ
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
			: base(mess)
		{ }
	}

	interface IRateAndCopy
	{
		double Rating { get; }
		object DeepCopy();
	}


	[Serializable]
	public class Edition : IComparable<Edition>
	{
		protected string editName;
		protected DateTime dateOfRealese;
		protected int circulation;

		public int CompareTo(Edition o)
		{
			return this.editName.CompareTo(o.editName);
		}
		public int CompareTo(Edition c2, EditionComparer.ComparisonType comparisonType)
		{
			switch (comparisonType)
			{
				case EditionComparer.ComparisonType.circulation:
					return circulation.CompareTo(c2.Circulation);
				case EditionComparer.ComparisonType.dateOfRealese:
					return dateOfRealese.CompareTo(c2.dateOfRealese);
				default:
					return EditName.CompareTo(c2.EditName);
			}
		}
		public class EditionComparer : IComparer<Edition>
		{
			public EditionComparer(ComparisonType typ)
			{
				ComparisonMethod = typ;
			}
			public enum ComparisonType
			{
				Make = 1, dateOfRealese, circulation
			}
			public ComparisonType ComparisonMethod
			{
				set;
				get;
			}
			public int Compare(Edition x, Edition y)
			{
				Edition c1;
				Edition c2;

				if (x is Edition)
					c1 = x as Edition;
				else
					throw new ArgumentException("Object is not of type Car.");

				if (y is Edition)
					c2 = y as Edition;
				else
					throw new ArgumentException("Object is not of type Car.");

				return c1.CompareTo(c2, ComparisonMethod);
			}
		}
		public Edition()
		{
			this.EditName = "";
			this.dateOfRealese = new DateTime(2001, 1, 1);
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
	[Serializable]
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
		public string FirstName
		{
			get
			{
				return (this.f_name);
			}
		}
		public string	SecondName
		{
			get
			{
				return (this.s_name);
			}
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
	[Serializable]
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
	[Serializable]
	public class Magazine : Edition, IRateAndCopy
	{
		private Frequency howoften;
		System.Collections.Generic.List<Person> autors;
		System.Collections.Generic.List<Article> articles;
		public Magazine(string n, Frequency ho, DateTime re, int nc) : base(n, re, nc)
		{
			this.howoften = ho;
		}
		public Magazine() : base()
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
		public List<Person> Autors
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
		public List<Article> Articles
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
				if (articles != null)
				{
					foreach (Article elem in articles)
					{
						i++;
						rez += elem.Rating;
					}
					if (i > 0) rez /= i;
				}
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
				this.articles = new List<Article>();
			this.articles.AddRange(mass);
			foreach (Article el in mass)
				AddEditors(el.Author);

		}
		public void AddArticles(ArrayList list)
		{
			if (this.Articles == null)
				this.Articles = new List<Article>();
			foreach (Article elem in list)
			{
				this.Articles.Add(elem);
				AddEditors(elem.Author);

			}
		}
		public void AddArticles(List<Article> arts)
		{
			foreach (Article el in arts)
				AddEditors(el.Author);
			this.Articles.AddRange(arts);
		}
		public void AddEditors(params Person[] mass)
		{
			if (mass == null)
				return;
			if (this.autors == null)
				this.autors = new List<Person>();
			this.autors.AddRange(mass);
		}
		public void AddEditors(ArrayList list)
		{
			if (this.Autors == null)
				this.Autors = new List<Person>();
			foreach (Person elem in list)
			{
				this.Autors.Add(elem);
			}
		}
		public void AddEditors(List<Person> eds)
		{
			this.Autors.AddRange(eds);
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
			if (this.articles != null)
			{
				foreach (Article elem in this.articles)
				{
					arts = String.Concat(arts, elem.ToString(), "\n\n");
				}
				arts = String.Concat(arts, "\nСписок авторов:\n");
				foreach (Person elem in this.autors)
				{
					arts = String.Concat(arts, elem.ToString(), "\n\n");
				}
			}
			return (String.Concat(inf, arts));
		}
		public string ToShortString()
		{
			return (String.Concat(ShortMag(), "Средний рейтинг статей: ", Rating));
		}

		public override object DeepCopy()
		{
			/*Magazine cpy = new Magazine(this.EditName, this.howoften, this.DateOfRealese, this.Circulation);
			cpy.howoften = this.howoften;
			if (Articles != null)
			{
				foreach (Article el in Articles)
				{
					cpy.AddArticles(el.DeepCopy() as Article);
				}
			}
			if (Autors != null)
			{
				foreach (Person el in Autors)
				{
					cpy.AddEditors(el.DeepCopy() as Person);
				}
			}
			return (cpy);*/
			MemoryStream stream = new MemoryStream();
			try
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream, this);
				stream.Seek(0, SeekOrigin.Begin);
				Magazine cpy = (Magazine)formatter.Deserialize(stream);
				stream.Close();
				return (cpy);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return (null);
			}
		}

		public IEnumerable GetArticles(double r)
		{
			for (int i = 0; i < Articles.Count; i++)
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

		public static bool Save(string filename, Magazine obj)
		{
			BinaryFormatter formatter = new BinaryFormatter();
			using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
			{
				try
				{
					formatter.Serialize(fs, obj);
					fs.Close();
					return (true);
				}
				catch
				{
					return (false);
				}
			}
		}
		public static bool Load(string filename, ref Magazine obj)
		{
			BinaryFormatter formatter = new BinaryFormatter();
			try
			{
				using (FileStream fs = new FileStream(filename, FileMode.Open))
				{

					obj = (Magazine)formatter.Deserialize(fs);
					fs.Close();
					return (true);
				}
			}
			catch
			{
				return (false);
			}
		}
		public bool AddFromConsole()
		{
			bool flag = true;
			while (flag)
			{
				Console.WriteLine("Добавление новой статьи\nВведите имя, фамилию и дату рождения автора через пробел, дату рождения необходимо ввести в формате ДД.ММ.ГГГГ");
				try
				{
					string str = Console.ReadLine();
					char[] sep = new char[] { ' ' };
					string[] strs = str.Split(sep, 3);
					sep = new char[] { '.', '\\', '/', ':' };
					string[] date = strs[2].Split(sep, 3);
					DateTime mydate = new DateTime(Convert.ToInt16(date[2]), Convert.ToInt16(date[1]), Convert.ToInt16(date[0]));
					Person pr = new Person(strs[0], strs[2], mydate);
					Console.WriteLine("Введите название статьи");
					string s = Console.ReadLine();
					Console.WriteLine("Введите рейтинг статьи");
					bool b = true;
					Double d;
					while (b)
					{
						try
						{
							d = Convert.ToDouble(Console.ReadLine());
							b = false;
							if (Autors != null)
							{
								foreach (Person el in Autors)
								{
									if (el.Birthday == pr.Birthday && el.FirstName == pr.FirstName && pr.SecondName == el.SecondName)
									{
										pr = el;
										break;
									}
								}
							}
							Article ar = new Article(pr, s, d);
							this.AddArticles(ar);
							return (true);
						}
						catch
						{
							Console.WriteLine("Неправильнй ввод!");
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Вы допустили ошибку! Попробуйте снова.\n{0}", ex);
					flag = true;
				}
			}
			return (false);
		}

	}
	public class MagazineCollection
	{
		System.Collections.Generic.List<Magazine> mags;
		public MagazineCollection()
		{
			mags = new List<Magazine>();
		}
		public void AddDefaults()
		{
			Person p1 = new Person("Святая", "Мария", new DateTime(1000, 1, 1));
			Person p2 = new Person("Антон", "Лавей", new DateTime(666, 6, 6));
			Person p3 = new Person("Гомер", "Симспон");
			Person p4 = new Person();
			Article art1 = new Article(p1, "Ешь, молись, лби", 7.3);
			Article art2 = new Article(p2, "Holly Bible 2.0", 9.8);
			Article art3 = new Article(p3, "Пончики, Пончики", 10.0);
			Article art4 = new Article(p1, "Домострой", 3);
			Article art5 = new Article(p4, "no title", 5);
			Magazine magOne = new Magazine("Новое начало", Frequency.Monthly, new DateTime(2018, 11, 11), 1000000);
			magOne.AddArticles(new Article[] { art1, art2, art3, art4, art5 });
			magOne.AddEditors(new Person[] { p1, p2, p3, p4 });
			this.mags = new List<Magazine>();
			mags.Add(magOne);
			mags.Add(new Magazine());
			mags.Add(new Magazine());
			p1 = new Person("Новый", "человек", new DateTime(2020, 1, 1));
			p2 = new Person("Я", "Легенда", new DateTime(2000, 6, 6));
			p3 = new Person("Хаю", "Хай");
			p4 = new Person();
			art1 = new Article(p1, "Средний рейтинг", 5.3);
			art2 = new Article(p2, "Низкий рейтинг", 1.8);
			art3 = new Article(p3, "Высший", 10.0);
			art4 = new Article(p1, "Русская классика", 3);
			art5 = new Article(p4, "Выше срежнего", 8);
			magOne = new Magazine("Посредственность", Frequency.Weekly, new DateTime(2018, 10, 11), 10000000);
			magOne.AddArticles(new Article[] { art1, art2, art3, art4, art5 });
			magOne.AddEditors(new Person[] { p1, p2, p3, p4 });
			mags.Add(magOne);
			for (int i = 0; i < mags.Count; i++)
			{
				if (MagazineReplaced != null)
					MagazineReplaced(this, new MagazineListHandlerEventArgs(CollectionName, "Новый элемент", i));
			}
		}
		public void AddMagazines(params Magazine[] ms)
		{
			foreach (Magazine el in ms)
			{
				if (MagazineReplaced != null)
					MagazineReplaced(this, new MagazineListHandlerEventArgs(CollectionName, "Новый элемент", mags.Count));
				this.mags.Add(el);
			}
		}
		public override string ToString()
		{
			string str = "";
			foreach (Magazine m in this.mags)
			{
				str = String.Concat(str, "\n", m.ToString());
			}
			return (str);
		}
		public virtual string ToShortString()
		{
			int i;
			int j;
			string str = "";
			foreach (Magazine m in this.mags)
			{
				str = String.Concat(str, m.ToShortString(), "Число Статей: ");
				i = m.Articles == null ? 0 : m.Articles.Count();
				j = m.Autors == null ? 0 : m.Autors.Count();
				str = String.Concat(str, i, "Количество редакторов: ", j, "\n\n");
			}
			return (str);
		}
		public string CollectionName { get; set; }
		public bool Replace(int j, Magazine mg)
		{
			if (mags != null)
			{
				if (mags.Count > j)
				{
					mags[j] = mg;
					if (MagazineReplaced != null)
						MagazineReplaced(this, new MagazineListHandlerEventArgs(CollectionName, "Замена элемента", j));
					return (true);
				}
			}
			return (false);
		}
		public Magazine this[int i]
		{
			get
			{
				return (mags[i]);
			}
			set
			{
				if (mags.Count > i)
				{
					mags[i] = value;
					if (MagazineReplaced != null)
						MagazineReplaced(this, new MagazineListHandlerEventArgs(CollectionName, "Замена элемента", i));
				}
			}
		}
		public void SortName()
		{
			this.mags.Sort();
		}
		public void SortDate()
		{
			mags.Sort(new Edition.EditionComparer(Edition.EditionComparer.ComparisonType.dateOfRealese));
		}
		public void SortCirc()
		{
			mags.Sort(new Edition.EditionComparer(Edition.EditionComparer.ComparisonType.circulation));
		}
		public double mRait
		{
			get
			{
				double m = 0;
				if (mags != null)
				{
					foreach (Magazine el in mags)
					{
						if (el.Rating > m)
							m = el.Rating;
					}
					return (m);
				}
				else
					return (3.0);
			}
		}
		public IEnumerable<Magazine> Monthly
		{
			get
			{
				return (from m in mags where (m.How_Often == Frequency.Monthly) select m);
			}
		}
		public List<Magazine> RatingGroup(double value)
		{
			return (from m in mags where (m.Rating >= value) select m).ToList<Magazine>();
		}
		public delegate void MagazineListHandler(object source, MagazineListHandlerEventArgs args);
		public event MagazineListHandler MagazineAdded;
		public event MagazineListHandler MagazineReplaced;
	}
	public class MagazineListHandlerEventArgs : System.EventArgs
	{
		public string CollectionName { get; set; }
		public string CollectionType { get; set; }
		public int ChangedIndex { get; set; }
		public MagazineListHandlerEventArgs()
		{
			CollectionName = "Имя колллекции";
			CollectionType = "Тип коллекции";
			ChangedIndex = 0;
		}
		public MagazineListHandlerEventArgs(string n, string t, int i)
		{
			CollectionName = n;
			CollectionType = t;
			ChangedIndex = i;
		}
		public override string ToString()
		{
			return (String.Concat("Имя коллекции", CollectionName, "\nТип:", CollectionType, "\nИзмененный элемент", ChangedIndex, "\n"));
		}
	}
	public class TestCollections : Dictionary<Edition, Magazine>
	{
		System.Collections.Generic.List<Edition> eds;
		System.Collections.Generic.List<string> tits;
		System.Collections.Generic.Dictionary<Edition, Magazine> dEM;
		System.Collections.Generic.Dictionary<string, Magazine> dNM;
		int Length;
		static Magazine Meth(int a)
		{
			return (new Magazine());
		}
		public TestCollections(int a)
		{
			tits = new List<string>();
			eds = new List<Edition>();
			dEM = new Dictionary<Edition, Magazine>();
			dNM = new Dictionary<string, Magazine>();
			for (int i = 0; a > i; i++)
			{
				Edition e = new Edition(i.ToString(), new DateTime(2000, i % 11 + 1, 1), i * 20);
				string s = String.Concat(e.ToString(), " ", i.ToString());
				tits.Add(s);
				eds.Add(e);
				Magazine m = new Magazine(s, Frequency.Monthly, new DateTime(), 0);
				dEM.Add(e, m);
				dNM.Add(s, m);
			}
			Length = a;
		}
		public string Timerr()
		{
			string rez = "";
			Edition e = eds[Length - 1];
			int clock = Environment.TickCount;
			e = eds.Find(x => x == e);
			clock = Environment.TickCount - clock;
			rez = String.Concat(rez, $"Поиск 1: {clock}мс\n");
			string s = tits[Length - 1];
			clock = Environment.TickCount;
			s = tits.Find(x => x == s);
			clock = Environment.TickCount - clock;
			rez = String.Concat(rez, $"Поиск 2: {clock}мс\n");
			clock = Environment.TickCount;
			Magazine m = dEM[e];
			clock = Environment.TickCount - clock;
			rez = String.Concat(rez, $"Поиск 3: {clock}мс\n");
			clock = Environment.TickCount;
			Magazine m1 = dNM[s];
			clock = Environment.TickCount - clock;
			rez = String.Concat(rez, $"Поиск 4: {clock}мс\n");
			clock = Environment.TickCount;
			dEM.ContainsValue(m);
			clock = Environment.TickCount - clock;
			rez = String.Concat(rez, $"Поиск 5: {clock}мс\n");
			clock = Environment.TickCount;
			dNM.ContainsValue(m1);
			clock = Environment.TickCount - clock;
			rez = String.Concat(rez, $"Поиск 6: {clock}мс\n");
			return (rez);
		}
	}
	public class Listener
	{
		List<ListEntry> events = new List<ListEntry>();
		public void MagazineListHandler(object source, MagazineListHandlerEventArgs args)
		{
			events.Add(new ListEntry(args.CollectionName, args.CollectionType, args.ChangedIndex));
		}
		public override string ToString()
		{
			string str = "";
			foreach (ListEntry el in events)
			{
				str = String.Concat(str, el.ToString(), "\n");
			}
			return (str);
		}
	}
	public class ListEntry
	{
		public string CollectionName { get; set; }
		public string CollectionType { get; set; }
		public int Index { get; set; }
		public ListEntry(string s, string t, int i)
		{
			CollectionName = s;
			CollectionType = t;
			Index = i;
		}
		public override string ToString()
		{
			return (String.Concat("Имя коллекции: ", CollectionName, "\nТип: ", CollectionType, "\nИзмененный элемент: ", Index, "\n"));
		}
	}
}

using System;
using ClassLibLab33;
using System.Collections;
using System.Collections.Generic;

namespace lab3
{
	class Program
	{
		static void Main(string[] args)
		{
			MagazineCollection mc = new MagazineCollection();
			mc.AddDefaults();
			Magazine mag1 = new Magazine("Fee", Frequency.Monthly, new DateTime(2012, 12, 12), 300);
			mag1.AddArticles(new Article(new Person("aid", "Mong",new DateTime(1999, 1, 1)), "alala", 2.6), new Article(), new Article(), new Article(), new Article(), new Article());
			mc.AddMagazines(mag1);
			Console.WriteLine(mc.ToString());
			/*****************************************/
			
			mc.SortCirc();
			Console.WriteLine("\nСортировка по тиражу\n{0}", mc.ToShortString());
			mc.SortDate();
			Console.WriteLine("\nСортировка по дате\n{0}", mc.ToShortString());
			mc.SortName();
			Console.WriteLine("\nСортировка по названию\n{0}", mc.ToShortString());
			/*****************************************/
			Console.WriteLine($"Максимальный рейтинг {mc.mRait}\n");
			Console.WriteLine($"\nЕжемесячные издания:\n");

			IEnumerable<Magazine> monthly =  mc.Monthly;
			foreach (Magazine el in monthly)
			{
				Console.WriteLine(el.ToShortString());
			}
			Console.WriteLine($"\nИздания с рейтингом 5 и выше:\n");

			List<Magazine> mags = mc.RatingGroup(5);
			foreach (Magazine el in mags)
			{
				Console.WriteLine(el.ToShortString());
			}
			/*****************************************/
			TestCollections tc = new TestCollections(1999989);
			Console.WriteLine(tc.Timerr());
		}
	}


}


using System;
using System.IO;
using ClassLibraryIPZ;

namespace IPZ
{
	class Program
	{
		static void Main(string[] args)
		{
			Magazine mag = new Magazine("MagOne", Frequency.Monthly, new DateTime(2017, 07, 21), 200000);
			Article art1 = new Article(new Person("Name", "notName", new DateTime(1969, 12, 12)), "Title", 6.7);
			mag.AddArticles(art1);
			Magazine cpmag = (Magazine)mag.DeepCopy();
			Console.WriteLine("Первый объект\n{0}", cpmag.ToShortString());
			Console.WriteLine("Второй объект\n{0}", mag.ToShortString());
			/******************************************************************************************************/
			Console.WriteLine("\n\nВведите мия файла");
			string fn = Console.ReadLine();
			Magazine mag2 = null;
			FileInfo fileInf = new FileInfo(fn);
			if (!fileInf.Exists)
			{
				Console.WriteLine("\nФайл не обнаружен и создан\n");
				Magazine.Save(fn, mag);
			}
			Magazine.Load(fn, ref mag2);

			Console.WriteLine(mag2.ToShortString());
			mag2.AddFromConsole();
			Magazine.Save(fn, mag2);
			Console.WriteLine(mag2.ToShortString());
			Magazine.Load(fn, ref mag2);
			mag2.AddFromConsole();
			Magazine.Save(fn, mag2);
			Console.WriteLine(mag2.ToShortString());

		}
	}
}

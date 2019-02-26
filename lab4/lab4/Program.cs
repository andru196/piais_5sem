using System;
using System.Collections;
using System.Collections.Generic;
using ClassLibrary4;

namespace lab4
{
	class Program
	{
		static void Main(string[] args)
		{
			MagazineCollection mc1 = new MagazineCollection();
			MagazineCollection mc2 = new MagazineCollection();
			mc1.CollectionName = "Alpha";
			mc2.CollectionName = "Beta";
			Listener l1 = new Listener();
			Listener l2 = new Listener();
			mc1.MagazineAdded += l1.MagazineListHandler;
			mc2.MagazineAdded += l2.MagazineListHandler;
			mc1.MagazineReplaced += l1.MagazineListHandler;
			mc2.MagazineReplaced += l2.MagazineListHandler;
			mc1.AddDefaults();
			mc1[2] = new Magazine();
			mc2.AddMagazines(new Magazine(), new Magazine(), new Magazine());
			mc2.Replace(0, new Magazine("LOL", Frequency.Yearly, new DateTime(1990, 5, 12), 20000000));
			mc2.AddMagazines(new Magazine("NewLine", Frequency.Weekly, new DateTime(2020, 1, 1), 25000));
			Console.WriteLine($"Первый объект:\n{l1.ToString()}");
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine($"Второй объект:\n{l2.ToString()}");
			Console.ReadKey();
		}
	}
}

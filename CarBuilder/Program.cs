using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace CarBuilder
{
    public class Program
    {
        public static bool added = false;
        

        private static void Main(string[] args)
        {
           
            if (args != null && args.Length == 1)
            {
                var dependenciesPath = args[0];
                if (File.Exists(dependenciesPath))
                {
                    BuildCar(dependenciesPath);
                }

            }
            else
            {
                Console.Write("Usage: CarBuilder [dependenciesfilePath]");
            }
        }

        private static void BuildCar(string dependenciesPath)
        {
            StreamReader input;
            input = File.OpenText(dependenciesPath);
            TopologicalSorter<String> sorter = new TopologicalSorter<string>(); 
            
            for (string line; (line = input.ReadLine()) != null; )
            {
                string[] splitDependancy = Regex.Split(line, "->");
                var objectName = splitDependancy[0].Trim();
                var dependantObject = splitDependancy[1].Trim();
                sorter.AddObjects(objectName);
                sorter.AddObjects(dependantObject);
                sorter.SetDependencies(objectName, dependantObject);
                Console.WriteLine(line);
            }

            var result =  sorter.Sort();

            Console.Write(result);

        }
    }
}

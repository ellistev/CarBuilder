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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Usage: CarBuilder [dependenciesfilePath]");
                Console.ResetColor();
            }
        }

        private static void BuildCar(string dependenciesPath)
        {
            //builds the car
            var input = File.OpenText(dependenciesPath);
            DependencySorter<String> sorter = new DependencySorter<string>(); 
            
            for (string line; (line = input.ReadLine()) != null; )
            {//go through each line in input file, parse the dependencies, and add to the DependencySorter object

                //split line based on dependancy
                string[] splitDependancy = Regex.Split(line, "->");
                
                //trim the objects
                var objectName = splitDependancy[0].Trim();
                var dependantObject = splitDependancy[1].Trim();

                //add the parts required
                sorter.AddCarPart(objectName);
                sorter.AddCarPart(dependantObject);

                //add a dependancy relationship
                sorter.AddDependantRelationship(dependantObject, objectName);
                }

            try
            {
                var result = sorter.SortDependancies();

                StreamWriter file = new System.IO.StreamWriter("output.txt");
                result.ForEach(file.WriteLine);
                file.Close();

                foreach (var val in result)
                {
                    Console.Write(val + "\n");
                }
            }
            catch (CircularException ce)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Error: " + ce.Message);
                Console.ResetColor();
            }
            catch (Exception e)
            {
                
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\nOutput complete\nPress Return To Continue, Please");
            Console.ResetColor();
            Console.ReadLine();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace CarBuilder
{
    public static class Program
    {

        static void Main(string[] args)
        {
            StreamReader input;
            List<String> objects = new List<string>();
            Dictionary<String, String> dependencies = new Dictionary<string, string>();

            if (args != null && args.Length == 2)
            {
                var objectsPath = args[0];
                var dependenciesPath = args[1];
                if (File.Exists(objectsPath))
                {
                    input = File.OpenText(objectsPath);

                    for (string line; (line = input.ReadLine()) != null; )
                    {
                        objects.Add(line);
                        Console.WriteLine(line);
                    }

                    input = File.OpenText(dependenciesPath);

                    for (string line; (line = input.ReadLine()) != null; )
                    {
                        string splitDependancy = line.Split(string,"->");
                        Console.WriteLine(line);
                    }
                }


            }
            else
            {
                Console.Write("Usage: CarBuilder [objectfile] [dependenciesfile]");
            }


        }

        public static IEnumerable<T> TSort<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> dependencies, bool throwOnCycle = false)
        {
            var sorted = new List<T>();
            var visited = new HashSet<T>();

            foreach (var item in source)
                Visit(item, visited, sorted, dependencies, throwOnCycle);

            return sorted;
        }

        private static void Visit<T>(T item, HashSet<T> visited, List<T> sorted, Func<T, IEnumerable<T>> dependencies, bool throwOnCycle)
        {
            if (!visited.Contains(item))
            {
                visited.Add(item);

                foreach (var dep in dependencies(item))
                    Visit(dep, visited, sorted, dependencies, throwOnCycle);

                sorted.Add(item);
            }
            else
            {
                if (throwOnCycle && !sorted.Contains(item))
                    throw new Exception("Cyclic dependency found");
            }
        }
    }
}

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
            ITopologicSortable dependencies = new Dictionary<string, string>();

            if (args != null && args.Length == 1)
            {
                var dependenciesPath = args[0];
                if (File.Exists(dependenciesPath))
                {
                    input = File.OpenText(dependenciesPath);

                    for (string line; (line = input.ReadLine()) != null; )
                    {

                        var foo = new Foo();
                        var bar = new Foo();
                        var parent = new Foo();
                        bar.DependsOn.Add(foo); // bar depends on foo; it will be sorted to come after foo
                        parent.Children.Add(bar);
                        parent.Children.Add(foo);
                        parent.SortSelfAndDescendents(); // parent.Children now contains the sorted list

                        string[] splitDependancy = Regex.Split(line,"->");

                        dependencies.Add(splitDependancy[0].Trim(), splitDependancy[1].Trim());
                        Console.WriteLine(line);
                    }
                }

                var result = TSort<List<String>>(dependencies, dependencies, false); 

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

using System;
using System.Collections.Generic;

namespace CarBuilder
{
    public class DependencySorter<T>
    {
        private readonly Dictionary<T, Dictionary<T, object>> carPartArray = new Dictionary<T, Dictionary<T, object>>();

        public void AddCarPart(params T[] objects)
        {//add car part object to array

            foreach (var part in objects)
            {
                //check that this is not a duplicate part
                if (carPartArray.ContainsKey(part))
                {
                    return;
                }

                //add if not a duplicate part
                carPartArray.Add(part, new Dictionary<T, object>());
            }
        }

        public void AddDependantRelationship(T obj, params T[] dependsOnObjects)
        {
            var dependencies = carPartArray[obj];

            //built to allow for multiple dependancies
            foreach (var dependsOnObject in dependsOnObjects)
            {
                dependencies.Add(dependsOnObject, null);
            }
        }

        public T[] SortDependancies()
        {
            var result = new List<T>(carPartArray.Count);

            while (carPartArray.Count > 0)
            {
                T loneObject;
                if (!GetLoneObject(out loneObject))
                {
                    throw new CircularException();
                }

                result.Add(loneObject);

                DeleteLoneObject(loneObject);
            }

            return result.ToArray();
        }

        private bool GetLoneObject(out T result)
        {
            foreach (var dependancy in carPartArray)
            {
                if (dependancy.Value.Count > 0)
                {
                    continue;
                }

                result = dependancy.Key;
                return true;
            }

            result = default(T);
            return false;
        }

        private void DeleteLoneObject(T obj)
        {
            carPartArray.Remove(obj);

            foreach (var pair in carPartArray)
            {
                pair.Value.Remove(obj);
            }
        }
    }

    public class CircularException : Exception
    {
        public CircularException()
            : base("Circular exception: You have just caused an infinite loop.  Bad! \nCheck the input file for errors")
        {
        }
    }
}
using System;
using System.Collections.Generic;

namespace CarBuilder
{
    public class DependencySorter<T>
    {
        private readonly Dictionary<T, Dictionary<T, object>> _carPartArray = new Dictionary<T, Dictionary<T, object>>();

        public T[] SortDependancies()
        {
            var sortResult = new List<T>(_carPartArray.Count);

            while (_carPartArray.Count > 0)
            {
                T loneObject;

                if (!GetLoneObject(out loneObject))
                {
                    throw new CircularException();
                }

                sortResult.Add(loneObject);

                DeleteLoneObject(loneObject);
            }

            return sortResult.ToArray();
        }

        private bool GetLoneObject(out T loneResult)
        {//search through array of parts and return the first that has no dependancies remaining

            foreach (var dependancy in _carPartArray)
            {
                if (dependancy.Value.Count > 0)
                {
                    continue;
                }

                //this dependancy is lone, return this part
                loneResult = dependancy.Key;
                Console.Write(dependancy.Key + ",");
                //return true;
            }
            Console.Write("\n");
            loneResult = default(T);
            return false;
        }

        private void DeleteLoneObject(T obj)
        {//remove this lone part/object, so we do not see it in the results again.

            _carPartArray.Remove(obj);

            foreach (var pair in _carPartArray)
            {
                pair.Value.Remove(obj);
            }
        }

        public void AddCarPart(params T[] objects)
        {//add car part object to array

            foreach (var part in objects)
            {
                //check that this is not a duplicate part
                if (_carPartArray.ContainsKey(part))
                {
                    return;
                }

                //add if not a duplicate part
                _carPartArray.Add(part, new Dictionary<T, object>());
            }
        }

        public void AddDependantRelationship(T dependantPart, params T[] dependsOnObjects)
        {
            var dependenciesForPart = _carPartArray[dependantPart];

            //built to allow for multiple dependancies
            foreach (var dependsOnObject in dependsOnObjects)
            {
                dependenciesForPart.Add(dependsOnObject, null);
            }
        }

    }


}
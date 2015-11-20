using System;
using System.Collections.Generic;

namespace CarBuilder
{
    public class DependencySorter<T>
    {
        private readonly Dictionary<String, Dictionary<String, object>> _carPartArray = new Dictionary<String, Dictionary<String, object>>();
        private List<String> sortResult;

        public DependencySorter()
        {
            sortResult = new List<String>(_carPartArray.Count);  
        }

        public List<String> SortDependancies()
        {
            while (_carPartArray.Count > 0)
            {
                List<String> loneObjects;

                if (!GetLoneObject(out loneObjects))
                {
                    throw new CircularException();
                }

                foreach (var loneObject in loneObjects)
                {
                    if (loneObject != null)
                    {
                        DeleteLoneObject(loneObject); 
                    }
                }
            }

            return sortResult;
        }

        private bool GetLoneObject(out List<String> loneResult)
        {//search through array of parts and return the first that has no dependancies remaining
            
            bool foundLone = false;
            int loneResultSize = 0;
            loneResult = new List<string>();
            
            foreach (var dependancy in _carPartArray)
            {
                if (dependancy.Value.Count > 0)
                {
                    continue;
                }

                //this dependancy is lone, return this part
                loneResult.Add(dependancy.Key);

                foundLone = true;
            
            }

            if (foundLone)
            {
                loneResult.Sort();
                String outputLine = string.Join(",", loneResult);
                
                sortResult.Add(outputLine);
                //Console.Write(outputLine + "\n");
                return true;
            }
            
            loneResult[0] = default(String);
            return false;
        }

        private void DeleteLoneObject(String obj)
        {//remove this lone part/object, so we do not see it in the results again.

            _carPartArray.Remove(obj);

            foreach (var pair in _carPartArray)
            {
                pair.Value.Remove(obj);
            }
        }

        public void AddCarPart(params String[] objects)
        {//add car part object to array

            foreach (var part in objects)
            {
                //check that this is not a duplicate part
                if (_carPartArray.ContainsKey(part))
                {
                    return;
                }

                //add if not a duplicate part
                _carPartArray.Add(part, new Dictionary<String, object>());
            }
        }

        public void AddDependantRelationship(String dependantPart, params String[] dependsOnObjects)
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
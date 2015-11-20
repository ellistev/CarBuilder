using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace CarBuilder
{
    public class TopologicalSorter<T>
        {

            private readonly Dictionary<T, Dictionary<T, object>> _matrix = new Dictionary<T, Dictionary<T, object>>();


            public void AddObjects(params T[] objects)
            {
                Debug.Assert(objects != null);
                Debug.Assert(objects.Length > 0);
                foreach (T obj in objects)
                {
                    if (_matrix.ContainsKey(obj))
                    {
                        return;
                    }
                    else
                    {
                        _matrix.Add(obj, new Dictionary<T, object>());    
                    }
                    
                }
            }

            public void SetDependencies(T obj, params T[] dependsOnObjects)
            {
                Debug.Assert(dependsOnObjects != null);

                Dictionary<T, object> dependencies = _matrix[obj];

                foreach (T dependsOnObject in dependsOnObjects)
                {
                    dependencies.Add(dependsOnObject, null);
                }
            }

            public T[] Sort()
            {
                List<T> result = new List<T>(_matrix.Count);

                while (_matrix.Count > 0)
                {
                    T independentObject;
                    if (!this.GetIndependentObject(out independentObject))
                    {
                        throw new CircularReferenceException();
                    }

                    result.Add(independentObject);
                    
                    this.DeleteObject(independentObject);
                }

                return result.ToArray();
            }



            private bool GetIndependentObject(out T result)
            {
                foreach (KeyValuePair<T, Dictionary<T, object>> pair in _matrix)
                {
                    if (pair.Value.Count > 0)
                    {

                        continue;
                    }

                    result = pair.Key;
                    return true;
                }

                result = default(T);
                return false;
            }


            private void DeleteObject(T obj)
            {
                _matrix.Remove(obj);

                foreach (KeyValuePair<T, Dictionary<T, object>> pair in _matrix)
                {
                    pair.Value.Remove(obj);
                }
            }


        }

        public class CircularReferenceException : Exception
        {
            public CircularReferenceException()
                : base("Circular reference found.")
            {            
            }
        }
    }

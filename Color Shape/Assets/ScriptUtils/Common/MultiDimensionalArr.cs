using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptUtils.Common
{
    public class MultiDimensionalArr<T> : IEnumerable
    {
        /// <summary>
        /// The actual array of elements.
        /// </summary>
        public T[] elements;
        public MultiDimensionalArr(int size)
        {
            elements = new T[size];
        }
        public int Length
        {
            get
            {
                return elements.Length;
            }
        }
        /// <summary>
        /// Indexer used to access items for the elements array.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                try
                {
                    return elements[index];
                }
                catch (System.IndexOutOfRangeException ex)
                {
                    throw (new System.IndexOutOfRangeException(ex.Message + "(" + index + ")"));
                }
            }
            set
            {
                elements[index] = value;
            }
        }
        public IEnumerator GetEnumerator()
        {
            for (int j = 0; j < this.Length; j++)
            {
                yield return this[j];
            }
        }
    }
}
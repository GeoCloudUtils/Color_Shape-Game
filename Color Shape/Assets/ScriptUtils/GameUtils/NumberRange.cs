using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptUtils.GameUtils
{
    public class NumberRange : System.Object
    {
        public enum ComparisonMethod
        {
            EQ_MIN_ONLY,
            EQ_MAX_ONLY,
            EQ_NONE,
            EQ
        }
        [SerializeField] private int nrMin = 0;
        [SerializeField] private int nrMax = 1;
        public int min
        {
            get { return nrMin; }
        }

        public int max
        {
            get { return nrMax; }
        }
        [SerializeField] private string _name = "unnamed";


        [SerializeField]
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }

        public NumberRange() : this(0, 1, "unnamed") { }

        public NumberRange(int _min, int _max) : this(_min, _max, "unnamed") { }


        public NumberRange(int _min, int _max, string name)
        {
            if (_min == _max)
                throw (new Exception("Minimum and maximum cannot be equal!"));
            nrMin = _min;
            nrMax = _max;
            _name = name;
        }

        public bool verify(int nr)
        {
            return (nr >= min && nr <= max);
        }

        public bool verify(int nr, ComparisonMethod m)
        {
            switch (m)
            {
                case ComparisonMethod.EQ:
                    return (nr >= min && nr <= max);
                case ComparisonMethod.EQ_NONE:
                    return (nr > min && nr < max);
                case ComparisonMethod.EQ_MAX_ONLY:
                    return (nr > min && nr <= max);
                case ComparisonMethod.EQ_MIN_ONLY:
                    return (nr >= min && nr < max);
            }
            throw (new Exception("Bad comparison method!"));
        }

        public int[] getArray()
        {
            int realMax;
            int realMin;
            if (max > min)
            {
                realMax = max;
                realMin = min;
            }
            else
            {
                realMax = min;
                realMin = max;
            }
            int[] array = new int[(realMax - realMin) + 1];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = realMin + i;
            }
            return array;
        }

        public new string ToString()
        {
            return "[" + min.ToString() + "-" + max.ToString() + "]";
        }
    }
}
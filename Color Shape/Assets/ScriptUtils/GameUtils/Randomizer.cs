using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptUtils.GameUtils
{
    /// <summary>
    /// Used for the random generation of int numbers based on IRandomizerRule implementations
    /// </summary>
    public class Randomizer
    {
        /// <summary>
        /// The IRandomizerRule implementation being used.
        /// </summary>
        public IRandomizer<int> randomRule;

        public bool autoReset = false;
        private int min;
        private int max;
        public int Min
        {
            get
            {
                return min;
            }
        }

        public int Max
        {
            get
            {
                return max;
            }
        }
        /// <summary>
        /// Creates a new Randomizer
        /// </summary>
        /// <typeparam name="T">Type of explicit IRandomizerRule implementation to be used by the new instance.</typeparam>
        /// <param name="minMax"></param>
        /// <param name="autoReset">If true then Reset() will be called automatically when no more numbers left</param>
        /// <returns></returns>
        public static Randomizer CreateRandomizer<T>(NumberRange minMax, bool autoReset = false) where T : IRandomizer<int>
        {
            return new Randomizer(minMax.min, minMax.max, (T)System.Activator.CreateInstance(typeof(T), new object[] { minMax.min, minMax.max }), autoReset);
        }
        /// <summary>
        /// Creates a new Randomizer
        /// </summary>
        /// <typeparam name="T">Type of explicit IRandomizerRule implementation to be used by the new instance.</typeparam>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="autoReset">If true then Reset() will be called automatically when no more numbers left</param>
        /// <returns></returns>
        public static Randomizer CreateRandomizer<T>(int min, int max, bool autoReset = false) where T : IRandomizer<int>
        {
            return new Randomizer(min, max, (T)System.Activator.CreateInstance(typeof(T), new object[] { min, max }), autoReset);
        }
        /// <summary>
        /// Creates a new Randomizer
        /// </summary>
        /// <typeparam name="T">Type of explicit IRandomizerRule implementation to be used by the new instance.</typeparam>
        /// <param name="array">Min will be 0, max will be array.Length-1</param>
        /// <param name="autoReset">If true then Reset() will be called automatically when no more numbers left</param>
        /// <returns></returns>
        public static Randomizer CreateRandomizer<T>(System.Array array, bool autoReset = false) where T : IRandomizer<int>
        {
            return new Randomizer(0, array.Length - 1, (T)System.Activator.CreateInstance(typeof(T), new object[] { 0, array.Length - 1 }), autoReset);
        }

        /// <summary>
        /// Creates a new Randomizer
        /// </summary>
        /// <typeparam name="T">Type of explicit IRandomizerRule implementation to be used by the new instance.</typeparam>
        /// <param name="array">Min will be 0, max will be array.Length-1</param>
        /// <param name="autoReset">If true then Reset() will be called automatically when no more numbers left</param>
        /// <returns></returns>
        public static Randomizer CreateRandomizer<T>(IList array, bool autoReset = false) where T : IRandomizer<int>
        {
            return new Randomizer(0, array.Count - 1, (T)System.Activator.CreateInstance(typeof(T), new object[] { 0, array.Count - 1 }), autoReset);
        }
        /// <summary>
        /// Creates a new Randomizer use DefaultRandomRule
        /// </summary>
        /// <param name="array">Min will be 0, max will be array.Length-1</param>
        /// <param name="autoReset">If true then Reset() will be called automatically when no more numbers left</param>
        public Randomizer(IList array, bool autoReset = false)
            : this(0, array.Count - 1, new DefaultRandomizerRules(0, array.Count - 1), autoReset)
        {

        }
        /// <summary>
        /// Creates a new Randomizer use DefaultRandomRule
        /// </summary>
        /// <param name="array">Min will be 0, max will be array.Length-1</param>
        /// <param name="autoReset">If true then Reset() will be called automatically when no more numbers left</param>
        public Randomizer(System.Array array, bool autoReset = false)
            : this(0, array.Length - 1, new DefaultRandomizerRules(0, array.Length - 1), autoReset)
        {

        }
        /// <summary>
        /// Creates a new Randomizer use DefaultRandomRule
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="autoReset">If true then Reset() will be called automatically when no more numbers left</param>
        public Randomizer(int min, int max, bool autoReset = false)
            : this(min, max, new DefaultRandomizerRules(min, max), autoReset)
        {

        }
        /// <summary>
        /// Creates a new Randomizer
        /// </summary>
        /// <param name="array">>Min will be 0, max will be array.Length-1</param>
        /// <param name="rules"></param>
        /// <param name="autoReset">If true then Reset() will be called automatically when no more numbers left</param>
        public Randomizer(System.Array array, IRandomizer<int> rules, bool autoReset = false) : this(0, array.Length - 1, rules, autoReset)
        {

        }
        /// <summary>
        /// Creates a new Randomizer
        /// </summary>
        /// <param name="array">>Min will be 0, max will be array.Length-1</param>
        /// <param name="rules"></param>
        /// <param name="autoReset">If true then Reset() will be called automatically when no more numbers left</param>
        public Randomizer(IList array, IRandomizer<int> rules, bool autoReset = false)
            : this(0, array.Count - 1, rules, autoReset)
        {

        }
        /// <summary>
        /// Should be depracted?
        /// </summary>
        /// <param name="minMax"></param>
        /// <param name="autoReset">If true then Reset() will be called automatically when no more numbers left</param>
        public Randomizer(NumberRange minMax, bool autoReset = false) : this(minMax, new DefaultRandomizerRules(minMax.min, minMax.max), autoReset)
        {

        }
        /// <summary>
        /// Should be depracated?
        /// </summary>
        /// <param name="minMax"></param>
        /// <param name="rules"></param>
        /// <param name="autoReset">If true then Reset() will be called automatically when no more numbers left</param>
        public Randomizer(NumberRange minMax, IRandomizer<int> rules, bool autoReset = false) : this(minMax.min, minMax.max, rules, autoReset)
        {

        }
        /// <summary>
        /// Creates a new Randomizer
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="rules"></param>
        /// <param name="autoReset">If true then Reset() will be called automatically when no more numbers left</param>
        public Randomizer(int min, int max, IRandomizer<int> rules, bool autoReset)
        {
            this.min = min;
            this.max = max;
            this.randomRule = rules;
            this.autoReset = autoReset;
        }
        /// <summary>
        /// Gets a random number using the rule this instance was created with.
        /// </summary>
        /// <param name="doNotUseRules">If true will return a number within the given range but ignoring any rules. Default is false</param>
        /// <returns></returns>
        public int getRandom(bool doNotUseRules = false)
        {
            if (doNotUseRules)
            {
                return Random.Range(min, max + 1);
            }
            else
            {
                return getRandom(randomRule);
            }
        }
        /// <summary>
        /// Gets a random number using the given rule.
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        public int getRandom(IRandomizer<int> rule)
        {
            if (!randomRule.hasNumbersLeft())
            {
                if (autoReset)
                {
                    randomRule.Reset();
                    Debug.Log("No more numbers left, auto reseting!");
                }
                else
                {
                    throw (new System.Exception("No more numbers left, should use randomRule.Reset maybe?"));
                }
            }
            int number = Random.Range(min, max + 1);
            while (rule.IsBlocked(number))
            {
                number = Random.Range(min, max + 1);
            }
            rule.addBlock(number);
            return number;
        }
    }
}
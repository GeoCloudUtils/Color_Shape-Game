using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptUtils.GameUtils
{
    public interface IRandomizer<T>
    {
        /// <summary>
        /// Check if the element is valid
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        bool IsBlocked(T arg);
        /// <summary>
        /// Blocks an element
        /// </summary>
        /// <param name="arg"></param>
        void addBlock(T arg);
        /// <summary>
        /// Removes a block
        /// </summary>
        /// <param name="arg"></param>
        void removeBlock(T arg);
        /// <summary>
        /// Gets the lat blocked element.
        /// </summary>
        /// <returns></returns>
        T getLastBlock();
        /// <summary>
        /// Checks if there are any unblocked elements left.
        /// </summary>
        /// <returns></returns>
        bool hasNumbersLeft();
        /// <summary>
        /// Returns the number of unblocked elements.
        /// </summary>
        /// <returns></returns>
        int getNumbersLeft();
        /// <summary>
        /// Reset the rule.
        /// </summary>
        void Reset();
    }
}
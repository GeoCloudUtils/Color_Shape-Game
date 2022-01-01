using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptUtils.Common;
namespace ScriptUtils.GameUtils
{
    /// <summary>
    /// Explicitly defined dummy class for MultiDimensionalArray<GameObject>
    /// </summary>
    [System.Serializable]
    public class MultidimensionalArrSystem : MultiDimensionalArr<GameObject>
    {
        public MultidimensionalArrSystem(int size) : base(size)
        {

        }
    }
}
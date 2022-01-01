using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptUtils.Common;
namespace ScriptUtils.GameUtils
{
    public class GridRepG : MonoBehaviour
    {
        /// <summary>
        /// Default implementation of GridRepGeneric with a GameObject target type.
        /// </summary>
        public class GridRep : GridRGeneric<GameObject, MultidimensionalArrSystem>
        {

        }
    }
}
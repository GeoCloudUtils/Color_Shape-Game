using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptUtils
{
    public class ScalingBarSystem : MonoBehaviour
    {
        /// <summary>
        /// Replacemnt for ProgressBar
        /// Scales a given bar based on given precentage.
        /// </summary>
        public class ScalingBar : MonoBehaviour
        {
            public enum ScaleOn
            {
                X,
                Y
            }
            /// <summary>
            /// Current precentage
            /// </summary>
            public float precentage
            {
                get
                {
                    return _precentage;
                }
                set
                {
                    if (value < 0f)
                        _precentage = 0f;
                    else if (value > 100f)
                        _precentage = 100f;
                    else
                        _precentage = value;

                }

            }
            [SerializeField]
            [Range(0f, 100f)]
            private float _precentage = 0;
            /// <summary>
            /// Which axis to scale on
            /// </summary>
            public ScaleOn scaleType = ScaleOn.Y;
            /// <summary>
            /// used to store initial scale(might not be 0)
            /// </summary>
            private Dictionary<ScaleOn, float> originalScale = new Dictionary<ScaleOn, float>();
            /// <summary>
            /// Used to check when scale should be updated in Editor
            /// </summary>
            private float oldPrecentage = float.NegativeInfinity;
            /// <summary>
            /// Intialize originalScale
            /// </summary>
            private void Awake()
            {
                originalScale[ScaleOn.X] = transform.localScale.x;
                originalScale[ScaleOn.Y] = transform.localScale.y;
            }
            /// <summary>
            /// Calculate and apply scale based on precentage and original scale
            /// </summary>
            private void Update()
            {
                if (_precentage > 100f || _precentage < 0f)
                    throw (new System.ArgumentOutOfRangeException());
                if (oldPrecentage != _precentage)
                {
                    oldPrecentage = _precentage;
                    float newScale = (_precentage * originalScale[scaleType] / 100f);
                    Vector3 currentScale = transform.localScale;
                    if (scaleType == ScaleOn.X) currentScale.x = newScale;
                    else currentScale.y = newScale;
                    transform.localScale = currentScale;
                }
            }
            /// <summary>
            /// Calls <see cref="EduUtils.Visual.ScalingBar.calculatePrecentage(float, float)"/> 
            /// </summary>
            /// <param name="current"></param>
            /// <param name="total"></param>
            /// <returns></returns>
            public static float calculatePrecentage(int current, int total)
            {
                return calculatePrecentage((float)current, (float)total);
            }
            /// <summary>
            /// Calculate a precentage based on a given current value an maxValue
            /// </summary>
            /// <param name="current">Current Value</param>
            /// <param name="total">Max value</param>
            /// <returns></returns>
            public static float calculatePrecentage(float current, float total)
            {
                return (current * 100f) / total;
            }
        }
    }
}
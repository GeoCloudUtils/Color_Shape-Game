using UnityEngine;

namespace ScriptUtils.Visual
{
    /// <summary>
    /// Used to smooth out transitions. When distance is variable it will calcualte the needed duration of a transition
    /// to make tweens visualy consitent
    /// </summary>
    /// <summary>
    [System.Serializable]
    public class TimeDistanceCalculator : MonoBehaviour
    {      
        /// Caluclates distacne between paramters and calls the other construcor.
        /// </summary>
        /// <param name="refPoint1"></param>
        /// <param name="refPoint2"></param>
        /// <param name="time"></param>
        public TimeDistanceCalculator(Vector3 refPoint1, Vector3 refPoint2, float time) : this(Vector3.Distance(refPoint1, refPoint2), time) { }
        [SerializeField]
        private float _distance;
        [SerializeField]
        private float _time;
        public float Distance
        {
            get { return _distance; }
        }
        public float Time
        {
            get { return _time; }
        }
        /// <summary>
        /// All other constructors lead here.
        /// Sets the reference distance and time to be used to calculate future times
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="time"></param>
        public TimeDistanceCalculator(float distance, float time)
        {
            _distance = distance;
            _time = time;
        }
        public float calcTime(Vector3 point1, Vector3 point2)
        {
            return calcTime(Vector3.Distance(point1, point2));
        }
        /// <summary>
        /// Calcualte time needed based on set refenerce.
        /// </summary>
        /// <param name="newDistance"></param>
        /// <returns></returns>
        public float calcTime(float newDistance)
        {
            return (newDistance * _time) / _distance;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptUtils.GameUtils
{
    /// <summary>
    /// Alternative to Vector2, it uses int values.
    /// Mostly used in conjunction with <see cref="EduUtils.GameUtil.GridRepGenereic{T1, T2}"/>
    /// </summary>
    [System.Serializable]
    public class GridPoint : MonoBehaviour
    {
        public int x;
        public int y;

        public GridPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        /// <summary>
        /// Gives a string reperesentation of the instance without surrounding brackets.
        /// For ex: 1,2
        /// To be used in case given string might need to be parsed.
        /// </summary>
        /// <param name="raw"></param>
        /// <returns></returns>
        public string ToString(bool raw)
        {
            if (raw)
                return x + "," + y;
            return this.ToString();
        }
        /// <summary>
        /// Gives string repesentation of instance with surounding brackets.
        /// For ex: (1,2)
        /// Used to make string repesnetation more readable than "raw" format
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "(" + x + "," + y + ")";
        }
        /// <summary>
        /// Adds two Points
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>A new point isntace which has a value of (a.x+b.x,a.y+b.y)</returns>
        public static GridPoint operator +(GridPoint a, GridPoint b)
        {
            return new GridPoint(a.x + b.x, a.y + b.y);
        }

        /// <summary>
        /// Substracts two Points
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>A new point isntace which has a value of (a.x+b.x,a.y+b.y)</returns>
        public static GridPoint operator -(GridPoint a, GridPoint b)
        {
            return new GridPoint(a.x - b.x, a.y - b.y);
        }

        /// <summary>
        /// Compares two points
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>True if both a and b are null or a.x==b.x && a.y==b.y</returns>
        public static bool operator ==(GridPoint a, GridPoint b)
        {
            if (System.Object.ReferenceEquals(a, b))
                return true;
            bool aIsNull = false;
            bool bIsNull = false;
            try
            {
                a.ToString();
            }
            catch
            {
                aIsNull = true;
            }
            try
            {
                b.ToString();
            }
            catch
            {
                bIsNull = true;
            }
            if (aIsNull && bIsNull)
                return true;
            else if (aIsNull || bIsNull)
                return false;
            return a.x == b.x && a.y == b.y;
        }
        /// <summary>
        /// Same as !(a==b)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(GridPoint a, GridPoint b)
        {
            return !(a == b);
        }
        /// <summary>
        /// Same is ==
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }
            GridPoint b = obj as GridPoint;
            return this.x == b.x && this.y == b.y;
        }
        /// <summary>
        /// Same as ==
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool Equals(GridPoint b)
        {
            if (b == null)
            {
                return false;
            }
            return this.x == b.x && this.y == b.y;
        }
        /// <summary>
        /// Conversion operator to convert Point to Vector2
        /// </summary>
        /// <param name="p"></param>
        public static implicit operator UnityEngine.Vector2(GridPoint p)
        {
            return new UnityEngine.Vector2(p.x, p.y);
        }
        /// <summary>
        /// Creates a new Point by parsing the given string.
        /// Format ex: 1,2
        /// </summary>
        /// <param name="raw"></param>
        /// <returns></returns>
        public static GridPoint parseRaw(string raw)
        {
            string[] coordStrings = raw.Split(',');
            return new GridPoint(int.Parse(coordStrings[0]), int.Parse(coordStrings[1]));
        }
        /// <summary>
        /// Dummy implementation to get rid of warning
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ x;
        }
        /// <summary>
        /// Always returns a new Point with x=0 y=0
        /// </summary>
        public static GridPoint Zero
        {
            get
            {
                return new GridPoint(0, 0);
            }
        }
    }
}
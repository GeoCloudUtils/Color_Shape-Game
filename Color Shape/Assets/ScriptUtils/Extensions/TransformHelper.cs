using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptUtils.Extensions
{
    /// <summary>
    /// Transform helper class. It contains many helpful methods
    /// </summary>
    public static class TransformHelper
    {
        /// <summary>
        /// Get child by name
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Transform GetChildByName(this Transform transform, string name)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (child.name == name)
                    return transform.GetChild(i);
            }
            return null;
        }

        /// <summary>
        /// Trys to get attached component of type T, if no such component is attached, attaches one.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this GameObject target) where T : Component
        {
            T component = target.GetComponent<T>();
            if (component != null)
                return component;
            else
                return target.AddComponent<T>();
        }
        /// <summary>
        /// Get transform children
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static List<Transform> GetChildren(this Transform transform)
        {
            List<Transform> children = new List<Transform>();
            for (int i = 0; i < transform.childCount; i++)
                children.Add(transform.GetChild(i));
            return children;
        }
        /// <summary>
        /// Perfrom a 2D lookAt (Z axis is ignored)
        /// </summary>
        /// <param name="currentTransform"></param>
        /// <param name="target"></param>
        public static void LookAt2D(this Transform currentTransform, Transform target)
        {
            Vector3 dir = target.position - currentTransform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            currentTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        /// <summary>
        /// Perfrom a 2D lookAt (Z axis is ignored)
        /// </summary>
        /// <param name="currentTransform"></param>
        /// <param name="target"></param>
        public static void LookAt2D(this Transform currentTransform, Vector3 target)
        {
            Vector3 dir = target - currentTransform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            currentTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        /// <summary>
        /// Calculate a 2D lookAt rotation (Z axis is ignored)
        /// </summary>
        /// <param name="currentTransform"></param>
        /// <param name="target"></param>
        public static Vector3 GetLookRotaion2D(this Transform currentTransform, Transform target)
        {
            return GetLookRotaion2D(currentTransform, target.position);
        }
        /// <summary>
        /// Calculate a 2D lookAt rotation (Z axis is ignored)
        /// </summary>
        /// <param name="currentTransform"></param>
        /// <param name="target"></param>
        public static Vector3 GetLookRotaion2D(this Transform currentTransform, Vector3 target)
        {
            Vector3 dir = target - currentTransform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            return Quaternion.AngleAxis(angle, Vector3.forward).eulerAngles;
        }
        /// <summary>
        /// Applies a 2d rotation (Z axis only)
        /// </summary>
        /// <param name="currentTransform"></param>
        /// <param name="target"></param>
        public static void set2DRotation(this Transform currentTransform, float rotation)
        {
            Vector3 rot = currentTransform.eulerAngles;
            rot.z = rotation;
            currentTransform.eulerAngles = rot;
        }
        /// <summary>
        /// Gets a 2D rotation (float Z axis only)
        /// </summary>
        /// <param name="currentTransform"></param>
        /// <returns></returns>
        public static float get2DRotation(this Transform currentTransform)
        {
            Vector3 rot = currentTransform.eulerAngles;
            return rot.z;
        }
        /// <summary>
        /// Applies a 2d local rotation (Z axis only)
        /// </summary>
        /// <param name="currentTransform"></param>
        /// <param name="target"></param>
        public static void setLocal2DRotation(this Transform currentTransform, float rotation)
        {
            Vector3 rot = currentTransform.localEulerAngles;
            rot.z = rotation;
            currentTransform.localEulerAngles = rot;
        }
        /// <summary>
        /// Chnages transform.position.x (Shortcut function)
        /// </summary>
        /// <param name="currentTransform"></param>
        /// <param name="value"></param>
        public static void setX(this Transform currentTransform, float value)
        {
            Vector3 cPos = currentTransform.position;
            cPos.x = value;
            currentTransform.position = cPos;
        }
        /// <summary>
        /// Chnages transform.position.y (Shortcut function)
        /// </summary>
        /// <param name="currentTransform"></param>
        /// <param name="value"></param>
        public static void setY(this Transform currentTransform, float value)
        {
            Vector3 cPos = currentTransform.position;
            cPos.y = value;
            currentTransform.position = cPos;
        }
        /// <summary>
        /// Return children's as gameObject
        /// </summary>
        /// <param name="currentTransform"></param>
        /// <returns></returns>
        public static GameObject[] GetChildrenAsGameObjects(this Transform currentTransform)
        {
            GameObject[] children = new GameObject[currentTransform.childCount];
            for (int i = 0; i < currentTransform.childCount; i++)
                children[i] = currentTransform.GetChild(i).gameObject;
            return children;
        }
    }
}
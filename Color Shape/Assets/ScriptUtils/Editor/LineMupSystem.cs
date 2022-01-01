using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ScriptUtils.Editor
{
    /// <summary>
    /// Third party code used for adding menu items to help with alligning and distributing gameObjects
    /// </summary>
    public class LineMupSystem : MonoBehaviour
    {
        // align in the x translation axis
        [MenuItem("LineMup/Align/Translation X")]
        public static void AlignmentTransX()
        {
            // execute alignment for the x axis
            AlignOrDistribute(false, "transX");
        }

        // determine if the function can be executed.
        [MenuItem("LineMup/Align/Translation X", true)]
        public static bool ValidateAlignmentTransX()
        {
            // only return true if there is a transform in the selection.
            return (Selection.activeTransform != null);
        }

        // align in the y translation axis
        [MenuItem("LineMup/Align/Translation Y")]
        public static void AlignmentTransY()
        {
            // execute alignment for the y axis
            AlignOrDistribute(false, "transY");
        }

        // determine if the function can be executed.
        [MenuItem("LineMup/Align/Translation Y", true)]
        public static bool ValidateAlignmentTransY()
        {
            // only return true if there is a transform in the selection.
            return (Selection.activeTransform != null);
        }

        // align in the z translation axis
        [MenuItem("LineMup/Align/Translation Z")]
        public static void AlignmentTransZ()
        {
            // execute alignment for the z axis
            AlignOrDistribute(false, "transZ");
        }

        // determine if the function can be executed.
        [MenuItem("LineMup/Align/Translation Z", true)]
        public static bool ValidateAlignmentTransZ()
        {
            // only return true if there is a transform in the selection.
            return (Selection.activeTransform != null);
        }

        // align the rotation
        [MenuItem("LineMup/Align/Rotation")]
        public static void AlignmentRotation()
        {
            // execute alignment in all axes
            AlignOrDistribute(false, "rotAll");
        }

        // determine if the function can be executed.
        [MenuItem("LineMup/Align/Rotation", true)]
        public static bool ValidateAlignmentRotation()
        {
            // only return true if there is a transform in the selection.
            return (Selection.activeTransform != null);
        }

        // align in the x scale axis
        [MenuItem("LineMup/Align/Scale X")]
        public static void AlignmentScaleX()
        {
            // execute alignment for the x axis
            AlignOrDistribute(false, "scaleX");
        }

        // determine if the function can be executed.
        [MenuItem("LineMup/Align/Scale X", true)]
        public static bool ValidateAlignmentScaleX()
        {
            // only return true if there is a transform in the selection.
            return (Selection.activeTransform != null);
        }

        // align in the y scale axis
        [MenuItem("LineMup/Align/Scale Y")]
        public static void AlignmentScaleY()
        {
            // execute alignment for the y axis
            AlignOrDistribute(false, "scaleY");
        }

        // determine if the function can be executed.
        [MenuItem("LineMup/Align/Scale Y", true)]
        public static bool ValidateAlignmentScaleY()
        {
            // only return true if there is a transform in the selection.
            return (Selection.activeTransform != null);
        }

        // align in the z scale axis
        [MenuItem("LineMup/Align/Scale Z")]
        public static void AlignmentScaleZ()
        {
            // execute alignment for the z axis
            AlignOrDistribute(false, "scaleZ");
        }

        // determine if the function can be executed.
        [MenuItem("LineMup/Align/Scale Z", true)]
        public static bool ValidateAlignmentScaleZ()
        {
            // only return true if there is a transform in the selection.
            return (Selection.activeTransform != null);
        }

        // distribute in the x translation axis
        [MenuItem("LineMup/Distribute/Translation X")]
        public static void DistributeTransX()
        {
            // execute distribution for the x axis
            AlignOrDistribute(true, "transX");
        }

        // determine if the function can be executed.
        [MenuItem("LineMup/Distribute/Translation X", true)]
        public static bool ValidateDistributeTransX()
        {
            // only return true if there is a transform in the selection.
            return (Selection.activeTransform != null);
        }

        // distribute in the y translation axis
        [MenuItem("LineMup/Distribute/Translation Y")]
        public static void DistributeTransY()
        {
            // execute distribution for the y axis
            AlignOrDistribute(true, "transY");
        }

        // determine if the function can be executed.
        [MenuItem("LineMup/Distribute/Translation Y", true)]
        public static bool ValidateDistributeTransY()
        {
            // only return true if there is a transform in the selection.
            return (Selection.activeTransform != null);
        }

        // distribute in the z translation axis
        [MenuItem("LineMup/Distribute/Translation Z")]
        public static void DistributeTransZ()
        {
            // execute distribution for the z axis
            AlignOrDistribute(true, "transZ");
        }

        // determine if the function can be executed.
        [MenuItem("LineMup/Distribute/Translation Z", true)]
        public static bool ValidateDistributeTransZ()
        {
            // only return true if there is a transform in the selection.
            return (Selection.activeTransform != null);
        }

        // distribute the rotation
        [MenuItem("LineMup/Distribute/Rotation")]
        public static void DistributeRotation()
        {
            // execute distribution in all axes
            AlignOrDistribute(true, "rotAll");
        }

        // determine if the function can be executed.
        [MenuItem("LineMup/Distribute/Rotation", true)]
        public static bool ValidateDistributeRotation()
        {
            // only return true if there is a transform in the selection.
            return (Selection.activeTransform != null);
        }

        // distribute in the x scale axis
        [MenuItem("LineMup/Distribute/Scale X")]
        public static void DistributeScaleX()
        {
            // execute distribution for the x axis
            AlignOrDistribute(true, "scaleX");
        }

        // determine if the function can be executed.
        [MenuItem("LineMup/Distribute/Scale X", true)]
        public static bool ValidateDistributeScaleX()
        {
            // only return true if there is a transform in the selection.
            return (Selection.activeTransform != null);
        }

        // distribute in the y scale axis
        [MenuItem("LineMup/Distribute/Scale Y")]
        public static void DistributeScaleY()
        {
            // execute distribution for the y axis
            AlignOrDistribute(true, "scaleY");
        }

        // determine if the function can be executed.
        [MenuItem("LineMup/Distribute/Scale Y", true)]
        public static bool ValidateDistributeScaleY()
        {
            // only return true if there is a transform in the selection.
            return (Selection.activeTransform != null);
        }

        // distribute in the z scale axis
        [MenuItem("LineMup/Distribute/Scale Z")]
        public static void DistributeScaleZ()
        {
            // execute distribution for the z axis
            AlignOrDistribute(true, "scaleZ");
        }

        // determine if the function can be executed.
        [MenuItem("LineMup/Distribute/Scale Z", true)]
        public static bool ValidateDistributeScaleZ()
        {
            // only return true if there is a transform in the selection.
            return (Selection.activeTransform != null);
        }

        private static int sortBySiblingIndex(Transform t1, Transform t2)
        {
            if (t1.GetSiblingIndex() == t2.GetSiblingIndex())
                return 0;
            if (t1.GetSiblingIndex() < t2.GetSiblingIndex())
                return -1;
            else
                return 1;
        }

        public static void AlignOrDistribute(bool shouldDist, string theAxis)
        {

            // create some variables to store values
            Transform[] workingArray = Selection.transforms;
            Array.Sort<Transform>(workingArray, sortBySiblingIndex);
            Transform firstObj = workingArray[0];
            Transform furthestObj = firstObj;
            float firstVal = 0.0f;
            float furthestVal = 0.0f;
            float curDist = 0.0f;
            float lastDist = 0.0f;
            float selCount = 0;

            // collect the number of tranforms in the selection and find the object that is furthest away from the active selected object
            foreach (Transform transform in workingArray)
            {
                // collect the current distance
                curDist = Vector3.Distance(firstObj.position, transform.position);

                // get the object with the greatest distance from the first selected object
                if (curDist > lastDist)
                {
                    furthestObj = transform;
                    lastDist = curDist;
                }

                // increment count
                selCount += 1;
            }

            // distribute or align?
            if (shouldDist)
            {
                // collect the first value and furthest value to distribute between
                switch (theAxis)
                {
                    case "transX":
                        firstVal = firstObj.position.x;
                        furthestVal = furthestObj.position.x;
                        break;
                    case "transY":
                        firstVal = firstObj.position.y;
                        furthestVal = furthestObj.position.y;
                        break;
                    case "transZ":
                        firstVal = firstObj.position.z;
                        furthestVal = furthestObj.position.z;
                        break;
                    case "scaleX":
                        firstVal = firstObj.localScale.x;
                        furthestVal = furthestObj.localScale.x;
                        break;
                    case "scaleY":
                        firstVal = firstObj.localScale.y;
                        furthestVal = furthestObj.localScale.y;
                        break;
                    case "scaleZ":
                        firstVal = firstObj.localScale.z;
                        furthestVal = furthestObj.localScale.z;
                        break;
                    default:
                        break;
                }

                // calculate the spacing for the distribution
                float objSpacing = (firstVal - furthestVal) / (selCount - 1);
                float curSpacing = objSpacing;
                float rotSpacing = 1.0f / (selCount - 1);
                float curRotSpacing = rotSpacing;

                // update every object in the selection to distribute evenly
                foreach (Transform transform in workingArray)
                {
                    //Debug.Log(transform.name);
                    Undo.RecordObject(transform, "Move transform");
                    Vector3 newPosition = transform.position;
                    Quaternion newRotation = transform.rotation;
                    Vector3 newlocalScale = transform.localScale;
                    switch (theAxis)
                    {
                        case "transX":
                            if ((transform != firstObj) && (transform != furthestObj))
                            {
                                newPosition.x = firstVal - curSpacing;
                                curSpacing += objSpacing;
                            }
                            break;
                        case "transY":
                            if ((transform != firstObj) && (transform != furthestObj))
                            {
                                newPosition.y = firstVal - curSpacing;
                                curSpacing += objSpacing;
                            }
                            break;
                        case "transZ":
                            if ((transform != firstObj) && (transform != furthestObj))
                            {
                                newPosition.z = firstVal - curSpacing;
                                curSpacing += objSpacing;
                            }
                            break;
                        case "rotAll":
                            if ((transform != firstObj) && (transform != furthestObj))
                            {
                                newRotation = Quaternion.Slerp(firstObj.rotation, furthestObj.rotation, curRotSpacing);
                                curRotSpacing += rotSpacing;
                            }
                            break;
                        case "scaleX":
                            if ((transform != firstObj) && (transform != furthestObj))
                            {
                                newlocalScale.x = firstVal - curSpacing;
                                curSpacing += objSpacing;
                            }
                            break;
                        case "scaleY":
                            if ((transform != firstObj) && (transform != furthestObj))
                            {
                                newlocalScale.y = firstVal - curSpacing;
                                curSpacing += objSpacing;
                            }
                            break;
                        case "scaleZ":
                            if ((transform != firstObj) && (transform != furthestObj))
                            {
                                newlocalScale.z = firstVal - curSpacing;
                                curSpacing += objSpacing;
                            }
                            break;
                        default:
                            break;
                    }
                    transform.position = newPosition;
                    transform.rotation = newRotation;
                    transform.localScale = newlocalScale;
                }
            }
            else
            {
                // snap every object in the selection to the first objects value
                foreach (var transform in workingArray)
                {
                    Undo.RecordObject(transform, "Move transform");
                    Vector3 newPosition = transform.position;
                    Quaternion newRotation = transform.rotation;
                    Vector3 newlocalScale = transform.localScale;
                    switch (theAxis)
                    {
                        case "transX":
                            newPosition.x = firstObj.position.x;
                            break;
                        case "transY":
                            newPosition.y = firstObj.position.y;
                            break;
                        case "transZ":
                            newPosition.z = firstObj.position.z;
                            break;
                        case "rotAll":
                            newRotation = firstObj.rotation;
                            break;
                        case "scaleX":
                            newlocalScale.x = firstObj.localScale.x;
                            break;
                        case "scaleY":
                            newlocalScale.y = firstObj.localScale.y;
                            break;
                        case "scaleZ":
                            newlocalScale.z = firstObj.localScale.z;
                            break;
                        default:
                            break;
                    }
                    transform.position = newPosition;
                    transform.rotation = newRotation;
                    transform.localScale = newlocalScale;
                }
            }
        }
    }
}
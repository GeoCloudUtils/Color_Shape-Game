using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ScriptUtils.Common;

namespace EduUtils.Events
{
    /// <summary>
    /// Used for monitoring mouse events and dispatching them to IMouseEventSystem implementations when necessary.
    /// This class follows the singelton pattern.
    /// </summary>
    public class MouseEventProccessor : MonoBehaviour
    {
        private static MouseEventProccessor _instance = null;
        /// <summary>
        /// Holds current instance of MouseEventProcessor
        /// </summary>
        public static MouseEventProccessor Instance
        {
            get
            {
                if (_instance == null)
                    createInstance();
                return _instance;
            }
        }
        /// <summary>
        /// Used tod etermine wether mouse move events are being tracked. (mouse over, mouse out, etc).
        /// This setting is global so it should be used with care.
        /// </summary>
        public bool captureMouseMouveEvents = false;
        private bool _captureEvents = true;
        /// <summary>
        /// Gets or sets wether mouse events are being monitored.
        /// </summary>
        public bool captureEvents
        {
            get
            {
                return _captureEvents;
            }
            set
            {
                _captureEvents = value;
            }
        }
        /// <summary>
        /// The implementation of IMouseEvnetSystem that was last under the mouse pointer.
        /// </summary>
        private IMouseEventSystem lastHit;
        /// <summary>
        /// Creates a new gameObject and attaches MouseEventProcessor to it, saves the newly created instance to <seealso cref="EduUtils.Events.MouseEventProccessor.Instance"/>
        /// </summary>
        public static void createInstance()
        {
            GameObject myObject = new GameObject("MOUSE_EVENT_PROCCESSOR");
            _instance = myObject.AddComponent<MouseEventProccessor>();
        }
        /// <summary>
        /// Main mouse monitoring logic.
        /// if captureEvents is flase no code is ran.
        /// Uses <seealso cref="EduUtils.Visual.EDUCam"/> to determine current mouse position on screen and performs a physics
        /// raycast to all objects on screen and dispatches events as necessary based on current mouse state and <seealso cref="EduUtils.Events.MouseEventProccessor.captureMouseMouveEvents"/>
        /// </summary>
        public void Update()
        {
            if (!captureEvents)
                return;
#if UNITY_ANDROID && !UNITY_EDITOR
            if (Input.touchCount == 0)
                return;
#endif
#if UNITY_STANDALONE || !USE_MULTI_TOUCH
            Vector3 position = Input.mousePosition;
            CheckPosition(position);
#endif
#if UNITY_ANDROID && USE_MULTI_TOUCH
            foreach (Touch touch in Input.touches)
                CheckPosition(touch.position);
#endif
        }

        private void CheckPosition(Vector3 position)
        {
            bool dispatchedButtonEvent = false;
#if !STUPID_MOUSE
            if (Input.GetMouseButtonDown(0))
#else
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
#endif
            {

                Ray ray = Camera.main.ScreenPointToRay(position);
                RaycastHit2D hit = getHit(ray.origin, ray.direction);
                if (checkHit(hit))
                {
                    dispatchedButtonEvent = true;
                    dispatchOnGameObject(hit.collider.gameObject, MouseEventType.DOWN);
                }
            }
#if !STUPID_MOUSE
            if (Input.GetMouseButtonUp(0))
#else
            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2))
#endif
            {
                Ray ray = Camera.main.ScreenPointToRay(position);
                RaycastHit2D hit = getHit(ray.origin, ray.direction);
                if (checkHit(hit))
                {
                    dispatchedButtonEvent = true;
                    dispatchOnGameObject(hit.collider.gameObject, MouseEventType.UP);
                    dispatchOnGameObject(hit.collider.gameObject, MouseEventType.CLICK);
                }
            }
            if (captureMouseMouveEvents)
            {
                if (!dispatchedButtonEvent)
                {
#if !STUPID_MOUSE
                    if (Input.GetMouseButton(0))
#else
                    if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
#endif
                    {
                        Vector3 pressedPosition = position;
                        Ray pressedRay = Camera.main.ScreenPointToRay(pressedPosition);
                        RaycastHit2D pressedHit = getHit(pressedRay.origin, pressedRay.direction);
                        if (checkHit(pressedHit))
                        {
                            dispatchedButtonEvent = true;
                            dispatchOnGameObject(pressedHit.collider.gameObject, MouseEventType.PRESSED);
                        }
                    }
                }
                Ray ray = Camera.main.ScreenPointToRay(position);
                RaycastHit2D hit = getHit(ray.origin, ray.direction);
                if (checkHit(hit))
                {
                    IMouseEventSystem system = hit.collider.gameObject.GetComponent<IMouseEventSystem>();
                    if (system != null)
                    {
                        if (lastHit != null)
                        {
                            if (lastHit != system)
                            {
                                if (lastHit.canProces)
                                {
                                    lastHit.entered = false;
                                    dispatchOnGameObject((lastHit as Component).gameObject, MouseEventType.EXIT);
                                    //lastHit.dispatchMouse(MouseEventType.EXIT);
                                }
                                else
                                {
                                    lastHit = null;
                                }
                            }
                        }

                        if (lastHit != system)
                        {
                            lastHit = system;
                            system.entered = true;
                            dispatchOnGameObject((system as Component).gameObject, MouseEventType.ENTER);
                            //system.dispatchMouse(MouseEventType.ENTER);
                        }
                        else
                        {
                            dispatchOnGameObject((system as Component).gameObject, MouseEventType.OVER);
                            //system.dispatchMouse(MouseEventType.OVER);
                        }
                    }
                    else
                    {
                        if (lastHit != null)
                        {
                            if (lastHit.canProces)
                            {
                                lastHit.entered = false;
                                lastHit.dispatchMouse(MouseEventType.EXIT);
                            }
                            lastHit = null;
                        }
                    }
                }
                else
                {
                    if (lastHit != null)
                    {
                        if (lastHit.canProces)
                        {
                            lastHit.entered = false;
                            lastHit.dispatchMouse(MouseEventType.EXIT);
                        }
                        lastHit = null;
                    }
                }
            }

        }
        /// <summary>
        /// Dispatches an event to all IMouseEventSystem implementations attached to a GameObject
        /// </summary>
        /// <param name="target"></param>
        /// <param name="mouseEventType"></param>
        private void dispatchOnGameObject(GameObject target, MouseEventType mouseEventType)
        {
            IMouseEventSystem[] systems = target.GetComponents<IMouseEventSystem>();
            foreach (IMouseEventSystem system in systems)
            {
                if (system != null)
                {
                    if(system.canProces && (system as Behaviour).enabled)
                        system.dispatchMouse(mouseEventType);
                }
            }
        }
        /// <summary>
        /// Preforms a Physics2D.RaycastAll, either returns an empty RaycastHit2D or the topmost objects hit(topmost determination is defined by <seealso cref="EduUtils.Events.MouseEventProccessor.compareRenderOrder(RaycastHit2D, RaycastHit2D)"/>
        /// Parameters are determined internaly using the current mouse position.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        RaycastHit2D getHit(Vector3 origin, Vector3 direction)
        {
            RaycastHit2D[] allHits = Physics2D.RaycastAll(origin, direction);
            if (allHits.Length == 0)
                return new RaycastHit2D();
            System.Array.Sort<RaycastHit2D>(allHits, compareRenderOrder);
            return allHits[allHits.Length - 1];
        }
        /// <summary>
        /// Firstly makes sure hits that are empty  or do not have IMouseEventSystem implementations attached (and there for not actual hits) have the lowest Z priority.
        /// After intial filtering gameObjects with attached sprite renderers are givven higher priority.
        /// gameObjects with attached SpriteRenderersare given Z prioerty based on render layers and sorting orders.
        /// Render layer names should be formated as: AnyNameHere{ZZ} where ZZ is a single or double digit number.
        /// </summary>
        /// <param name="hit1"></param>
        /// <param name="hit2"></param>
        /// <returns></returns>
        private int compareRenderOrder(RaycastHit2D hit1, RaycastHit2D hit2)
        {
            if (hit1.collider == null || hit2.collider == null)
            {
                if (hit1.collider == null && hit2.collider == null)
                    return 0;
                else
                {
                    if (hit1.collider == null)
                        return -1;
                    else
                        return 1;
                }
            }
            IMouseEventSystem system1 = hit1.collider.gameObject.GetComponent<IMouseEventSystem>();
            IMouseEventSystem system2 = hit2.collider.gameObject.GetComponent<IMouseEventSystem>();
            if (system1 == null || system2 == null)
            {
                if (system1 == null && system2 == null)
                    return 0;
                else
                {
                    if (system1 == null)
                        return -1;
                    else
                    {
                        return 1;
                    }
                }
            }
            SpriteRenderer sprite1 = hit1.collider.gameObject.GetComponent<SpriteRenderer>();
            SpriteRenderer sprite2 = hit2.collider.gameObject.GetComponent<SpriteRenderer>();
            if (sprite1 == null || sprite2 == null)
            {
                if (sprite1 == null && sprite2 == null)
                    return 0;
                else
                {
                    if (sprite1 == null)
                        return -1;
                    else
                        return 1;
                }
            }
            int indexFromName1 = sprite1.sortingLayerName == "Default" ? -1 : getIntFromEnd(sprite1.sortingLayerName);
            int indexFromName2 = sprite2.sortingLayerName == "Default" ? -1 : getIntFromEnd(sprite2.sortingLayerName);
            if (indexFromName1 == indexFromName2)
            {
                if (sprite1.sortingOrder == sprite2.sortingOrder)
                    return 0;
                else
                {
                    if (sprite1.sortingOrder < sprite2.sortingOrder)
                        return -1;
                    else
                        return 1;
                }
            }
            else
            {
                if (indexFromName1 < indexFromName2)
                {
                    return -1;
                }
                else
                    return 1;
            }
        }
        /// <summary>
        /// Get Z priority from render layer name.
        /// Render layer names should be formated as: AnyNameHere{ZZ} where ZZ is a single or double digit number.
        /// The higher ZZ is the higher priority is.
        /// </summary>
        /// <param name="sortingLayerName"></param>
        /// <returns></returns>
        private int getIntFromEnd(string sortingLayerName)
        {
            return 0;
        }
        /// <summary>
        /// Generic cleanupo
        /// </summary>
        private void OnDestroy()
        {
            _instance = null;
        }
        /// <summary>
        /// Checks ifg a Raycast2DHit is an actual hit.
        /// </summary>
        /// <param name="hit"></param>
        /// <returns></returns>
        bool checkHit(RaycastHit2D hit)
        {
            if(hit.collider!=null)
            {
                return hit.collider.gameObject.activeInHierarchy;
            }
            return false;
        }
    }
}
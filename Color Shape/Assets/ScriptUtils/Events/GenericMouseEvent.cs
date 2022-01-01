using UnityEngine;
using System.Collections;
namespace EduUtils.Events
{
    /// <summary>
    /// Generic base class for all IMouseEventSystem implementations
    /// </summary>
    /// <typeparam name="T">Type of parameter of MouseDelegate (and therefor event param). This should either be a subclass of Component or GameObject.</typeparam>
    [System.Serializable]
    public class GenericMouseEvent<T> : MonoBehaviour, IMouseEventSystem where T : UnityEngine.Object
    {
        private const float DOUBLE_CLICK_DELAY = 0.5f;
        /// <summary>
        /// Delegate used for dispatching events.
        /// </summary>
        /// <param name="target">The gameObject or attached component of type T </param>
        /// <param name="type">The type of event that occured</param>
        public delegate void MouseDelegate(T target, MouseEventType type);
        /// <summary>
        /// The actual event
        /// </summary>
        public event MouseDelegate MouseEvent;
        private bool _entered=false;
		private bool _canProces=true;
        /// <summary>
        /// Wether mouse has entered the area of target. Is reset to false when mouse exits.
        /// </summary>
        public bool entered
        {
            get
            {
                return _entered;
            }
            set
            {
                _entered = value;
            }
        }
        /// <summary>
        /// Gets or sets if this instnace can process events (in general)
        /// </summary>
		public bool canProces
		{
			get
			{
				return _canProces;
			}
            set
            {
                _canProces = value;
            }
		}

        public bool hasDoubleClick = false;

        /// <summary>
        /// The attached Collider2D instance
        /// </summary>
        protected Collider2D spriteCollider;
        /// <summary>
        /// The target that will be used when dispathing event.
        /// </summary>
        protected T actualTarget;
        /// <summary>
        /// Last Time.time  when CLICK was dispatched used to check if second click is "fast" enough to be cosnidered a double click
        /// </summary>
        private float lastClick;
        /// <summary>
        /// Used to determine wether click was dispatched before, used for double click
        /// </summary>
        private bool clickedOnce=false;
        /// <summary>
        /// Initializez the instnace; checks for MouseEventProcessor instance, create one if none exist;
        /// determines <seealso cref="EduUtils.Events.GenericMouseEvent{T}.actualTarget"/>
        /// </summary>
        protected virtual void Awake()
        {
            if (MouseEventProccessor.Instance == null)
                MouseEventProccessor.createInstance();
            spriteCollider = gameObject.GetComponent<Collider2D>();
            if (spriteCollider==null)
            {
                Debug.LogWarning("2D Collider not found("+gameObject.name+")!");
            }
            if (typeof(T) == typeof(GameObject))
            {
                actualTarget = gameObject as T;
            }
            else
            {
                actualTarget = gameObject.GetComponent<T>();
            }
        }
        /// <summary>
        /// Called by the MouseEventProcessor to dispatch an event.
        /// </summary>
        /// <param name="type"></param>
        public void dispatchMouse(MouseEventType type)
        {
            if (!MouseEventProccessor.Instance.captureEvents || !this.enabled)
                return;
            if(type==MouseEventType.CLICK)
            {
                if (hasDoubleClick)
                {
                    if (!clickedOnce)
                    {
                        clickedOnce = true;
                        lastClick = Time.time;
                    }
                    else
                    {
                        if (Time.time - lastClick <= DOUBLE_CLICK_DELAY)
                        {
                            type = MouseEventType.DOUBLE_CLICK;
                        }
                        else
                        {
                            lastClick = Time.time;
                        }
                    }
                }
            }
            if(MouseEvent!=null && gameObject!=null)
                MouseEvent(actualTarget,type);
        }
        /// <summary>
        /// Ensures instacne does not try to process events after it Destroy is called but before garbage collector cleans up.
        /// </summary>
		private void OnDestroy()
		{
			_canProces = false;
		}
        /// <summary>
        /// Remove all events.
        /// </summary>
        public void RemoveAllListeners()
        {
            MouseEvent = null;
        }
        /*
        #if UNITY_ANDROID
        private void OnMouseUp()
        {
            if (!MouseEventProccessor.Instance.captureEvents)
                return;
            if (MouseEvent != null)
            {
                MouseEvent(actualTarget, MouseEventType.UP);
                MouseEvent(actualTarget, MouseEventType.CLICK);
            }
        }

        private void OnMouseOver()
        {
            if (!MouseEventProccessor.Instance.captureEvents)
                return;
            if(MouseEvent!=null)
                MouseEvent(actualTarget,MouseEventType.OVER);
        }

        private void OnMouseEnter()
        {
            if (!MouseEventProccessor.Instance.captureEvents)
                return;
            if(MouseEvent!=null)
                MouseEvent(actualTarget,MouseEventType.ENTER);
        }

        private void OnMouseExit()
        {
            if (!MouseEventProccessor.Instance.captureEvents)
                return;
            if(MouseEvent!=null)
                MouseEvent(actualTarget,MouseEventType.EXIT);
        }

        private void OnMouseDown()
        {
            if (!MouseEventProccessor.Instance.captureEvents)
                return;
            if(MouseEvent!=null)
                MouseEvent(actualTarget,MouseEventType.DOWN);
        }
        #endif*/

        /*private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 position=EDUCam.Instance.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(position);
                RaycastHit2D hit=Physics2D.Raycast(ray.origin,ray.direction);
                if(hit.collider==spriteCollider)
                {
                   
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                Vector3 position=EDUCam.Instance.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(position);
                RaycastHit2D hit=Physics2D.Raycast(ray.origin,ray.direction);
                if(hit.collider==spriteCollider)
                {
                    if(MouseEvent!=null)
                    {
                        MouseEvent(gameObject,MouseEventType.UP);
                        MouseEvent(gameObject, MouseEventType.CLICK);
                    }
                }
            }

            if (captureMouseMouveEvents)
            {
                Vector3 position=EDUCam.Instance.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(position);
                RaycastHit2D hit=Physics2D.Raycast(ray.origin,ray.direction);
                if(hit.collider==spriteCollider)
                {
                    if(!entered)
                    {
                        if(MouseEvent!=null)
                            MouseEvent(gameObject,MouseEventType.ENTER);
                        entered=true;
                    }
                    else
                    {
                        if(MouseEvent!=null)
                            MouseEvent(gameObject,MouseEventType.OVER);
                    }
                }
                else
                {
                    if(entered)
                    {
                        entered=false;
                    }
                    else
                    {
                        if(MouseEvent!=null)
                            MouseEvent(gameObject,MouseEventType.EXIT);
                    }
                }

            }
        }*/
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptUtils
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class ObjectSequence : MonoBehaviour
    {
        /// <summary>
        /// This script will construct an array with the direct children of the gameObject it is attached to and save that array
        /// in the field called sprites.
        /// Then based on the CurrentSpriteIndex property it will make sure that only the GameObject at the given index is active.
        /// Also supports showing a random index each time Start is called.
        /// </summary>
      
            public delegate void CurrentChildDelegate(ObjectSequence target, uint index);
            /// <summary>
            /// Event to be used to notify other scripts that the current index has changed.
            /// </summary>
            public event CurrentChildDelegate CurrentChildChanged;
            /// <summary>
            /// Array containing all DIRECT children
            /// </summary>
            public GameObject[] children;
            /// <summary>
            /// Underlying field for CurrentSpriteIndex property.
            /// Also used to make component functional in Editor
            /// </summary>
            [SerializeField]
            private uint _currentChild = 0;
            /// <summary>
            /// Used to determine when an update should take place
            /// </summary>
            private uint _actualCurrentChild = 0;
            public string NameOfComponentToSave = "Transform";
            public Component[] SavedComponents;
            /// <summary>
            /// If set to true each time Start is called a random gameObject will be shown
            /// </summary>
            public bool startRandom = false;
            public GameObject CurrentChild
            {
                get
                {
                    return children[_currentChild];
                }
            }

            /// <summary>
            /// Gets or sets the index to be displayed.
            /// Also triggers CurrentSpriteChanged and updates which gameObjects are visible
            /// </summary>
            public uint CurrentChildIndex
            {
                get { return _currentChild; }
                set
                {
                    _currentChild = value;
                    if (_currentChild >= children.Length)
                    {
                        _currentChild = (uint)children.Length - 1;
                        Debug.LogWarning("Cannot set current sprite index to " + value + ", using " + _currentChild + " instead!");
                    }
                    if (_currentChild < 0)
                    {
                        _currentChild = 0;
                        Debug.LogWarning("Cannot set current sprite index to negative number, using 0 instead!");
                    }
                    _actualCurrentChild = _currentChild;
                    renderCurrentChild();
                    DispatchChangeEvent();
                }
            }
            /// <summary>
            /// Intialize spritearray if needed
            /// Will only be included in Editor
            /// </summary>
            private void Awake()
            {

            }
            /// <summary>
            /// RendersCurrentSprite
            /// </summary>
            private void Start()
            {
                Update();
                if (startRandom)
                {
                    setRandomCurrentChildIndex();
                }
                renderCurrentChild();
            }

            public void setRandomCurrentChildIndex()
            {
                setCurrentChildIndex(Random.Range(0, children.Length));
            }

            /// <summary>
            /// Utility function to avoid conversion problems beween uint and int
            /// </summary>
            /// <param name="spriteIndex"></param>
            public void setCurrentChildIndex(int spriteIndex)
            {
                this.CurrentChildIndex = (uint)spriteIndex;
            }
            /// <summary>
            /// Utility function to avoid conversion problems beween uint and int
            /// </summary>
            /// <param name="spriteIndex"></param>
            public void setCurrentChildIndex(uint spriteIndex)
            {
                this.CurrentChildIndex = spriteIndex;
            }

            public T GetSavedComponentAt<T>(int index) where T : Component
            {
                int actualIndex = index;
                if (index >= children.Length)
                {
                    actualIndex = children.Length - 1;
                    Debug.LogWarning("Cannot get saved component " + index + ", using " + actualIndex + " instead!");
                }
                if (!(SavedComponents[actualIndex] is T))
                {
                    Debug.LogError("Type missmatch, returning null");
                }
                return SavedComponents[actualIndex] as T;
            }

            public T GetCurrentSavedComponent<T>() where T : Component
            {
                return GetSavedComponentAt<T>((int)CurrentChildIndex) as T;
            }
            /// <summary>
            /// Show the gameObject based on _currentSprite
            /// </summary>
            void renderCurrentChild()
            {
                foreach (GameObject sprite in children)
                {
                    sprite.SetActive(false);
                }
                children[_currentChild].SetActive(true);
            }

            // Update is called once per frame
            /// <summary>
            /// utility logic in order to render changes made to _currentSprite indierectly trough Editor
            /// </summary>
            protected void Update()
            {
                if (_actualCurrentChild != _currentChild)
                {
                    if (_currentChild >= children.Length)
                        _currentChild = (uint)children.Length - 1;
                    if (_currentChild < 0)
                        _currentChild = 0;
                    renderCurrentChild();
                    _actualCurrentChild = _currentChild;
                }
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    bool needToPopulate = false;
                    //Debug.Log("Runing editor code!");
                    if (children == null || SavedComponents == null)
                    {
                        needToPopulate = true;
                    }
                    else
                    {
                        if (children.Length == 0 || SavedComponents.Length == 0)
                            needToPopulate = true;
                    }
                    if (needToPopulate)
                    {
                        List<GameObject> children = new List<GameObject>();
                        foreach (Transform child in transform)
                        {
                            children.Add(child.gameObject);
                        }
                        SavedComponents = new Component[children.Count];
                        for (int i = 0; i < children.Count; i++)
                        {
                            Component[] allComponents = children[i].GetComponents<Component>();
                            foreach (Component component in allComponents)
                            {
                                if (component.GetType().Name == NameOfComponentToSave)
                                {
                                    SavedComponents[i] = component;
                                }
                            }
                        }
                        this.children = children.ToArray();
                        Debug.Log("Population done!");
                    }
                }
#endif
            }
            /// <summary>
            /// Triggers CurrentSpriteChanged
            /// </summary>
            void DispatchChangeEvent()
            {
                if (CurrentChildChanged != null)
                    CurrentChildChanged(this, _currentChild);
            }
        }
}
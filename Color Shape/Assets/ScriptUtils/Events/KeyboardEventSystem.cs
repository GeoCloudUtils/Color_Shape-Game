using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace EduUtils.Events
{
    /// <summary>
    /// Used for procession keyboard events on standalone platforms.
    /// Also provides support for custom key mappings.
    /// This class follows the singelton pattern.
    /// </summary>
    public class KeyboardEventSystem : MonoBehaviour
    {
        /// <summary>
        /// Dictionary used to retain keys user is pressing/holding down.
        /// </summary>
        private Dictionary<KeyCode, bool> _isPressed=new Dictionary<KeyCode, bool>();
        /// <summary>
        /// Used for storeing key mappints. Structure is mappedKey=>originalKey for example: A=>Left Arrow
        /// </summary>
        private Dictionary<KeyCode, KeyCode> map= new Dictionary<KeyCode,KeyCode>();
        /// <summary>
        /// Keys for witch events will dispatched for.
        /// </summary>
        private List<KeyCode> keysToCheck = new List<KeyCode>();
        /// <summary>
        /// Used to hold current instance.
        /// </summary>
        private static KeyboardEventSystem instance;
        /// <summary>
        /// Delegate used for events
        /// </summary>
        /// <param name="keyboardEventType">Event type</param>
        /// <param name="keyCode"></param>
        public delegate void KeyboardDelegate(KeyboardEventType keyboardEventType, KeyCode keyCode);
        public event KeyboardDelegate KeyBoardEvent;
        /// <summary>
        /// Returns or creates an isntance of this class.
        /// </summary>
        /// <returns></returns>
        public static KeyboardEventSystem getInstance()
        {
            if (instance == null)
            {
                GameObject temp = new GameObject("KEYBOARD_EVENT_SYSTEM");
                instance = temp.AddComponent<KeyboardEventSystem>();
            }
            return instance;
        }

        public static void reset()
        {
            if (instance != null)
                DestroyImmediate(instance.gameObject);
        }
        /// <summary>
        /// Generic clean uo tasks.
        /// </summary>
        private void OnDestroy()
        {
            KeyBoardEvent = null;
            instance = null;
        } 
        /// <summary>
        /// Add a key for monitoring.
        /// </summary>
        /// <param name="keyCode"></param>
        public void addKey(KeyCode keyCode)
        {
            _isPressed.Add(keyCode, false);
            keysToCheck.Add(keyCode);
        }
        /// <summary>
        /// Add a list of keys for monitoring.
        /// </summary>
        /// <param name="keyCodes"></param>
        public void addKey(KeyCode[] keyCodes)
        {
            foreach (KeyCode keyCode in keyCodes)
            {
                addKey(keyCode);
            }
        }
        /// <summary>
        /// Add all keys from the KeyCode collection for monitoring. (not recomended)
        /// </summary>
        public void addAllKeys()
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                this.addKey(keyCode);
        }
        /// <summary>
        /// Remove key from the list of monitored keys.
        /// </summary>
        /// <param name="keyCode"></param>
        public void removeKey(KeyCode keyCode)
        {
            _isPressed.Remove(keyCode);
            keysToCheck.Remove(keyCode);
        }
        /// <summary>
        /// Dispatch event with type KeyboardEventType.UP
        /// </summary>
        /// <param name="keyCode"></param>
        private void dispatchKeyUp(KeyCode keyCode)
        {
            if (KeyBoardEvent != null)
                KeyBoardEvent(KeyboardEventType.UP, keyCode);
        }
        /// <summary>
        /// Dispatch event with type KeyboardEventType.DOWN
        /// </summary>
        /// <param name="keyCode"></param>
        private void dispatchKeyDown(KeyCode keyCode)
        {
            if (KeyBoardEvent != null)
                KeyBoardEvent(KeyboardEventType.DOWN, keyCode);
        }
        /// <summary>
        /// Dispatch event with type KeyboardEventType.PRESSED
        /// </summary>
        /// <param name="keyCode"></param>
        private void dispatchPressed(KeyCode keyCode)
        {
            if (KeyBoardEvent != null)
                KeyBoardEvent(KeyboardEventType.PRESSED, keyCode);
        }
        /// <summary>
        /// Checks if a key is pressed, respects key mappings(original key is conisdered pressed if any of the keys mapped to it are pressed).
        /// </summary>
        /// <param name="keyCode"></param>
        /// <returns></returns>
        public bool isPressed(KeyCode keyCode)
        {
            if(map.ContainsValue(keyCode))
            {
                return _isPressed[(from p in map where p.Value == keyCode select p.Key).FirstOrDefault()];
            }
            if (_isPressed.ContainsKey(keyCode))
            {
                return _isPressed[keyCode];
            }
            else
            {
                Debug.LogWarning("You are trying to check that status of:" + keyCode + " but this key is not being monitored!");
            }
            return false;
        }
        /// <summary>
        /// Maps a key to a nother key.
        /// </summary>
        /// <param name="originalKey">Key actualy pressed (should not be monitorred since this will break mapping)</param>
        /// <param name="mappedKey">Key its mapped to (must be monitorred)</param>
        public void addMap(KeyCode originalKey, KeyCode mappedKey)
        {
            if (keysToCheck.Contains(originalKey))
            {
                Debug.LogWarning("Mapping unsuccessfull, original key is akready monitored.");
            }
            if (keysToCheck.Contains(mappedKey))
                map.Add(mappedKey, originalKey);
            else
                Debug.LogWarning("Mapping unsuccessfull, mapped key is not monitored");
        }
        /// <summary>
        /// Remove a mapped key.
        /// </summary>
        /// <param name="mappedKey"></param>
        public void removeMap(KeyCode mappedKey)
        {
            map.Remove(mappedKey);
        }
        /// <summary>
        /// Grunt work is here. Monitors keyboard for all events for all monitored/mapped keys.
        /// </summary>
        private void Update()
        {
            foreach (KeyCode keyCode in keysToCheck)
            {
                if (Input.GetKeyDown(keyCode))
                {
                    _isPressed[keyCode] = true;
                    dispatchKeyDown(keyCode);
                }
                if (Input.GetKeyUp(keyCode))
                {
                    _isPressed[keyCode] = false;
                    dispatchKeyUp(keyCode);
                }
                if (Input.GetKey(keyCode))
                {
                    dispatchPressed(keyCode);
                }
            }
            foreach (KeyCode key in map.Keys)
            {
                KeyCode keyCode = map[key];
                if (Input.GetKeyDown(keyCode))
                {
                    _isPressed[key] = true;
                    dispatchKeyDown(key);
                }
                if (Input.GetKeyUp(keyCode))
                {
                    _isPressed[key] = false;
                    dispatchKeyUp(key);
                }
                if (Input.GetKey(keyCode))
                {
                    dispatchPressed(key);
                }
            }
        }
    }
}
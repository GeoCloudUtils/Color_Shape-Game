using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using ScriptUtils.GameUtils;

namespace ScriptUtils.Interface
{
    /// <summary>
    /// Main logic for action navigation (scene loading)
    /// Implements the Singelton design pattern
    /// </summary>
    public class Navigator
    {
        #region SINGELTION
        private static Navigator instance;
        /// <summary>
        /// Gets the uniqe instantace of Navigator, makes sure it syncronizez with the instance of NavigationEventSystem
        /// </summary>
        /// <returns></returns>
        public static Navigator getInstance()
        {
            if (instance == null)
            {
                new Navigator();
            }
            if (instance.navigationEventSystem != NavigationEventSystem.getInstance())
            {
                instance.registerEvents();
            }
            return instance;
        }
        #endregion
        private string _sceneToLoad="";
        /// <summary>
        /// Gets the name of scene currently/about to be loaded.
        /// </summary>
        public string sceneToLoad
        {
            get
            {
                return _sceneToLoad;
            }           
        }
        /// <summary>
        /// Gets the name of the current scene
        /// </summary>
        public string currentScene 
        {
            get
            {
                return SceneManager.GetActiveScene().name;
            }
        }
        /// <summary>
        /// Returns true if in the process of loading a new scene
        /// </summary>
        public bool isLoading 
        {
            get
            {
                return _sceneToLoad != "";
            }
        }
        /// <summary>
        /// Instance of the navigation event system to be used for dipsatching navigation events
        /// </summary>
        private NavigationEventSystem navigationEventSystem;
        /// <summary>
        /// The loading screen to be instantiated
        /// </summary>
        private GameObject loadingScreenPrefab;
        /// <summary>
        /// The ILoadingScreen implemnetation attatached to the instatiated loading screen.
        /// </summary>
        private ILoadingScreen loadingScreen;
        private System.Type componentType;
        /// <summary>
        /// Navigation event parameters to be used for Navgation complete event
        /// </summary>
        public NavigationEventParams currentNavParam;
        /// <summary>
        /// Name of the scene that was loaded before this one
        /// </summary>
        public string previouseScene = "";       
        private Navigator()
        {
            Debug.Log("Created Navigator instance!");
            instance = this;
        }
        /// <summary>
        /// Should be called once to intialize the Navigator instance with a loading screen
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefab"></param>
        public void setLoadingScreenPrefab<T>(GameObject prefab)
        {
            Debug.Log("Set loading screen prefab!");
            loadingScreenPrefab = prefab;
            componentType = typeof(T);
        }
        /// <summary>
        /// Registers navigation events with the current NavigationEventSystem instance 
        /// </summary>
        private void registerEvents()
        {
			navigationEventSystem = NavigationEventSystem.getInstance ();
			navigationEventSystem.NavigationStartEvent += HandleNavigationEvent;
		}
        /// <summary>
        /// Handle <see cref="NavigationEventParams.EventType.START">Start</see> events.
        /// </summary>
        /// <param name="param"></param>
        void HandleNavigationEvent(NavigationEventParams param)
        {
            if (param.type != NavigationEventParams.EventType.START)
                return;
            currentNavParam = param;
            if (param.targetType == NavigationEventParams.NavigationTargetType.CAPITOL)
            {
                LoadLevel("Interface_Capitol");
            }
            if (param.targetType == NavigationEventParams.NavigationTargetType.GAME)
            {
                LoadLevel(param.navigationTarget);
            }
            if (param.targetType == NavigationEventParams.NavigationTargetType.GAME_DIFICULTY)
            {
                LoadLevel("DifficultySelector");
            }
            if (param.targetType == NavigationEventParams.NavigationTargetType.MOVIE)
            {
                #if !NO_VIDEO
                LoadLevel("VideoPlayer");
                #endif
            }
        }
        /// <summary>
        /// Loads scene with given name
        /// </summary>
        /// <param name="levelName"></param>
        /// <returns></returns>
        public bool LoadLevel(string levelName)
        {
			previouseScene = SceneManager.GetActiveScene().name;
            Cursor.SetCursor(null,Vector2.zero,CursorMode.Auto);
            if (sceneToLoad != "")
            {
                Debug.LogWarning("Could not load level "+levelName+", because level: " + sceneToLoad + " is already loading.");
                return false;
            } else
            {
                Debug.Log("Loading:"+levelName);
            }
			if(loadingScreenPrefab==null)
			{
				if(currentNavParam!=null)
				{
					currentNavParam.type= NavigationEventParams.EventType.COMPLETE;
				}
				SceneManager.LoadScene(levelName);
				Debug.LogWarning("Loading screen prefab not found, doing load wihtout!");
				return true;
			}
			_sceneToLoad = levelName;
            //Debug.Log("Loading level: " + levelName);
            loadingScreen = GameObject.Instantiate<GameObject>(loadingScreenPrefab).GetComponent(componentType) as ILoadingScreen;
            loadingScreen.FadeDone+= HandleFadeDone;
            loadingScreen.startFadeIn();
            return true;
        }

        public ILoadingScreen LoadScemeDelayed(string levelName)
        {
            if(loadingScreenPrefab==null)
            {
                throw new System.Exception("Cannot use this function if loadingScreenPrefab is not set!");
            }
            _sceneToLoad = levelName;
            //Debug.Log("Loading level: " + levelName);
            loadingScreen = GameObject.Instantiate<GameObject>(loadingScreenPrefab).GetComponent(componentType) as ILoadingScreen;
            loadingScreen.startFadeIn();
            return loadingScreen;
        }
        /// <summary>
        /// handles fade done evetns
        /// </summary>
        /// <param name="loadingScreen">Instance fo the dispathing loading screen</param>
        /// <param name="fState">1 if fade is done</param>
        void HandleFadeDone (ILoadingScreen loadingScreen, int fState)
        {
            //registerEvents();
            if (fState == 1)
            {
				//Debug.Log("Fade in was done, starting fade out!");
				AsyncOperation op = SceneManager.LoadSceneAsync(sceneToLoad);
                loadingScreen.startFadeOut(op);
            } else
            {
				loadingScreen.FadeDone-= HandleFadeDone;
                if(currentNavParam!=null)
                {
                    currentNavParam.type= NavigationEventParams.EventType.COMPLETE;
                }
                else
                {
                    currentNavParam= new NavigationEventParams(NavigationEventParams.EventType.COMPLETE,NavigationEventParams.NavigationTargetType.MAIN);
                }
                _sceneToLoad = "";
                navigationEventSystem.dispatchNavivigationCompleteEvent(currentNavParam);
            }
        }
    }
}
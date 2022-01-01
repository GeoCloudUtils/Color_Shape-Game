using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace ScriptUtils.GameUtils
{
    /// <summary>
    /// The default implementation of ILoadingScreen
    /// </summary>
    public class LoadingScreen : MonoBehaviour, ILoadingScreen
    {
        /// <summary>
        /// Fade duration. This is one directional, so fade in+fade out= duration*2
        /// </summary>
        public float duration = 0.5f;
        /// <summary>
        /// background sprite
        /// </summary>
        public SpriteRenderer bg;
        /// <summary>
        /// Any aditional sprites
        /// </summary>
        public SpriteRenderer[] characters;
        /// <summary>
        /// Animated objects
        /// </summary>
        public GameObject animObject;

        private AsyncOperation currentOp;

        private void Awake()
        {

            Color zeroAlpha = Color.black;
            zeroAlpha.a = 0f;
            //bg.color = zeroAlpha;
            foreach (SpriteRenderer charRender in characters)
            {
                charRender.color = zeroAlpha;
            }
            bg.color = zeroAlpha;
            if (animObject != null)
                animObject.GetComponent<SpriteRenderer>().color = zeroAlpha;
            DontDestroyOnLoad(gameObject);
        }

        public float getDuration()
        {
            return duration;
        }
        public event UnityEngine.Events.UnityAction<ILoadingScreen, int> FadeDone;

        public void startFadeIn()
        {
            Color maxAlpha = Color.white;
            foreach (SpriteRenderer charRender in characters)
            {
                charRender.DOColor(maxAlpha, duration).SetEase(Ease.Linear);
            }
            bg.DOColor(maxAlpha, duration).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (FadeDone != null)
                    FadeDone(this, 1);
            });
            if (animObject != null)
                animObject.GetComponent<SpriteRenderer>().DOColor(maxAlpha, duration).SetEase(Ease.Linear);

        }

        public void startFadeOut(AsyncOperation op)
        {
            currentOp = op;
            InvokeRepeating("FadeOut", 0f, 0.2f);
        }

        private void FadeOut()
        {
            if (!currentOp.isDone)
                return;
            //Debug.Log("Load op is done!");
            //Destroy(gameObject);
            CancelInvoke();
            //Debug.Log("Canceled Invoke");
            Color zeroAlpha = Color.white;
            zeroAlpha.a = 0f;
            if (animObject != null)
                animObject.GetComponent<SpriteRenderer>().DOColor(zeroAlpha, duration).SetEase(Ease.Linear);
            //Debug.Log("Started animation 1");
            foreach (SpriteRenderer charRender in characters)
            {
                charRender.DOColor(zeroAlpha, duration).SetEase(Ease.Linear);
            }
            //Debug.Log("Started animation 2");
            bg.DOColor(zeroAlpha, duration).SetEase(Ease.Linear).OnComplete(() =>
            {
                //Debug.Log("Finished animation 3");
#if UNITY_ANDROID
                //if (MouseEventProccessor.Instance != null && (SceneManager.GetActiveScene().name != "Interface" || !SaveDataManager.getInstance().OfflineVideoStorage))
                //    MouseEventProccessor.Instance.captureEvents = true;
                Debug.Log("Re-enabled mouse");
                if (FadeDone != null)
                    FadeDone(this, 0);
                Debug.Log("Sent Fade Done 0");
                Destroy(gameObject);
                Debug.Log("Destroyed loading screen");
#else
                if (FadeDone != null)
                    FadeDone(this, 0);
                Destroy(gameObject);
#endif
            });
            //Debug.Log("Started animation 3");
        }

        public void forceVisible(bool visible)
        {
            SpriteRenderer[] allRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
            if (visible)
            {
                foreach (SpriteRenderer charRender in allRenderers)
                {
                    charRender.color = Color.white;
                }
            }
            else
            {
                Color zeroAlpha = Color.white;
                foreach (SpriteRenderer charRender in allRenderers)
                {
                    charRender.color = zeroAlpha;
                }
            }
        }

        public void Update()
        {
            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, -8f);
        }
    }
}
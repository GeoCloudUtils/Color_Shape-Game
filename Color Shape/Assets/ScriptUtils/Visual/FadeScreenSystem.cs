using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace ScriptUtils.Visual
{
    public class FadeScreenSystem : MonoBehaviour
    {
        public delegate void FadeDeleage(FadeScreenSystem target, FadeState state);
        public event FadeDeleage FadeEvent;
        public static FadeScreenSystem CreateFadeScreen(FadeState initialState, string layerName = "foreground9")
        {
            GameObject temp = new GameObject();
            temp.name = "FadeScreen";
            FadeScreenSystem fScreeen = temp.AddComponent<FadeScreenSystem>();
            fScreeen.layerName = layerName;
            fScreeen.currentState = initialState;
            return fScreeen;
        }
        public FadeState CurrentState
        {
            get
            {
                return currentState;
            }
        }
        [SerializeField] private FadeState currentState;
        private SpriteRenderer sRenderer;
        private string layerName;
        private Color transparent = new Color(0f, 0f, 0f, 0f);
        private void Start()
        {
            sRenderer = gameObject.AddComponent<SpriteRenderer>();
            Texture2D tempTexture = new Texture2D(1, 1);
            tempTexture.SetPixel(0, 0, Color.black);
            tempTexture.Apply();
            Sprite sprite = Sprite.Create(tempTexture, new Rect(Vector2.zero, new Vector2(1, 1)), new Vector2(0.5f, 0.5f));
            sRenderer.sprite = sprite;
            sRenderer.sortingLayerName = layerName;
            transform.localScale = new Vector3(2000f, 2000f, 1f);
            if (currentState == FadeState.INVISIBLE)
            {
                sRenderer.color = transparent;
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(false);
                sRenderer.color = Color.black;
            }
        }
        public void FadeInOut(float speed = 0.7f, float delay = 0.5f)
        {
            if (currentState == FadeState.IN_TRANSITION)
            {
                Debug.LogWarning("Cannot start new fade operation because one is already in progress!");
                return;
            }
            sRenderer.color = transparent;
            gameObject.SetActive(true);
            StartCoroutine(doFadeInOut(speed, delay));
        }
        private IEnumerator doFadeInOut(float speed, float delay)
        {
            FadeIn(speed);
            yield return new WaitForSeconds(delay + speed);
            yield return new WaitForEndOfFrame();
            FadeOut(speed);
        }
        public void FadeIn(float speed = 0.7f)
        {
            if (currentState == FadeState.IN_TRANSITION)
            {
                Debug.LogWarning("Cannot start new fade operation because one is already in progress!");
                return;
            }
            sRenderer.color = transparent;
            gameObject.SetActive(true);
            currentState = FadeState.IN_TRANSITION;
            dispatchStateChange();
            sRenderer.DOColor(Color.black, speed).SetEase(Ease.Linear).OnComplete(() => { currentState = FadeState.VISIBLE; dispatchStateChange(); });
        }
        public void FadeOut(float speed = 0.7f)
        {
            if (currentState == FadeState.IN_TRANSITION)
            {
                Debug.LogWarning("Cannot start new fade operation because one is already in progress!");
                return;
            }
            sRenderer.color = Color.black;
            gameObject.SetActive(true);
            currentState = FadeState.IN_TRANSITION;
            dispatchStateChange();
            sRenderer.DOColor(transparent, speed).SetEase(Ease.Linear).OnComplete(() => { currentState = FadeState.INVISIBLE; gameObject.SetActive(false); ; dispatchStateChange(); });
        }
        private void dispatchStateChange()
        {
            if (FadeEvent != null)
                FadeEvent.Invoke(this, currentState);
        }
        public enum FadeState
        {
            VISIBLE,
            INVISIBLE,
            IN_TRANSITION
        }
    }
}
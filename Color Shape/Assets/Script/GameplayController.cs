using EduUtils.Events;
using ScriptUtils.GameUtils;
using System;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using DG.Tweening;
using Audio;
using UnityEngine.Events;

public class GameplayController : MonoBehaviour
{
    public GameObject mainScreen;
    public GameObject LevelDoneScreen;
    public GameObject nextButton;
    public GameObject backButton;
    public SpriteRenderer fadeScreen;
    public Level level;
    public int[] levelSequence;
    public event UnityAction OnBack;
    [SerializeField] [ReadOnly] private Level _currentLevel;
    public ShapeType shapeType;
    private Randomizer askedShapeRandom;
    private int currentLevelSequenceIndex = 0;
    private bool canClick = true;
    public void Awake()
    {
        askedShapeRandom = new Randomizer(Enum.GetValues(typeof(ShapeType)).Cast<ShapeType>().ToList());
        nextButton.AddComponent<MouseEventSystem>().MouseEvent += DoNext;
        backButton.AddComponent<MouseEventSystem>().MouseEvent += OnBackClick;
        ShowLevel();
        canClick = true;
    }
    private void OnBackClick(GameObject target, MouseEventType type)
    {
        if (type == MouseEventType.CLICK && canClick)
        {
            canClick = false;
            _currentLevel.StopAudio();
            fadeScreen.DOFade(1f, 0.5f).OnComplete(() =>
            {
                if (_currentLevel != null)
                {
                    Destroy(_currentLevel.gameObject);
                }
                LevelDoneScreen.SetActive(false);
                OnBack?.Invoke();
                mainScreen.SetActive(true);
                gameObject.SetActive(false);
                Destroy(gameObject);
            });
            fadeScreen.DOFade(0f, 0.5f).SetDelay(0.5f);
        }
    }

    private void DoNext(GameObject target, MouseEventType type)
    {
        if (type == MouseEventType.CLICK && canClick)
        {
            LevelDoneScreen.SetActive(false);
            Invoke("ShowLevel", 0.5f);
        }
    }

    public void ShowLevel()
    {
        _currentLevel = Instantiate(level);
        _currentLevel.gameObject.SetActive(true);
        _currentLevel.transform.position = Vector3.zero;
        _currentLevel.transform.SetParent(transform);
        if (!askedShapeRandom.randomRule.hasNumbersLeft())
        {
            askedShapeRandom.randomRule.Reset();
        }
        int type = askedShapeRandom.getRandom();
        _currentLevel.CreateLevel(_currentLevel.transform, (ShapeType)type, levelSequence[currentLevelSequenceIndex], currentLevelSequenceIndex);
        _currentLevel.OnLevelDone += LevelDone;
    }

    private void LevelDone()
    {
        if (_currentLevel)
        {
            Destroy(_currentLevel.gameObject);
        }
        currentLevelSequenceIndex++;
        LevelDoneScreen.SetActive(true);
        if (currentLevelSequenceIndex >= levelSequence.Length)
        {
            Debug.LogError("No more levels!");
            nextButton.gameObject.SetActive(false);
            return;
        }
    }
}

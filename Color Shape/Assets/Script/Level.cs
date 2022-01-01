using ScriptUtils.GameUtils;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using DG.Tweening;
using Audio;
using UnityEngine.Events;
using EduUtils.Events;
using System;
using ScriptUtils;

public class Level : MonoBehaviour
{
    [SerializeField] private ObjectSequence levelQuestionTextSequence;
    [SerializeField] private ObjectSequence levelTextSequence;
    [SerializeField] private GameObject listenButton;
    [SerializeField] private AudioClip victoryClip;
    [SerializeField] private AudioClip wrongClip;
    [SerializeField] private AudioClip[] levelClips;
    [SerializeField] private Transform[] _positions;
    [SerializeField] private ClickableShape _shapePrefab;

    [SerializeField] private int _goodShapesToAsk;
    [SerializeField] [ReadOnly] private int _clickedGoodShapes;

    public event UnityAction OnLevelDone;

    private List<ClickableShape> _spawnedShapes;
    private Randomizer shapeRandom;

    private PlayingAudio levelAudio;
    private PlayingAudio victoryAudio;
    private PlayingAudio wrongAudio;
    private ShapeType levelShapeType;

    private bool canRelisten = true;
    private bool canClick = true;
    public void DoRelisten()
    {
        if (canRelisten)
        {
            StartCoroutine(Listen());
        }
    }

    public void CreateLevel(Transform parent, ShapeType type, int toAsk, int levelIndex)
    {
        _goodShapesToAsk = toAsk;
        levelShapeType = type;
        _spawnedShapes = new List<ClickableShape>();
        shapeRandom = new Randomizer(_shapePrefab.children);
        shapeRandom.randomRule.addBlock((int)type);
        List<int> goodPositions = new List<int>();
        Randomizer goodRnd = new Randomizer(_positions);
        for (int i = 0; i < _goodShapesToAsk; i++)
        {
            goodPositions.Add(goodRnd.getRandom());
        }
        for (int i = 0; i < _positions.Length; i++)
        {
            if (!shapeRandom.randomRule.hasNumbersLeft())
            {
                shapeRandom.randomRule.Reset();
                shapeRandom.randomRule.addBlock((int)type);
            }
            ClickableShape shape = Instantiate(_shapePrefab);
            shape.transform.position = _positions[i].position;
            shape.transform.SetParent(parent);
            _spawnedShapes.Add(shape);
            if (goodPositions.Contains(i))
            {
                shape.setCurrentChildIndex((int)type);
            }
            else
            {
                shape.setCurrentChildIndex(shapeRandom.getRandom());
            }
            shape.AddClickEvent();
            shape.transform.DOMove(_positions[i].position, 0.5f).From(new Vector3(_positions[i].position.x, -7f)).SetEase(Ease.OutExpo).SetDelay(i * 0.1f);
        }
        _spawnedShapes.ForEach(shape => shape.ShapeClick += ShapeClick);
        StartCoroutine(Listen());
        canClick = true;
        listenButton.gameObject.SetActive(true);
        listenButton.AddComponent<MouseEventSystem>().MouseEvent += DoListen;
        levelTextSequence.setCurrentChildIndex(levelIndex);
        levelQuestionTextSequence.setCurrentChildIndex((int)type);
    }

    public void OnDisable()
    {
        Destroy(listenButton.GetComponent<MouseEventSystem>());
    }

    private void DoListen(GameObject target, MouseEventType type) => StartCoroutine(Listen());

    private IEnumerator Listen()
    {
        if (!canRelisten)
        {
            yield break;
        }
        canRelisten = false;
        int levelAudioID = SoundManager.PrepareMusic(levelClips[(int)levelShapeType]);
        levelAudio = SoundManager.GetMusicAudio(levelAudioID);
        levelAudio.Play();
        float length = levelAudio.Clip.length;
        yield return new WaitForSeconds(length);
        canRelisten = true;
    }

    private void ShapeClick(ClickableShape shape, string shapeName, ShapeType shapeType)
    {
        if (!canClick)
        {
            return;
        }
        Debug.Log($"Clicked on {shapeName} of type {shapeType}");
        if (shapeType == levelShapeType)
        {
            int victoryAudioID = SoundManager.PrepareSound(victoryClip);
            victoryAudio = SoundManager.GetSoundAudio(victoryAudioID);
            victoryAudio.Play();
            canClick = false;
            shape.SetLayerPriority();
            shape.transform.DOMove(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);
            shape.transform.DOScale(2.0f, 0.5f).OnComplete(() =>
            {
                _clickedGoodShapes++;
                if (_clickedGoodShapes >= _goodShapesToAsk)
                {
                    StartCoroutine(OnWin());
                }
                else
                {
                    Destroy(shape.gameObject);
                    canClick = true;
                }
            });
            Debug.Log("Good shape click!");
        }
        else
        {
            int wrongAudioID = SoundManager.PrepareSound(wrongClip);
            wrongAudio = SoundManager.GetSoundAudio(wrongAudioID);
            wrongAudio.Play();
            shape.clickable = false;
            shape.SetLayerPriority();
            shape.transform.DOShakePosition(1.0f).OnComplete(() =>
            {
                shape.transform.DOMoveY(-7f, 0.5f).SetEase(Ease.InOutBack);
            });
        }
    }
    public void StopAudio()
    {
        if (wrongAudio != null)
        {
            wrongAudio.Stop();
        }
        if (victoryAudio != null)
        {
            victoryAudio.Stop();
        }
        if (levelAudio != null)
        {
            levelAudio.Stop();
        }
    }

    private IEnumerator OnWin()
    {
        yield return new WaitForSeconds(2.0f);
        Debug.Log("Level done!");
        Destroy(listenButton.GetComponent<MouseEventSystem>());
        listenButton.gameObject.SetActive(false);
        levelTextSequence.gameObject.SetActive(false);
        OnLevelDone?.Invoke();
    }
}

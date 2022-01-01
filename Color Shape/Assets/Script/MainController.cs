using Audio;
using EduUtils.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainController : MonoBehaviour
{
    [SerializeField] private GameObject mainScreen;
    [SerializeField] private GameObject modeShapeType;
    [SerializeField] private GameObject modeColorShape;
    public AudioClip bgClip;
    public Transform mask;
    public Sprite audioOnSprite;
    public Sprite audioOffSprite;

    public SpriteRenderer fadeScreen;
    public GameObject playButton;
    public GameObject audioButton;
    public GameObject shapeTypeButton;
    public GameObject shapeColorButton;

    private SpriteRenderer audioRenderer;
    private PlayingAudio bgAudio;
    private bool audiIsOn = false;
    private bool canClick = true;
    private bool canClickOnModes = false;
    private GameObject _currentMode;
    private float defX;
    private Vector3 maskInitPosition;

    public void Awake()
    {
        maskInitPosition = mask.transform.position;
    }
    public void Start()
    {
        playButton.AddComponent<MouseEventSystem>().MouseEvent += OnPlayClick;
        audioButton.AddComponent<MouseEventSystem>().MouseEvent += OnAudioButtonClick;
        shapeTypeButton.AddComponent<MouseEventSystem>().MouseEvent += OnShapeTypeModeClick;
        shapeColorButton.AddComponent<MouseEventSystem>().MouseEvent += OnShapeColorTypeModeClick;
        audioRenderer = audioButton.transform.GetChild(0).GetComponent<SpriteRenderer>();
        defX = mask.transform.position.x;
        maskInitPosition = mask.transform.position;
        if (!PlayerPrefs.HasKey("Audio"))
        {
            PlayerPrefs.SetInt("Audio", 1);
        }
        audiIsOn = PlayerPrefs.GetInt("Audio") == 1;
        if (audiIsOn)
        {
            audioRenderer.sprite = audioOnSprite;
            PlayAudioBg();
        }
        else
        {
            audioRenderer.sprite = audioOffSprite;
        }
    }

    public void OnEnable()
    {
        mask.transform.position = maskInitPosition;
    }

    private void OnShapeColorTypeModeClick(GameObject target, MouseEventType type)
    {
        if (type == MouseEventType.CLICK)
        {
            //fadeScreen.DOFade(1f, 0.5f).OnComplete(() =>
            //{
            //    OpenGame(1);
            //});
            //fadeScreen.DOFade(0f, 0.5f).SetDelay(0.5f);
        }
    }

    private void OnShapeTypeModeClick(GameObject target, MouseEventType type)
    {
        if (type == MouseEventType.CLICK && canClickOnModes)
        {
            canClickOnModes = false;
            fadeScreen.DOFade(1f, 0.5f).OnComplete(() =>
            {
                OpenGame(2);
            });
            fadeScreen.DOFade(0f, 0.5f).SetDelay(0.5f);
        }
    }

    public void OpenGame(int gameType)
    {
        mainScreen.SetActive(false);
        if (gameType == 1)
        {
            //_currentMode = Instantiate(modeShapeType);
            //_currentMode.SetActive(true);
        }
        else
        {
            _currentMode = Instantiate(modeColorShape);
            _currentMode.SetActive(true);
        }
    }

    private void OnAudioButtonClick(GameObject target, MouseEventType type)
    {
        if (type == MouseEventType.CLICK)
        {
            if (PlayerPrefs.GetInt("Audio") == 1)
            {
                PlayerPrefs.SetInt("Audio", 0);
            }
            else
            {
                PlayerPrefs.SetInt("Audio", 1);
            }
            audiIsOn = PlayerPrefs.GetInt("Audio") == 1;
            if (!audiIsOn)
            {
                if (bgAudio != null)
                {
                    bgAudio.Stop();
                }
                audioRenderer.sprite = audioOffSprite;
            }
            else
            {
                PlayAudioBg();
                audioRenderer.sprite = audioOnSprite;
            }
        }
    }

    private void PlayAudioBg()
    {
        int id = SoundManager.PrepareMusic(bgClip);
        bgAudio = SoundManager.GetMusicAudio(id);
        bgAudio.Play();
    }

    private void OnPlayClick(GameObject target, MouseEventType type)
    {
        if (type == MouseEventType.CLICK && canClick)
        {
            canClickOnModes = false;
            canClick = false;
            mask.transform.DOKill();
            if (mask.transform.position.x != defX)
            {
                mask.transform.DOMoveX(defX, 0.5f).OnComplete(() =>
                {
                    canClick = true;
                });
                return;
            }
            mask.DOMoveX(12f, 0.5f).OnComplete(() =>
            {
                canClickOnModes = true;
                canClick = true;
            });
        }
    }
}

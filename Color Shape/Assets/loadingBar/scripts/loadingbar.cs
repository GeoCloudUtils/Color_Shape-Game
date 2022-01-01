using ScriptUtils.GameUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class loadingbar : MonoBehaviour
{
    public GameObject lean;
    private RectTransform rectComponent;
    private Image imageComp;
    public float speed = 0.0f;
    public bool initLoad = false;
    private bool loadStart = false;
    public event UnityAction fillDone;
    void Start()
    {
        rectComponent = GetComponent<RectTransform>();
        imageComp = rectComponent.GetComponent<Image>();
        imageComp.fillAmount = 0.0f;
    }
    void Update()
    {
        if (imageComp.fillAmount != 1f)
            imageComp.fillAmount = imageComp.fillAmount + Time.deltaTime * speed;
        else
        {
            if (!loadStart)
            {
                loadStart = true;
                if (fillDone != null)
                    fillDone.Invoke();

            }
        }
    }
}

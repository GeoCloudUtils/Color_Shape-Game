using EduUtils.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCloseButton : MonoBehaviour
{
    public void Start()
    {
        gameObject.AddComponent<MouseEventSystem>().MouseEvent += DoExit;
    }

    private void DoExit(GameObject target, MouseEventType type)
    {
        if (type == MouseEventType.CLICK)
        {
            Application.Quit();
        }
    }
}

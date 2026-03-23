using System;
using UnityEngine;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

    public Func<string, UIActor> find;
    private Dictionary<string, UIActor> uiObjectDic = new Dictionary<string, UIActor>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void Init()
    {
        find = FindUI;

        UIObject[] objs = FindObjectsByType<UIObject>(FindObjectsSortMode.None);
        foreach (UIObject obj in objs)
        {
            uiObjectDic.Add(obj.Name, new UIActor(obj, null));
        }

        uiObjectDic["UI_G_Judgement"].action = Score.Instance.Ani;
        uiObjectDic["UI_G_Combo"].action = Score.Instance.Ani;

        //uiObjectDic["UI_E_ProgressBar"].action = Editor.Instance.Progress;
        //uiObjectDic["UI_E_Play"].action = Editor.Instance.Play;
        //uiObjectDic["UI_E_Stop"].action = Editor.Instance.Stop;
    }

    public UIActor FindUI(string uiName)
    {
        UIActor actor = uiObjectDic[uiName];
        if (actor.action != null)
            actor.action.Invoke(actor.uiObject);
        return actor;
    }

    public UIActor GetUI(string uiName)
    {
        UIActor actor = uiObjectDic[uiName];
        return actor;
    }
}

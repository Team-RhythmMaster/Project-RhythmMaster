using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

    public Func<string, UIActor> find;
    private Dictionary<string, UIActor> uiObjectDic = new Dictionary<string, UIActor>();

    public Text judgeText;

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
    }

    public void ShowJudge(string result)
    {
        StopAllCoroutines();
        StartCoroutine(ShowRoutine(result));
    }

    private IEnumerator ShowRoutine(string result)
    {
        judgeText.text = result;

        // 색상 변경
        switch (result)
        {
            case "Perfect": 
                judgeText.color = Color.yellow; 
                break;
            case "Great": 
                judgeText.color = Color.green; 
                break;
            case "Good": 
                judgeText.color = Color.blue; 
                break;
            case "Bad": 
                judgeText.color = Color.gray; 
                break;
            case "Miss": 
                judgeText.color = Color.red;
                break;
        }

        yield return new WaitForSeconds(0.5f);
        judgeText.text = "";
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

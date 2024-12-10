using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public   class UIBig : UIBehaviourData
{
    private void Awake()
    {
        Debug.Log("UIBig Awake");
    }
    private void Start()
    {
        Debug.Log("UIBig Start");
    }

    public void OnOpenPage() 
    {
        UIManager.Instance.ShowUI<UITest2>("UITest2");
    }

    public override void InitData(System.Object args)
    {
        Debug.Log($"UIBig InitData {args}");
    }
 
}

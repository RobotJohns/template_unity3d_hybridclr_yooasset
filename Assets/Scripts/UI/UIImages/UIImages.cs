using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIImages : MonoBehaviour
{
    public Sprite[] sprites;
    public UnityEngine.UI.Image prafabe;
    public Transform parent;
    public ScrollView mScrollView;
    public UnityEngine.UI.Button button;
    // Start is called before the first frame update
    private void Awake()
    {
        button = transform.Find("Button (Legacy)").GetComponent<UnityEngine.UI.Button>();
    }
    void Start()
    {
        UnityEngine.UI.Image clone;
        foreach (var sprite in sprites) 
        { 
            clone = Instantiate(this.prafabe) ;
            clone.transform.SetParent(this.parent) ;
            clone.sprite = sprite;
            clone.gameObject.SetActive(true);
        }

        button.onClick.AddListener(() => {

            UIManager.Instance.ShowUI<UIBig>("UIBig","ABC");
        
        });
    }
 
}

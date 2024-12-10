using UnityEngine;

public class UIBehaviour : MonoBehaviour
{
    public void OnClose()
    {
        DestroyImmediate(this.gameObject);
    }
}

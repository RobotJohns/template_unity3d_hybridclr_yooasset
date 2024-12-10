using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBehaviourData : MonoBehaviour
{
   abstract
   public void InitData(System.Object args);

   public void OnClose() { 
     DestroyImmediate(this.gameObject);
   }
}

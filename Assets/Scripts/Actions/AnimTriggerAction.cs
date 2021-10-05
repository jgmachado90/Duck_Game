using CoreInterfaces;
using UnityEngine;

namespace GameFramework
{
   public class AnimTriggerAction : MonoBehaviour, IActionMonoBehaviour
   {
      public Animator anim;
      public string triggerParam;
   
      public void InvokeAction()
      {
         anim.SetTrigger(triggerParam);
      }
   }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_SpeedingByDamage : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        switch (animator.GetInteger("ShellHealth"))
        {
            case 40:
                animator.SetFloat("_shellSpeed", 1);
                animator.SetFloat("FireRate", .8f);

                break;
            case 38:
                animator.SetFloat("_shellSpeed", 1);
                animator.SetFloat("FireRate", .7f);
                break;
            case 35:
                animator.SetFloat("_shellSpeed", 1.5f);
                animator.SetFloat("FireRate", .5f);

                break;
            case 33:
                animator.SetFloat("_shellSpeed", 2);
                break;
            case 30:
                animator.SetFloat("_shellSpeed", 2.5f);
                animator.SetFloat("FireRate", .4f);

                break;
            case 25:
                animator.SetFloat("_shellSpeed", 3);
                break;
            case 20:
                animator.SetFloat("_shellSpeed", 4);
                break;
            case 15:
                animator.SetFloat("_shellSpeed", 5);
                animator.SetFloat("FireRate", .3f);

                break;
            case 10:
                animator.SetFloat("_shellSpeed", 8);
                break;
            case 5:
                animator.SetFloat("_shellSpeed", 10);
                animator.SetFloat("FireRate", .25f);

                break;
            case 0:
                
                break;
            default:
                break;
        }
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

}

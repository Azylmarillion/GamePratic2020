// ===== Game Pratic 2020 - https://github.com/Azylmarillion/GamePratic2020 ===== //
//
// Notes :
//
// ============================================================================== //

using UnityEngine;
using TMPro;

namespace GamePratic2020
{
	public class ANI_UpdateStageScore : StateMachineBehaviour
    {
        [SerializeField] private float delay = 1;
        [SerializeField] private float duration = 3;

        private bool isInitialized = false;
        private TextMeshProUGUI text = null;

        private float score = 1000000f;

        private bool isWaitingDelay = true;
        private bool isIncreasingScore = true;
        private float timerVar = 0;

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (isWaitingDelay)
            {
                timerVar += Time.deltaTime;
                if (timerVar > delay)
                {
                    isWaitingDelay = false;
                    timerVar -= delay;
                }
            }
            else if (isIncreasingScore)
            {
                timerVar += Time.deltaTime;
                if (timerVar > duration)
                {
                    timerVar = duration;
                    isIncreasingScore = false;
                }

                text.text = "+ " + ((score / duration) * timerVar).ToString("### ### 000");
            }
        }

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!isInitialized)
            {
                isInitialized = true;
                text = animator.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            }

            isWaitingDelay = true;
            isIncreasingScore = true;
            timerVar = 0;
            text.text = "+ 000";
        }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
}

using System.Collections;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private static readonly int ShipsEnter = Animator.StringToHash("ships_enter");
    private static readonly int PlayerBoardEnter = Animator.StringToHash("player_board_enter");
    private static readonly int TargetingBoardEnter = Animator.StringToHash("targeting_board_enter");
    private static readonly int LogWindowEnter = Animator.StringToHash("log_window_enter");
    private static readonly int LogGrow = Animator.StringToHash("log_grow");
    private static readonly int UIEnter = Animator.StringToHash("ui_enter");

    [SerializeField] private Animator shipsAnimator;
    [SerializeField] private Animator playerBoardAnimator;
    [SerializeField] private Animator targetingBoardAnimator;
    [SerializeField] private Animator logWindowAnimator;
    [SerializeField] private Animator uiAnimator;

    private void Start()
    {
        uiAnimator.SetTrigger(UIEnter);
        shipsAnimator.SetTrigger(ShipsEnter);

        StartCoroutine(LogWindowEnterCoroutine());
    }

    public void PlayerBoardEnterAnimation()
    {
        playerBoardAnimator.SetTrigger(PlayerBoardEnter);
    }

    public void TargetingBoardEnterAnimation()
    {
        targetingBoardAnimator.SetTrigger(TargetingBoardEnter);
    }

    public void GrowLogAnimation()
    {
        StartCoroutine(LogGrowCoroutine());
    }

    private IEnumerator LogWindowEnterCoroutine()
    {
        logWindowAnimator.SetTrigger(LogWindowEnter);
        yield return new WaitForSeconds(2.5f);
        logWindowAnimator.enabled = false;
    }
    
    private IEnumerator LogGrowCoroutine()
    {
        logWindowAnimator.enabled = true;
        logWindowAnimator.SetTrigger(LogGrow);
        yield return new WaitForSeconds(2.5f);
        logWindowAnimator.enabled = false;
    }
}

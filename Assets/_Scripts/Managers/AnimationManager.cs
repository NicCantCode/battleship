using System;
using System.Collections;
using System.Collections.Generic;
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
        logWindowAnimator.SetTrigger(LogWindowEnter);
        uiAnimator.SetTrigger(UIEnter);
        shipsAnimator.SetTrigger(ShipsEnter);
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
        logWindowAnimator.SetTrigger(LogGrow);
    }
}

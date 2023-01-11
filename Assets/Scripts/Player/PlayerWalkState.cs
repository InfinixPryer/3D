using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine currentCtx, PlayerStateFactory playerStateFactory) : base (currentCtx, playerStateFactory)
    {
        
    }

    public override void EnterState()
    {
        Debug.Log("Walk");
        Ctx.Animator.SetBool("isWalking", true);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {
        Ctx.Rb.AddForce(Ctx.MovementDirection * Ctx.Rb.mass * Ctx.MoveSpeed, ForceMode.Force);
    }

    public override void ExitState()
    {
        Ctx.Animator.SetBool("isWalking", false);
    }

    public override void CheckSwitchStates()
    {
        if ( Ctx.IsRunPressed ) {
            SwitchState(Factory.Run());
        } else if ( !Ctx.IsMovePressed ) {
            SwitchState(Factory.Idle());
        }
    }

    public override void InitializeSubState()
    {

    }
}

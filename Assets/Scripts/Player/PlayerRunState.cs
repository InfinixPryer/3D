using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine currentCtx, PlayerStateFactory playerStateFactory) : base (currentCtx, playerStateFactory)
    {
        
    }

    public override void EnterState()
    {
        Ctx.Animator.SetBool("isRunning", true);
        Debug.Log("Run");
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {
        Ctx.Rb.AddForce(Ctx.RunMovementDirection * Ctx.Rb.mass * Ctx.MoveSpeed);
    }

    public override void ExitState()
    {
        Ctx.Animator.SetBool("isRunning", false);
    }

    public override void CheckSwitchStates()
    {
        if( !Ctx.IsRunPressed && Ctx.IsMovePressed ) {
            SwitchState(Factory.Walk());
        } else if ( !Ctx.IsRunPressed && !Ctx.IsMovePressed ) {
            SwitchState(Factory.Idle());
        }
    }

    public override void InitializeSubState()
    {
        
    }
}

using UnityEngine;

public class PlayerGroundState : PlayerBaseState
{
    public PlayerGroundState(PlayerStateMachine currentCtx, PlayerStateFactory playerStateFactory) : base (currentCtx, playerStateFactory)
    {
        IsRootState = true;
        
    }

    public override void EnterState()
    {
        InitializeSubState();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {

    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        if(Ctx.IsJumpPressed && Ctx.IsGrounded) {
            Debug.Log("TO JUMP");
            SwitchState(Factory.Jump());
        }else if ( !Ctx.IsJumpPressed && !Ctx.IsGrounded) {
            Debug.Log("TO FALL");
            SwitchState(Factory.Fall());
        }
    }

    public override void InitializeSubState()
    {
        if(!Ctx.IsMovePressed && !Ctx.IsRunPressed) {
            SetSubState(Factory.Idle());
        } else if ( Ctx.IsMovePressed && !Ctx.IsRunPressed) { 
            SetSubState(Factory.Walk());
        } else {
            SetSubState(Factory.Run());
        }
    }
}

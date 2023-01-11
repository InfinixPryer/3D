using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    public PlayerFallState(PlayerStateMachine currentCtx, PlayerStateFactory playerStateFactory) : base (currentCtx, playerStateFactory)
    {
        IsRootState = true;
        
    }

    public override void EnterState()
    {
        Debug.Log("Fall");
        InitializeSubState();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {
        Debug.Log("FALLING");
        Ctx.Rb.AddForce(Vector3.down * Ctx.Rb.mass * Ctx.Gravity, ForceMode.Force);
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        if(Ctx.IsGrounded){
            SwitchState(Factory.Ground());
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

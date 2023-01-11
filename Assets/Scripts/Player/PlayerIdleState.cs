using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentCtx, PlayerStateFactory playerStateFactory) : base (currentCtx, playerStateFactory)
    {
        
    }

    public override void EnterState()
    {
        Debug.Log("Idle");
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
        if ( Ctx.IsMovePressed ) {
            SwitchState(Factory.Walk());
        }
    }

    public override void InitializeSubState()
    {

    }
}

using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine currentCtx, PlayerStateFactory playerStateFactory) : base (currentCtx, playerStateFactory)
    {
        IsRootState = true;
        Ctx.Rb.velocity = Vector3.zero;
        Ctx.Rb.AddForce(Vector3.up * Ctx.Rb.mass * Ctx.InitialJumpForce , ForceMode.Impulse);    
        Ctx.IsJumping = true;
        Ctx.JumpTimer = 0;
    }

     public override void EnterState()
     {
        InitializeSubState();
        Debug.Log("Jump");
        
        
     }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {
        if (Ctx.IsJumpPressed && Ctx.JumpTimer < Ctx.JumpTime)
        {
            JumpRoutine();
        } else if (!Ctx.IsJumpPressed)
        {
            Ctx.Rb.velocity = new Vector2 ( Ctx.Rb.velocity.x, 0);
            Ctx.IsJumping = false;
        }
    }

    void JumpRoutine()
    {
        float portionCompleted = Ctx.JumpTimer / Ctx.JumpTime;
        Vector3 jumpVector = Vector3.Lerp (Vector3.up * (Ctx.JumpForce * 5), Vector3.zero, portionCompleted);
        Ctx.Rb.AddForce(jumpVector * Ctx.Rb.mass, ForceMode.Force);
        Ctx.JumpTimer += Time.fixedDeltaTime;

        if(Ctx.JumpTimer >= Ctx.JumpTime)
        {
            Ctx.IsJumping = false;
        }
    }

    public override void ExitState()
    {
        Ctx.JumpTimer = 0;
    }

    public override void CheckSwitchStates()
    {
        if( !Ctx.IsJumping && Ctx.Rb.velocity.y <= 0 ) {
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

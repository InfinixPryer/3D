using UnityEngine;

public abstract class PlayerBaseState
{
    private bool isRootState = false;
    private PlayerStateMachine baseCtx;
    private PlayerStateFactory baseFactory;
    private PlayerBaseState currentSubState;
    private PlayerBaseState currentSuperState;

    protected bool IsRootState {set { isRootState = value;} }
    protected PlayerStateMachine Ctx {get { return baseCtx;} }
    protected PlayerStateFactory Factory {get { return baseFactory; } }

    public PlayerBaseState(PlayerStateMachine currentCtx, PlayerStateFactory playerStateFactory){
        baseCtx = currentCtx;
        baseFactory = playerStateFactory;
    }
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void FixedUpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubState();

    public void UpdateStates(){

        
        UpdateState();
        if(currentSubState != null){
            currentSubState.UpdateStates();
        }
    }

    public void FixedUpdateStates(){
        FixedUpdateState();
        if(currentSubState != null){
            currentSubState.FixedUpdateStates();
        }
    }

    protected void SwitchState(PlayerBaseState newState){
        //current state exits
        ExitState();

        //new state enters
        newState.EnterState();

        //switch current state
        // baseCtx.CurrentState = newState;
        if(isRootState){
            baseCtx.CurrentState = newState;    
        } else if ( currentSuperState != null){
            currentSuperState.SetSubState(newState);
        }
    }

    protected void SetSuperState(PlayerBaseState newSuperState){
        currentSuperState = newSuperState;
    }
    
    protected void SetSubState(PlayerBaseState newSubState){
        currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}

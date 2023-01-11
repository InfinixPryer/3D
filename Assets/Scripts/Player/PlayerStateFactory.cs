using System;
using System.Collections.Generic;

public enum PlayerStatesEnum {
    Walk,
    Idle,
    Run,
    Fall,
    Grounded,
    Jump,
}

public class PlayerStateFactory
{
    PlayerStateMachine context;

    Dictionary<PlayerStatesEnum, PlayerBaseState> playerStates = new Dictionary<PlayerStatesEnum, PlayerBaseState>();
    // Dictionary<string, PlayerBaseState>

    public PlayerStateFactory(PlayerStateMachine currentContext){
        context = currentContext;
    }
    public PlayerBaseState Ground(){
        return GetInstance<PlayerGroundState>(PlayerStatesEnum.Grounded);
    }

    public PlayerBaseState Idle(){
        return GetInstance<PlayerIdleState>(PlayerStatesEnum.Idle);
    }
    
    public PlayerBaseState Jump(){
        return GetInstance<PlayerJumpState>(PlayerStatesEnum.Jump);
    }

    public PlayerBaseState Walk(){
        return GetInstance<PlayerWalkState>(PlayerStatesEnum.Walk);
    }

    public PlayerBaseState Run() {
        return GetInstance<PlayerRunState>(PlayerStatesEnum.Run);
    }

    public PlayerBaseState Fall() {
        return GetInstance<PlayerFallState>(PlayerStatesEnum.Fall);
    }

    private PlayerBaseState GetInstance<T>(PlayerStatesEnum key) where T: PlayerBaseState {
        if (!playerStates.ContainsKey(key)) {
            playerStates.Add(key, Activator.CreateInstance(typeof(T), new object[] { context, this }) as PlayerBaseState);
        }
        return playerStates[key];
    }
}

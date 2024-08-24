using jbzd.MinorSystems.InputSystem.Inputs;

namespace jbzd.MainHero.PlayerStateLogic
{
    public class PlayerStateIdle : IPlayerStateLogic
    {
        private readonly PlayerInput _playerInput;
        
        public PlayerStateIdle(PlayerInput playerInput)
        {
            _playerInput = playerInput;
        }

        public PlayerState InitPlayerState => PlayerState.Idle;
        
        public PlayerState StateLogicForFixedUpdate()
        {
            return PlayerState.Idle;
        }

        public PlayerState StateLogicForUpdate()
        {
            var playerState = InitPlayerState;
            
            if(_playerInput.attackInputStatus.Basic) playerState = PlayerState.Attack;

            if(_playerInput.movementInputStatus.Down || _playerInput.movementInputStatus.Up || _playerInput.movementInputStatus.Left || _playerInput.movementInputStatus.Right)
            {
                playerState = PlayerState.Move;
            }

            return playerState;
        }

        public PlayerState StateLogicForLateUpdate()
        {
            return PlayerState.Idle;
        }
    }
}
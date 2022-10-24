using jbzd.Common.InputSystem.Inputs;

namespace jbzd.MainHero.PlayerStateLogic
{
    public class PlayerStateIdleUpdate : IPlayerStateLogic
    {
        private readonly PlayerInput _playerInput;
        private readonly PlayerController _playerController;
        
        public PlayerStateIdleUpdate(PlayerController playerController, PlayerInput playerInput)
        {
            _playerController = playerController;
            _playerInput = playerInput;
        }
        
        public MonoBehaviourMethod MonoBehaviourMethodInWhichInvoked() => MonoBehaviourMethod.Update;

        public PlayerState PlayerStateInWhichInvoked() => PlayerState.Idle;

        public PlayerState StateLogic()
        {
            var playerState = PlayerStateInWhichInvoked();
            
            if(_playerInput.attackInputStatus.Basic) playerState = PlayerState.Attack;

            if(_playerInput.movementInputStatus.Down || _playerInput.movementInputStatus.Up || _playerInput.movementInputStatus.Left || _playerInput.movementInputStatus.Right)
            {
                playerState = PlayerState.Move;
            }

            return playerState;
        }
    }
}
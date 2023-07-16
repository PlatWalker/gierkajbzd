using jbzd.Common.Enums;
using jbzd.Common.InputSystem.Inputs;

namespace jbzd.MainHero.PlayerStateLogic
{
    public class PlayerStateMoveLateUpdate : IPlayerStateLogic
    {
        private readonly PlayerInput _playerInput;
        private readonly PlayerManager _playerManager;
        public PlayerStateMoveLateUpdate(
            PlayerManager playerManager,
            PlayerInput playerInput)
        {
            _playerInput = playerInput;
            _playerManager = playerManager;
        }
        
        public MonoBehaviourMethod MonoBehaviourMethodInWhichInvoked() => MonoBehaviourMethod.LateUpdate;

        public PlayerState PlayerStateInWhichInvoked() => PlayerState.Move;
        public PlayerState StateLogic()
        {
            if (!_playerManager.CanPlayerMove)
            {
                return PlayerState.Idle;
            }
            
            return _playerInput.attackInputStatus.Basic ? PlayerState.Attack : PlayerState.Move;
        }
    }
}
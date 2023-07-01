using jbzd.Common.Enums;
using jbzd.Common.InputSystem.Inputs;

namespace jbzd.MainHero.PlayerStateLogic
{
    public class PlayerStateMoveLateUpdate : IPlayerStateLogic
    {
        private readonly PlayerInput _playerInput;

        public PlayerStateMoveLateUpdate(
            PlayerInput playerInput)
        {
            _playerInput = playerInput;
        }
        
        public MonoBehaviourMethod MonoBehaviourMethodInWhichInvoked() => MonoBehaviourMethod.LateUpdate;

        public PlayerState PlayerStateInWhichInvoked() => PlayerState.Move;
        public PlayerState StateLogic()
        {
            return _playerInput.attackInputStatus.Basic ? PlayerState.Attack : PlayerState.Move;
        }
    }
}
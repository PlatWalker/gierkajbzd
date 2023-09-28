namespace jbzd.MainHero.PlayerStateLogic
{
    public class ErrorPlayerState : IPlayerStateLogic
    {
        public PlayerState StateLogicForFixedUpdate()
        {
            return PlayerState.Unrecognized;
        }

        public PlayerState StateLogicForUpdate()
        {
            return PlayerState.Unrecognized;
        }

        public PlayerState StateLogicForLateUpdate()
        {
            return PlayerState.Unrecognized;
        }

        public PlayerState InitPlayerState => PlayerState.Unrecognized;
    }
}
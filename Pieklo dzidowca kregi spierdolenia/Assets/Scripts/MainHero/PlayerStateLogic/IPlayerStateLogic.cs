
namespace jbzd.MainHero.PlayerStateLogic
{
    public interface IPlayerStateLogic
    {
        PlayerState StateLogicForFixedUpdate();
        PlayerState StateLogicForUpdate();
        PlayerState StateLogicForLateUpdate();
        PlayerState InitPlayerState { get; }
    }
}
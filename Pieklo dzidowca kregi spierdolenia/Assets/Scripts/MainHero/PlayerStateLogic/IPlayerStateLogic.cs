using System;

namespace jbzd.MainHero.PlayerStateLogic
{
    public interface IPlayerStateLogic
    {
        MonoBehaviourMethod MonoBehaviourMethodInWhichInvoked();
        PlayerState PlayerStateInWhichInvoked();
        PlayerState StateLogic();
    }
}
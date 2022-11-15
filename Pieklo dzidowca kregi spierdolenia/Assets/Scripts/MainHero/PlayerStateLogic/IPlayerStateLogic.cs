using System;
using jbzd.Common.Enums;

namespace jbzd.MainHero.PlayerStateLogic
{
    public interface IPlayerStateLogic
    {
        MonoBehaviourMethod MonoBehaviourMethodInWhichInvoked();
        PlayerState PlayerStateInWhichInvoked();
        PlayerState StateLogic();
    }
}
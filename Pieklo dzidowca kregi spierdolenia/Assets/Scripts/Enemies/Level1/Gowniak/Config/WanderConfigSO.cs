using UnityEngine;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using jbzd.MainHero;
namespace jbzd.Enemies.Level1.Gowniak.Config
{
    [CreateAssetMenu(menuName = "Gowniak/Wander Config", fileName = "Gowniak Wander Config", order = 1)]
    public class WanderConfigSO : ScriptableObject
    {

        public Vector2 WaitRangeBetweenWanders = new(1, 5);
        public float WanderRadius = 5f;
    }
}
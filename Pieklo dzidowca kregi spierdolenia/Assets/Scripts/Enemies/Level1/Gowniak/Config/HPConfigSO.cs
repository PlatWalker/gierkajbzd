using UnityEngine;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using jbzd.MainHero;
namespace jbzd.Enemies.Level1.Gowniak.Config
{
    [CreateAssetMenu(menuName = "Gowniak/HP Config", fileName = "Gowniak HP Config", order = 1)]
    public class HPConfigSO : ScriptableObject
    {
        public int MaximumHealth;
        public int CurrentHealth; 
    }
}
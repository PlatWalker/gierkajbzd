using CrashKonijn.Goap.Classes.References;
using UnityEngine;

namespace jbzd.Enemies.Level1.Gowniak.Actions
{
    public class AttackData : CommonData
    {
        
        [GetComponent]
        public Animator Animator { get; set; }
        [GetComponent]
        public DamageControllersManager DamageControllersManager { get; set; }

    }
}
using CrashKonijn.Goap.Interfaces;

namespace jbzd.Enemies.Level1.Gowniak.Actions
{
    public class CommonData : IActionData
    {
        public ITarget Target { get; set; }
        public float Timer { get; set; }
    }
}
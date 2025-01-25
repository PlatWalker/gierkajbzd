using UnityEngine;
using UnityEngine.VFX;

namespace jbzd.Enemies.Level1.Madka
{
    public class OnCollidePlayEffect: MonoBehaviour
    {
        [SerializeField]
        private VisualEffect visualEffect;
        
        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Player")) return;

            var collidePostition = new Vector3(other.transform.position.x, visualEffect.gameObject.transform.position.y, other.transform.position.z);
            visualEffect.transform.SetPositionAndRotation(collidePostition, visualEffect.transform.rotation);
            visualEffect.Play();
        }
    }
}
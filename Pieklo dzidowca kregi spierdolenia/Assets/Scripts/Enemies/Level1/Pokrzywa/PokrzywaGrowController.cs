using System.Collections;
using UnityEngine;

namespace jbzd.Enemies.Level1.Pokrzywa
{
    public class PokrzywaGrowController:MonoBehaviour
    {
        [SerializeField]
        [Range(0.01f, 0.999f)]
        [Tooltip("Początkowa wielkość pokrzywy wylatującej")]
        private float startingScale = 0.2f;
        
        [SerializeField]
        [Range(0.01f, 0.999f)]
        [Tooltip("Czas w trakcie którego pokrzywa nie rośnie, tylko leci")]
        private float whenStartGrowing = 0.8f;
        
        [SerializeField]
        [Range(0.01f, 10f)]
        [Tooltip("Końcowa wielkość pokrzywy wylatującej")]
        private float endingScale = 1f;
        
        [SerializeField]
        [Range(0.01f, 10f)]
        [Tooltip("Wysokość skoku pokrzywy wylatującej")]
        private float jumpingHeight = 2.5f;
        
        private IEnumerator Grow(GameObject objectToMove, Vector3 startPosition, Vector3 targetPosition, float duration)
        {
            var elapsedTime = 0f;
            var growthspeed = (1-startingScale)/(1-whenStartGrowing);
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                var journeyTime = elapsedTime / duration;
                objectToMove.transform.position = Vector3.Lerp(startPosition, targetPosition, journeyTime);
        
                objectToMove.transform.position = new Vector3(objectToMove.transform.position.x, objectToMove.transform.position.y + Mathf.Sin(journeyTime*3.14f)*jumpingHeight, objectToMove.transform.position.z);
                objectToMove.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, 1f);
                if(journeyTime < whenStartGrowing){
                    var multiplier = startingScale*endingScale;
                    objectToMove.transform.localScale = Vector3.one*multiplier;
                }
                else{
                    var multiplier = (startingScale+(journeyTime-whenStartGrowing)*growthspeed)*endingScale;
                    objectToMove.transform.localScale = Vector3.one*multiplier;
                }
                yield return null;
            }
            objectToMove.transform.position = targetPosition;
        }
    }
}
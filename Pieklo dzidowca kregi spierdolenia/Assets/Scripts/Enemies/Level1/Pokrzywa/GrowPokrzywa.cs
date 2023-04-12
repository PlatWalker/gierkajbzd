using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class GrowPokrzywa : ActionNode
{
    private float _baseTime = 10f;
    private float _deviation = 2f;
    private float _startTime;
    private float _maxSpreadDistance = 3f;
    private bool _growPokrzywa;
    private float _timeRemaining;

    protected override void OnStart() {   
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        if (blackboard.startTimer)
        {
            _startTime = Time.time;
            blackboard.waitBeforeSpreadDuration = _baseTime + Random.Range(-_deviation, _deviation);
            blackboard.startTimer = false;
            _growPokrzywa = false;
        }

        _timeRemaining = Time.time - _startTime;
        if (_timeRemaining > blackboard.waitBeforeSpreadDuration) 
        {
            _growPokrzywa = true;
        }

        if (!blackboard.isDead && _growPokrzywa)
        {
            Vector3 newPoint = Vector3.zero;
            RaycastHit hit;

            newPoint = new Vector3(context.transform.position.x + Random.Range(-_maxSpreadDistance, _maxSpreadDistance),
                                context.transform.position.y,
                                context.transform.position.z + Random.Range(-_maxSpreadDistance, _maxSpreadDistance));

            Ray ray = new Ray(new Vector3(context.transform.position.x, context.transform.position.y + 2, context.transform.position.z),
                newPoint - new Vector3(context.transform.position.x, context.transform.position.y + 2, context.transform.position.z));

            if (Physics.Raycast(ray, out hit, _maxSpreadDistance * 200))
            {
                if (hit.transform.tag == "Terrain")
                {
                    GameObject newOne = Object.Instantiate(blackboard.pokrzywaPrefab, newPoint, context.transform.rotation);
                    newOne.gameObject.transform.Rotate(0f, Random.Range(-180f, 180f), 0f);
                }
            }
            blackboard.startTimer = true;
        }
        return State.Success;
    }
}

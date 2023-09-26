using System.Collections;
using System.Collections.Generic;
using static System.Math;
using UnityEngine.SceneManagement;
using UnityEngine;
using TheKiwiCoder;
using jbzd.Enemies.Level1.Pokrzywa;
using jbzd.Dialogues.RuntimeData;
using jbzd.Enemies;
using jbzd.Common.Enums;
using jbzd.MainHero;
using Zenject;

[System.Serializable]
public class GrowPokrzywa : ActionNode
{
    private float _startTime;
    private float _maxSpreadDistance = 10f;
    private bool _growPokrzywa;
    private float _timeRemaining;
    private Scene _targetScene;
    private int _numberOfSamples = 20;
    private float _minDistance = 1.4f;
    private int _numberOfSons = 0;
    List<PokrzywaController> Sons = new List<PokrzywaController>();

    protected override void OnStart() {
        blackboard.waitBeforeSpreadDuration = blackboard.baseTime + Random.Range(-blackboard.deviation, blackboard.deviation) + (float)Sqrt(_numberOfSons) * blackboard.squareRootMultiplayer + _numberOfSons * blackboard.multiplayer;
        for(int i = 0; i < SceneManager.sceneCount;i++){
            if(SceneManager.GetSceneAt(i).name.Contains("Interactive")){
                _targetScene = SceneManager.GetSceneAt(i);
            }

        }
    }

    protected override void OnStop() {
    }


    private void OnSonsDeath(){
        _numberOfSons--;
    }

    private float DistanceToClosestSon(Vector3 position)
    {
        if(Sons.Count == 0){
            return float.MaxValue;
        }        
        
        float minDistance = float.MaxValue;
        
        foreach (PokrzywaController son in Sons)
        {
            float distance = Vector3.Distance(position, son.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
            }
        }
        
        return minDistance;
    }

    private Vector3 GenerateCoordinates(float range, Vector3 position){
        Vector3 newPoint = Vector3.zero;

        float newx = Random.Range(0, range);
        float newz = Random.Range(0, range);
        if(Random.Range(0, 2) == 0){
            newx = -newx;
        }
        if(Random.Range(0, 2) == 0){
            newz = -newz;
        }

        newPoint = new Vector3(position.x + newx, position.y, position.z + newz);
        return newPoint;
    }

    private Vector3 FindLocationForNewSon(float range, Vector3 position){
        if(range > _maxSpreadDistance){
            return Vector3.zero;
        }
        Sons.RemoveAll(item => item == null);
        for(int i = 0; i < _numberOfSamples; i++){
            Vector3 newPoint = GenerateCoordinates(range, position);
            
            if(DistanceToClosestSon(newPoint) > _minDistance){

                return newPoint;

            }
        }
        return FindLocationForNewSon(range*1.3f,position);
    }

    private float DistanceToPlayer(Vector3 position)
    {
        return Vector3.Distance(position, blackboard._playerManager.gameObject.transform.position);
    }

    protected override State OnUpdate() {
        if(!blackboard.isMother){
            return State.Success;
        }

        if (blackboard.startTimer)
        {
            _startTime = Time.time;
            blackboard.waitBeforeSpreadDuration = blackboard.baseTime + Random.Range(-blackboard.deviation, blackboard.deviation) + (float)Sqrt(_numberOfSons) * blackboard.squareRootMultiplayer + _numberOfSons * blackboard.multiplayer;
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

            if(DistanceToPlayer(context.transform.position) > 8f){

                blackboard.startTimer = true;
                return State.Success;
            }
            newPoint = FindLocationForNewSon(2,context.transform.position);

            if(newPoint == Vector3.zero){
                blackboard.startTimer = true;
                return State.Success;
            }

            _numberOfSons++;

            GameObject newOne = Object.Instantiate(blackboard.pokrzywaPrefab, newPoint, context.transform.rotation);

            PokrzywaController newOneController = newOne.GetComponent<PokrzywaController>();
            newOneController.isMother = false;
            newOneController.OnDeath += OnSonsDeath;
            Sons.Add(newOneController);

            newOne.gameObject.transform.Rotate(0f, Random.Range(-180f, 180f), 0f);
            SceneManager.MoveGameObjectToScene(newOne, _targetScene);

            blackboard.startTimer = true;
        }
        return State.Success;
    }
}

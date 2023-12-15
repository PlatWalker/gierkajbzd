using System.Collections;
using System.Collections.Generic;
using static System.Math;
using UnityEngine.SceneManagement;
using UnityEngine;
using TheKiwiCoder;
using jbzd.Enemies.Level1.Pokrzywa;
using jbzd.QuestSystem.QuestStructureElements;  
using jbzd.Dialogues.RuntimeData;
using jbzd.Enemies;
using jbzd.Common.Enums;
using jbzd.MainHero;
using Zenject;

[System.Serializable]
public class GrowPokrzywa : ActionNode
{
    private float _startTime;
    private bool _growPokrzywa;
    private float _timeRemaining;
    private Scene _targetScene;
    private int _numberOfSamples = 50;
    private int _numberOfSons = 0;
    private float _minDistanceToPlayer = 8f;
    private float _rangeMultiplier = 1.3f;
    private float _SphereCastRadiusForDetectingBuildings = 0.5f;
    private float _SphereCastRadiusForNeighbourhood = 0.333f;
    private List<PokrzywaController> Sons = new List<PokrzywaController>();

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

    private bool IsInBuilding(Vector3 pokrzywaPosition){
        Collider[] hitColliders = new Collider[1];
        int numColliders = Physics.OverlapSphereNonAlloc(pokrzywaPosition, blackboard.maxDimension*_SphereCastRadiusForDetectingBuildings,hitColliders, 1);
        return numColliders > 0;
    }

    private float DistanceToClosestSon(Vector3 pokrzywaPosition)
    {
        if(Sons.Count == 0){
            return float.MaxValue;
        }        
        
        float minDistance = float.MaxValue;
        
        foreach (PokrzywaController son in Sons)
        {
            float distance = Vector3.Distance(pokrzywaPosition, son.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
            }
        }
        
        return minDistance;
    }

    private Vector3 GenerateCoordinates(float range, Vector3 pokrzywaPosition){
        Vector3 newPoint = pokrzywaPosition;

        float newx = Random.Range(0, range);
        float newz = Random.Range(0, range);
        if(Random.Range(0, 2) == 0){
            newx = -newx;
        }
        if(Random.Range(0, 2) == 0){
            newz = -newz;
        }

        newPoint = new Vector3(pokrzywaPosition.x + newx, pokrzywaPosition.y, pokrzywaPosition.z + newz);
        return newPoint;
    }

    private Vector3 FindLocationForNewSon(float range, Vector3 pokrzywaPosition){
        if(range > blackboard.maxSpreadDistance){
            return Vector3.zero;
        }
        Sons.RemoveAll(item => item == null);
        for(int i = 0; i < _numberOfSamples; i++){
            Vector3 newPoint = GenerateCoordinates(range, pokrzywaPosition);
            
            if(DistanceToClosestSon(newPoint) > blackboard.maxDimension*_SphereCastRadiusForNeighbourhood && !IsInBuilding(newPoint)){

                return newPoint;

            }
        }
        return FindLocationForNewSon(range*_rangeMultiplier,pokrzywaPosition);
    }

    private float DistanceToPlayer(Vector3 pokrzywaPosition)    
    {
        if(blackboard._playerManager == null){
            Debug.LogError($"Player manager is null in Pokrzywa");
            return float.MaxValue;
        }
        return Vector3.Distance(pokrzywaPosition, blackboard._playerManager.gameObject.transform.position);
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
            Vector3 newPoint = context.transform.position;

            if(DistanceToPlayer(context.transform.position) > _minDistanceToPlayer){
                blackboard.startTimer = true;
                return State.Success;
            }
            newPoint = FindLocationForNewSon(2,context.transform.position);

            if(newPoint == Vector3.zero){
                blackboard.startTimer = true;
                return State.Success;
            }
            
            
            if(!blackboard.isInvincible){
                _numberOfSons++;
                GameObject newOne = GameObject.Instantiate(blackboard.Prefab, context.transform.position, context.transform.rotation);
                PokrzywaController newOneController = newOne.GetComponent<PokrzywaController>();
                newOne.GetComponent<Actor>().enabled = false;
                newOneController.isMother = false;
                newOneController.OnDeath += OnSonsDeath;
                
                Sons.Add(newOneController);

                newOne.gameObject.transform.Rotate(0f, Random.Range(-180f, 180f), 0f);
                SceneManager.MoveGameObjectToScene(newOne, _targetScene);

                newOneController.StartGrowing(context.transform.position, newPoint, blackboard.moveDuration);
            }

            blackboard.startTimer = true;
        }
        return State.Success;
    }


}

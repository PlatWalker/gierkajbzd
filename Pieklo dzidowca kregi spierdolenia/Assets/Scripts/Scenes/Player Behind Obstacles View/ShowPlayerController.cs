using System.Collections;
using System.Collections.Generic;
using jbzd;
using jbzdy.Player;
using UnityEngine;
using Zenject;

public class ShowPlayerController : MonoBehaviour
{
    public LayerMask Mask;
    private Material hitedMaterial,lastHitedMaterial;
    private int SizeID;
    [SerializeField] private float Size =2f;
    [SerializeField] private float transitionDuration=2f;
    [SerializeField] private Shader shaderToCompare = null;
    private Transform playerTransform;

    private enum State {
        notShowing,
        startingToShow,
        stoppingShowing,
        showing
    }

    private State state;
    
    private PlayerController _playerController;
        
    [Inject]
    public void Construct(PlayerController playerController)
    {
        _playerController = playerController;
    }
    
    private void Start()
    {
        playerTransform = _playerController.transform;
        SizeID = Shader.PropertyToID("_size");
        state = State.notShowing;
    }

    void Update()
    {
        Vector3 dir = playerTransform.position - transform.position;
        Ray ray = new Ray(transform.position, dir.normalized);
        RaycastHit hitinfo;
        if (Physics.Raycast(ray,out hitinfo, 100, Mask))
        {
            Renderer renderer = hitinfo.transform.GetComponent<Renderer>();
            if (!renderer)
            {
                if (state == State.showing || state == State.startingToShow)
                    state = State.stoppingShowing;
            }
            else
            {
                if (renderer.material.shader.Equals(shaderToCompare))
                {
                    hitedMaterial = renderer.material;

                    if (state == State.notShowing || state == State.stoppingShowing)
                    {
                        state = State.startingToShow;
                    }
                }
                else
                {
                    if (state == State.showing || state == State.startingToShow)
                        state = State.stoppingShowing;
                }
            }
        }
        else
        {
            if (state == State.showing || state == State.startingToShow)
                state = State.stoppingShowing;
        }

        float value;
        switch (state)
        {
            case State.startingToShow:
                if (!hitedMaterial) break;
                value = hitedMaterial.GetFloat(SizeID);
                value = value + Time.deltaTime / transitionDuration * Size;
                hitedMaterial.SetFloat(SizeID, value);
                if (value >= Size) state = State.showing;
                break;
            case State.stoppingShowing:
                if (!hitedMaterial) break;
                value = hitedMaterial.GetFloat(SizeID);
                value = value - Time.deltaTime / transitionDuration * Size;
                if (value < 0) value = 0;
                hitedMaterial.SetFloat(SizeID, value);
                if (value == 0) state = State.notShowing;
                break;
        }

        if (hitedMaterial && lastHitedMaterial)
        {
            if (hitedMaterial != lastHitedMaterial)
            {
                lastHitedMaterial.SetFloat(SizeID, 0);
            }
        }
        lastHitedMaterial = hitedMaterial;
    }
}

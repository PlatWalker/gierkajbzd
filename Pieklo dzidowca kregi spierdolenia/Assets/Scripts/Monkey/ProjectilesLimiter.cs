using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesLimiter : MonoBehaviour
{
    private static long nextID = 0;
    private static long IdToBeDestroyed = -1;
    private long ID;
    [SerializeField] private int projectilesLimit=10;
    private int pLimit;

    public ProjectilesLimiter()
    {
        ID = nextID;
        nextID++;
        pLimit = projectilesLimit;
        if (ID >= projectilesLimit)
        {
            IdToBeDestroyed = ID - pLimit;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.ID <= IdToBeDestroyed)
        {
            Destroy(this.gameObject);
        }   
    }
}

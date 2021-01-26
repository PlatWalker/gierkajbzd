using UnityEngine;
/// <summary>
/// Created by Kumdzio
/// </summary>
public class ProjectilesLimiter : MonoBehaviour
{
    private static long nextID = 0;
    private static long IdToBeDestroyed = -1;
    private long ID;
    private int projectilesLimit=500;
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

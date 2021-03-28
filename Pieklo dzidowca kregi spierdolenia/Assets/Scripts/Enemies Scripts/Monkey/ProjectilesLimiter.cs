using UnityEngine;
/// <summary>
/// Created by Kumdzio
/// </summary>
public class ProjectilesLimiter : MonoBehaviour
{
    private static long nextID = 0;
    private static long IdToBeDestroyed = -1;
    private long ID;
    private int projectilesLimit=50;

    public ProjectilesLimiter()
    {
        ID = nextID;
        nextID++;
        if (ID >= projectilesLimit)
        {
            IdToBeDestroyed = ID - projectilesLimit;
        }
    }

    void Update()
    {
        if (this.ID <= IdToBeDestroyed)
        {
            Destroy(this.gameObject);
        }   
    }
}

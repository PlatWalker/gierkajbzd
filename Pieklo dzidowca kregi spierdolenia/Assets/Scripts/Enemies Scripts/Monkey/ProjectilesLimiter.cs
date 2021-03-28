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

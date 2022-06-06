using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadController : MonoBehaviour
{
    [SerializeField] private float growUpRatio = 10f;
    [SerializeField] private float variesBy = 2f;
    [SerializeField] private float maxSpreadDistance = 3f;
    private float nextSeedlingAt = 0f;
    private float growUpTimer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        ChooseNewTimePoint();
    }

    // Update is called once per frame
    void Update()
    {
        growUpTimer += Time.deltaTime;
        
        if (growUpTimer >= nextSeedlingAt)
        {
            ChooseNewTimePoint();
            HandleGrowing();
        }

    }

    private void ChooseNewTimePoint()
    {
        growUpTimer = 0;
        nextSeedlingAt = growUpRatio + Random.Range(-variesBy, variesBy);
    }
    private void HandleGrowing()
    {
        Vector3 newPoint = Vector3.zero;
        RaycastHit hit;

        newPoint = new Vector3(transform.position.x + Random.Range(-maxSpreadDistance, maxSpreadDistance),
                               transform.position.y,
                               transform.position.z + Random.Range(-maxSpreadDistance, maxSpreadDistance));

        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y + 2, transform.position.z),
            newPoint - new Vector3(transform.position.x, transform.position.y + 2, transform.position.z));

        if (Physics.Raycast(ray, out hit, maxSpreadDistance * 200))
        {
            if (hit.transform.tag == "Terrain")
            {
                GameObject newOne = Instantiate(gameObject, newPoint, transform.rotation);
                newOne.gameObject.transform.Rotate(0f, Random.Range(-180f, 180f), 0f);
            }
        }
    }
}

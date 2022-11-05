using UnityEngine;

// Developer: Vhart

public class PoisonPoolScript : MonoBehaviour
{
    private bool fadeOut;
    private float fadeSpeed = 0.3f;
    public int DamagePerSecond;
    private bool TriggerDamage;
    [SerializeField] DamageController PoisonPoolCollider = null;
    float timePassed;
    int i;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            TriggerDamage = true;
        }   
    }

    private void OnTriggerStay(Collider other) 
    {
        if (other.CompareTag("Player") && TriggerDamage == true)
        {
            timePassed += Time.deltaTime;
            if(timePassed > 1f && TriggerDamage == true)
            {
                PoisonPoolCollider.DamageDealed = false;
                timePassed = 0f;
            } 
        }   
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            TriggerDamage = false;
        }   
    }

    private void Start()
    {
		PoisonPoolCollider.SetUp(DamagePerSecond);
    }
    void Update()
    {  
        i = i + 1;
        if (i <= 450)
        {
            PoisonPoolCollider.transform.localScale += new Vector3(0.01f, 0f, 0.01f);
        }

        if (i > 3000)
        {
            FadeOutObject();
        }

        if (fadeOut)
        {
            Color objectColor = this.GetComponent<Renderer>().material.color;
            float fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            this.GetComponent<Renderer>().material.color = objectColor;

            if(objectColor.a <= 0)
            {
                fadeOut = false;
                Destroy(gameObject);
            }
        }

    }

    public void FadeOutObject()
    {
        fadeOut = true;
    }
}
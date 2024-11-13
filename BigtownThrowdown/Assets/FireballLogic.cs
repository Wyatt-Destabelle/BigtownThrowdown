using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballLogic : MonoBehaviour
{
    public GameObject exp;
     // Start is called before the first frame update
    float lifeSpan;
    public float Speed;
    void Start()
    {
        lifeSpan = Random.Range(.4f,.6f);
        Vector2 v = Random.insideUnitCircle.normalized * Speed;

        v.y = -Mathf.Abs(v.y);
        GetComponent<Rigidbody2D>().velocity = v;
        


    }

    // Update is called once per frame
    void Update()
    {
        lifeSpan -= Time.deltaTime;
        if(lifeSpan < 0)
        {
            Instantiate(exp,transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

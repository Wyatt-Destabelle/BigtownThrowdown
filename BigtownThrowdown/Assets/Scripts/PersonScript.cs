using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonScript : MonoBehaviour
{
    public Vector2 topBoundary, bottomBoundary;
    Vector3 wanderDirection;
    float directionTime;
    public float minTime,maxTime;
    float speed = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Wander();
    }

    void Wander()
    {
        if(transform.position.x > topBoundary.x || transform.position.x < bottomBoundary.x)
        {     
            wanderDirection.x = bottomBoundary.x+ ((topBoundary.x - bottomBoundary.x)/2) - transform.position.x;
        }

        if(transform.position.y > topBoundary.y || transform.position.y < bottomBoundary.y)
        {
            wanderDirection.y = bottomBoundary.y + ((topBoundary.y - bottomBoundary.y)/2) - transform.position.y;
        }

        if(directionTime > 0)
        {
            transform.position += wanderDirection.normalized  *speed * Time.deltaTime;
            directionTime -= Time.deltaTime;
        }
        else
        {
            speed = Random.Range(1.5f,2.5f);
            directionTime = Random.Range(minTime,maxTime);
            wanderDirection = Random.insideUnitCircle.normalized;
        }
        

    }
}

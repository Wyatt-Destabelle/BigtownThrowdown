using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PersonScript : MonoBehaviour
{
    public Vector2 topBoundary, bottomBoundary;
    Vector3 wanderDirection,fleeDirection;
    float directionTime;
    public float minTime,maxTime;
    public float speed = 2;
    float s;
    public enum PersonState{Wander,Flee,Build,Die}
    public Sprite W,F,B;
    public PersonState _currentState;


    public float buildTime;
    BuildingScript currentBuilding;

    CityManager cityInfo;

    public float fearDistance;
    

    void Start()
    {
        cityInfo = FindAnyObjectByType<CityManager>();
        CityManager.peopleAmnt += 1;
    }
    public void StartState(PersonState newState)
    {
        if(_currentState == PersonState.Die)
            return;
        EndState(_currentState);
        
        switch (newState)
        {
            case PersonState.Flee:
                directionTime = 0;
                GetComponent<SpriteRenderer>().sprite = F;
                break;
            case PersonState.Wander: 
                GetComponent<SpriteRenderer>().sprite = W;
                break;
            case PersonState.Build: 
                GetComponent<SpriteRenderer>().sprite = B;
                break;
            case PersonState.Die: 
                break;
        }

    _currentState = newState;
    }
    private void UpdateState()
    {
        switch (_currentState)
        {
            case PersonState.Build:
                Build();
                break;
            case PersonState.Flee:
                Flee();
                break;
            case PersonState.Wander: 
                Wander();
                break;
            case PersonState.Die: 
                Die();
                break;
        }
    }
    private void EndState(PersonState oldState)
    {

    }
    void Update()
    {
        UpdateState();
    }
    void Die()
    {
        Color c = GetComponent<SpriteRenderer>().color;
        c.a -= Time.deltaTime;
        GetComponent<SpriteRenderer>().color =  c;
        if(c.a <= 0)
        {
            CityManager.peopleAmnt -= 1;
            Destroy(gameObject);
        }
        
    }
    void Build()
    {
        if(!currentBuilding)
            return;
        if(buildTime <= 0)
        {
            buildTime = .5f;
            if(!currentBuilding.Repair(5))
            {
                currentBuilding = null;
                StartState(PersonState.Wander);
            }
        }
        else
        {
            buildTime -= Time.deltaTime;
        }
        

    }

    void Flee()
    {
        if(directionTime > 0)
        {
            Debug.Log("STuck here");
            transform.position += fleeDirection.normalized  * 2 * s * Time.deltaTime;
            directionTime -= Time.deltaTime;
        }
        else
        {
            Debug.Log("here");
            if(!cityInfo.currentMonster)
            {
                StartState(PersonState.Wander);
                return;
            }
            if((cityInfo.currentMonster.transform.position - transform.position).magnitude < fearDistance)
            {
            s = Random.Range(1.15f * speed,1.35f * speed);
            directionTime = Random.Range(minTime,maxTime);
            fleeDirection = -(cityInfo.currentMonster.transform.position - transform.position).normalized;
            }
            else
            {
                StartState(PersonState.Wander);
            }
        }
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
            transform.position += wanderDirection.normalized  *s * Time.deltaTime;
            directionTime -= Time.deltaTime;
        }
        else
        {
            s = Random.Range(.75f * speed,1.25f * speed);
            directionTime = Random.Range(minTime,maxTime);
            wanderDirection = Random.insideUnitCircle.normalized;
        }
        

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(_currentState == PersonState.Wander && col.gameObject.tag == "Building")
        {
            if(col.GetComponent<BuildingScript>().isDestroyed)
            {
                currentBuilding = col.GetComponent<BuildingScript>();
                StartState(PersonState.Build);
            }
        }
    }
}

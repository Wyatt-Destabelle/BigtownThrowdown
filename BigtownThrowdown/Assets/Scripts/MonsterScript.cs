using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{
    // Start is called before the first frame update
    CityManager cityInfo;
    public enum MonsterState{Menace, Fireball, Destroy, Die,BreakBuilding};
    public MonsterState _currentState;
    public Sprite M, F, D,B;

    public int hp = 3;

    public GameObject fireball;

    float timer;
    float s = .85f;
    Vector3 direction;
    BuildingScript buildingTarget = null;
    void Start()
    {
        cityInfo = FindAnyObjectByType<CityManager>();
        StartState(MonsterState.Menace);
        Debug.Log(cityInfo);
    }
    void Update()
    {
        UpdateState();
    }
    public void StartState(MonsterState newState)
    {
    //First run the endstate of our old state
        EndState(_currentState);
        if(_currentState == MonsterState.Die)
            return;
        //Run any code that should run once at 
        //the beginning of a new state
        switch (newState)
        {
            case MonsterState.Die:
                cityInfo.currentMonster = null;
                break;
            case MonsterState.Menace:
                GetComponent<SpriteRenderer>().sprite = M;
                timer = 1.2f;
                direction = Random.insideUnitCircle.normalized;
                break;
            case MonsterState.Fireball:
                GetComponent<SpriteRenderer>().sprite = F;
                Instantiate(fireball,transform.position,Quaternion.identity);
                timer = .5f;
                //Noise
                break;
            case MonsterState.Destroy:
                GetComponent<SpriteRenderer>().sprite = M;
                buildingTarget = cityInfo.GetBuilt();
                if(buildingTarget == null)
                {
                     GetComponent<SpriteRenderer>().sprite = M;
                    newState = MonsterState.Menace;
                    direction = Random.insideUnitCircle.normalized;
                }
                break;
            case MonsterState.BreakBuilding:
                GetComponent<SpriteRenderer>().sprite = D;
                timer = .75f;
                break;
        }

    _currentState = newState;
    }
    private void UpdateState()
    {
        switch (_currentState)
        {
            case MonsterState.Die:
                Die();
                break;
            case MonsterState.Menace:
                Menace();
                break;
            case MonsterState.Fireball:
                Fireball();
                break;
            case MonsterState.Destroy:
                Destroy();
                break;
            case MonsterState.BreakBuilding:
                BreakBuilding();
                break;
            
        }
    }
    private void EndState(MonsterState oldState)
    {
    //Stop anything that might have been looping,
    //clean up loose ends from whatever state needs it
        switch (oldState)
        {
            case MonsterState.Menace:
                break;
            case MonsterState.Fireball:
                break;
            case MonsterState.Destroy:
                break;
            case MonsterState.BreakBuilding:
                break;
        }
    }
    public void dmg()
    {
        hp -= 1;
        if(hp == 0)
            StartState(MonsterState.Die);
    }
    void Die()
    {
        Color c = GetComponent<SpriteRenderer>().color;
        c.a -= Time.deltaTime;

        GetComponent<SpriteRenderer>().color =  c;
        if(c.a <= 0)
        {
            Destroy(gameObject);
        }
        
    }
    void DecisionMaker(int L)
    {
        int c = L;
        while(c == L)
            c = Random.Range(0,3);

        switch(c)
        {
            case 0:
                StartState(MonsterState.Menace);
                break;
            case 1:
                StartState(MonsterState.Fireball);
                break;
            case 2:
                StartState(MonsterState.Destroy);
                break;

        }
    }
    void Fireball()
    {
        
        if(timer <= 0)
        {
            DecisionMaker(1);
            return;
        }
        timer -= Time.deltaTime;
    }
    void Destroy()
    {
        transform.position = Vector3.MoveTowards(transform.position,buildingTarget.transform.position, s*1.5f*Time.deltaTime);
    }
    void BreakBuilding()
    {
        if(buildingTarget == null)
                DecisionMaker(2);
         if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        { 
            buildingTarget.StartState(BuildingScript.BuildingState.Destroyed);
            buildingTarget = null;
            DecisionMaker(2);
        }
    }

    void Menace()
    {

         if(timer > 0)
        {
            transform.position += direction.normalized  * s * Time.deltaTime;
            timer -= Time.deltaTime;
        }
        else
        {
            DecisionMaker(0);
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(_currentState == MonsterState.Destroy && col.gameObject.tag == "Building")
        {
            if(col.GetComponent<BuildingScript>() == buildingTarget)
            {
                StartState(MonsterState.BreakBuilding);
            }
        }
    }
}

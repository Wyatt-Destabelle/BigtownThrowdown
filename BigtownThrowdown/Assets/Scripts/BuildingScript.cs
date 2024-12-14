using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{
    int myIndice;
    CityManager cityInfo;
    public int hp = 100;
    public bool isDestroyed = false;

    public enum BuildingState{Built,Constructing,Destroyed}
    public Sprite B,D,C;
    public BuildingState _currentState;
    // Start is called before the first frame update
    void Start()
    {
        cityInfo = FindAnyObjectByType<CityManager>();
        cityInfo.addBuilding(this);
        if(isDestroyed)
            StartState(BuildingState.Destroyed);
    }
     public void StartState(BuildingState newState)
    {
    //First run the endstate of our old state
        EndState(_currentState);

        //Run any code that should run once at 
        //the beginning of a new state
        switch (newState)
        {
            case BuildingState.Built:
                isDestroyed = false;
                GetComponent<SpriteRenderer>().sprite =  B;
                break;
            case BuildingState.Constructing:
                GetComponent<SpriteRenderer>().sprite =  C;
                break;
            case BuildingState.Destroyed:
                isDestroyed = true;
                hp = 0;
                GetComponent<SpriteRenderer>().sprite = D;
                break;

        }

    _currentState = newState;
    }
    private void UpdateState()
    {

    }
    private void EndState(BuildingState oldState)
    {
    //Stop anything that might have been looping,

    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public bool Repair(int n)
    {
        if(hp < 100)
        {
            StartState(BuildingState.Constructing);
            hp += n;
        }
        else
        {
            hp = 100;
            StartState(BuildingState.Built);
            return false;
        }
        return true;
    }
}

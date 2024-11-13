using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityManager : MonoBehaviour
{
    float timer = 5;
    public static int peopleAmnt;
    public GameObject currentMonster;
    public GameObject monster;
    public GameObject plane;
    public GameObject person;
    public List<BuildingScript> buildings = null;
    // Start is called before the first frame update
    void Start()
    {
        if(buildings == null)
        {
            buildings = new List<BuildingScript>();
        }
    }

    public void addBuilding(BuildingScript b)
    {
        if(buildings == null)
        {
            buildings = new List<BuildingScript>();
        }
        buildings.Add(b);
    }
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            if(peopleAmnt < 20)
            {
                Instantiate(person, buildings[0].transform.position, Quaternion.identity);
            }
            timer = Random.Range(14,25);
            if(currentMonster != null)
            {
                if(Random.Range(0,2) == 1)
                    Instantiate(plane,new Vector3(-20,5,0),Quaternion.identity);
                
            }
            else
            {
                currentMonster = Instantiate(monster,new Vector3(15,0,0),Quaternion.identity);
            }
            
        }
    }
    public BuildingScript GetBuilt()
    {
        int i = Random.Range(0,buildings.Count);
        for(int j = i + 1; i != j; j++)
        {
            if(j == buildings.Count)
                j = 0;
            if(buildings[i]._currentState != BuildingScript.BuildingState.Destroyed)
            {
                return buildings[i];
            }
        }
        return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class JetScript : MonoBehaviour
{
    // Start is called before the first frame update
    public enum JetState{Climb, Dive, Leave, Fly};
    public MonsterScript target;
    public GameObject Misss;
    JetState _currentState;
    Rigidbody2D rb;
    float speed = 5;
    float timer;
    float h;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = FindAnyObjectByType<CityManager>().currentMonster.GetComponent<MonsterScript>();
        StartState(JetState.Fly);
    }

    // Update is called once per frame
    void Update()
    {

        UpdateState();
    }
    void Fly()
    {
        
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            StartState(JetState.Dive);
        }
    }
    void Leave()
    {
        if(transform.position.x > 21)
            Destroy(gameObject);
    }
    void Dive()
    {
        if(timer <= 0)
        {
            StartState(JetState.Leave);
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
    public void StartState(JetState newState)
    {
        EndState(_currentState);
        switch (newState)
        {
            case JetState.Fly:
                transform.eulerAngles = Vector3.zero;
                rb.velocity = speed * transform.right;
                timer = Random.Range(2f,3.5f); 
                break;
            case JetState.Dive: 
                if(target == null)
                {
                    StartState(JetState.Leave);
                    return;
                }
                timer = Random.Range(1.5f,2.25f);
                transform.eulerAngles = new Vector3(0,0, -20);
                rb.velocity = (.75f * speed) * transform.right;
                break;
            case JetState.Leave: 
                float a = 25 * Mathf.Sign(transform.localScale.x);
                transform.eulerAngles = new Vector3(0,0,a);
                rb.velocity *= 2;
                break;
        }
    _currentState = newState;
    }
    private void UpdateState()
    {
        switch (_currentState)
        {
            case JetState.Leave:
                Leave();
                break;
            case JetState.Fly:
                Fly();
                break;
            case JetState.Dive: 
                Dive();
                break;
        }
    }
    private void EndState(JetState oldState)
    {
        switch (oldState)
        {
            case JetState.Leave:
                break;
            case JetState.Fly:
                break;
            case JetState.Dive: 
                Instantiate(Misss,transform.position,transform.rotation);
                break;
        }
    }
}

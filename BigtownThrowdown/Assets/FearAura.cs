using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearAura : MonoBehaviour
{
    public GameObject following;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(following == null)   
            Destroy(gameObject);
        transform.position = following.transform.position;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("HEYEYEY");
        if(col.gameObject.tag == "Person")
        {
            col.gameObject.GetComponent<PersonScript>().StartState(PersonScript.PersonState.Flee);
        }
    }
}

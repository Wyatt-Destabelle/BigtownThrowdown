using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Missile : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject exp;
    public Vector3 target;
    void Start()
    {
        target = FindAnyObjectByType<CityManager>().currentMonster.transform.position;
        if(target == null)
            Destroy(gameObject);
        GetComponent<Rigidbody2D>().velocity = 15 * (target - transform.position).normalized; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Monster")
        {
            col.gameObject.GetComponent<MonsterScript>().dmg();
            Instantiate(exp,transform.position,Quaternion.identity);
            Destroy(gameObject);

        }
    }
}

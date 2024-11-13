using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class explosionLogic : MonoBehaviour
{
   public AudioClip otherClip;
    AudioSource audioSource;
    float Timer = 5;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = otherClip;
            audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            Destroy(this.gameObject);
        }
        Timer -= Time.deltaTime;
        if(Timer <= 0)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Person")
        {
            col.gameObject.GetComponent<PersonScript>().StartState(PersonScript.PersonState.Die);
        }
    }
}

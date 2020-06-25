using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    public AudioSource audioSource;

    private bool play;

    // Start is called before the first frame update
    void Start()
    {
        play = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (play)
            {
                audioSource.PlayOneShot(audioSource.clip);
                play = false;
            }

        }
    }

}

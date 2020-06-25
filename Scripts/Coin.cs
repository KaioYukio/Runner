using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private AudioSource audioSource;
    private bool canPlay;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        canPlay = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 1, 0);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (canPlay)
            {
                CoinManager.instance.AddCoin();
                transform.GetComponent<SpriteRenderer>().enabled = false;
                audioSource.PlayOneShot(audioSource.clip);
                Invoke("DestroyGO", 1f);
                canPlay = false;
            }

        }
    }

    public void DestroyGO()
    {
        Destroy(gameObject);
    }
}

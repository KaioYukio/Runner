using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUI : MonoBehaviour
{
    public GameObject[] life;
    private int index;

    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        index = 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void TakeDamage()
    {
        if (index > -1)
        {
            life[index].SetActive(false);
        }


        index--;

        if (index < 0)
        {
            CheckPointManager.instance.ReloadScene(Player.instance.rb);

            Debug.Log("Game Over!");
        }
    }

    public void RecoverHealth()
    {

        if (index < life.Length - 1)
        {
            index++;

            life[index].SetActive(true);

            audioSource.PlayOneShot(audioSource.clip);

            

        }


    }

}

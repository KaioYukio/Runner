using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpressoraBoss : MonoBehaviour
{
    [Header("Assign")]
    public GameObject ataque;
    public GameObject movimento;
    public GameObject dead;
    public GameObject[] words;
    public Transform[] pos;
    public Transform spawnPoint;

    [Header("Stats")]
    public int indexWord;
    public float speedY;
    public float spawnTime;

    private int indexPos;
    private bool spawning;

    private Animator an;
    private Rigidbody2D rb;

    public AudioSource audioSource;


    private bool isDead;

    


    // Start is called before the first frame update
    void Start()
    {
        an = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!CheckPointManager.instance.isPaused)
        {
            if (indexWord < words.Length)
            {
                MoveToPos();
            }


            if (indexWord >= words.Length && !isDead)
            {
                ataque.SetActive(false);
                movimento.SetActive(true);

                StartCoroutine(Dead());

            }
        }
        else
        {
            //Time.timeScale = 0;
        }


    }



    public void MoveToPos()
    {
        
        if (transform.localPosition == pos[indexPos].localPosition && !spawning)
        {
            StartCoroutine(SpawnWord());
        }
        else if (!spawning)
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, pos[indexPos].localPosition, speedY * Time.deltaTime);
            ataque.SetActive(false);
            movimento.SetActive(true);
        }

    }

    public void SpawnBlockWords(int i)
    {
        Instantiate(words[i],new Vector3(spawnPoint.position.x, spawnPoint.position.y, 0) , Quaternion.identity);
    }

    public IEnumerator SpawnWord()
    {
        if (!CheckPointManager.instance.isPaused)
        {
            spawning = true;
            movimento.SetActive(false);
            ataque.SetActive(true);

            yield return new WaitForSeconds(spawnTime);
            audioSource.PlayOneShot(audioSource.clip);

            ataque.SetActive(false);
            movimento.SetActive(true);

            Debug.Log("Spawn Word!");


            if (indexWord < words.Length)
            {
                SpawnBlockWords(indexWord);
                indexWord++;
            }

            int rand;

            if (indexPos == 0)
            {
                rand = Random.Range(0, 2);
            }
            else
            {
                rand = Random.Range(0, pos.Length);

            }

            indexPos = rand;

            spawning = false;
        }
        
    }

    public IEnumerator Dead()
    {
        isDead = true;

        while (transform.localPosition != pos[2].localPosition)
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, pos[2].localPosition, speedY * Time.deltaTime);

            yield return null;
        }

        ataque.SetActive(false);
        movimento.SetActive(false);
        dead.SetActive(true);

        an.SetBool("Death", true);
        rb.bodyType = RigidbodyType2D.Dynamic;


    }


    public void OnBecameInvisible()
    {
        Destroy(transform.parent.gameObject);
    }

}

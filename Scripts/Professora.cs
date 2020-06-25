using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Professora : MonoBehaviour
{
    [Header("Assign")]
    public GameObject[] enzima;
    public Transform[] pos;
    private Vector2 posTemp;
    public Transform spawnPoint;
    public Transform hitPoint;
    public Transform hitPoint2;
    public Animator an;
    public ParticleSystem vfx;

    [Header("Stats")]
    public float speedY;
    public float spawnTime;
    public float waitTime;

    public int enzimaNum;
    public int maxEnzima;

    public int indexPos;
    private bool spawning;

    private Rigidbody2D rb;

    public AudioSource audioSource;


    private bool isDead;
    public int tempIndex;




    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isDead = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!CheckPointManager.instance.isPaused)
        {
            if (enzimaNum < maxEnzima)
            {
                MoveToPos();
            }


            if (enzimaNum >= maxEnzima && !isDead)
            {

                StartCoroutine(Dead());

            }
        }


    }



    public void MoveToPos()
    {


        //else
        //{
        //    an.SetBool("Tras", false);
        //    an.SetBool("Frente", false);
        //}

        transform.position = Vector2.MoveTowards(transform.position, pos[indexPos].position, speedY * Time.deltaTime);

        if (transform.position == pos[indexPos].position && !spawning)
        {
            StartCoroutine(SpawnWord());
            audioSource.PlayOneShot(audioSource.clip);
        }

        if (spawning)
        {
            transform.position = pos[indexPos].position;
        }
        else
        {
            //if (tempIndex > indexPos)
            //{
            //    an.SetBool("Frente", true);
            //    an.SetBool("Tras", false);
            //}
            //else if (tempIndex < indexPos)
            //{
            //    an.SetBool("Tras", true);
            //    an.SetBool("Frente", false);
            //}
        }


    }

    public void SpawnEnzima()
    {
        int rand = Random.Range(0, enzima.Length);

        if (indexPos == 3 || indexPos == 2 || indexPos == 1)
        {
            GameObject go = Instantiate(enzima[rand], new Vector3(spawnPoint.position.x, spawnPoint.position.y, 0), Quaternion.identity);
            go.GetComponent<Enzima>().Destinitaion(hitPoint2);
        }
        else
        {
            GameObject go = Instantiate(enzima[rand], new Vector3(spawnPoint.position.x, spawnPoint.position.y, 0), Quaternion.identity);
            go.GetComponent<Enzima>().Destinitaion(hitPoint);
        }

        an.SetTrigger("Attack");
        vfx.Stop();

    }

    public IEnumerator SpawnWord()
    {
        if (!CheckPointManager.instance.isPaused)
        {
            spawning = true;
            vfx.Play();


            an.SetTrigger("Prepare");

            yield return new WaitForSeconds(spawnTime);



            tempIndex = indexPos;

            Debug.Log("Spawn Enzima!");


            if (enzimaNum < maxEnzima)
            {
                SpawnEnzima();
                enzimaNum++;
            }

            yield return new WaitForSeconds(waitTime);

            int rand;

            rand = Random.Range(0, pos.Length);

            indexPos = rand;
            

            spawning = false;
        }

    }

    public IEnumerator Dead()
    {
        isDead = true;

        while (transform.position != pos[2].position)
        {
            transform.position = Vector2.MoveTowards(transform.position, pos[2].position, speedY * Time.deltaTime);

            yield return null;
        }

        //an.SetBool("Death", true);
        rb.bodyType = RigidbodyType2D.Dynamic;

        Invoke("GameOver", 2);
    }


    public void OnBecameInvisible()
    {
        
    }


    public void GameOver()
    {
        Destroy(gameObject);
        CheckPointManager.instance.GameOver();
    }
}

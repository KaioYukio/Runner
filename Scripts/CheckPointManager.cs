using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPointManager : MonoBehaviour
{
    public static CheckPointManager instance;

    public GameObject gameOver;

    public Vector3 playerPos;
    public int coinsSaved;
    public GameObject[] colectables;

    public bool isPaused;
    public bool isDead;

    public Animator an;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (an == null)
        {
            an = GameObject.FindGameObjectWithTag("CineCam").GetComponent<Animator>();
        }

        if (gameOver == null)
        {
            gameOver = GameObject.FindGameObjectWithTag("GameOverScreen");
        }
    }

    public void SavePos(Vector3 posToSave)
    {
        playerPos = posToSave;
        coinsSaved = CoinManager.instance.coin;
    }

    public void ReloadScene(Rigidbody2D rb)
    {
        if (!isPaused)
        {
            StartCoroutine(DeathAnimation(rb));
        }

    }

    public IEnumerator DeathAnimation(Rigidbody2D rb)
    {
        isPaused = true;
        isDead = true;

        rb.GetComponent<BoxCollider2D>().enabled = false;
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(0, 8), ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.3f);

        an.SetTrigger("Death");

        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        yield return new WaitForSeconds(0.5f);

        rb.bodyType = RigidbodyType2D.Dynamic;

        yield return new WaitForSeconds(0.5f);

        isPaused = false;
        isDead = false;
        SceneManager.LoadScene(1);

    }

    
    public void GameOver()
    {
        isPaused = true;
        gameOver.transform.GetChild(0).gameObject.SetActive(true);
    }

}

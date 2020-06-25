using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumbersObstacle : MonoBehaviour
{
    public bool isVisible;

    public float speed;

    public Transform[] pos;
    public int index;

    public bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        isVisible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isVisible)
        {
            transform.Translate(new Vector2(-speed * Time.deltaTime, 0));
        }

        if (transform.localPosition == pos[0].localPosition)
        {
            index = 1;
        }
        else if (transform.localPosition == pos[1].localPosition)
        {
            index = 0;
        }

        if (canMove)
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, pos[1].localPosition, speed * Time.deltaTime);
        }
        

    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //isVisible = true;
            canMove = true;

            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = Vector2.zero;

                //rb.GetComponent<Player>().canMove = false;
            }

        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        
    }

}

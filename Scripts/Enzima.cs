using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enzima : MonoBehaviour
{
    private Vector2 initPlayerPos;
    private Vector2 tempY;
    public float speed;
    public float minDist;
    public bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        //initPlayerPos = GameObject.FindGameObjectWithTag("HitPos").transform.position;

        Vector3 dir = Player.instance.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }

    // Update is called once per frame
    void Update()
    {

        if (canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, initPlayerPos, speed * Time.deltaTime);
        }

       
    }

    public void Destinitaion(Transform dest)
    {
        initPlayerPos = dest.position;
        canMove = true;
        Debug.Log("Update Destination");
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" || collision.collider.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }

}

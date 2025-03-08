using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBounceController : MonoBehaviour
{
    Rigidbody2D rb;
    ProjectileController pCon;

    int bounceCounter;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;

        pCon = transform.parent.GetComponent<ProjectileController>();
        bounceCounter = 3;

        if (pCon.ProjType != "Bouncy")
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (bounceCounter <= 0)
        {
            gameObject.SetActive(false);
        }

        if (-GameController.bounceInsideBoxCorner.x <= pCon.transform.position.x && pCon.transform.position.x <= GameController.bounceInsideBoxCorner.x &&
            -GameController.bounceInsideBoxCorner.y <= pCon.transform.position.y && pCon.transform.position.y <= GameController.bounceInsideBoxCorner.y &&
            rb.bodyType == RigidbodyType2D.Static)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Wall")
        {
            pCon.DirVect = new(-pCon.DirVect.y, pCon.DirVect.x, 0);
            bounceCounter--;
        }
    }
}

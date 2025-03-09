using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator ani;

    public Sprite[] ProjSpriteArray;

    public GameObject burstShrapnelPrefab;

    public string ProjType;

    public float SpeedMod;
    public Vector3 DirVect;
    public float ScaleMod;

    bool bounceBuffer;
    public int bounceCounter;

    float rotateRate = 0.0015f;
    float rotateVariance = 0.0002f;

    public float burstTime = 0;

    float ExpireTimer = 8;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (ProjType != "Tracking")
        {
            ani = GetComponent<Animator>();
        }

        GetComponent<SpriteRenderer>().sprite = ProjSpriteArray[GameController.CurrentLevel];

        transform.localScale *= ScaleMod;

        bounceCounter = 3;
        bounceBuffer = true;

        rotateRate = Random.Range(rotateRate - rotateVariance, rotateRate + rotateVariance);
        if (Random.Range(0, 2) == 0) { rotateRate *= -1; transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, 1f); }

        if (ProjType == "Burst")
        {
            StartCoroutine(burstCountdown());
        }

        if (ProjType == "Tracking")
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(DirVect.y, DirVect.x) * Mathf.Rad2Deg - 90));
        }

        if (ProjType == "Bends")
        {
            ExpireTimer = 4;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ExpireTimer -= Time.deltaTime;

        if (ExpireTimer <= 0)
        {
            GameController.ActiveProjectiles.Remove(gameObject);
            Destroy(gameObject);
        }

        switch (ProjType)
        {
            case "Basic":
                rb.velocity = DirVect * SpeedMod * GameController.CurrentGameSpeed;
                ani.speed = (SpeedMod/4) * GameController.CurrentGameSpeed;
                break;
            case "Bouncy":
                rb.velocity = DirVect * SpeedMod * GameController.CurrentGameSpeed;
                ani.speed = (SpeedMod / 4) * GameController.CurrentGameSpeed;

                if (-GameController.bounceInsideBoxCorner.x < transform.position.x && transform.position.x < GameController.bounceInsideBoxCorner.x &&
                    -GameController.bounceInsideBoxCorner.y < transform.position.y && transform.position.y < GameController.bounceInsideBoxCorner.y &&
                    bounceBuffer) { bounceBuffer = false; }

                if (bounceCounter > 0 && !bounceBuffer)
                {

                    if (transform.position.x > GameController.bounceInsideBoxCorner.x) {
                        transform.position = new(GameController.bounceInsideBoxCorner.x, transform.position.y, 0); 
                        DirVect = new(-DirVect.x, DirVect.y, 0); 
                        bounceCounter--;
                    }
                    if (transform.position.x < -GameController.bounceInsideBoxCorner.x) { 
                        transform.position = new(-GameController.bounceInsideBoxCorner.x, transform.position.y, 0); 
                        DirVect = new(-DirVect.x, DirVect.y, 0); 
                        bounceCounter--; 
                    }

                    if (transform.position.y > GameController.bounceInsideBoxCorner.y) { 
                        transform.position = new(transform.position.x, GameController.bounceInsideBoxCorner.y, 0);
                        DirVect = new(DirVect.x, -DirVect.y, 0); 
                        bounceCounter--; 
                    }
                    if (transform.position.y < -GameController.bounceInsideBoxCorner.y) { 
                        transform.position = new(transform.position.x, -GameController.bounceInsideBoxCorner.y, 0);
                        DirVect = new(DirVect.x, -DirVect.y, 0); 
                        bounceCounter--; }
                }

                if (bounceCounter <= 0)
                {
                    StartCoroutine(bounceReorderLayerBuffer());
                }

                break;
            case "Tracking":
                rb.velocity = DirVect * SpeedMod * GameController.CurrentGameSpeed;
                break;
            case "Bends":
                rb.velocity = DirVect * SpeedMod * GameController.CurrentGameSpeed;
                DirVect = new(DirVect.x * Mathf.Cos(rotateRate) - DirVect.y * Mathf.Sin(rotateRate), DirVect.x * Mathf.Sin(rotateRate) + DirVect.y * Mathf.Cos(rotateRate), 0);
                ani.speed = (SpeedMod / 4) * GameController.CurrentGameSpeed;
                break;
            case "Burst":
                rb.velocity = DirVect * SpeedMod * GameController.CurrentGameSpeed;
                ani.speed = (SpeedMod / 4) * GameController.CurrentGameSpeed;
                break;
        }
    }

    IEnumerator bounceReorderLayerBuffer()
    {
        yield return new WaitForSeconds(0.75f);
        GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    IEnumerator burstCountdown()
    {
        yield return new WaitForSeconds(burstTime);

        for (int i = 0; i < 15; i++)
        {
            GameObject shrapnel = Instantiate(burstShrapnelPrefab);
            shrapnel.transform.position = transform.position;
            shrapnel.transform.localScale *= ScaleMod * 1.4f;
            shrapnel.GetComponent<ProjectileShrapnelController>().DirVect = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
            GameController.ActiveProjectiles.Add(shrapnel);
        }

        GameController.ActiveProjectiles.Remove(gameObject);
        Destroy(gameObject);
    }

}

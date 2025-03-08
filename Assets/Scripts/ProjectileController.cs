using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{

    public string ProjType;

    public float SpeedMod;
    public Vector3 DirVect;
    public float ScaleMod;

    float baseSpeed = 0.008f;

    bool bounceBuffer;
    int bounceCounter;


    // Start is called before the first frame update
    void Start()
    {
        transform.localScale *= ScaleMod;

        bounceCounter = 3;
        bounceBuffer = true;
        StartCoroutine(bounceStartBuffer());
    }

    // Update is called once per frame
    void Update()
    {
        switch (ProjType)
        {
            case "Basic":
                transform.position += DirVect * baseSpeed * SpeedMod * GameController.CurrentGameSpeed;
                break;
            case "Bouncy":
                transform.position += DirVect * baseSpeed * SpeedMod * GameController.CurrentGameSpeed;

                if (bounceCounter > 0 && !bounceBuffer)
                {
                    GetComponent<SpriteRenderer>().sortingOrder = 3;

                    if (transform.position.x - ScaleMod >= GameController.bounceInsideBoxCorner.x) { transform.position = new(GameController.bounceInsideBoxCorner.x + ScaleMod, transform.position.y, 0); DirVect = new(-DirVect.x, DirVect.y, 0); bounceCounter--; }
                    if (transform.position.x + ScaleMod <= -GameController.bounceInsideBoxCorner.x) { transform.position = new(-GameController.bounceInsideBoxCorner.x - ScaleMod, transform.position.y, 0); DirVect = new(-DirVect.x, DirVect.y, 0); bounceCounter--; }

                    if (transform.position.y - ScaleMod >= GameController.bounceInsideBoxCorner.y) { transform.position = new(transform.position.x, GameController.bounceInsideBoxCorner.y + ScaleMod, 0); DirVect = new(DirVect.x, -DirVect.y, 0); bounceCounter--; }
                    if (transform.position.y + ScaleMod <= -GameController.bounceInsideBoxCorner.y) { transform.position = new(transform.position.x, -GameController.bounceInsideBoxCorner.y - ScaleMod, 0); DirVect = new(DirVect.x, -DirVect.y, 0); bounceCounter--; }
                }

                if (bounceCounter <= 0)
                {
                    StartCoroutine(bounceReorderLayerBuffer());
                }

                break;
        }
    }

    IEnumerator bounceStartBuffer()
    {
        yield return new WaitForSeconds(1f);
        bounceBuffer = false;
    }

    IEnumerator bounceReorderLayerBuffer()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    void OnBecameInvisible()
    {
        GameController.ActiveProjectiles.Remove(gameObject);
        Destroy(gameObject);
    }
}

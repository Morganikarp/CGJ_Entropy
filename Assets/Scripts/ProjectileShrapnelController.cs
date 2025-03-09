using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShrapnelController : MonoBehaviour
{
    public Vector3 DirVect;

    void Start()
    {
        transform.eulerAngles = new(0, 0, Random.Range(0, 360));        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = DirVect * 10 * GameController.CurrentGameSpeed;
    }

    void OnBecameInvisible()
    {
        GameController.ActiveProjectiles.Remove(gameObject);
        Destroy(gameObject);
    }
}

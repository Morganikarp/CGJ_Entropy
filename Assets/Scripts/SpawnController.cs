using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public GameObject Buzzsaw;
    public Transform PlayerTrans;

    public float SpawnPointHorizontal;
    public float SpawnPointVertical;

    Vector3 TL_pos;
    Vector3 TR_pos;
    Vector3 BL_pos;
    Vector3 BR_pos;

    void Start()
    {
        TL_pos = new(-SpawnPointHorizontal, SpawnPointVertical, 0);
        TR_pos = new(SpawnPointHorizontal, SpawnPointVertical, 0);
        BL_pos = new(-SpawnPointHorizontal, -SpawnPointVertical, 0);
        BR_pos = new(SpawnPointHorizontal, -SpawnPointVertical, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject newProjectile = Instantiate(Buzzsaw);
            newProjectile.transform.position = TR_pos;
            newProjectile.GetComponent<ProjectileController>().DirVect = (PlayerTrans.position - newProjectile.transform.position).normalized;
        }
    }
}

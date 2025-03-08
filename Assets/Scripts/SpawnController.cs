using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public GameObject[] ProjectileArray;
    public Transform PlayerTrans;

    public float SpawnPointHorizontal;
    public float SpawnPointVertical;

    Vector3 TL_pos;
    Vector3 TM_pos;
    Vector3 TR_pos;

    Vector3 ML_pos;
    Vector3 MR_pos;

    Vector3 BL_pos;
    Vector3 BM_pos;
    Vector3 BR_pos;

    void Start()
    {
        TL_pos = new(-SpawnPointHorizontal, SpawnPointVertical, 0);
        TM_pos = new(0, SpawnPointVertical, 0);
        TR_pos = new(SpawnPointHorizontal, SpawnPointVertical, 0);

        ML_pos = new(-SpawnPointHorizontal, 0, 0);
        MR_pos = new(SpawnPointHorizontal, 0, 0);

        BL_pos = new(-SpawnPointHorizontal, -SpawnPointVertical, 0);
        BM_pos = new(0, -SpawnPointVertical, 0);
        BR_pos = new(SpawnPointHorizontal, -SpawnPointVertical, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(LevelSchedule(GameController.CurrentLevel));
        }
    }

    IEnumerator LevelSchedule(int level)
    {
        yield return new WaitForSeconds(1f);

        switch (level)
        {
            case 0:
                GameObject newProjectile = Instantiate(ProjectileArray[0]);
                newProjectile.transform.position = TM_pos;
                newProjectile.GetComponent<ProjectileController>().DirVect = Vector3.down;
                //newProjectile.GetComponent<ProjectileController>().DirVect = (PlayerTrans.position - newProjectile.transform.position).normalized;

                yield return new WaitForSeconds(0.5f);

                newProjectile = Instantiate(ProjectileArray[0]);
                newProjectile.transform.position = (TM_pos+TL_pos) / 2;
                newProjectile.GetComponent<ProjectileController>().DirVect = Vector3.down;

                newProjectile = Instantiate(ProjectileArray[0]);
                newProjectile.transform.position = (TM_pos + TR_pos) / 2;
                newProjectile.GetComponent<ProjectileController>().DirVect = Vector3.down;

                yield return new WaitForSeconds(2f);

                newProjectile = Instantiate(ProjectileArray[0]);
                newProjectile.transform.position = (TL_pos + ML_pos) / 2;
                newProjectile.GetComponent<ProjectileController>().DirVect = Vector3.right;

                newProjectile = Instantiate(ProjectileArray[0]);
                newProjectile.transform.position = (BR_pos + MR_pos) / 2;
                newProjectile.GetComponent<ProjectileController>().DirVect = Vector3.left;

                for (int i = 0; i < 2; i++)
                {

                }

                break;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public GameObject ProjectilePrefab;
    public Transform PlayerTrans;

    bool LevelActive;
    float LevelTimer;

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
        LevelActive = false;

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
        if (Input.GetKeyDown(KeyCode.Space) && !LevelActive)
        {
            LevelActive = true;
            StartCoroutine(LevelSchedule(GameController.CurrentLevel));
        }

        if (LevelActive)
        {
            LevelTimer += Time.deltaTime;

            if (LevelTimer >= 30f)
            {
                LevelActive = false;
                LevelTimer = 0;
            }
        }

        if (!GameController.PlayerAlive)
        {
            LevelActive = false;
            StopAllCoroutines();
        }
    }

    void CreateProjectile(string projType, Vector3 spawnV, float speedMod, Vector3 dirV, float scaleMod)
    {
        GameObject newProjectile = Instantiate(ProjectilePrefab);
        GameController.ActiveProjectiles.Add(newProjectile);

        ProjectileController pCon = newProjectile.GetComponent<ProjectileController>();

        pCon.transform.position = spawnV;
        pCon.ProjType = projType;
        pCon.SpeedMod = speedMod;
        pCon.DirVect = dirV;
        pCon.ScaleMod = scaleMod;
    }

    IEnumerator LevelSchedule(int level)
    {
        yield return new WaitForSeconds(1f);

        switch (level)
        {
            case 0:
                CreateProjectile("Basic", TM_pos, 1, Vector3.down, 2);

                yield return new WaitForSeconds(0.5f);

                CreateProjectile("Basic", (TM_pos + TL_pos) / 2, 1, Vector3.down, 1.2f);
                CreateProjectile("Basic", (TM_pos + TR_pos) / 2, 1, Vector3.down, 1.2f);

                yield return new WaitForSeconds(2f);

                CreateProjectile("Basic", (TL_pos + ML_pos) / 2, 1, Vector3.right, 1.5f);
                CreateProjectile("Basic", (BR_pos + MR_pos) / 2, 1, Vector3.left, 1.5f);

                yield return new WaitForSeconds(2f);

                for (int i = 0; i < 3; i++)
                {
                    Vector3 posLerp = Vector3.Lerp(BL_pos, ML_pos, 0.35f + (0.25f * i));
                    CreateProjectile("Basic", posLerp, 2, Vector3.right, 1);
                }

                for (int i = 0; i < 3; i++)
                {
                    Vector3 posLerp = Vector3.Lerp(TR_pos, MR_pos, 0.35f + (0.25f * i));
                    CreateProjectile("Basic", posLerp, 2, Vector3.left, 1);
                }

                yield return new WaitForSeconds(2f);

                CreateProjectile("Bouncy", TM_pos, 1.5f, new(0.5f, -0.5f, 0), 1.25f);
                CreateProjectile("Bouncy", BM_pos, 1.5f, new(-0.5f, 0.5f, 0), 1.25f);

                yield return new WaitForSeconds(2f);

                CreateProjectile("Basic", BM_pos, 1, Vector3.up, 2);
                CreateProjectile("Bouncy", (BL_pos + ML_pos) / 2, 2, new(0.5f, 0.5f, 0), 1f);
                CreateProjectile("Bouncy", (BR_pos + MR_pos) / 2, 2, new(-0.5f, 0.5f, 0), 1f);

                yield return new WaitForSeconds(4f);

                CreateProjectile("Bouncy", TL_pos, 2, new(0.5f, -0.5f, 0), 1.25f);
                CreateProjectile("Bouncy", TR_pos, 2, new(-0.5f, -0.5f, 0), 1.25f);
                CreateProjectile("Bouncy", (TL_pos + ML_pos) / 2, 2, new(0.3f, -0.7f, 0), 1f);
                CreateProjectile("Bouncy", (TR_pos + MR_pos) / 2, 2, new(-0.3f, -0.7f, 0), 1f);

                yield return new WaitForSeconds(2f);

                CreateProjectile("Basic", (BL_pos + BM_pos) / 2, 0.8f, Vector3.up, 3f);
                CreateProjectile("Basic", (BM_pos + BR_pos) / 2, 0.8f, Vector3.up, 3f);

                break;
        }

    }
}

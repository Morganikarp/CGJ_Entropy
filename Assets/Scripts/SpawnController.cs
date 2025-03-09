using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public GameObject ProjectilePrefab;
    public Transform PlayerTrans;

    public TextMeshProUGUI OnscreenTimer;
    public bool LevelActive;
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
        LevelTimer = 30;

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
        OnscreenTimer.text = Mathf.CeilToInt(LevelTimer).ToString();

        if (Input.GetKeyDown(KeyCode.Space) && !LevelActive)
        {
            LevelActive = true;
            //StartCoroutine(LevelSchedule(GameController.CurrentLevel));
            StartCoroutine(LevelSchedule(1));
        }

        if (LevelActive)
        {
            LevelTimer -= Time.deltaTime;

            if (LevelTimer <= 0)
            {
                LevelActive = false;
                LevelTimer = 30;
            }
        }

        else
        {
            LevelTimer = 30;
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

        pCon.transform.position = spawnV + new Vector3(0,-0.5f,0);
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
                CreateProjectile("Basic", TM_pos, 5, Vector3.down, 2); // 1

                yield return new WaitForSeconds(0.75f);

                CreateProjectile("Basic", (TM_pos + TL_pos) / 2, 5, Vector3.down, 1.2f);
                CreateProjectile("Basic", (TM_pos + TR_pos) / 2, 5, Vector3.down, 1.2f);

                yield return new WaitForSeconds(2f);

                Vector3 vertAdjust = new(0, 2.5f, 0);
                CreateProjectile("Basic", ML_pos + vertAdjust, 5, Vector3.right, 1.5f);
                CreateProjectile("Basic", MR_pos - vertAdjust, 5, Vector3.left, 1.5f);

                yield return new WaitForSeconds(2f);

                for (int i = 0; i < 3; i++)
                {
                    Vector3 posLerp = Vector3.Lerp(BL_pos, ML_pos, 0.4f + (0.25f * i));
                    CreateProjectile("Basic", posLerp, 7, Vector3.right, 1);
                }

                for (int i = 0; i < 3; i++)
                {
                    Vector3 posLerp = Vector3.Lerp(TR_pos, MR_pos, 0.4f + (0.25f * i));
                    CreateProjectile("Basic", posLerp, 7, Vector3.left, 1);
                }

                yield return new WaitForSeconds(2f); // 2

                CreateProjectile("Bouncy", ML_pos, 8, Vector3.right, 1.75f);

                yield return new WaitForSeconds(4.5f);

                CreateProjectile("Basic", BM_pos, 6, Vector3.up, 1.4f);
                CreateProjectile("Bouncy", (BL_pos + BM_pos) / 2, 6, Vector3.up, 1.4f);
                CreateProjectile("Bouncy", (BM_pos + BR_pos) / 2, 6, Vector3.up, 1.4f);

                yield return new WaitForSeconds(3f);

                CreateProjectile("Basic", TL_pos, 8f, new(0.5f, -0.5f, 0), 1.6f);
                CreateProjectile("Basic", TR_pos, 8f, new(-0.5f, -0.5f, 0), 1.6f);

                yield return new WaitForSeconds(3f); // 3

                CreateProjectile("Bouncy", TM_pos, 8f, new(0.5f, -0.5f, 0), 1.25f);
                CreateProjectile("Bouncy", BM_pos, 8f, new(-0.5f, 0.5f, 0), 1.25f);

                yield return new WaitForSeconds(3f);

                CreateProjectile("Basic", BM_pos, 4, Vector3.up, 5);
                CreateProjectile("Bouncy", (BL_pos + ML_pos) / 2, 5.5f, new(0.5f, 0.5f, 0), 1f);
                CreateProjectile("Bouncy", (BR_pos + MR_pos) / 2, 5.5f, new(-0.5f, 0.5f, 0), 1f);

                yield return new WaitForSeconds(3f);

                CreateProjectile("Bouncy", TL_pos, 10, new(0.5f, -0.5f, 0), 1.25f);
                CreateProjectile("Bouncy", TR_pos, 10, new(-0.5f, -0.5f, 0), 1.25f);
                CreateProjectile("Bouncy", (TL_pos + ML_pos) / 2, 10, new(0.5f, -0.65f, 0), 1f);
                CreateProjectile("Bouncy", (TR_pos + MR_pos) / 2, 10, new(-0.5f, -0.65f, 0), 1f);

                yield return new WaitForSeconds(3f);

                CreateProjectile("Basic", (BL_pos + BM_pos) / 2, 10, Vector3.up, 2.5f);
                CreateProjectile("Basic", (BM_pos + BR_pos) / 2, 10, Vector3.up, 2.5f);

                break;

            case 1:
                CreateProjectile("Bends", TM_pos, 5, Vector3.down, 2); // 1

                yield return new WaitForSeconds(0.75f);

                CreateProjectile("Burst", TM_pos, 3, Vector3.down, 2); // 1

                break;
        }

    }
}

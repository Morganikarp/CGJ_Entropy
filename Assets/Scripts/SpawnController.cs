using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnController : MonoBehaviour
{
    public GameObject BasicProjPrefab;
    public GameObject BouncyProjPrefab;
    public GameObject TrackingProjPrefab;
    public GameObject BendsProjPrefab;
    public GameObject BurstProjPrefab;

    Dictionary<string, GameObject> ProjPrefabDict;
    public Transform PlayerTrans;

    public SpriteRenderer FloorSR;
    public Sprite[] FloorSprites;

    public SpriteRenderer WallsSR;
    public Sprite[] WallSprites;

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
        ProjPrefabDict = new Dictionary<string, GameObject>();
        ProjPrefabDict.Add("Basic", BasicProjPrefab);
        ProjPrefabDict.Add("Bouncy", BouncyProjPrefab);
        ProjPrefabDict.Add("Tracking", TrackingProjPrefab);
        ProjPrefabDict.Add("Bends", BendsProjPrefab);
        ProjPrefabDict.Add("Burst", BurstProjPrefab);

        LevelActive = false;
        LevelTimer = 30;

        FloorSR.sprite = FloorSprites[GameController.CurrentLevel];
        WallsSR.sprite = WallSprites[GameController.CurrentLevel];

        TL_pos = new(-SpawnPointHorizontal, SpawnPointVertical, 0);
        TM_pos = new(0, SpawnPointVertical, 0);
        TR_pos = new(SpawnPointHorizontal, SpawnPointVertical, 0);

        ML_pos = new(-SpawnPointHorizontal, 0, 0);
        MR_pos = new(SpawnPointHorizontal, 0, 0);

        BL_pos = new(-SpawnPointHorizontal, -SpawnPointVertical, 0);
        BM_pos = new(0, -SpawnPointVertical, 0);
        BR_pos = new(SpawnPointHorizontal, -SpawnPointVertical, 0);

        LevelActive = true;
        StartCoroutine(LevelSchedule(GameController.CurrentLevel));
    }

    // Update is called once per frame
    void Update()
    {
        OnscreenTimer.text = "Survive for " + Mathf.CeilToInt(LevelTimer).ToString() + " more seconds";

        if (LevelActive)
        {
            LevelTimer -= Time.deltaTime * GameController.CurrentGameSpeed;

            if (LevelTimer <= 0)
            {
                if (GameController.CurrentLevel != 2)
                {
                    GameController.CurrentLevel++;
                    SceneManager.LoadScene("SampleScene");
                }

                else
                {
                    GameController.CurrentLevel = 0;
                    SceneManager.LoadScene("Victory");
                }
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

    void CreateProjectile(string projType, Vector3 spawnV, float speedMod, Vector3 dirV, float scaleMod, float bTime)
    {
        GameObject newProjectile = Instantiate(ProjPrefabDict[projType]);
        GameController.ActiveProjectiles.Add(newProjectile);

        ProjectileController pCon = newProjectile.GetComponent<ProjectileController>();

        pCon.transform.position = spawnV + new Vector3(0,-0.5f,0);
        pCon.ProjType = projType;
        pCon.SpeedMod = speedMod;
        pCon.DirVect = dirV;

        if (projType == "Tracking")
        {
            pCon.DirVect = (PlayerTrans.transform.position - spawnV).normalized;
        }

        pCon.ScaleMod = scaleMod;

        if (projType == "Burst")
        {
            pCon.burstTime = bTime;
        }
    }

    IEnumerator LevelSchedule(int level)
    {
        Vector3 vertAdjust;
        int randomProj;
        yield return new WaitForSeconds(1f);

        switch (level)
        {
            case 0:
                CreateProjectile("Basic", TM_pos, 5, Vector3.down, 2, 0); // 1

                yield return new WaitForSeconds(0.75f);

                CreateProjectile("Basic", (TM_pos + TL_pos) / 2, 5, Vector3.down, 0.8f, 0);
                CreateProjectile("Basic", (TM_pos + TR_pos) / 2, 5, Vector3.down, 0.8f, 0);

                yield return new WaitForSeconds(2f);

                vertAdjust = new(0, 2.5f, 0);
                CreateProjectile("Tracking", ML_pos + vertAdjust, 5, Vector3.zero, 1.1f, 0);
                CreateProjectile("Tracking", MR_pos - vertAdjust, 5, Vector3.zero, 1.1f, 0);

                yield return new WaitForSeconds(2f);

                for (int i = 0; i < 3; i++)
                {
                    Vector3 posLerp = Vector3.Lerp(BL_pos, ML_pos, 0.4f + (0.25f * i));
                    CreateProjectile("Basic", posLerp, 7, Vector3.right, 0.7f, 0);
                }

                for (int i = 0; i < 3; i++)
                {
                    Vector3 posLerp = Vector3.Lerp(TR_pos, MR_pos, 0.4f + (0.25f * i));
                    CreateProjectile("Basic", posLerp, 7, Vector3.left, 0.7f, 0);
                }

                yield return new WaitForSeconds(2f); // 2

                CreateProjectile("Bouncy", ML_pos, 8, Vector3.right, 1f, 0);

                yield return new WaitForSeconds(4.5f);

                CreateProjectile("Basic", BM_pos, 6, Vector3.up, 1.4f, 0);
                CreateProjectile("Bouncy", (BL_pos + BM_pos) / 2, 6, Vector3.up, 1f, 0);
                CreateProjectile("Bouncy", (BM_pos + BR_pos) / 2, 6, Vector3.up, 1f, 0);

                yield return new WaitForSeconds(3f);

                CreateProjectile("Basic", TL_pos, 8f, new(0.5f, -0.5f, 0), 1.2f, 0);
                CreateProjectile("Basic", TR_pos, 8f, new(-0.5f, -0.5f, 0), 1.2f, 0);

                yield return new WaitForSeconds(3f); // 3

                CreateProjectile("Bouncy", TM_pos, 8f, new(0.5f, -0.5f, 0), 1.05f, 0);
                CreateProjectile("Bouncy", BM_pos, 8f, new(-0.5f, 0.5f, 0), 1.05f, 0);

                yield return new WaitForSeconds(3f);

                CreateProjectile("Basic", BM_pos, 4, Vector3.up, 2, 0);

                yield return new WaitForSeconds(3f);

                CreateProjectile("Bouncy", (TL_pos + ML_pos) / 2, 10, new(0.5f, -0.65f, 0), 0.6f, 0);
                CreateProjectile("Bouncy", (TR_pos + MR_pos) / 2, 10, new(-0.5f, -0.65f, 0), 0.6f, 0);

                yield return new WaitForSeconds(3f);

                CreateProjectile("Basic", (BL_pos + BM_pos) / 2, 10, Vector3.up, 2.1f, 0);
                CreateProjectile("Basic", (BM_pos + BR_pos) / 2, 10, Vector3.up, 2.1f, 0);

                break;

            case 1:

                CreateProjectile("Basic", TM_pos, 5, Vector3.down, 2, 0); // 1

                yield return new WaitForSeconds(0.75f);

                CreateProjectile("Bends", (TM_pos + TL_pos) / 2, 5, Vector3.down, 0.8f, 0);
                CreateProjectile("Basic", (TM_pos + TR_pos) / 2, 5, Vector3.down, 0.8f, 0);

                yield return new WaitForSeconds(2f);

                vertAdjust = new(0, 2.5f, 0);
                CreateProjectile("Tracking", ML_pos + vertAdjust, 5, Vector3.zero, 1.1f, 0);
                CreateProjectile("Tracking", MR_pos - vertAdjust, 5, Vector3.zero, 1.1f, 0);

                yield return new WaitForSeconds(2f);

                randomProj = Random.Range(0, 3);
                for (int i = 0; i < 3; i++)
                {
                    Vector3 posLerp = Vector3.Lerp(BL_pos, ML_pos, 0.4f + (0.25f * i));
                    if (i == randomProj)
                    {
                        CreateProjectile("Bends", posLerp, 7, Vector3.right, 0.7f, 0);

                    }
                    else
                    {
                        CreateProjectile("Basic", posLerp, 7, Vector3.right, 0.7f, 0);
                    }
                }

                randomProj = Random.Range(0, 3);
                for (int i = 0; i < 3; i++)
                {
                    Vector3 posLerp = Vector3.Lerp(TR_pos, MR_pos, 0.4f + (0.25f * i));
                    if (i == randomProj)
                    {
                        CreateProjectile("Bends", posLerp, 7, Vector3.left, 0.7f, 0);
                    }
                    else
                    {
                        CreateProjectile("Basic", posLerp, 7, Vector3.left, 0.7f, 0);
                    }
                }

                yield return new WaitForSeconds(2f); // 2

                CreateProjectile("Bouncy", ML_pos, 8, Vector3.right, 1f, 0);

                yield return new WaitForSeconds(4.5f);

                CreateProjectile("Basic", BM_pos, 6, Vector3.up, 1.4f, 0);
                CreateProjectile("Bouncy", (BL_pos + BM_pos) / 2, 6, Vector3.up, 1f, 0);
                CreateProjectile("Bends", (BM_pos + BR_pos) / 2, 6, Vector3.up, 1f, 0);

                yield return new WaitForSeconds(3f);

                CreateProjectile("Basic", TL_pos, 8f, new(0.5f, -0.5f, 0), 1.2f, 0);
                CreateProjectile("Basic", TR_pos, 9f, new(-0.5f, -0.5f, 0), 1.2f, 0);

                yield return new WaitForSeconds(3f); // 3

                CreateProjectile("Bouncy", TM_pos, 8f, new(0.5f, -0.5f, 0), 1.05f, 0);
                CreateProjectile("Bouncy", BM_pos, 8f, new(-0.5f, 0.5f, 0), 1.05f, 0);

                yield return new WaitForSeconds(3f);

                CreateProjectile("Basic", BM_pos, 3, Vector3.up, 2, 0);

                yield return new WaitForSeconds(3f);

                CreateProjectile("Bouncy", (TL_pos + ML_pos) / 2, 8, new(0.5f, -0.65f, 0), 0.6f, 0);
                CreateProjectile("Bouncy", (TR_pos + MR_pos) / 2, 12, new(-0.5f, -0.65f, 0), 0.6f, 0);

                yield return new WaitForSeconds(3f);

                CreateProjectile("Basic", (BL_pos + BM_pos) / 2, 10, Vector3.up, 2.1f, 0);
                CreateProjectile("Basic", (BM_pos + BR_pos) / 2, 10, Vector3.up, 2.1f, 0);

                break;

            case 2:

                CreateProjectile("Basic", TM_pos, 5, Vector3.down, 2, 0); // 1

                yield return new WaitForSeconds(0.75f);

                CreateProjectile("Bends", (TM_pos + TL_pos) / 2, 5, Vector3.down, 0.8f, 0);
                CreateProjectile("Basic", (TM_pos + TR_pos) / 2, 5, Vector3.down, 0.8f, 0);

                yield return new WaitForSeconds(2f);

                vertAdjust = new(0, 2.5f, 0);
                CreateProjectile("Tracking", ML_pos + vertAdjust, 5, Vector3.zero, 1.1f, 0);
                CreateProjectile("Tracking", MR_pos - vertAdjust, 5, Vector3.zero, 1.1f, 0);

                yield return new WaitForSeconds(2f);

                randomProj = Random.Range(0, 3);
                for (int i = 0; i < 3; i++)
                {
                    Vector3 posLerp = Vector3.Lerp(BL_pos, ML_pos, 0.4f + (0.25f * i));
                    if (i == randomProj)
                    {
                        CreateProjectile("Basic", posLerp, 7, Vector3.right, 0.7f, 0);

                    }
                    else
                    {
                        CreateProjectile("Bends", posLerp, 7, Vector3.right, 0.7f, 0);
                    }
                }

                randomProj = Random.Range(0, 3);
                for (int i = 0; i < 3; i++)
                {
                    Vector3 posLerp = Vector3.Lerp(TR_pos, MR_pos, 0.4f + (0.25f * i));
                    if (i == randomProj)
                    {
                        CreateProjectile("Basic", posLerp, 7, Vector3.left, 0.7f, 0);
                    }
                    else
                    {
                        CreateProjectile("Bends", posLerp, 7, Vector3.left, 0.7f, 0);
                    }
                }

                yield return new WaitForSeconds(2f); // 2

                CreateProjectile("Bouncy", ML_pos, 8, Vector3.right, 1f, 0);

                yield return new WaitForSeconds(4.5f);

                CreateProjectile("Burst", BM_pos, 6, Vector3.up, 1.4f, 1.5f);
                CreateProjectile("Bouncy", (BL_pos + BM_pos) / 2, 6, Vector3.up, 1f, 0);
                CreateProjectile("Bends", (BM_pos + BR_pos) / 2, 6, Vector3.up, 1f, 0);

                yield return new WaitForSeconds(3f);

                CreateProjectile("Basic", TL_pos, 8f, new(0.5f, -0.5f, 0), 1.2f, 0);
                CreateProjectile("Burst", TR_pos, 9f, new(-0.5f, -0.5f, 0), 1.2f, 1.5f);

                yield return new WaitForSeconds(3f); // 3

                CreateProjectile("Bouncy", TM_pos, 8f, new(0.5f, -0.5f, 0), 1.05f, 0);
                CreateProjectile("Bouncy", BM_pos, 8f, new(-0.5f, 0.5f, 0), 1.05f, 0);

                yield return new WaitForSeconds(3f);

                CreateProjectile("Basic", BM_pos, 3, Vector3.up, 2, 0);

                yield return new WaitForSeconds(3f);

                CreateProjectile("Bouncy", (TL_pos + ML_pos) / 2, 8, new(0.5f, -0.65f, 0), 0.6f, 0);
                CreateProjectile("Bouncy", (TR_pos + MR_pos) / 2, 12, new(-0.5f, -0.65f, 0), 0.6f, 0);

                yield return new WaitForSeconds(3f);

                CreateProjectile("Burst", (BL_pos + BM_pos) / 2, 10, Vector3.up, 2.1f, 0.5f);
                CreateProjectile("Burst", (BM_pos + BR_pos) / 2, 10, Vector3.up, 2.1f, 1);

                break;
        }

    }
}

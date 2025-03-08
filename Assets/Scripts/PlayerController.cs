using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Animator ani;
    float playerSize;

    Rigidbody2D rb;

    public TextMeshProUGUI DeathCounter;

    public GameObject BulletTimeBar;
    RectTransform BulletTimeBarTrans;
    RawImage BulletTimeBarImage;

    public float BulletTimeDuration;
    float BulletTimeTimer;

    bool BulletTimePenalty;
    public float BulletTimePenaltyDuration;

    bool BulletTimeRegenDelay;
    public float BulletTimeRegenDelayDuration;
    float BulletTimeRegenDelayTimer;

    public float speedMod;

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        playerSize = transform.localScale.x;

        rb = GetComponent<Rigidbody2D>();

        DeathCounter.text = GameController.DeathCount.ToString();

        BulletTimeBarTrans = BulletTimeBar.GetComponent<RectTransform>();
        BulletTimeBarImage = BulletTimeBar.GetComponent<RawImage>();
        BulletTimeTimer = BulletTimeDuration;

        BulletTimePenalty = false;

        BulletTimeRegenDelay = false;
        BulletTimeRegenDelayTimer = BulletTimeRegenDelayDuration;
    }

    // Update is called once per frame
    void Update()
    {
        DeathCounter.text = GameController.DeathCount.ToString();

        Move();
        BulletTime();
    }

    void FixedUpdate()
    {
        float CurrentBarWidth = Mathf.Lerp(0, 1, BulletTimeTimer / BulletTimeDuration);
        BulletTimeBarTrans.localScale = new Vector3(CurrentBarWidth, 1, 1);

        float CurrentBarAlpha = Mathf.Lerp(1, 0, BulletTimeTimer);
        BulletTimeBarImage.color = new Color(1, 1, 1, CurrentBarAlpha);
    }

    void Move()
    {
        Vector3 ControlVect = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);

        if (ControlVect.magnitude > 0)
        {
            rb.velocity = ControlVect * speedMod * GameController.CurrentGameSpeed;
        }

        if (ControlVect.x != 0) { transform.localScale = new Vector3(ControlVect.x * playerSize, playerSize, 1f); }

        ani.SetInteger("ControlVectX", (int)ControlVect.x);
        ani.SetInteger("ControlVectY", (int)ControlVect.y);
        ani.speed = GameController.CurrentGameSpeed;
    }

    void BulletTime()
    {
        if (!BulletTimePenalty)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && BulletTimeTimer != 0)  // Pressing shift AND bar is not empty
            {
                GameController.CurrentGameSpeed = 0.3f;

                BulletTimeRegenDelay = true;
                BulletTimeRegenDelayTimer = BulletTimeRegenDelayDuration;
            }

            if (Input.GetKey(KeyCode.LeftShift)) // Holding shift
            {
                BulletTimeTimer -= Time.deltaTime / BulletTimeDuration;
                if (BulletTimeTimer <= 0) { BulletTimeTimer = 0; BulletTimePenalty = true; }
            }

            else
            {
                if (BulletTimeRegenDelay)
                {
                    BulletTimeRegenDelayTimer -= Time.deltaTime;
                    if (BulletTimeRegenDelayTimer <= 0) { BulletTimeRegenDelay = false; }
                }

                else
                {
                    BulletTimeTimer += Time.deltaTime / BulletTimeDuration;
                    if (BulletTimeTimer > BulletTimeDuration) { BulletTimeTimer = BulletTimeDuration; }
                }
            }

            if (Input.GetKeyUp(KeyCode.LeftShift) || BulletTimeTimer == 0) // releasing shift OR bar is empty
            {
                GameController.CurrentGameSpeed = GameController.BaseGameSpeed;
            }
        }

        else
        {
            BulletTimeTimer += Time.deltaTime / BulletTimePenaltyDuration;
            if (BulletTimeTimer > BulletTimeDuration) { BulletTimeTimer = BulletTimeDuration; BulletTimePenalty = false; }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Projectile")
        {
            Debug.Log("dead!");
            GameController.DeathCount++;
            transform.position = new Vector3(0, -1, 0);
        }
    }
}

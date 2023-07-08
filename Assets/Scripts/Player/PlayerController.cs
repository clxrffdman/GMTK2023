using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : UnitySingleton<PlayerController>
{
    public PlayerCursor cursor;
    public Rigidbody2D rb;
    public SpriteRenderer playerSprite;
    public Animator anim;
    public bool isHeld = false;
    public bool locked = true;
    public float holdDuration;
    public float charge;
    public float chargeIncrement;
    public float maxChargeTimer;
    public bool invalidMovement;
    public Vector2 minimumVelocity;
    public float baseMovementCooldown;
    private float cooldownTimer;
    private Vector3 baseSpriteScale;

    public override void Awake() {
        base.Awake();
        playerSprite = playerSprite != null ? playerSprite : GlobalFunctions.FindComponent<SpriteRenderer>(gameObject);
        baseSpriteScale = playerSprite.transform.localScale;
        anim = anim != null ? anim : GlobalFunctions.FindComponent<Animator>(gameObject);
    }
    public void Start() {
        StartCoroutine(SpawnAnim());
    }

    public IEnumerator SpawnAnim() {
        transform.position = CourseController.Instance.playerSpawnPosition.position;
        rb.velocity = Vector2.zero;
        locked = true;
        isHeld = false;
        anim.SetBool("Jump", false);
        anim.Play("PlayerIdle");
        Color spriteColor = playerSprite.color;
        Color prevColor = spriteColor;
        prevColor.a = 0f;
        LeanTween.value(playerSprite.gameObject, (Color val) => { playerSprite.color = val; }, prevColor, spriteColor, 0.2f);
        LeanTween.value(playerSprite.gameObject, (float val) => { playerSprite.transform.localPosition = new Vector2(0f, val); }, 5f, 0f, 0.3f);
        yield return new WaitForSeconds(0.6f);
        locked = false;
    }

    public void PlayerMovement(InputAction.CallbackContext context)
    {
        if (cooldownTimer >= 0 || anim.GetBool("Hit") || locked)
        {
            return;
        }

        switch (context.phase) {
            case InputActionPhase.Started:
                cursor.LockCursorMovement(true);
                rb.velocity = Vector2.zero;
                isHeld = true;
                anim.SetBool("Jump", true);
                LeanTween.value(this.gameObject, ChargeRoutine, 0, maxChargeTimer, maxChargeTimer).setEaseOutCubic().setLoopPingPong();
                LeanTween.value(cursor.gameObject, cursor.ChargeRoutine, cursor.baseRadius, cursor.maxRadius, maxChargeTimer).setEaseOutCubic().setLoopPingPong();
                LeanTween.color(cursor.gameObject, Color.red, maxChargeTimer).setEaseOutCubic().setLoopPingPong();
                LeanTween.value(this.gameObject, CameraController.Instance.SetVignetteStrength, 0, 0.3f, maxChargeTimer).setEaseOutCubic();
                LeanTween.value(this.gameObject, CameraController.Instance.SetPlayerCamZoom, 
                    CameraController.Instance.defaultSize, CameraController.Instance.zoomedSize, maxChargeTimer).setEaseOutCubic();

                break;
            case InputActionPhase.Canceled:
                if (isHeld)
                {
                    cooldownTimer = baseMovementCooldown;
                }

                EndCharge();

                anim.SetBool("Jump", false);
                rb.AddForce(cursor.GetDirection() * charge * rb.mass);
                playerSprite.transform.localScale = baseSpriteScale;
                LeanTween.scale(playerSprite.gameObject, baseSpriteScale*1.06f, 0.15f).setEaseOutQuint().setLoopPingPong(1).setOnComplete(()=>{ playerSprite.transform.localScale = baseSpriteScale; });
                LeanTween.value(this.gameObject, CameraController.Instance.SetVignetteStrength, CameraController.Instance.GetVignetteStrength(), 0, baseMovementCooldown).setEaseOutQuint();
                LeanTween.value(this.gameObject, CameraController.Instance.SetPlayerCamZoom,
                CameraController.Instance.GetPlayerCamZoom(), CameraController.Instance.defaultSize, baseMovementCooldown/2).setEaseOutQuint();

                break;
        }
    }

    void Update()
    {
        if (cooldownTimer >= 0)
        {            
            cooldownTimer -= Time.deltaTime;
        }
        if (!cursor.isLocked && !anim.GetBool("Hit")) {
            playerSprite.flipX = cursor.GetCursorPos().x < 0;
        }
    }

    public void ChargeRoutine(float value)
    {
        holdDuration = value;
        charge = value * chargeIncrement;
    }

    
    public void EndCharge()
    {
        LeanTween.cancel(this.gameObject);
        LeanTween.cancel(cursor.gameObject);
        cursor.ResetColor();
        anim.SetBool("Jump", false);
        isHeld = false;
        holdDuration = 0;
        charge = 0;
        cursor.LockCursorMovement(false);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Hazard")
        {
            playerSprite.flipX = collision.transform.position.x < transform.position.x;
            if (!anim.GetBool("Hit") || !LevelManager.Instance.currentLevel.currWave.waveDone) {
                StartCoroutine(PlayerHit());
            }
            //cause lose 
        }
    }
    public IEnumerator PlayerHit() {
        anim.SetBool("Hit", true);
        LevelManager.Instance.hasFailedCurrentWave = true;
        StartCoroutine(CourseController.Instance.DeleteBalls(false));
        GameplayUIManager.Instance.bannerQuipController.RequestBannerQuip("You suck!", 0.25f, 1.5f, 0.15f);
        yield return null;
    }

    public void RequestPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameManager.Instance.TogglePause(!GameManager.Instance.isPaused);
        }
    }
}

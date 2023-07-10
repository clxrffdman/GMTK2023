using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FMOD.Studio;

public class PlayerController : UnitySingleton<PlayerController>
{
    public PlayerCursor cursor;
    public Rigidbody2D rb;
    public SpriteRenderer playerSprite;
    public SpriteRenderer playerShadow;
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
    private EventInstance chargingSound;
    private float convertedRadius;
    private Vector3 shadowScale;

    public override void Awake() {
        base.Awake();
        shadowScale = playerShadow.transform.localScale;
        playerSprite = playerSprite != null ? playerSprite : GetComponentInChildren<SpriteRenderer>();
        baseSpriteScale = playerSprite.transform.localScale;
        anim = anim != null ? anim : GetComponent<Animator>();
    }
    public void Start() {
        //StartCoroutine(SpawnAnim());
        chargingSound = AudioManager.instance.CreateEventInstance(FMODEventReferences.instance.ChargingSound);        
    }

    public IEnumerator SpawnAnim() {
        transform.position = CourseController.Instance.playerSpawnPosition.position;
        cursor.gameObject.SetActive(false);
        playerShadow.transform.localScale = Vector3.zero;
        rb.velocity = Vector2.zero;
        locked = true;
        isHeld = false;
        anim.SetBool("Jump", false);
        anim.Play("PlayerIdle");
        Color spriteColor = playerSprite.color;
        Color prevColor = spriteColor;
        Color newColor = spriteColor;
        prevColor.a = 0f;
        newColor.a = 1f;
        playerSprite.gameObject.SetActive(true);
        playerShadow.gameObject.SetActive(true);
        playerSprite.color = prevColor;
        LeanTween.value(playerSprite.gameObject, (Color val) => { playerSprite.color = val; }, prevColor, newColor, 0.2f);
        LeanTween.value(playerSprite.gameObject, (float val) => { playerSprite.transform.localPosition = new Vector2(0f, val); }, 5f, 0f, 0.3f);

        LeanTween.scale(playerShadow.gameObject, shadowScale, 0.2f);

        yield return new WaitForSeconds(0.6f);
        cursor.gameObject.SetActive(true);
    }

    public IEnumerator PickUpAnim(bool intoSpawn=true) {
        cursor.gameObject.SetActive(false);
        playerSprite.gameObject.SetActive(true);
        playerShadow.gameObject.SetActive(true);
        playerShadow.transform.localScale = shadowScale;
        //transform.position = CourseController.Instance.playerSpawnPosition.position;
        rb.velocity = Vector2.zero;
        locked = true;
        isHeld = false;
        anim.SetBool("Jump", false);
        anim.Play("PlayerIdle");
        Color spriteColor = playerSprite.color;
        Color prevColor = spriteColor;
        Color newColor = spriteColor;
        prevColor.a = 1f;
        newColor.a = 0f;
        LeanTween.value(playerSprite.gameObject, (Color val) => { playerSprite.color = val; }, prevColor, newColor, 0.2f);
        LeanTween.value(playerSprite.gameObject, (float val) => { playerSprite.transform.localPosition = new Vector2(0f, val); }, 0f, 5f, 0.3f);
        LeanTween.scale(playerShadow.gameObject, Vector3.zero, 0.2f);
        yield return new WaitForSeconds(1f);
        if (intoSpawn) {
            yield return SpawnAnim();
        }
    }

    public void PlayerMovement(InputAction.CallbackContext context)
    {
        if (cooldownTimer >= 0 || anim.GetBool("Hit") || locked || GameManager.Instance.isPaused)
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
                LeanTween.value(this.gameObject, SlowTime, 1, .5f, maxChargeTimer).setEaseOutCubic().setLoopPingPong();
                LeanTween.value(cursor.gameObject, cursor.ChargeRoutine, cursor.baseRadius, cursor.maxRadius, maxChargeTimer).setEaseOutCubic().setLoopPingPong();
                LeanTween.color(cursor.gameObject, Color.red, maxChargeTimer).setEaseOutCubic().setLoopPingPong();
                LeanTween.value(this.gameObject, CameraController.Instance.SetVignetteStrength, 0, 0.3f, maxChargeTimer).setEaseOutCubic();
                LeanTween.value(this.gameObject, CameraController.Instance.SetPlayerCamZoom, 
                CameraController.Instance.defaultSize, CameraController.Instance.zoomedSize, maxChargeTimer).setEaseOutCubic();
                PLAYBACK_STATE playbackState;
                chargingSound.getPlaybackState(out playbackState);
                if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
                {
                    chargingSound.start();
                }
                break;
            case InputActionPhase.Canceled:
                if (isHeld)
                {
                    cooldownTimer = baseMovementCooldown;
                }
                anim.SetBool("Jump", false);
                rb.AddForce(cursor.GetDirection() * charge * rb.mass);
                playerSprite.transform.localScale = baseSpriteScale;
                LeanTween.scale(playerSprite.gameObject, baseSpriteScale*1.06f, 0.15f).setEaseOutQuint().setLoopPingPong(1).setOnComplete(()=>{ playerSprite.transform.localScale = baseSpriteScale; });
                FMODUnity.RuntimeManager.PlayOneShot(FMODEventReferences.instance.JumpSound);
                EndCharge();



                break;
        }
    }

    void Update()
    {
        if (cooldownTimer >= 0)
        {            
            cooldownTimer -= Time.deltaTime;
        }
        if (!cursor.isLocked && !GameManager.Instance.isPaused && !anim.GetBool("Hit")) {
            playerSprite.flipX = cursor.GetCursorPos().x < 0;
        }
        if (locked || anim.GetBool("Hit") && isHeld) {
            EndCharge();
        }
    }

    public void ChargeRoutine(float value)
    {
        holdDuration = value;
        charge = value * chargeIncrement;
        convertedRadius = cursor.radius/cursor.maxRadius;
        chargingSound.setParameterByName("PlayerCursorRadius", convertedRadius);
    }

    public void SlowTime(float value)
    {
        if (GameManager.Instance.isPaused)
        {
            return;
        }
        Time.timeScale = value;
    }

    
    public void EndCharge()
    {
        LeanTween.cancel(this.gameObject);
        LeanTween.cancel(cursor.gameObject);
        chargingSound.stop(STOP_MODE.ALLOWFADEOUT);
        cursor.ResetColor();
        isHeld = false;
        holdDuration = 0;
        charge = 0;
        cursor.LockCursorMovement(false);

        if (!GameManager.Instance.isPaused)
        {
            Time.timeScale = 1;
        }

        

        LeanTween.value(this.gameObject, CameraController.Instance.SetVignetteStrength, CameraController.Instance.GetVignetteStrength(), 0, baseMovementCooldown).setEaseOutQuint();
        LeanTween.value(this.gameObject, CameraController.Instance.SetPlayerCamZoom,
        CameraController.Instance.GetPlayerCamZoom(), CameraController.Instance.defaultSize, baseMovementCooldown/2).setEaseOutQuint();

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
        EndCharge();
        if (GameplayUIManager.Instance.transitionPanelController.isBlack) {
            GameplayUIManager.Instance.transitionPanelController.FadeFromBlack(0.5f);
        }
        anim.SetBool("Hit", true);
        LevelManager.Instance.hasFailedCurrentWave = true;
        StartCoroutine(CourseController.Instance.DeleteBalls(false));
        GameplayUIManager.Instance.bannerQuipController.RequestBannerQuip("Strike!", 0.25f, 1.5f, 0.15f);
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

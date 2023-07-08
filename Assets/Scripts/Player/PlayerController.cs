using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerCursor cursor;
    public Rigidbody2D rb;
    public SpriteRenderer playerSprite;
    public Animator anim;
    public bool isHeld = false;
    public float holdDuration;
    public float charge;
    public float chargeIncrement;
    public float maxChargeTimer;
    public bool invalidMovement;
    public Vector2 minimumVelocity;
    public float baseMovementCooldown;
    private float cooldownTimer;
    private Vector3 baseSpriteScale;

    public void Awake() {
        playerSprite = playerSprite != null ? playerSprite : GlobalFunctions.FindComponent<SpriteRenderer>(gameObject);
        baseSpriteScale = playerSprite.transform.localScale;
        anim = anim != null ? anim : GlobalFunctions.FindComponent<Animator>(gameObject);
    }

    public void PlayerMovement(InputAction.CallbackContext context)
    {
        if (cooldownTimer >= 0 || anim.GetBool("Hit"))
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

                break;
            case InputActionPhase.Canceled:
                if (isHeld)
                {
                    cooldownTimer = baseMovementCooldown;
                }

                LeanTween.cancel(this.gameObject);
                LeanTween.cancel(cursor.gameObject);
                anim.SetBool("Jump", false);
                rb.AddForce(cursor.GetDirection() * charge);
                LeanTween.scale(playerSprite.gameObject, baseSpriteScale*1.06f, 0.15f).setEaseOutQuint().setLoopPingPong(1);
                
                cursor.LockCursorMovement(false);

                isHeld = false;

                holdDuration = 0;
                charge = 0;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Hazard")
        {
            playerSprite.flipX = collision.transform.position.x < transform.position.x;
            anim.SetBool("Hit", true);
            StartCoroutine(PlayerHit());
            //cause lose 
        }
    }
    public IEnumerator PlayerHit() {
        yield return new WaitForSeconds(3f);
        anim.SetBool("Hit", false);
    }
}

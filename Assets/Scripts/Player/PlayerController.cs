using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerCursor cursor;
    public Rigidbody2D rb;
    public bool isHeld = false;
    public float holdDuration;
    public float charge;
    public float chargeIncrement;
    public float maxChargeTimer;
    public bool isReversed;

    public void PlayerMovement(InputAction.CallbackContext context)
    {
        switch (context.phase) {
            case InputActionPhase.Started:
                cursor.LockCursorMovement(true);
                rb.velocity = Vector2.zero;
                isHeld = true;
                LeanTween.value(this.gameObject, ChargeRoutine, 0, maxChargeTimer, maxChargeTimer).setEaseOutCubic().setLoopPingPong();
                LeanTween.value(cursor.gameObject, cursor.ChargeRoutine, cursor.baseRadius, cursor.maxRadius, maxChargeTimer).setEaseOutCubic().setLoopPingPong();

                break;
            case InputActionPhase.Canceled:
                cursor.LockCursorMovement(false);

                LeanTween.cancel(this.gameObject);
                LeanTween.cancel(cursor.gameObject);

                rb.AddForce(cursor.GetCursorPos() * charge);
                isHeld = false;

                holdDuration = 0;
                charge = 0;

                break;
        }
    }

    void Update()
    {

    }

    public void ChargeRoutine(float value)
    {
        holdDuration = value;
        charge = value * chargeIncrement;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Hazard")
        {
            //cause lose 
        }
    }
}

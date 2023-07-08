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
                break;
            case InputActionPhase.Canceled:
                cursor.LockCursorMovement(false);
                rb.AddForce(cursor.GetCursorPos() * charge);
                isHeld = false;
                holdDuration = 0;
                charge = 0;
                isReversed = false;
                break;
        }
    }

    void Update()
    {
        if (isHeld)
        {
            StartCharge();
            holdDuration += Time.deltaTime;
        }
    }

    public void StartCharge()
    {
        if (!isReversed)
        {
            if (holdDuration >= maxChargeTimer)
            {
                isReversed = true;
                holdDuration = 0;
            }

            charge += chargeIncrement;
        }
        else
        {
            if (holdDuration >= maxChargeTimer)
            {
                isReversed = false;
                holdDuration = 0;
            }

            charge -= chargeIncrement;
        }

        cursor.StartCharge(isReversed);
    }
}

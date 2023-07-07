using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerCursor cursor;
    public Rigidbody2D rb;
    public float force;
    public bool isHeld = false;
    public float holdDuration;

    public void PlayerMovement(InputAction.CallbackContext context)
    {
        switch (context.phase) {
            case InputActionPhase.Started:
                cursor.LockCursorMovement(true);
                isHeld = true;
                break;
            case InputActionPhase.Canceled:
                cursor.LockCursorMovement(false);
                rb.AddForce(cursor.GetCursorPos() * force);
                isHeld = false;
                holdDuration = 0;
                break;
        }
    }

    void Update()
    {
        if (isHeld)
        {
            cursor.StartCharge();
            holdDuration += Time.deltaTime;
            Debug.Log(holdDuration);
        }
    }
}

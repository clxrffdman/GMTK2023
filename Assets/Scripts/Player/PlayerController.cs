using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerCursor cursor;
    public void PlayerMovement(InputAction.CallbackContext context)
    {
        Debug.Log(cursor.GetCursorPos());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
    [SerializeField]
    [Header("Basic Properties")]
    public Transform playerPos;
    public float baseRadius;
    public float maxRadius;
    private float radius;
    private Vector2 center;
    private float angle;
    private Vector2 offset;
    private Vector2 dir;
    private float dist;

    [Header("Charging")]
    public float chargeSpeed;
    private bool isLocked = false;
    private bool isReversed = false;
    // Start is called before the first frame update
    void Start()
    {
        radius = baseRadius;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isLocked)
        {
            //Rotate cursor around player's position using mouse position
            dir = GetCursorPos();
        }

        center = playerPos.position;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        offset = dir * radius;

        transform.position = center + offset;
        dist = transform.position.x - playerPos.position.x;

        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        transform.rotation = rotation;
    }

    //Finds mouse position in world space relative to player
    public Vector2 GetCursorPos()
    {
        Vector2 cursorScreenPosition = Input.mousePosition;
        cursorScreenPosition = new Vector2(Mathf.Clamp(cursorScreenPosition.x, 0, Screen.width), Mathf.Clamp(cursorScreenPosition.y, 0, Screen.height));

        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(cursorScreenPosition);
        Vector2 cursorWorldPosition = new Vector2(worldPoint.x, worldPoint.y);

        Vector2 mouseDirection = cursorWorldPosition - (Vector2)playerPos.position;
        mouseDirection.Normalize();

        return mouseDirection;
    }

    public void LockCursorMovement(bool locked)
    {
        isLocked = locked;

        if (!locked)
        {
            ResetCursor();
        }
    }

    public void StartCharge()
    {
        if (radius < maxRadius && !isReversed)
        {
            radius += chargeSpeed * Time.deltaTime;
        }
        else
        {
            isReversed = true;

            radius -= chargeSpeed * Time.deltaTime;

            if (radius < baseRadius)
            {
                isReversed = false;
            }
        }

        //transform.position += new Vector3(.1f, .1f, 0) + Vector3.forward;
    }

    public void ResetCursor()
    {
        radius = baseRadius;
        //this.dist = transform.position.x - playerPos.position.x;
    }
}

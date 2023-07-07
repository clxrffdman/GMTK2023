using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
    [SerializeField]
    public float radius;
    public Transform playerPos;
    private Vector2 center;
    private float angle;
    private Camera mainCam;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Rotate cursor around player's position using mouse position
        Vector2 dir = GetCursorPos();

        center = playerPos.position;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Vector2 offset = dir * radius;

        transform.position = center + offset;
        float dist = transform.position.x - playerPos.position.x;

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
}

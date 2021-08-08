using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    private Vector3 mousePosition;
    private Vector3 pos;
    private Vector3 initialPos;

    //Obstacle Properties
    public bool MoveableDuringPlay;
    public bool moveable;
    public bool liftable;
    Vector3 obstacleStartPoint;

    //Unity
    bool moveLeft = false;
    bool moveRight = false;
    Transform playerPos;
    string myTag;
    Camera mainCam;
    private void Start()
    {
        playerPos = FindObjectOfType<PlayerMovement>().transform;
        myTag = transform.tag;
        obstacleStartPoint = transform.position;

        mainCam = Camera.main;
    }

    bool dragable;
    float dist = 10;

    private void OnMouseDown()
    {
        mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
        initialPos = mainCam.ScreenToWorldPoint(mousePosition) - transform.position;
        dragable = true;
    }
    
    private void OnMouseDrag()
    {
        mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
        pos = mainCam.ScreenToWorldPoint(mousePosition);

        if ((pos - initialPos).z > playerPos.position.z + transform.localScale.z / 1.5f) //The obstacle cannot be too close to the player.
        {
            if (!liftable && dragable && moveable)
            {
                Vector3 dir = pos - initialPos;
                dir.x = 0;
                dir.y = transform.position.y;
                transform.position = dir;
            }
        }

        if (dragable && liftable)
        {
            Vector3 dir = pos - initialPos;
            if (dir.y > obstacleStartPoint.y) // player can't descend the door if dir is less than start point Y.
            {
                dir.x = 0;
                dir.z = transform.position.z;
                transform.position = dir;
            }
        }

    }

    private void OnMouseUp()
    {
        dragable = false;

        moveLeft = false;
        moveRight = false;
    }

    public void FreezeObject()
    {
        if (!MoveableDuringPlay)
        {
            moveable = false;
        }
    }

    private void Update()
    {
        if (myTag == "DoorObstacle")
            DoorMover();
    }

    private void DoorMover()
    {
        transform.position = Vector3.Lerp(transform.position, obstacleStartPoint, Time.deltaTime * 2);
    }
}

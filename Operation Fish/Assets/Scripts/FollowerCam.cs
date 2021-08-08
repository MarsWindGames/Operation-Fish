using UnityEngine;

public class FollowerCam : MonoBehaviour
{

    [Header("Camera Properties")]
    public float CamMoveSpeed = 3f;
    public float ScreenMoveOffset = 100;

    //Unity
    Transform target;
    Transform finish;
    Vector3 newPos;
    float newZ = 0;
    GameManager gameManager;
    bool moveLeft = false;
    bool moveRight = false;
    float screenSizeX;
    Vector3 camPosition;
    bool touching = false;
    

    void Start()
    {
        target = FindObjectOfType<PlayerMovement>().transform;
        finish = FindObjectOfType<Fish>().transform;
        gameManager = GameManager.instance;
        screenSizeX = Screen.width;
        camPosition = new Vector3();
    }

    void LateUpdate()
    {
        if (gameManager.GameStarted)
        {
            newPos = target.position;
            newPos.x = transform.position.x;
            newPos.y = transform.position.y;
            transform.position = newPos;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
            touching = true;
        else
            touching = false;

        if (touching)
        {
            if (Input.mousePosition.x > screenSizeX - ScreenMoveOffset && finish.position.z > transform.position.z)
            {
                camPosition = transform.position;
                camPosition.z += CamMoveSpeed * Time.deltaTime;
                transform.position = camPosition;
            }
            else if (Input.mousePosition.x < ScreenMoveOffset && target.position.z < transform.position.z)
            {
                camPosition = transform.position;
                camPosition.z -= CamMoveSpeed * Time.deltaTime;
                transform.position = camPosition;
            }

        }
        //camera can move between cat and fish.
        if (moveLeft && target.position.z < transform.position.z)
        {
            transform.position -= Vector3.forward * Time.deltaTime * CamMoveSpeed;
        }

        if (moveRight && finish.position.z > transform.position.z)
        {
            transform.position += Vector3.forward * Time.deltaTime * CamMoveSpeed;
        }

    }

    public void MoveCameraRight(bool _move)
    {
        moveRight = _move;
    }

    public void MoveCameraLeft(bool _move)
    {
        moveLeft = _move;
    }
}

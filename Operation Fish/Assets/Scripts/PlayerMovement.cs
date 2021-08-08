using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Options")]
    public float moveSpeed = 3f;
    public float jumpForce = 5f;
    Vector3 direction;


    //Unity
    Rigidbody rb;
    bool move = false;
    bool canJump = true;
    GameObject oldCollision;
    Transform fishObject;
    Collider coll;
    AudioSource audioSource;
    public AudioClip meow, coinAudio, winAudio, angryAudio;


    private void Start()
    {
        coll = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        oldCollision = null;
    }

    void Update()
    {
        if (move)
        {
            direction = new Vector3(0, rb.velocity.y, moveSpeed);
            rb.velocity = direction;
        }

        if (transform.position.y <= -20f && transform.position.z > fishObject.position.z + 3)   // if players go outside map, kill.
        {
            GameManager.instance.failGame();
            stopMoving();
        }
    }

    public void StartMoving()
    {
        rb.isKinematic = false;
        move = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            ScoreManager.instance.IncreaseScore(200);
            audioSource.PlayOneShot(coinAudio);
            GameManager.instance.coinCount--;
            Destroy(other.gameObject);
        }
        if (other.gameObject != oldCollision)
        {
            if (canJump && other.transform.CompareTag("DogObstacle"))
            {
                oldCollision = other.gameObject;
                GameManager.instance.failGame();
                stopMoving();
            }
        }

        if (other.CompareTag("Fish"))
        {
            audioSource.PlayOneShot(meow);
            audioSource.clip = winAudio;
            audioSource.PlayDelayed(3f);
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject != oldCollision)     //we check it to prevent jump more than once on a jumpable.
        {
            if (canJump && other.transform.CompareTag("JumpObstacle"))
            {
                oldCollision = other.gameObject;
                Invoke("Jump", 0.2f);
            }
        }

        if (other.gameObject != oldCollision)
        {

            if (canJump && other.transform.CompareTag("WalkableObstacle"))
            {
                oldCollision = other.gameObject;
                rb.AddForce(0, 5, 0, ForceMode.Impulse);
            }
        }

        if (other.gameObject != oldCollision)
        {
            if (canJump && other.transform.CompareTag("DoorObstacle") || other.transform.CompareTag("DeadlyObstacle") || other.transform.CompareTag("ToxicObstacle") || other.transform.CompareTag("WaterObstacle"))
            {
                oldCollision = other.gameObject;
                audioSource.PlayOneShot(angryAudio);
                GameManager.instance.failGame();
                stopMoving();
            }
        }

    }

    float speedHolder = 0;
    private void Jump()
    {
        speedHolder = moveSpeed;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        moveSpeed = jumpForce / 2;

        Invoke("moveSpeedToDefault", 2);

    }

    void moveSpeedToDefault()
    {
        moveSpeed = speedHolder;
    }

    public void stopMoving()
    {

        move = false;
        rb.isKinematic = true;
        coll.enabled = false;
    }
}

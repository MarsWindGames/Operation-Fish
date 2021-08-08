using UnityEngine;
using System.Collections;
public class Dog : MonoBehaviour
{
    [Header("Dog Properties")]
    public float moveRange = 10f;

    //Unity
    bool moveLeft = true;
    Rigidbody rb;
    Vector3 startPoint;
    Vector3 wayPoint;
    bool moveable = true;
    public SpriteRenderer dogSprite;
    Animator animator;
    public AudioSource audioSource;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        wayPoint = startPoint;
        animator = GetComponentInChildren<Animator>();
        StartCoroutine(bark()); //dog will bark randomly
    }

    private void Update()
    {
        if (moveable)
        {
            if (moveLeft)
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -1);
            }
            else
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 1);
            }

            if (moveLeft && transform.position.z < wayPoint.z - moveRange)
            {
                moveLeft = false;
                dogSprite.flipX = false;
            }
            else if (!moveLeft && transform.position.z > wayPoint.z + moveRange)
            {
                moveLeft = true;
                dogSprite.flipX = true;
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player")) 
        {
            moveable = false;
            animator.enabled = false;
        }
    }

    IEnumerator bark()
    {
        float sec = Random.Range(3, 12); // hardcoded random.
        yield return new WaitForSeconds(sec);
        audioSource.PlayOneShot(audioSource.clip);
        StartCoroutine(bark());
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() 
    {
        Vector3 lineStart = transform.position;
        Vector3 lineEnd = lineStart;
        lineStart.z += moveRange;
        lineEnd.z -= moveRange;

        Gizmos.DrawLine(lineStart, lineEnd);
    }
#endif
}

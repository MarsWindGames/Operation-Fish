using UnityEngine;

public class Fish : MonoBehaviour
{
    GameManager gameManager;
    PlayerMovement playerMovement;

    void Start()
    {
        gameManager = GameManager.instance;
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            gameManager.finishGame(); //when player touches the fish finish game.
            playerMovement.stopMoving();
        }
    }
}

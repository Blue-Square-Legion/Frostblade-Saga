using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    [SerializeField] Checkpoint checkPoint;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            checkPoint.CheckpointRespawn();
        }
    }
}

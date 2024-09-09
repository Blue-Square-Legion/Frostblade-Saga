using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    private Transform currentCheckpoint;
    private Health playerHealth;
    [SerializeField] BossTrigger BossTrigger;

    private void Start()
    {
        playerHealth = GetComponent<Health>();
    }

    public void CheckpointRespawn()
    {
        if (currentCheckpoint != null)
        {
            transform.position = currentCheckpoint.position + new Vector3(0,1,0);
            GameManager.Instance.RespawnEnemies();
            BossTrigger.ResetBoss();
            playerHealth.Respawn();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            currentCheckpoint = collision.transform;
            collision.GetComponent<Collider2D>().enabled = false;

            Animator flagAnimator = collision.GetComponent<Animator>();
            if (flagAnimator != null)
            {
                flagAnimator.SetTrigger("Checkpoint");
            }
        }
    }
}

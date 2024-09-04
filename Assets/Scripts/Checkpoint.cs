using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    private Transform currentCheckpoint;
    private Health playerHealth;

    private void Start()
    {
        playerHealth = GetComponent<Health>();
    }

    public void CheckpointRespawn()
    {
        if (currentCheckpoint != null)
        {
            transform.position = currentCheckpoint.position;
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

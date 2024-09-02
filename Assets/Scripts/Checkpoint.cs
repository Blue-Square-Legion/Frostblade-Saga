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
        } else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Checkpoint")
        {
            currentCheckpoint = collision.transform;
            collision.GetComponent<Collider2D>().enabled = false;
            collision.GetComponent<SpriteRenderer>().color = Color.green;
        }
            
    }
}

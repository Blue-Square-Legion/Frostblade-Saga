using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(GameManager)) as GameManager;
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    #endregion

    public int expectedFrameRate;

    void Start()
    {
        
    }

    public void RespawnEnemies()
    {
        var enemies = Resources.FindObjectsOfTypeAll(typeof(GenericEnemy));

        foreach (var enemy in enemies)
        {
            if (!enemy.GameObject().CompareTag("Boss"))
                enemy.GetComponent<GenericEnemy>().Respawn();
        }
    }
}

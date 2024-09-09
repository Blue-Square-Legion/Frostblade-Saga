using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] GameObject boss;
    [SerializeField] GameObject bossDoor;

    private Vector3 bossInitPos;

    private void Start()
    {
        bossInitPos = boss.transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if (!boss.activeInHierarchy)
                boss.SetActive(true);
            if (!bossDoor.activeInHierarchy)
                bossDoor.SetActive(true);
        }
        gameObject.SetActive(false);
    }

    public void ResetBoss()
    {
        gameObject.SetActive(true);
        bossDoor.SetActive(false);
        boss.transform.position = bossInitPos;
        boss.GetComponent<Boss>().DeactivateFireballs();
        boss.SetActive(false);
    }
}
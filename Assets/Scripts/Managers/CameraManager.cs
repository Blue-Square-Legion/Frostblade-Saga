using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private AudioClip bgm;

    private void Start()
    {
        SoundManager.Instance.PlaySound(bgm);
    }

    private void Update()
    {
        //Follow player
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z); 
    }
}

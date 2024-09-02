using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] Health playerHealth;
    [SerializeField] Slider slider;
    [SerializeField] int maxHealth;


    private void Start()
    {
        slider.value = maxHealth;
    }

    private void Update()
    {
        slider.value = playerHealth.CurrentHealth;
    }
}

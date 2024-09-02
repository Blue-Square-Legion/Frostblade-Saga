using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] Slider slider;
    [SerializeField] int maxMana;

    private void Start()
    {
        slider.value = maxMana;
    }

    void Update()
    {
        slider.value = playerController.GetMana();
    }
}
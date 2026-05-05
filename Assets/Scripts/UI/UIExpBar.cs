using UnityEngine;
using UnityEngine.UI;

public class UIExpBar : MonoBehaviour
{
    [SerializeField] private PlayerLevel playerLevel;
    private Slider expBar;
    private void Awake()
    {
        expBar = GetComponent<Slider>();
        playerLevel.OnGainExp += SetExpBar;
        expBar.value = 0f;
    }
    private void SetExpBar()
    {
        float ratio = playerLevel.ExpRatio;
        expBar.value = ratio;
    }
}

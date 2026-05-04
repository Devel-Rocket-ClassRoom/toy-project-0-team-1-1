using UnityEngine;
using UnityEngine.UI;

public class UIExpBar : MonoBehaviour
{
    [SerializeField] private PlayerLevel playerLevel;
    private Slider expBar;
    private void Awake()
    {
        expBar = GetComponent<Slider>();
        playerLevel.OnLevelUp += SetExpBar;
        expBar.value = 0f;
    }
    private void SetExpBar()
    {
        float ratio = playerLevel.ExpRatio;
        expBar.value = ratio;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            expBar.value += 0.1f;
            Debug.Log(expBar.value);
        }
    }
}

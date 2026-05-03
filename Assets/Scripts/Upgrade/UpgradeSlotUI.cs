using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSlotUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private Button button;

    private IUpgrade upgrade;
    private System.Action<IUpgrade> onSelect;

    public void Setup(IUpgrade upgrade, System.Action<IUpgrade> onSelect,
                      string description, string levelLabel)
    {
        this.upgrade = upgrade;
        this.onSelect = onSelect;

        iconImage.sprite = upgrade.Icon;
        nameText.text = upgrade.Name;
        descText.text = description;
        levelText.text = levelLabel;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        onSelect?.Invoke(upgrade);
    }

    public void Clear()
    {
        button.onClick.RemoveAllListeners();
        upgrade = null;
    }
}

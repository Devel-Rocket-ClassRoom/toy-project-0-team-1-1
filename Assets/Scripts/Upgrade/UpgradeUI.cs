using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private UpgradeSlotUI[] slots;

    private System.Action<IUpgrade> onSelected;

    public void Show(List<IUpgrade> choices, System.Action<IUpgrade> onSelected)
    {
        this.onSelected = onSelected;

        Time.timeScale = 0f;
        panel.SetActive(true);

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < choices.Count)
            {
                slots[i].gameObject.SetActive(true);
                var upgrade = choices[i];
                slots[i].Setup(upgrade, OnSlotSelected, "", "");
            }
            else
            {
                slots[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnSlotSelected(IUpgrade upgrade)
    {
        panel.SetActive(false); // 패널 닫기
        Time.timeScale = 1f;

        foreach (var slot in slots) // 모든 슬롯 정리
            slot.Clear();

        onSelected?.Invoke(upgrade); // 선택 결과 전달
    }
}
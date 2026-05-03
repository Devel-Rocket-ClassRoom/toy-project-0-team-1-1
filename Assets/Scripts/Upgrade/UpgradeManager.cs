using UnityEngine;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }
    public List<WeaponData> weaponList = new List<WeaponData>();                    // 모든 무기 데이터 리스트
    public List<UpgradeItemData> upgradeItemList = new List<UpgradeItemData>();     // 모든 업그레이드 아이템 데이터 리스트
    public PlayerWeapon playerWeapon;                                               // 현재 플레이어가 장착한 무기 불러오기
    public PlayerStatus playerStatus;                                               // 현재 플레이어의 업그레이드 아이템 불러오기
}
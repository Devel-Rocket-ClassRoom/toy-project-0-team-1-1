using System;
using UnityEngine;

public class UITest : MonoBehaviour
{
    [SerializeField] private PlayerLevel playerLevel;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerLevel.GainExp(10f);
        }
    }
}

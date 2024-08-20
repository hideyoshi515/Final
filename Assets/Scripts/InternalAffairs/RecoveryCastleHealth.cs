using System;
using System.IO;
using DefenseScripts;
using UnityEngine;

public class RecoveryCastleHealth : MonoBehaviour
{
    private string path;
    private float maxHealth;
    private float crntHealth;
    private float emptyHealth;
    private string json;
    
    private void Awake()
    {
        crntHealth = PlayerLocalManager.Instance.lCastleHp;
        maxHealth = PlayerLocalManager.Instance.lCastleMaxHp;
        emptyHealth = maxHealth - crntHealth;
    }

    public void OnRecovery()
    {
        if (Mathf.Approximately(crntHealth, maxHealth))
        {
            CustomLogger.Log("HP is already full!");
            return;
        }
        CustomLogger.Log("Now HP is "+emptyHealth,"red");
        // 여기에 골드 연동 관련 로직 추가
        // 예: playerGold -= recoveryCost;

        // 체력을 회복하고 데이터를 저장
        crntHealth = maxHealth;
        PlayerLocalManager.Instance.lCastleHp = PlayerLocalManager.Instance.lCastleMaxHp;
        PlayerLocalManager.Instance.Save();
    }
}
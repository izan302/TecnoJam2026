using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsLoader : MonoBehaviour
{
    [SerializeField] private Toggle m_SyncBands;
    [SerializeField] public GameObject m_SettingsMenu;

    void OnEnable()
    {
        if (m_SyncBands != null && GabeNewell.Instance != null)
        {
            m_SyncBands.isOn = GabeNewell.Instance.m_CrtEffect;
            
            m_SyncBands.onValueChanged.AddListener(OnSyncBandsChanged);
        }
    }

    private void OnSyncBandsChanged(bool value)
    {
        GabeNewell.Instance.m_CrtEffect = value;
    }
}
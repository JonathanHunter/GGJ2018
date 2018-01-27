using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadUI : MonoBehaviour {

    private int bulletsInClip, maxClipSize = 6;
    private float rechargeTimer, toggleFlashTimer;

    /// <summary>
    /// Whether or not the Reload is recharging
    /// </summary>
    public bool IsRecharging { get; private set; }

    /// <summary>
    /// Call this to kick off the reload presentation
    /// </summary>
    /// <param name="duration"></param>
    /// How long the reload will take
    public void StartRecharge()
    {
        rechargeTimer = 3.0f;
        toggleFlashTimer = 0.5f;
        IsRecharging = true;
    }

    /// <summary>
    /// Used to update UI values
    /// </summary>
    /// <param name="bulletsInClip"></param>
    /// Number of bullets in your clip
    public void UpdateValues(int bulletsInClip)
    {
        this.bulletsInClip = bulletsInClip;
    }

    void Update()
    {
        //if = 
        if (rechargeTimer > 0)
        {
            rechargeTimer -= Time.deltaTime;
            if (rechargeTimer <= 0)
            {
                IsRecharging = false;
            }
        }
    }
}

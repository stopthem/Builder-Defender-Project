﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;
    
    private Transform barTransform;
    private void Awake()
    {
        barTransform = transform.Find("bar");
    }
    private void Start()
    {
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;
        UpdateBar();
        UpdateHealthBarVisible();
    }

    private void HealthSystem_OnHealed(object sender, EventArgs e)
    {
        UpdateBar();
        UpdateHealthBarVisible();
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        UpdateBar();
        UpdateHealthBarVisible();
    }

    private void UpdateBar()
    {
        barTransform.localScale = new Vector3(healthSystem.GetHealthNormalized(),1,1);
    }
    private void UpdateHealthBarVisible()
    {
        if (healthSystem.IsFullHealth())
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}

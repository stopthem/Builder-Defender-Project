﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;
    private Transform seperatorContainer;
    private Transform barTransform;
    private void Awake()
    {
        barTransform = transform.Find("bar");
    }
    private void Start()
    {
        seperatorContainer = transform.Find("seperatorContainer");
        ConstructHealthBarSeperators();
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;
        healthSystem.OnHealthAmountMaxChanged += HealthSystem_OnHealthAmountMaxChanged;
        UpdateBar();
        UpdateHealthBarVisible();
    }

    private void ConstructHealthBarSeperators()
    {
        
        Transform seperatorTemplate = seperatorContainer.Find("seperatorTemplate");
        seperatorTemplate.gameObject.SetActive(false);

        foreach (Transform seperatorTransform in seperatorContainer)
        {
            if (seperatorTransform == seperatorTemplate)
            {
                continue;
            }
            else
            {
                Destroy(seperatorTransform.gameObject);
            }
            
        }

        int healthAmountPerSeperator = 10;
        float barSize = 4f;
        float barOneHealthAmountSize = barSize / healthSystem.GetHealthAmountMax();
        int healthSeparatorCount = Mathf.FloorToInt(healthSystem.GetHealthAmountMax() / healthAmountPerSeperator);
        for (int i = 1; i < healthSeparatorCount; i++)
        {
            Transform seperatorTransform = Instantiate(seperatorTemplate, seperatorContainer);
            seperatorTransform.gameObject.SetActive(true);
            seperatorTransform.localPosition = new Vector3(barOneHealthAmountSize * i * healthAmountPerSeperator, 0, 0);
        }
    }

    private void HealthSystem_OnHealthAmountMaxChanged(object sender, EventArgs e)
    {
        ConstructHealthBarSeperators();
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
        gameObject.SetActive(true);
    }
}

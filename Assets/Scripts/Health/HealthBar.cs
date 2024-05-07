using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _healtBar;
    [SerializeField] private Health _health;

    private void OnEnable()
    {
        _health.HealthValueChanged += OnHealthValueChanged;
    }

    private void OnDisable()
    {
        _health.HealthValueChanged -= OnHealthValueChanged;
    }

    private void OnHealthValueChanged(float value)
    {
        float procent = _health.MaxValue / 100;
        
        _healtBar.fillAmount = value * procent;
    }
}
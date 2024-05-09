using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    [SerializeField] private GameObject[] _weapons;

    [SerializeField] private GameObject _currentWeapon;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeWeapon(_weapons[0]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeWeapon(_weapons[1]);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeWeapon(_weapons[2]);
        }
    }

    public void ChangeWeapon(GameObject weapon)
    {
        if (_currentWeapon == weapon)
            return;
        _currentWeapon?.SetActive(false);
        _currentWeapon = weapon;
        _currentWeapon.SetActive(true);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Xamin;

public class RadialMenu : MonoBehaviour
{
    [SerializeField] private float _openedWheelTimeScale;

    [SerializeField] private CircleSelector _selector;
    [SerializeField] private Button[] _radialMenuItems;


    private CompositeDisposable _disposable = new CompositeDisposable();

    private bool _destroy;

    public Button CurrentRadialButton;

    public static RadialMenu Instance { get; private set; }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            return;
        }

        _destroy = true;
        Destroy(this);
    }


    private void OpenRadialMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;

        _selector.Open();
    }

    private void CloseRadialMenu()
    {
        _disposable.Clear();

        Cursor.lockState = CursorLockMode.Locked;

        for (int i = 0; i < _radialMenuItems.Length; i++)
        {
            if (_radialMenuItems[i].gameObject == _selector.SelectedSegment)
            {
                if (CurrentRadialButton != _radialMenuItems[i])
                    DeSelectCurrentItem();

                _radialMenuItems[i].ExecuteAction();
                if (_radialMenuItems[i].unlocked)
                    CurrentRadialButton = _radialMenuItems[i];
            }
        }

        _selector.Close();
    }

    public void DeSelectCurrentItem()
    {
        if (CurrentRadialButton != null)
        {
            CurrentRadialButton.DesecuteAction();
        }
    }

    public void SelectCurrentItem()
    {
        if (CurrentRadialButton != null)
        {
            CurrentRadialButton.ExecuteAction();
        }
    }

    public void OnWeaponPickuped(Item weaponItem)
    {
        for (int i = 0; i < _radialMenuItems.Length; i++)
        {
            if (_radialMenuItems[i].WeaponIn == weaponItem)
            {
                _radialMenuItems[i].SetButtonActive();
                return;
            }
        }
    }
}
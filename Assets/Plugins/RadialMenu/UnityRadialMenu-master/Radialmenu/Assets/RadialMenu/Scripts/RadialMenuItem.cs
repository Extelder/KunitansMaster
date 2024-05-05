using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RadialMenuItem : MonoBehaviour
{
    [field: SerializeField] public Item ItemIn { get; private set; }
    [SerializeField] private Image _itemBG;
    [SerializeField] private Image _itemPic;
    [SerializeField] private Sprite _bgSelected;
    [SerializeField] private Sprite _bgUnSelected;
    [SerializeField] private GameObject _weapon;

    private bool _canUsed;

    public void OnUse()
    {
        Debug.Log("I'm used");
        if (_canUsed)
        {
            _weapon.SetActive(true);
        }
    }

    public void Hide()
    {
        if (_canUsed == true)
        {
            _weapon.SetActive(false);
        }
    }

    public void SetItemState(bool ItemState)
    {
        if (ItemState)
        {
            _itemBG.sprite = _bgSelected;
        }
        else
        {
            _itemBG.sprite = _bgUnSelected;
        }
    }

    public void ItemPickuped()
    {
        _itemPic.sprite = ItemIn.Icon;
        _itemPic.gameObject.SetActive(true);
        _canUsed = true;
    }
}
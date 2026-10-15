using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemEquipment : MonoBehaviour {

    public Image _itemImg;
    public Text _nameItem;
    public GameObject _buyBtn;
    public Text _priceText;
    public GameObject _continue;
    public Weapon _weapon;
    public Hat _hat;
    public bool _isBought = false;
	// 初始化组件
	public void SetupDataItem(string nameSprite, string name, bool isBought, string _priceItem,Weapon _tempWeapon, Hat _tempHat)
    {
        _itemImg.sprite = Resources.Load<Sprite>("iconShop/" + nameSprite); ;
        _nameItem.text = name;
        if (_tempWeapon != null)
        {
            _weapon = _tempWeapon;
        }
        if (_tempHat != null)
        {
            _hat = _tempHat;
        }
        _isBought = isBought;
        _continue.GetComponent<Button>().onClick.AddListener(() => { ItemManager.instance.CloseItemMenu(); });
        if (_weapon != null)
        {
            _buyBtn.GetComponent<Button>().onClick.AddListener(() => { ItemManager.instance.BuyButtonPressed(_weapon.id); });
        }
        else
        {
            _buyBtn.GetComponent<Button>().onClick.AddListener(() => { ItemManager.instance.BuyButtonPressed(_hat.id); });
        }
        if (isBought)
        {
            _buyBtn.gameObject.SetActive(false);
            _continue.gameObject.SetActive(true);
        } else
        {
            _buyBtn.gameObject.SetActive(true);
            _priceText.text = _priceItem;
            _continue.gameObject.SetActive(false);
        }
    }
    public void UpdateBuyButton()
    {
        if(_weapon!=null && _weapon.isOwned)
        {
            _isBought = true;
            _buyBtn.gameObject.SetActive(false);
            _continue.gameObject.SetActive(true);
        }
        if (_hat != null && _hat.isOwned)
        {
            _isBought = true;
            _buyBtn.gameObject.SetActive(false);
            _continue.gameObject.SetActive(true);
        }
    }
    public void UpdatePrice(string price)
    {
        _priceText.text = price;
    }
}

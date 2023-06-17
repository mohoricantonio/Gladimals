using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChanger : MonoBehaviour
{
    public GameObject weapon;
    public GameObject weaponHolder;
    public GameObject sheetWeapon;
    private GameObject weaponInHand;
    private GameObject weaponInSheet;
    // Start is called before the first frame update
    void Start()
    {
        weaponInSheet = Instantiate(weapon, sheetWeapon.transform);
    }

    public void WeaponDraw()
    {
        Destroy(weaponInSheet);
        weaponInHand = Instantiate(weapon, weaponHolder.transform);
    }
    public void WeaponSheet()
    {
        Destroy(weaponInHand);
        weaponInSheet = Instantiate(weapon, sheetWeapon.transform);
    }
}

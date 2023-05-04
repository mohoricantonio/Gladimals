using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    private int selectedWeapon = 0;
    // Start is called before the first frame update
    void Start()
    {
        changeWeapon();
    }
    public void selectWeapon(int i)
    {
        selectedWeapon = i;
    }
    private void changeWeapon()
    {
        int i = 0;
        foreach(Transform t in transform)
        {
            if(i == selectedWeapon)
            {
                t.gameObject.SetActive(true);
            }
            else
            {
                t.gameObject.SetActive(false);
            }
            i++;
        }
    }
}

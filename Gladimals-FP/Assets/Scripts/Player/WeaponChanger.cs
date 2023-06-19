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
    private GameObject enemy;

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        weaponInSheet = Instantiate(weapon, sheetWeapon.transform);
        enemy = GameObject.FindGameObjectWithTag("Enemy");

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
    public void ResetWeaponDrawn()
    {
        anim.SetBool("Weapon drawn", false);
    }
    public void SetWeaponDrawn()
    {
        anim.SetBool("Weapon drawn", true);
    }
    public void ResetFirstAttack()
    {
        anim.SetBool("FirstAttack", false);
    }
    public void SetFirstAttack()
    {
        anim.SetBool("FirstAttack", true);
    }
    public void SetSecondAttack()
    {
        anim.SetBool("SecondAttack", true);
    }
    public void ResetSecondAttack()
    {
        anim.SetBool("SecondAttack", false);
    }
    public void SetPowerAttack()
    {
        anim.SetBool("PowerSlash", true);
    }
    public void ResetPowerAttack()
    {
        anim.SetBool("PowerSlash", false);
    }
    public void SetCanMove()
    {
        anim.SetBool("CanMove", true);
    }
    public void ResetCanMove()
    {
        anim.SetBool("CanMove", false);
    }

    public void NormalDamageAnimEvent()
    {
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth.swordCollision)
        {
            Debug.Log(anim.GetBool("PowerSlash"));
                enemyHealth.TakeDamage(20);
        }
    }

    public void HighDamageAnimEvent()
    {
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth.swordCollision)
        {
            Debug.Log(anim.GetBool("PowerSlash"));
            enemyHealth.TakeDamage(40);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    private Animator anim;
    private bool weaponDrawn;
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        weaponDrawn = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            DrawWeapon();
        }

        if (weaponDrawn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
            }
        }
    }
    private void Attack()
    {
        Debug.Log("Attack");
        anim.SetTrigger("Attack");
    }
    private void DrawWeapon()
    {
        if (!weaponDrawn)
        {
            anim.SetTrigger("Draw weapon");
            weaponDrawn = true;
        }
        else
        {
            anim.SetTrigger("Sheet weapon");
            weaponDrawn = false;
        }
    }
}

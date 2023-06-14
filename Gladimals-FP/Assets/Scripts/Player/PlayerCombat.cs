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
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            DrawWeapon();
        }
    }
    private void Attack()
    {
        Debug.Log("Attack");
        anim.SetTrigger("Attack");
    }
    private void DrawWeapon()
    {
        Debug.Log("Draw weapon");
        if (!weaponDrawn)
            anim.SetTrigger("Draw weapon");
        else anim.ResetTrigger("Draw weapon");
    }
}

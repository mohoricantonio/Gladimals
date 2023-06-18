using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerCombat : MonoBehaviour
{
    private Animator anim;
    private bool weaponDrawn;
    public AudioClip drawWeaponSound;
    private AudioSource audioSource;
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
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

        //CheckDeath();
    }
    private void Attack()
    {
        Debug.Log("Attack");
        anim.SetTrigger("Attack");
    }

    private IEnumerator PlayDrawWeaponSound()
    {
        yield return new WaitForSeconds(0.5f);
        audioSource.PlayOneShot(drawWeaponSound);
    }
    private void DrawWeapon()
    {
        if (!weaponDrawn)
        {
            anim.SetTrigger("Draw weapon");
            StartCoroutine(PlayDrawWeaponSound());
            weaponDrawn = true;
        }
        else
        {
            anim.SetTrigger("Sheet weapon");
            weaponDrawn = false;
        }
    }


    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Weapon")
        {
            Debug.Log("Enemy hit");
            GetComponent<PlayerHealth>().swordCollision = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Weapon")
        {
            GetComponent<PlayerHealth>().swordCollision = false;
        }
    }

    private void CheckDeath(){
        if(GetComponent<PlayerHealth>().isDead()){
            anim.SetTrigger("Death");
        }
    }
}

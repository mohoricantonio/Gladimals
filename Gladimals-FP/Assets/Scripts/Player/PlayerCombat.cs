using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerCombat : MonoBehaviour
{
    private Animator anim;
    public AudioClip drawWeaponSound;
    public AudioClip swordSound;
    public AudioClip crowdHeavySound;
    private AudioSource arenaAudioSource;
    private AudioSource audioSource;
    private float longPressDuration = 0.2f;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        GameObject arena = GameObject.FindGameObjectWithTag("Arena");
        if (arena != null)
        {
            arenaAudioSource = arena.GetComponent<AudioSource>();
        }
        anim.SetBool("FirstAttack", false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            DrawWeapon();
        }

        if (anim.GetBool("Weapon drawn"))
        {
            if (Input.GetMouseButtonDown(0))
            {
                Invoke("LongPressAttack", longPressDuration);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                CancelInvoke("LongPressAttack");
                if(anim.GetBool("PowerSlash") == false)
                {
                    Attack();
                }
            }
        }

        //CheckDeath();
    }
    private void Attack()
    {
        anim.SetTrigger("Attack");
        audioSource.PlayOneShot(swordSound);
    }

    private IEnumerator PlayDrawWeaponSound()
    {
        yield return new WaitForSeconds(0.5f);
        audioSource.PlayOneShot(drawWeaponSound);
    }
    private void DrawWeapon()
    {
        if (!anim.GetBool("Weapon drawn"))
        {
            anim.SetTrigger("Draw weapon");
            StartCoroutine(PlayDrawWeaponSound());
        }
        else
        {
            anim.SetTrigger("Sheet weapon");
        }
    }

    private IEnumerator LongAttackSound()
    {
        yield return new WaitForSeconds(0.4f);
        audioSource.PlayOneShot(swordSound);
    }

    private void LongPressAttack()
    {
        anim.SetTrigger("PowerAttack");
        StartCoroutine(LongAttackSound());
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Weapon")
        {
            //Debug.Log("Enemy hit");
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
            arenaAudioSource.PlayOneShot(crowdHeavySound);
        }
    }
}

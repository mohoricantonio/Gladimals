using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerCombat : MonoBehaviour
{
    private Animator anim;
    public AudioClip drawWeaponSound;

    private AudioSource audioSource;
    private float longPressDuration = 0.2f;

    private GameObject enemy;
    public float HeavyAttackStunTime = 2;
    private float powerAttackCooldown = 2f;

    [Header("Combo Fileds")]
    public static int numberOfClicks = 0;
    public float comboCooldownTime = 2f;
    public float nextFireTime = 0f;
    float lastClickedTime = 0;
    float maximumComboDelay = 1;


    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        anim.SetBool("FirstAttack", false);
        enemy = enemy = GameObject.FindGameObjectWithTag("Enemy");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            DrawWeapon();
        }
        if (Input.GetMouseButtonDown(1))
        {
            anim.SetTrigger("Block");
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

        if (powerAttackCooldown > 0)
        {
            powerAttackCooldown -= Time.deltaTime;
        }
    }
    private void Attack()
    {
        anim.SetTrigger("Attack");
    }

    private IEnumerator PlayDrawWeaponSound()
    {
        yield return new WaitForSeconds(0.5f);
        audioSource.PlayOneShot(drawWeaponSound);
    }
    public void DrawWeapon()
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

    private void LongPressAttack()
    {
        if (powerAttackCooldown <= 0){
            powerAttackCooldown = 2f;
            anim.SetTrigger("PowerAttack");
        }
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Weapon")
        {
            GetComponent<PlayerHealth>().swordCollision = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Weapon")
        {
            GetComponent<PlayerHealth>().swordCollision = false;
        }
    }

    private void OnClick()
    {
        
    }
}

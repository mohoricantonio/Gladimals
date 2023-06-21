using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Statistics : MonoBehaviour
{
    public static float startTime;
    public static float totalTime = 0;
    public float endTime;
    public static int hpLost = 0;
    public GameObject subTitleText;

    // Start is called before the first frame update
    public void SetStartTime()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    public void SetEndTime()
    {
        endTime = Time.time;
    }

    public void SetStat()
    {
        SetEndTime();
        totalTime += endTime - startTime;
        SetHPLost();
        if (subTitleText != null){
            subTitleText.GetComponent<TextMeshProUGUI>().text = "Total Time : " + ((int)totalTime) + "s" + "\n" + "HP Lost : " + hpLost;
        }
    }

    public void SetHPLost()
    {
        PlayerHealth playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
        hpLost += playerHealth.maxHealth - playerHealth.currentHealth;

    }
}

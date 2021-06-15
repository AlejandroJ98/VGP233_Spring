using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUps : MonoBehaviour
{
    public int healAmount;
    private bool collected;//just in case it can be pickup more than once
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !collected)
        {
            PlayerHealthController.instance.HealPlayer(healAmount);

            Destroy(gameObject);
            collected = true;
        }
    }
}

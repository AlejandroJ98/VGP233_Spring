using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    private bool collected;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !collected)//if player touch it
        {
            PlayerController.instance.activeGun.GetAmmo();//call the function
            //give ammo

            Destroy(gameObject);

            collected = true;

            AudioManager.instance.PlaySFX(3);
        }
    }
}

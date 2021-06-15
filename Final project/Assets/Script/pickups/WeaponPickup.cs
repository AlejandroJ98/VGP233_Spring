using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public string theGun;
    private bool collected;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !collected)//if player touch it
        {
            PlayerController.instance.AddGun(theGun);
            //give ammo

            Destroy(gameObject);


            AudioManager.instance.PlaySFX(4);
            collected = true;
        }
    }
}

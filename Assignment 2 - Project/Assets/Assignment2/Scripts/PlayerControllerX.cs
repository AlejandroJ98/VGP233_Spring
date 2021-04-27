using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public GameObject dogPrefab;
    //public float coolDown = 2;
    //public float coolDownTimer;
    private float coolDown = 0;

    // Update is called once per frame
    void Update()
    {
        // On spacebar press, send dog
        if (Input.GetKeyDown(KeyCode.Space) && (Time.time > coolDown))
        {
            Debug.Log(Time.time);
            coolDown = Time.time + 2;
            Instantiate(dogPrefab, transform.position, dogPrefab.transform.rotation);
            Debug.Log("Wait");
        }

    }
}

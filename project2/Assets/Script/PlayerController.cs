using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 30.0f;
    public GameObject projectilePrefab;
    public float projectileOffset;

    private float horizontalinput;
    private float xRange = 19.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalinput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalinput * speed * Time.deltaTime);

        if(transform.position.x > xRange)
        {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }
        else if(transform.position.x < -xRange)
        {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            //Launch a hamburger/food/projectile
            //instantiate()
            Instantiate(projectilePrefab, new Vector3(transform.position.x, transform.position.y + projectileOffset, transform.position.z), projectilePrefab.transform.rotation);
        }
    }
}

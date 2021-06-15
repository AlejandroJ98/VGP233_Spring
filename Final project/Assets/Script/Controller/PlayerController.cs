using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float moveSpeed, gravityModifier, jumpPower, runSpeed = 12;
    public CharacterController CharCon;

    private Vector3 moveInput;

    public Transform camTrans;

    public float mouseSensitivity;
    public bool invertX;
    public bool invertY;

    private bool canJump, canDoubleJump;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;

    public Animator anim;

    //public GameObject bullet;
    public Transform firePoint;

    public Gun activeGun;
    public List<Gun> allGuns = new List<Gun>();//for switching weapons create a list of gun
    public List<Gun> unlockableGuns = new List<Gun>();
    public int currenGun;

    public Transform adsPoint, gunHolder;//adjust zoom in
    private Vector3 gunStartPos;
    public float adsSpeed = 2.0f;

    public GameObject muzzleFlash;

    public AudioSource footstepFast, footstepSlow;

    private float bounceAmount;
    private bool bounce;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //activeGun = allGuns[currenGun];
        //activeGun.gameObject.SetActive(true);

        //UIController.instance.ammoText.text = "AMMO  " + activeGun.currentAmmo;
        
        currenGun--;
        SwitchGun();

        gunStartPos = gunHolder.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!UIController.instance.pauseScreen.activeInHierarchy)
        {


            //moveInput.x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            //moveInput.z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

            //store y velocity
            float yStore = moveInput.y;

            Vector3 vertMove = transform.forward * Input.GetAxis("Vertical");
            Vector3 horiMove = transform.right * Input.GetAxis("Horizontal");

            moveInput = horiMove + vertMove;
            moveInput.Normalize();

            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveInput = moveInput * runSpeed;
            }
            else
            {
                moveInput = moveInput * moveSpeed;
            }

            moveInput.y = yStore;

            moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;

            if (CharCon.isGrounded)
            {
                moveInput.y = Physics.gravity.y * gravityModifier * Time.deltaTime;
            }

            canJump = Physics.OverlapSphere(groundCheckPoint.position, .25f, whatIsGround).Length > 0;

            if (canJump)
            {
                canDoubleJump = true;
            }

            //handle Jumping
            if (Input.GetKeyDown(KeyCode.Space) && canJump)
            {
                canDoubleJump = true;
                moveInput.y = jumpPower;

                AudioManager.instance.PlaySFX(8);
            }
            else if (canDoubleJump && Input.GetKeyDown(KeyCode.Space))
            {
                canDoubleJump = false;
                moveInput.y = jumpPower;

                AudioManager.instance.PlaySFX(8);
            }

            if(bounce)
            {
                bounce = false;
                moveInput.y = bounceAmount;

                canDoubleJump = true;
            }

            CharCon.Move(moveInput * Time.deltaTime);

            //control camera rotation
            Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

            if (invertX)
            {
                mouseInput.x = -mouseInput.x;
            }
            if (invertY)
            {
                mouseInput.y = -mouseInput.y;
            }//invert mouse

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);

            camTrans.rotation = Quaternion.Euler(camTrans.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));

            muzzleFlash.SetActive(false);//set muzzle to false before

            //Handle shooting
            //single shot
            if (Input.GetMouseButtonDown(0) && activeGun.fireCounter <= 0)//make sure player can't keep shooting by mouse 0
            {
                RaycastHit hit;
                if (Physics.Raycast(camTrans.position, camTrans.forward, out hit, 50.0f))
                {
                    if (Vector3.Distance(camTrans.position, hit.point) > 2.0f)
                    {
                        firePoint.LookAt(hit.point);
                    }
                }
                else
                {
                    firePoint.LookAt(camTrans.position + (camTrans.forward * 30.0f));
                }

                // Instantiate(bullet, firePoint.position, firePoint.rotation);
                FireShot();
            }

            //repeating shots
            if (Input.GetMouseButton(0) && activeGun.canAutoFire)// fire until it meet both condition
            {
                if (activeGun.fireCounter <= 0)
                {
                    FireShot();
                }
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SwitchGun();
            }

            if (Input.GetMouseButtonDown(1))
            {
                CameraController.instance.ZoomIn(activeGun.zoomAmount);
            }

            if (Input.GetMouseButton(1))
            {
                gunHolder.position = Vector3.MoveTowards(gunHolder.position, adsPoint.position, adsSpeed * Time.deltaTime);
            }
            else
            {
                gunHolder.localPosition = Vector3.MoveTowards(gunHolder.localPosition, gunStartPos, adsSpeed * Time.deltaTime);
            }

            if (Input.GetMouseButtonUp(1))
            {
                CameraController.instance.ZoomOut();
            }

            anim.SetFloat("moveSpeed", moveInput.magnitude);
            anim.SetBool("onGround", canJump);
        }
    }
    public void FireShot()
    {
        if(activeGun.currentAmmo > 0)
        {
            activeGun.currentAmmo--;

            Instantiate(activeGun.bullets, firePoint.position, firePoint.rotation);

            activeGun.fireCounter = activeGun.fireRate;

            UIController.instance.ammoText.text = "AMMO  " + activeGun.currentAmmo;//keep updating

            muzzleFlash.SetActive(true);
        }

    }

    public void SwitchGun()
    {
        activeGun.gameObject.SetActive(false);//deactivated the gun at the moment

        currenGun++;//next gun in the List

        if(currenGun >= allGuns.Count)//when gun index is greater than the size
        {
            currenGun = 0;//reset the count
        }

        activeGun = allGuns[currenGun];
        activeGun.gameObject.SetActive(true);

        UIController.instance.ammoText.text = "AMMO  " + activeGun.currentAmmo;

        firePoint.position = activeGun.firepoint.position;//keep updating the firepoint
    }

    public void AddGun(string gunToAdd)
    {
        bool gunUnlocked = false;

        if(unlockableGuns.Count > 0)
        {
            for(int i = 0; i<unlockableGuns.Count; i++)
            {
                if(unlockableGuns[i].gunName == gunToAdd)
                {
                    gunUnlocked = true;

                    allGuns.Add(unlockableGuns[i]);

                    unlockableGuns.RemoveAt(i);

                    i = unlockableGuns.Count;
                }
            }
        }
        if(gunUnlocked)
        {
            currenGun = allGuns.Count - 2;
            SwitchGun();
        }
    }

    public void Bounce(float bounceForce)
    {
        bounceAmount = bounceForce;
        bounce = true;
    }
}

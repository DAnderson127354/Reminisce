using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkeletonControl : MonoBehaviour
{
	//VARIABLES
	//Movement
	CharacterController charCont;
    private Vector3 moveDirection = Vector3.zero;
    public float speed;
    public float runSpeed;
    public float sprintSpeed;                     
    public float gravity;
    bool canMove = true;

	private bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Slider StaminaBar;
	private bool hasStamina = true;
	public float maxStamina;
	public float recoverySpeed;
	public float exhaustSpeed;
	float stamina = 1f;

    private bool isInside = false;
    private bool onRug = false;

    //Animation
    private Animator anim;
    private Animator anim1;

    public Text interact;
    public GameObject popUp;

    private bool canOpen;

    private bool canSit;
    private bool isSitting;
    private float sitDirection;
    Vector3 currentEulerAngles;

    //Recovery
    public Slider RecoveryBar;
    public GameObject lostItems;
    public GhostControl ghost;

    //Projectile
    public Rigidbody projectile;
    public GameObject spawn;
    public float projectileSpeed;
    private bool canShoot;

    //GAME
    public GameManager gameManager;
    public GameObject map;
    public GameObject map1;
    public GameObject timerBG;

    //PICK UP
    public GameObject hand;
    private bool pickup;
    private bool canPickUp;
    private GameObject itemToPick;

    //Sounds
    public SoundManager soundManager;
    public AudioClip softFootsteps;
    public AudioClip loudFootsteps;

    // Start is called before the first frame update
    void Start()
    {
        charCont = GetComponent<CharacterController>(); //Finds and stores the reference to the character controller component
        anim = GetComponent<Animator>();
        canSit = false;
        isSitting = false;
        pickup = false;
        canPickUp = false;
        canShoot = true;
    }//Start

    // Update is called once per frame
    void Update()
    {
    	if (gameManager.gameDone())
    		return;

        if (Input.GetKeyDown(KeyCode.U))
        {
            if (map.activeSelf && map1.activeSelf && timerBG.activeSelf)
            {
                map.SetActive(false);
                map1.SetActive(false);
                timerBG.SetActive(false);
            }
            else
            {
                map.SetActive(true);
                map1.SetActive(true);
                timerBG.SetActive(true);
            }
        }

        if (Input.GetButtonDown("Fire1"))
            PlayerShoot();

        if (pickup)
        {
           // Debug.Log("howdy");
            stamina = StaminaBar.value;
            StaminaBar.value -= exhaustSpeed * Time.deltaTime * 0.2f;
            if ((hand.transform.childCount == 0 || StaminaBar.value == 0) && canPickUp == false)
            {
                //Debug.Log("drop");
                anim.SetTrigger("PickUp");
                canPickUp = true;
            }
        }    

    	PlayerInteractions();
    	if (canMove)
    		PlayerMovement();
    }//Update

    //PLAYER FUNCTIONS
    void PlayerMovement()
    {
    	//checking to see if player is on the ground move around
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && moveDirection.y < 0f)
            moveDirection.y = -2f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (StaminaBar.value == maxStamina)
        	hasStamina = true;

        if (StaminaBar.value == 0 || hasStamina == false)
        {
        	//spawn.SetActive(false);
        	anim.SetBool("isMoving", false);
        	anim.SetFloat("Stamina", stamina);
            soundManager.Stop();
            hasStamina = false;
            if (pickup == false)
            {
                StaminaBar.value += recoverySpeed * Time.deltaTime;
                stamina += Time.deltaTime * 0.4f;
            }
        	return;
        }//if

        if (x != 0 || z != 0) //if the player is moving
        {
        	//spawn.SetActive(false);
        	anim.SetBool("isMoving", true);

            
            if (isInside)
            {
                //Debug.Log(onRug);
                if (onRug)
                    soundManager.Footsteps(softFootsteps);
                else
                    soundManager.Footsteps(loudFootsteps);
            }   
            else
            {
               // Debug.Log("footsteps");
                soundManager.Footsteps(softFootsteps);
            }
                

        	if (z > 0) //if the player is moving forward
        	{
        		anim.SetFloat("Direction", 1);
        		if (Input.GetKey(KeyCode.LeftShift) && StaminaBar.value > 0) //if player is sprinting
        		{
                    soundManager.AdjustPitch(1f);
                    stamina -= Time.deltaTime;
        			speed = sprintSpeed;
        			StaminaBar.value -= exhaustSpeed * Time.deltaTime;
        		}//if
        		else //if player is not sprinting
        		{
                    speed = runSpeed;
                    soundManager.AdjustPitch(0.67f);
                    if (pickup == false)
                    {
                        stamina += Time.deltaTime;
                        
                        if (StaminaBar.value < maxStamina)
                            StaminaBar.value += recoverySpeed * Time.deltaTime;
                    }
        		}//else

        		stamina = Mathf.Clamp(stamina, 0f, 1f);
        		anim.SetFloat("Stamina", stamina);
        	}//if
        	else //if the player is not moving forward
        	{
        		anim.SetFloat("Direction", -1);
        		speed = runSpeed;
        	}
        }//if
        else //if the player is not moving
        {
        	//spawn.SetActive(true);
        	anim.SetBool("isMoving", false);
            soundManager.Stop();

        	if (StaminaBar.value < maxStamina && pickup == false)
        	{
        		StaminaBar.value += recoverySpeed * Time.deltaTime;
        		stamina += Time.deltaTime;
        	}

        	anim.SetFloat("Stamina", stamina);

        }//else

        Vector3 move = transform.right * x + transform.forward * z;
        charCont.Move(move * speed * Time.deltaTime);

        moveDirection.y += gravity;
        charCont.Move(moveDirection * Time.deltaTime);
    }//PlayerMovement

    void PlayerInteractions()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            anim.SetTrigger("PickUp");
        }

    	 if (canOpen && Input.GetKeyDown(KeyCode.E) && pickup == false)
    	{	
    		canMove = false;
    		//spawn.SetActive(false);
    		StartCoroutine("OpenAnimation");
    	}//if

    	if (canSit && anim.GetBool("isMoving") == false && pickup == false)
    	{
    		if (isSitting == false && Input.GetKeyDown(KeyCode.S))
    		{
    			//modifying the Vector3
        		currentEulerAngles = new Vector3(0.0f, sitDirection, 0.0f);
        		//apply the change to the gameObject
        		transform.eulerAngles = currentEulerAngles;

    			// anim.SetFloat("sitAction", Random.Range(0.0f, 1.0f));
    			anim.SetBool("sitDown", true);
    			isSitting = true;
    		}//if
    	}//if

    	if (isSitting)
    	{
            popUp.SetActive(true);
    		interact.text = "Reminiscing...";
    		//spawn.SetActive(false);
    		canMove = false;
            canShoot = false;

            if (Input.GetKeyDown(KeyCode.W))
            {
                Debug.Log("Get up");
                interact.text = "";
                popUp.SetActive(false);
                isSitting = false;
                RecoveryBar.value = 0;
                RecoveryBar.gameObject.SetActive(false);
                anim.SetBool("sitDown", false);
                //spawn.SetActive(true);
                canMove = true;
                canShoot = true;
                return;
            }//if

    		// Debug.Log("hi " + lostItems.transform.childCount);
    		if (lostItems.transform.childCount > 0 && ghost.isHit)
    		{
    			if (RecoveryBar.value == 1)
    			{
    				GameObject item = lostItems.transform.GetChild(0).gameObject;
                    if (map.activeSelf && map1.activeSelf)
                    {
                        int index = map.GetComponent<MapControl>().FindIndex(item.name);
                        map.GetComponent<MapControl>().AddIcon(index);
                        map.GetComponent<MapControl>().WarningColor(false, index);
                    }
                    
    				// Debug.Log(item.name);
    				item.transform.parent = null;
    				item.transform.position = transform.position + transform.forward * 10;
    				Vector3 pos = item.transform.position;
    				pos.y += 10;
    				item.transform.position = pos;
    				// Debug.Log(item.transform.parent);
    				// Debug.Log("bye " + lostItems.transform.childCount);
    				item.SetActive(true);
    				item.GetComponent<Rigidbody>().isKinematic = false;
    				RecoveryBar.value = 0;
    				RecoveryBar.gameObject.SetActive(false);
    			}
    			else
    			{
    				RecoveryBar.gameObject.SetActive(true);
    				// Debug.Log("hi");
    				RecoveryBar.value += Time.deltaTime * 0.5f;
    			}
    		}
    		return;
    	}//if
    }//PlayerInteractions

    public void PickUp()
    {
        //Debug.Log("pick up");
        if (pickup == false)
        {
            if (canPickUp)
            {
                interact.text = "";
                popUp.SetActive(false);
                itemToPick.transform.parent = hand.transform;
                itemToPick.GetComponent<Rigidbody>().isKinematic = true;
                itemToPick.transform.position = itemToPick.transform.parent.position;
                pickup = true;
                canShoot = false;
                canPickUp = false;
            }
        }
        else
        {
            itemToPick.transform.parent = null;
            itemToPick.GetComponent<Rigidbody>().isKinematic = false;
            pickup = false;
            canShoot = true;
        }
        
    }

    void PlayerShoot()
    {
       // Debug.Log("shoot " + canShoot);
        if (canShoot)
        {
            anim.SetTrigger("Shoot");
            soundManager.Shoot();
        }
    	       	
    }//PLayerShoot

    public void Shoot()
    {
    	Rigidbody clone;
    	clone = Instantiate(projectile, spawn.transform.position, Quaternion.identity);

    	clone.velocity = spawn.transform.forward * projectileSpeed;
    }


    //COLLISION DETECTIONS

    void OnTriggerEnter(Collider col)
    {
    	if (col.tag == "canSit")
    	{
            popUp.SetActive(true);
    		interact.text = "Reminisce?";
    		canSit = true;
    		sitDirection = col.gameObject.transform.rotation.eulerAngles.y;    		
    	}//if

    	if (col.tag == "canOpen")
    	{
            popUp.SetActive(true);
    		interact.text = "Open with 'E";
    		canOpen = true;
    		anim1 = col.gameObject.GetComponent<Animator>();
    	}//if

        if (col.tag == "targetItem")
        {
            if (pickup == false)
            {
                popUp.SetActive(true);
                interact.text = "Pick up?";
                canPickUp = true;
                itemToPick = col.gameObject;
            }  
        }

        if (col.tag == "doorway")
        {
            if (isInside)
                isInside = false;
            else
                isInside = true;
        }
        //Debug.Log(col.tag);
        if (col.tag == "Rug")
        {
           // Debug.Log("carpet");
            onRug = true;
        }
    }//OnTriggerEnter

    private void OnTriggerStay(Collider col)
    {
        if (col.tag == "Rug")
        {
            //Debug.Log("carpet");
            onRug = true;
        }
    }//OnTriggerStay


    void OnTriggerExit(Collider col)
    {
        if (pickup == false)
            itemToPick = null;
        interact.text = "";
        popUp.SetActive(false);
    	if (col.tag == "canSit")
    		canSit = false;

    	if (col.tag == "canOpen")
    		canOpen = false;

        if (col.tag == "Rug")
        {
            onRug = false;
        }
    }//OnTriggerExit

    //ANIMATIONS
    IEnumerator OpenAnimation()
    {
    	anim.SetTrigger("Open");

    	yield return new WaitForSeconds(0.1f);

    	if (anim1.GetBool("openDoor") == false)
        {
            anim1.SetBool("openDoor", true);
            soundManager.Open();
        }
		else
        {
            anim1.SetBool("openDoor", false);
            soundManager.Close();
        }

		canMove = true;
    }//OpenAnimation
}//SkeletonControl

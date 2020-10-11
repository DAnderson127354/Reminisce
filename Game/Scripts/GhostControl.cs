using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostControl : MonoBehaviour
{
	public GameObject target;

	public GameObject[] spawners;
	GameObject spawn;

	public GameObject lostItems;
    public GameObject map;
	public GameManager gameManager;

	public float speed;
	public float size;
	public bool isHit = false;

	bool gotTarget = false;
	Renderer rend;
	int index = -1;
	int spawnIndex;

    public SoundManager soundManager;

	void Start()
	{
		rend = GetComponent<Renderer>();
		NewSpawnPoint();
	}

    // Update is called once per frame
    void Update()
    {
    	if (gameManager.gameDone())
    		return;

        if (target != null)
        {
            if (target.transform.parent != this.transform)
                gotTarget = false;
        }

    	if (isHit)
    		return;
    	else
    	{
            //Debug.Log("ok");
    		if (index == -1 || index == 5)
    		{
                //Debug.Log("ok");
    			index = gameManager.CheckforTargets();
                //Debug.Log("index " + index);

                
    			if (index < 5)
    			{
    				target = gameManager.NewTarget(index);
    			}
    		}

    		if (gotTarget)
    		{
                map.GetComponent<MapControl>().WarningColor(true, index);
    			if (transform.position == spawn.transform.position)
    			{
    				NewSpawnPoint();
    				target.SetActive(false);
    				target.transform.parent = lostItems.transform;
                    map.GetComponent<MapControl>().RemoveIcon(index);
    				gotTarget = false;
                    index = -1;
    			}
        		GoTo(spawn.transform);
    		}
        	else
        	{
                if (index < 5)
                {
                   //Debug.Log(target.name);
                    GoTo(target.transform); 
                }
                
        	}
    	}
    }//Update

    void NewSpawnPoint()
    {
    	spawnIndex = Random.Range(0, 3);
		spawn = spawners[spawnIndex];
    }

    void GoTo(Transform target)
    {
    	// Move our position a step closer to the target
        float step =  speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }//GoTo

    void OnTriggerEnter(Collider col)
    {
    	if (col.tag == "projectile")
    	{
    		if (gotTarget)
    		{
    			gotTarget = false;
    			target.gameObject.transform.parent = null;
    			target.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                map.GetComponent<MapControl>().WarningColor(false, index);
    		}
    		StartCoroutine("Hit");
    	}

    	if (col.name == target.name)
    	{
    		gotTarget = true;
    		col.gameObject.transform.parent = this.transform;
    		col.gameObject.GetComponent<Rigidbody>().isKinematic = true;
    	}
    }//OnTriggerEnter

    IEnumerator Hit()
    {
        soundManager.Hit();
        NewSpawnPoint();
    	transform.position = spawn.transform.position;
    	rend.enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
    	isHit = true;

    	yield return new WaitForSeconds(15);

    	rend.enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
    	isHit = false;
        soundManager.GhostSpawn();
        index = gameManager.CheckforTargets();
        if (index < 5)
        {
            target = gameManager.NewTarget(index);
        }
    }//Hit
}

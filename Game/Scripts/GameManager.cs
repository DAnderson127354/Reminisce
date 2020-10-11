using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public GameObject[] targets;

	public Text time;
	public float timer;

	public Text win;
    public Text interaction;
    public GameObject popUp;

	static bool isDone;

	void Start()
	{
		isDone = false;
	}

	void Update()
	{
		if (timer > 0.0f)
			Timer();
		else
		{
			WinConditions();
			isDone = true;
		}

		if (Input.GetKeyDown(KeyCode.Q))
            SceneManager.LoadScene("Menu");
    }

	void Timer()
	{
		timer -= Time.deltaTime;
 
     	string minutes = Mathf.Floor(timer / 60).ToString("00");
     	string seconds = (timer % 60).ToString("00");
     	string display = string.Format("{0}:{1}", minutes, seconds);
     	time.text = "Timer: " + display;
     	//print(string.Format("{0}:{1}", minutes, seconds));
	}

    public GameObject NewTarget(int index)
    {
    	//Debug.Log("I am being called");
    	GameObject target = targets[index];
    	//Debug.Log(targets[index].name);
    	return target;
    }

    public int CheckforTargets()
    {
        List<int> temp = new List<int>();

    	for (int i = 0; i < targets.Length; i++)
    	{
    		if (targets[i].transform.parent == null)
            {
                Debug.Log(targets[i].name + " " + i);
                temp.Add(i);
            }
    	}

        Debug.Log("temp " + temp.Count);
        int index = Random.Range(0, temp.Count-1);
        if (temp.Count > 0)
            return temp[index];

    	return targets.Length;
    }

    void WinConditions()
    {
    	time.text = "Timer: 00:00";
    	int player = 0;
    	int ghost = 0;

    	for (int i = 0; i < targets.Length; i++)
    	{
    		if (targets[i].transform.parent == null)
    			player++;
    		else
    			ghost++;
    	}
        popUp.SetActive(true);
        interaction.text = "";
    	if (player > ghost)
    		win.text = "Player wins";
    	else
    		win.text = "Ghost wins";

        win.text += "\n" + "Press Q to Quit";
    }

    public bool gameDone()
    {
    	return isDone;
    }
}

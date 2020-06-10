using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour {

 //   [SerializeField] bool EndAfterFinish = false;

	DataSaver dS;
	Room rm;

	void Start()
	{
		dS = GameObject.Find ("DataMaster").GetComponent<DataSaver>();

		rm = GetComponentInParent<Room>();
	}

	void OnTriggerEnter()
	{
   //     if (EndAfterFinish)
        
        
     //   else
        
          	 dS.SaveData(rm.GetDungeonSeed(), rm.GetIsShrinking());


		PlayerPrefs.SetInt ("lvl", PlayerPrefs.GetInt("lvl") +1 );
		if (PlayerPrefs.GetInt ("lvl") == 4)
		{
			Application.Quit ();
		}
		else 
		{
			SceneManager.LoadScene (1);
		}
	}

}

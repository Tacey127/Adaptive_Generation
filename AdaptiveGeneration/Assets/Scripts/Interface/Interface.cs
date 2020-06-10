using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Interface : MonoBehaviour {

    [SerializeField] GameObject deleteone;

    [SerializeField] Text InputText;
    [SerializeField] Text OutputText;
    //begins the experiment by loading the dungeon scene
    public void BeginExperiment()
    {
        SceneManager.LoadScene(1);
    }

    public void ChangeParticipantInputfield()
    {
		PlayerPrefs.SetInt ("lvl", 0);
        OutputText.text = InputText.text;

        Destroy(deleteone);

    }

}

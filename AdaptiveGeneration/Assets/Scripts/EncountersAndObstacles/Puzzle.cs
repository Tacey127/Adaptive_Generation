using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour {

    Renderer rend;
    Material pMaterial;
    [SerializeField] bool StartActive = false;

    [SerializeField] List<Puzzle> pieces = new List<Puzzle>();

    [SerializeField] PuzzleMaster master;

    public bool active = false;

    void Start()
    {
        rend = gameObject.GetComponent<Renderer>();
        pMaterial = rend.material;
        if (Random.value < 0.5f)
        {
            activate();
        }
        else
        {
            pMaterial.color = Color.black;
            
        }
    }

    void ActivateNeighbours()
    {
        foreach (Puzzle piece in pieces)
        {
            piece.activate();
        }
    }

    public void activate()
    {
        active = !active;
        if (active)
        {
            pMaterial.color = Color.green;
        }
        else
        {
            pMaterial.color = Color.black;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ActivateNeighbours();
            activate();
            master.Check();
        }
    }


}

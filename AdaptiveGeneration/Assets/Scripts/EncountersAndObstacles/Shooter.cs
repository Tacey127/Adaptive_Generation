using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

    // Use this for initialization
	//if the bullet repeats itself
    [SerializeField] bool repeating = false;
    [SerializeField] float reloadTime = 1;

	[SerializeField] Vector3 bulletDirection;

	[SerializeField] Bullet bullet;

	void Start () {
        StartCoroutine(FireBullet());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator FireBullet()
    {
        do
        {
			Bullet bulletObject = Instantiate (bullet, transform);
			bulletObject.direction = bulletDirection;
             yield return new WaitForSeconds(reloadTime);

        } while (repeating);
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    [SerializeField] Room parentRoom;

    Vector3 rotate = new Vector3(0, 1, 0);

    void Start()
    {
        rotate = new Vector3(0, Random.value, Random.value);
    }
    void Update()
    {
        transform.Rotate(rotate, 90 * Time.deltaTime);
    }
    void OnTriggerEnter()
    {
        parentRoom.ReduceObjectives();
        Destroy(gameObject);
    }
}

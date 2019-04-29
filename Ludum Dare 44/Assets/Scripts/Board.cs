using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Rotate"))
            Rotate();
    }

    private void Rotate()
    {
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - 90);

        foreach (var block in GetComponentsInChildren<Block>())
        {
            block.transform.rotation = Quaternion.Euler(0, 0, block.transform.rotation.eulerAngles.z + 90);
        }
    }

    //public void MakeBlocksFall()
    //{
    //    foreach (var block in GetComponentsInChildren<Block>())
    //    {
    //        if (!block.GetNeighbor(Vector3.down))
    //            block.StartFalling();
    //    }
    //}
}

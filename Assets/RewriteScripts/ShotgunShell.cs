using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunShell : MonoBehaviour
{
    //Numero de tiros menores para spawnar
    public int peletCount = 5;
    //angulo máximo do "spread" do tiro
    public float rotationOffset = 10f;
    //"Pellet": tiro a ser instanciado
    public GameObject pellet;
    
    protected void Start()
    {
        System.Random rng = new System.Random();
        Quaternion rotation = Quaternion.Euler(new Vector3((float)rng.NextDouble() * rotationOffset, (float)rng.NextDouble() * rotationOffset, (float)rng.NextDouble() * rotationOffset));

        for(int i = 0; i < peletCount; i++)
        {
            Instantiate(pellet, transform.position, rotation);
        }
    }


    protected void Update()
    {
        //Destroir esse tiro or por em uma pool
        //Destroy(transform);
    }
}

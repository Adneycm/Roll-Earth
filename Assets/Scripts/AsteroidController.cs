using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AsteroidController : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent asteroid;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            asteroid.SetDestination(player.position);
        }   
    }
}

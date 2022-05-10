using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BorealCharacterController : EnemyCharacterMonoBehaviour
{
    // shock wave object
    // egg object
    // 2 points on the top of the map to travel between to drop egg bomb
    // 3 points in the middle of the map to travel in an arc for shock wave
    // 1 point that is the player's transform to dive towards. 

    [SerializeField]
    private GameObject Egg;
    [SerializeField]
    private GameObject ShockWave;
    [SerializeField]
    private Transform[] FlyPoints;
    private int EggPoint = 0;
    private bool notSpawn = true;
    private bool isDiving = false;
    [SerializeField]
    private Transform[] ShockWavePoints;
    private int ShockPoint = 0;
    [SerializeField]
    private Transform PlayerPoint;

    private Transform BorealBody;
    private System.Random rand;

    private Vector3 vect = Vector3.zero;

    private float currTime;
    private float timeLimit = 3.0f;

    void Start()
    {
        BorealBody = GetComponent<Transform>();
        Physics.IgnoreLayerCollision(3, 3);
        rand = new System.Random();
    }

    void Update()
    {
        currTime += Time.deltaTime;
        FlyDive();
    }

    private void FlyEgg() // move that has the boreal fly between 2 points and spawn eggs in mid air
    {
        // Move the Boreal between 2 points on the screen
        if (EggPoint == 0)
        {
            Move(Vector2.left, 2.0f);
        }
        else if (EggPoint == 1)
        {
            Move(Vector2.right, 2.0f);
        }

        if (BorealBody.position.x <= FlyPoints[0].position.x)
        { // TODO: Make this a for loop, or a seperate function.
            EggPoint = 1;
        }
        else if (BorealBody.position.x >= FlyPoints[1].position.x)
        {
            EggPoint = 0;
        }

        if (notSpawn) // lag spawning of eggs
        {
            notSpawn = false;
            StartCoroutine(DropEgg());
        }
    }

    private IEnumerator DropEgg()
    {
        GameObject eggSpawn = Instantiate(Egg, BorealBody.position, Quaternion.identity);
        yield return new WaitForSeconds(1.5f);
        Destroy(eggSpawn); // TODO : could just make a seperate script for eggs to explode when touching the ground
        notSpawn = true;
    }

    private void FlyDive() // move that has the boreal fly between 2 points and dive towards the player
    {
        if (currTime < timeLimit && !isDiving) // for 3 seconds , move in a loop between two points. 
        // Then dive towards the player for 2 seconds.
        {
            if (EggPoint == 0) // honestly just make this a function
            {
                Move(Vector2.left, 4.0f);
            }
            else if (EggPoint == 1)
            {
                 Move(Vector2.right, 4.0f);
            }
            if (BorealBody.position.x <= FlyPoints[0].position.x)
            { // Make this a for loop, or a seperate function.
                EggPoint = 1;
            }
            else if (BorealBody.position.x >= FlyPoints[1].position.x)
            {
                EggPoint = 0;
            }
            
        }
        else
        {
            currTime = 0;
            transform.position = Vector3.SmoothDamp(transform.position, playerTransform.position, ref vect, .6f);
            FacePlayer();
            if(!isDiving){ // made so there's not millions of coroutines
            StartCoroutine(Diving());
            isDiving = true;
            }
        }
    }


    private IEnumerator Diving()
    {
        yield return new WaitForSeconds(2f);
        isDiving = false;
    }
}

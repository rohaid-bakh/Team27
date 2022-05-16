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
    private GameObject Wave;
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

    private bool isAttack = false;

    private Rigidbody self;
    [SerializeField]
    private AudioManager audio;

    void Start()
    {
        BorealBody = GetComponent<Transform>();
        Physics.IgnoreLayerCollision(3, 3);
        rand = new System.Random();
        self = GetComponent<Rigidbody>();
        
    }

    void Update(){
        currTime += Time.deltaTime;
    }
    

    public void FlyEgg() // move that has the boreal fly between 2 points and spawn eggs in mid air
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
        audio.PlaySoundEffect(EnumSoundName.BorealProjectile);
        yield return new WaitForSeconds(0.75f);
        Destroy(eggSpawn, 1f); // TODO : could just make a seperate script for eggs to explode when touching the ground
        notSpawn = true;
    }

    public void FlyDive() // move that has the boreal fly between 2 points and dive towards the player
    {
        if (currTime < timeLimit && !isDiving) // for 3 seconds , move in a loop between two points. 
        // Then dive towards the player for 2 seconds.
        {
            if (EggPoint == 0) // honestly just make this a function
            {
               transform.position = Vector3.MoveTowards(transform.position, FlyPoints[0].transform.position, Time.deltaTime * 2);
            }
            else if (EggPoint == 1)
            {
                 transform.position = Vector3.MoveTowards(transform.position, FlyPoints[1].transform.position, Time.deltaTime * 2);
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
            transform.position = Vector3.SmoothDamp(transform.position, playerTransform.position, ref vect, .3f);
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

    public void FlyWave(){
        if(ShockPoint == 0){
            transform.position = Vector3.Slerp(transform.position, ShockWavePoints[2].position, Time.deltaTime * 2);
            FlipSprite(1f);
        } else if(ShockPoint == 1){
            transform.position = Vector3.Slerp(transform.position, ShockWavePoints[1].position, Time.deltaTime * 2);
            FlipSprite(1f);
        } else if (ShockPoint == 2){
            transform.position = Vector3.Slerp(transform.position, ShockWavePoints[2].position, Time.deltaTime * 2);
            FlipSprite(-1f);
        } else if (ShockPoint == 3){
            transform.position = Vector3.Slerp(transform.position, ShockWavePoints[0].position, Time.deltaTime * 2);
            FlipSprite(-1f);
        }
        // 0 and 1 should be paused.

         if (BorealBody.position.x >= ShockWavePoints[2].position.x-.5f && ShockPoint == 0)
            { // Make this a for loop, or a seperate function.
                ShockPoint = 1;
            }
            else if (BorealBody.position.x >= ShockWavePoints[1].position.x-.5f && ShockPoint == 1)
            {   FlipSprite(-1f);
                if(!isAttack){
                isAttack = true;
                StartCoroutine(ShockWave(2));
                
                }
                
            }
            else if (BorealBody.position.x <= ShockWavePoints[2].position.x + .5f && ShockPoint == 2 ){
                ShockPoint = 3;
            } else if (BorealBody.position.x <= ShockWavePoints[0].position.x + .5f && ShockPoint == 3 ){
                FlipSprite(1f);
                
                if(!isAttack){
                isAttack = true;
                StartCoroutine(ShockWave(0));
                
                }
            }
    }

    private IEnumerator ShockWave(int shockValue){
        int direct = -1; // default shoot to the right 
        if(shockValue == 2){
            direct = -1;
        } else if (shockValue == 0){
            direct = 1;
        }
            //make this a for loop
            Instantiate(Wave, transform.position, Quaternion.identity).GetComponent<ShockWave>().ProjectileDirection(direct);
            audio.PlaySoundEffect(EnumSoundName.BorealProjectile);
            yield return new WaitForSeconds(.75f);
            Instantiate(Wave, transform.position, Quaternion.identity).GetComponent<ShockWave>().ProjectileDirection(direct);
            audio.PlaySoundEffect(EnumSoundName.BorealProjectile);
            yield return new WaitForSeconds(.75f);
            Instantiate(Wave, transform.position, Quaternion.identity).GetComponent<ShockWave>().ProjectileDirection(direct);
            audio.PlaySoundEffect(EnumSoundName.BorealProjectile);
            yield return new WaitForSeconds(.75f);

        ShockPoint = shockValue;
        isAttack = false;
    }

    public void returnToOrigin(){ // returning everything to the middle of the screen.
        ShockPoint = 0;
        EggPoint = 0;

        transform.position = Vector3.SmoothDamp(transform.position, ShockWavePoints[2].position, ref vect, .3f);;
    }

    public bool atOrigin(){
        if(((transform.position.x >= ShockWavePoints[2].position.x - .2f) && 
        (transform.position.y >= ShockWavePoints[2].position.y - .2f)) || 
        ((transform.position.x <= ShockWavePoints[2].position.x + .2f)&& 
        (transform.position.y >= ShockWavePoints[2].position.y - .2f))){
            return true;
        } else {
            return false;
        }
    }

}

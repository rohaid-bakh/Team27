using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
   private Transform[] EggBombPoints;
   private int EggPoint = 0;
   [SerializeField]
   private Transform[] ShockWavePoints;
   private int ShockPoint = 0;
   [SerializeField]
   private Transform PlayerPoint;

   private Transform BorealBody;

   void Awake(){
       BorealBody = GetComponent<Transform>();
   }

   void Update(){
       FlyEgg();
   }

   private void FlyEgg(){
       if(BorealBody.position == EggBombPoints[0].position){ // Make this a for loop, or a seperate function.
           EggPoint = 1;
       } else if (BorealBody.position == EggBombPoints[1].position){
           EggPoint = 0;
       }
       Move(Vector2.left, 2.0f);
    //    if (EggPoint == 0){
    //        Vector3.MoveTowards(BorealBody.position, EggBombPoints[EggPoint].position, 1.0f);
    //    } else if (EggPoint == 1){
    //        Vector3.MoveTowards(BorealBody.position, EggBombPoints[EggPoint].position, 1.0f);
    //    }
   }
}

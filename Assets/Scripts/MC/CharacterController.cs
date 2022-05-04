using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Control controller;
    private Vector2 move = new Vector2();
    private Rigidbody _rigid;
    private static Vector3 velocity = Vector3.zero;
    [Range(30, 50)]
    [SerializeField]
    private int speed;
    [Range(.1f, .5f)]
    [SerializeField]
    private float movementSmoothing;

    //TODO: add a flip 
    private void Awake(){

        // turn on the input system and its subsets immediately 
        controller = new Control();
        controller.Enable();
        controller.Move.Enable();
        controller.Move.Move.Enable();

        _rigid = GetComponent<Rigidbody>();
    }
    
    private void OnEnable() {
        controller.Enable();
        controller.Move.Enable();
        controller.Move.Move.Enable();
    }

    private void OnDisable() {

        // important to disable input system when the script/character is set to unactive
        controller.Disable();
        controller.Move.Disable();
        controller.Move.Move.Disable();
    }

    // Fixed Update is used instead of Update because of physics calculations
   private void FixedUpdate() {
       move = controller.Move.Move.ReadValue<Vector2>() * speed;
       Vector3 targetVelocity = new Vector3(move.x*Time.fixedDeltaTime*speed, _rigid.velocity.y, move.y * speed * Time.fixedDeltaTime);
       _rigid.velocity = Vector3.SmoothDamp(_rigid.velocity, targetVelocity, ref velocity , movementSmoothing);

   }
}

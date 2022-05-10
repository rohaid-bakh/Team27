using System;
using System.Collections;
using UnityEngine;

public class TestEnemyCharacterController : CharacterMonoBehaviour
{
    // could have a multiple attacks, just need to serialize and add them here
    [SerializeField] TestEnemyAttack1 attack1;

    private void Start()
    {
        try
        {
            StartCoroutine(EnemyAIBehaviour());
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    
    private IEnumerator EnemyAIBehaviour()
    {
        // loop until enemy is dead
        while(IsDead() == false)
        {
            // move to the left for 2 seconds
            Move(Vector2.left);
            yield return new WaitForSeconds(2f);

            yield return new WaitForSeconds(0.5f);

            Attack(attack1);

            yield return new WaitForSeconds(1f);

            // move to the right for 2 seconds
            Move(Vector2.right);
            yield return new WaitForSeconds(2f);

            // stop moving
            Move(Vector2.zero);

            // jump
            Jump();

            yield return new WaitForSeconds(1f);

            // move faster than base speed (twice as fast) for half a second
            float speedModifier = 2; // double speed
            Move(Vector2.right, speedModifier);

            yield return new WaitForSeconds(0.5f);
        }
    }

}
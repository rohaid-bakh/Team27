using System;
using System.Collections;
using UnityEngine;

public class MagogCharacterController : CharacterMonoBehaviour
{
    // could have a multiple attacks, just need to serialize and add them here
    MagogAttack1 attack1;
    MagogAttack2 attack2;

    private void Start()
    {
        try
        {
            attack1 = GetComponentInChildren<MagogAttack1>();
            attack2 = GetComponentInChildren<MagogAttack2>();
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

            yield return new WaitForSeconds(1f);

            Attack(attack2);

            // move faster than base speed (twice as fast) for half a second
            float speedModifier = 2; // double speed
            Move(Vector2.right, speedModifier);

            yield return new WaitForSeconds(0.5f);
        }
    }

}
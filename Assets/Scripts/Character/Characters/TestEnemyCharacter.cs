﻿using System.Collections;
using UnityEngine;

public class TestEnemyCharacter : CharacterMonoBehaviour
{
    private void Start()
    {
        StartCoroutine(EnemyAIBehaviour());
    }

    private IEnumerator EnemyAIBehaviour()
    {
        // move to the left for 2 seconds
        Move(Vector2.left);
        yield return new WaitForSeconds(2f);

        Move(Vector2.zero);

        // jump
        Jump();

        yield return new WaitForSeconds(1f);

        // move left
        Move(Vector2.left);
        yield return new WaitForSeconds(2f);

        // jump
        Jump();

        // move to the right for 2 seconds
        Move(Vector2.right);
        yield return new WaitForSeconds(2f);

        Move(Vector2.zero);
    }
}
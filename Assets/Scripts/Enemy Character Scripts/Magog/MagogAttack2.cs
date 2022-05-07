﻿using System.Collections;
using UnityEngine;

// This attack involves firing projectiles at the character
public class MagogAttack2 : MonoBehaviour, IAttack
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawn;
    [SerializeField] float timeBetweenProjectiles = 0.75f;

    MagogCharacterController characterController;

    public EnumCharacterAnimationStateName? GetAnimationStateName() => EnumCharacterAnimationStateName.Attack2;
    public EnumSoundName? GetSoundEffectName() => null; //no sound effect for this attack, sound effects will be applied to projectiles

    void Start()
    {
        characterController = GetComponentInParent<MagogCharacterController>();
    }

    public void Attack()
    {
        // fires projectiles
        StartCoroutine(FireProjectiles());
    }

    IEnumerator FireProjectiles()
    {
        // keep firing when in attack state (basically lastts until animation is finished)
        while (characterController.IsAttacking())
        {
            yield return new WaitForSeconds(timeBetweenProjectiles);

            InstantiateProjectile();
        }
    }

    public void InstantiateProjectile()
    {
        GameObject projectileInstance = Instantiate(projectilePrefab, projectileSpawn.position, transform.parent.rotation);

        // set direction to shoot projectiles (same as parent)
        projectileInstance.GetComponent<ProjectileBehaviour>().projectileDirection = transform.parent.localScale.x;

        // sound effect
        AudioManager.instance?.PlaySoundEffect(EnumSoundName.MagogProjectile);
    }
}
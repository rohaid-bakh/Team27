using System.Collections;
using UnityEngine;

// This attack involves firing projectiles at the character
public class MagogAttack2 : MonoBehaviour, IAttack
{
    // projectile
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawn;

    // projectile force
    [SerializeField] float shootForce;
    [SerializeField] float upwardForce;

    public EnumCharacterAnimationStateName? GetAnimationStateName() => EnumCharacterAnimationStateName.Attack2;
    public EnumSoundName? GetSoundEffectName() => null; //no sound effect for this attack, sound effects will be applied to projectiles

    public void Attack()
    {
        // fires projectile
        InstantiateProjectile();
    }

    public GameObject InstantiateProjectile()
    {
        GameObject projectileInstance = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);

        // set direction to shoot projectiles (same as parent)
        ProjectileBehaviour projectileBehaviour = projectileInstance.GetComponent<ProjectileBehaviour>();
        projectileBehaviour.direction = transform.parent.localScale.x;
        projectileBehaviour.moveSpeed = shootForce;
        projectileBehaviour.jumpSpeed = upwardForce;
        projectileBehaviour.Throw();

        // sound effect
        AudioManager.instance?.PlaySoundEffect(EnumSoundName.MagogProjectile);

        return projectileInstance;
    }
}
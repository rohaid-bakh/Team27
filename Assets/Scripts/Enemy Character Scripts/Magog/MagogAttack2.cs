using System.Collections;
using UnityEngine;

// This attack involves firing projectiles at the character
public class MagogAttack2 : MonoBehaviour, IAttack
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawn;

    public EnumCharacterAnimationStateName? GetAnimationStateName() => EnumCharacterAnimationStateName.Attack2;
    public EnumSoundName? GetSoundEffectName() => null; //no sound effect for this attack, sound effects will be applied to projectiles

    public void Attack()
    {
        // fires projectile
        InstantiateProjectile();
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
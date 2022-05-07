using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField] int damageAmount = 2;
    [SerializeField] Vector3 Speed;

    Rigidbody rigidbody;

    bool forceApplied = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // for physics
    void FixedUpdate()
    {
        if (!forceApplied)
        {
            forceApplied = true;
            float direction = transform.parent.localScale.x; // what direction to shoot projectiles (same as parent)
            Vector3 force = new Vector3(Speed.x * direction, Speed.y, Speed.z); // create force with speed
            rigidbody?.AddForce(force, ForceMode.Impulse);
        }
    }

    // deal damage to player if hit. Destroy after 1 second
    private IEnumerator OnCollision(Collider other)
    {
        CharacterMonoBehaviour playerCharacter = other.gameObject.GetComponent<CharacterMonoBehaviour>();
        if (playerCharacter != null)
        {
            playerCharacter.TakeDamage(damageAmount);
        }

        //TODO: add some sort of particle effect or something
        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // can't collide with enemies
        if (other.gameObject.layer != LayerMask.NameToLayer("Enemy"))
        {
            StartCoroutine(OnCollision(other));
        }
    }
}

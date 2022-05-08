using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField] Rigidbody rigidbody;

    [SerializeField] int damageAmount = 2;
    [SerializeField] float maxLifeTime;

    public float moveSpeed { get; set; }
    public float jumpSpeed { get; set; }
    public float direction { get; set; }

    private void Start()
    {
        StartCoroutine(WaitAndDestroy());
    }

    void Update()
    {
        rigidbody.velocity = new Vector2(moveSpeed * direction, rigidbody.velocity.y);
    }

    public void Throw()
    {
        rigidbody.velocity += new Vector3(0f, jumpSpeed, 0f);
    }

    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(maxLifeTime);

        Destroy(gameObject);
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
        yield return new WaitForSeconds(0.1f);

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

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
        // want to avoid the collision with the boundary colliders & enemy (the player can fall of the map)
        // boundaries are for the enemies
        Collider projectilleCollider = GetComponent<Collider>();
        int ignoreBoundariesLayer = LayerMask.NameToLayer("Boundaries");
        int ignoreEnemyLayer = LayerMask.NameToLayer("Enemy");
        Collider[] colliders = FindObjectsOfType<Collider>();
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == ignoreBoundariesLayer || collider.gameObject.layer == ignoreEnemyLayer)
            {
                Physics.IgnoreCollision(projectilleCollider, collider, ignore: true);
            }
        }
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
        // collides with player or ground
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log($"Collision with {LayerMask.LayerToName(other.gameObject.layer)}");
            StartCoroutine(OnCollision(other));
        }
    }
}

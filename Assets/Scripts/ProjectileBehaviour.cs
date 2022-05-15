using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{

    [SerializeField] int damageAmount = 2;
    float maxLifeTime = 1.5f;

    public float moveSpeed { get; set; }
    public float jumpSpeed { get; set; }
    public float direction { get; set; }

    // throw information
    bool isThrown = false;
    Vector3 startPoint;
    Vector3 endPoint;
    private float startTime; // The time at which the throw animation started.
    private float journeyTime = 1f; // time to move from point a to b in seconds

    Coroutine fadeOutAndDestroy;

    private void Start()
    {
        StartCoroutine(WaitAndDestroy());
    }

    void Update()
    {
        if (isThrown)
        {
            // The center of the arc
            Vector3 center = (startPoint + endPoint) * 0.5F;

            // move the center a bit downwards to make the arc vertical
            center -= new Vector3(0, 1, 0);

            // Interpolate over the arc relative to center
            Vector3 riseRelCenter = startPoint - center;
            Vector3 setRelCenter = endPoint - center;

            // The fraction of the animation that has happened so far is
            // equal to the elapsed time divided by the desired time for
            // the total journey.
            float fracComplete = (Time.time - startTime) / journeyTime;

            transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
            transform.position += center;
        }

        if (Vector3.Distance(transform.position, endPoint) <= 0.5)
        {
            if(fadeOutAndDestroy == null)
                fadeOutAndDestroy = StartCoroutine(FadeOutAndDestroy());
        }
    }

    public void Throw()
    {
        startTime = Time.time;

        startPoint = transform.position;
        endPoint = new Vector3(transform.position.x + (direction * 7), -1.16f, transform.position.z);

        // rotate to face correct direcition
        transform.localScale = new Vector3(Mathf.Sign(direction), transform.localScale.y, transform.localScale.z);

        isThrown = true;
    }

    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(maxLifeTime);

        if (fadeOutAndDestroy == null)
        {
            fadeOutAndDestroy = StartCoroutine(FadeOutAndDestroy());
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
        yield return new WaitForSeconds(0.05f);

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // can't collide with Enemy
        if (other.gameObject.layer != LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log($"Collision with {LayerMask.LayerToName(other.gameObject.layer)}");
            StartCoroutine(OnCollision(other));
        }
    }

    IEnumerator FadeOutAndDestroy()
    {
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Color spriteColor = spriteRenderer.color;

        for (float f = 1f; f >= 0; f -= 0.05f)
        {
            spriteRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, f);

            yield return new WaitForSeconds(0.01f);
        }

        Destroy(gameObject);
    }
}

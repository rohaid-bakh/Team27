using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    bool powerUpActive = false;

    Animator animator;

    PowerUpManager powerUpSpawner;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        powerUpSpawner = FindObjectOfType<PowerUpManager>();

        // play sound effect on appear
        //audioPlayer.PlaySoundEffect(Enum.Sounds.PowerUpAppear);
    }

    public bool IsPowerUpActive()
    {
        return powerUpActive;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && powerUpActive == false)
        {
            powerUpActive = true;

            // hide sprite + play sound effect
            //audioPlayer.PlaySoundEffect(Enum.Sounds.PowerUpGained);
            ChangeAnimationState("PowerUp_Dissapear");

            powerUpSpawner.ActivatePowerUp(gameObject.tag);

            Destroy(gameObject);
        }
    }

    void ChangeAnimationState(string animationName)
    {
        Debug.Log("Playing: " + animationName);

        //play the animation
        animator.Play(animationName);
    }

}
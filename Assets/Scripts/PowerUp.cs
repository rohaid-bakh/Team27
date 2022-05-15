using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] public GameObject powerUpEffect;

    // power up stuff
    PowerUpManager powerUpSpawner;
    bool powerUpActive = false;

    // animations
    Animator animator;
    List<AnimationClip> animations;
    bool introAnimationCompleted = false;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        animations = animator?.runtimeAnimatorController.animationClips.ToList();

    }

    private void Start()
    {
        powerUpSpawner = FindObjectOfType<PowerUpManager>();

        // wait for animation intro to complete
        float? time = GetAnimationClipTime("Arrive");
        if (time != null)
        {
            StartCoroutine(WaitForIntroAnimationToComplete((float)time));
        }

        // sound effect
        AudioManager.instance?.PlaySoundEffect(EnumSoundName.PowerUpAppear);
    }

    private IEnumerator WaitForIntroAnimationToComplete(float animationTime)
    {
        yield return new WaitForSeconds(animationTime*0.8f);

        introAnimationCompleted = true;

        // sound effect to indicate power up is ready
        AudioManager.instance?.PlaySoundEffect(EnumSoundName.PowerUpReady);
    }

    public bool IsPowerUpActive()
    {
        return powerUpActive;
    }

    private void OnTriggerStay(Collider collision)
    {
        // check if collision with player, and power up not already picked up, and power up reached target position
        if (collision.CompareTag("Player") && powerUpActive == false && introAnimationCompleted == true)
        {
            powerUpActive = true;

            // hide sprite + play sound effect
            AudioManager.instance?.PlaySoundEffect(EnumSoundName.PowerUpPickUp);
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

    #region Helper Functions
    /// <summary>
    /// Used to get the duration of a specific animation clip
    /// </summary>
    /// <param name="animationClipName"></param>
    /// <returns></returns>
    public float? GetAnimationClipTime(string animationClipName)
    {
        // animation clip name, should contain a substring of the animation state name (ex. Animation State: Idling; Animation Clip: Player_Idling)
        AnimationClip clip = animations?.FirstOrDefault(animClip => animClip.name.ToUpper().Contains(animationClipName.ToUpper()));

        // check if anmation exists in list
        if (clip != null)
        {
            return clip.length;
        }
        else
        {
            Debug.Log($"{animationClipName} Animation clip not found in animator (from GetAnimationClipTime)");
            foreach (AnimationClip animClip in animations) { Debug.Log(animClip.name + " " + animClip.name.ToUpper().Contains(animationClipName.ToUpper())); }
        }

        return null;
    }

    #endregion
}
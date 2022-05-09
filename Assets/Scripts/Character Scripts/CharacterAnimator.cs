using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    Animator animator;
    public EnumCharacterAnimationStateName currentAnimationState { get; private set; }
    public bool waitingForAnimationToComplete { get; private set; }

    List<AnimationClip> animations;

    #region Awake, Start, etc,
    void Awake()
    {
        waitingForAnimationToComplete = false;
        animator = GetComponent<Animator>();
        animations = animator?.runtimeAnimatorController.animationClips.ToList();
    }

    #endregion

    #region public functions
    /// <summary>
    /// Used to set the animation state and play the animations.
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeAnimationState(EnumCharacterAnimationStateName newState)
    {
        // return if animation doesn't exist 
        if (!DoesAnimationExist(newState)) return;

        // stop if waiting for other animation to complete (only Die state can interrupt animation)
        if (waitingForAnimationToComplete && newState != EnumCharacterAnimationStateName.Die) return;

        //stop the same animation from interuptting itself
        if (currentAnimationState == newState) return;

        //check if we need to wait for animation to complete (ex. if animation doesn't loop, wait for it to complete)
        if (!IsAnimationClipLoopable(newState))
        {
            waitingForAnimationToComplete = true;

            // get animation clip length, and wait x seconds to set wait bool to false
            float? animationClipTime = GetAnimationClipTime(newState);
            Debug.Log($"{newState.ToString()} animation time {animationClipTime}");
            if (animationClipTime != null)
                StartCoroutine(WaitForAnimationToComplete((float)animationClipTime));
            else
                waitingForAnimationToComplete = false;
        }

        //play the animation
        animator.Play(newState.ToString());

        //reassign current state
        currentAnimationState = newState;
    }

    public void SetWaitForAnimationToComplete(bool waitToCompleteAnim)
    {
        this.waitingForAnimationToComplete = waitToCompleteAnim;
    }
    #endregion

    #region private functions
    /// <summary>
    /// Used to get the duration of a specific animation clip
    /// </summary>
    /// <param name="animationState"></param>
    /// <returns></returns>
    public float? GetAnimationClipTime(EnumCharacterAnimationStateName animationState)
    {
        // animation clip name, should contain a substring of the animation state name (ex. Animation State: Idling; Animation Clip: Player_Idling)
        AnimationClip clip = animations?.FirstOrDefault(animClip => animClip.name.ToUpper().Contains(animationState.ToString().ToUpper()));

        // check if anmation exists in list
        if (clip != null)
        {
            return clip.length;
        }
        else
        {
            Debug.Log($"{animationState.ToString()} Animation clip not found in animator (from GetAnimationClipTime)");
            foreach (AnimationClip animClip in animations) { Debug.Log(animClip.name + " " + animClip.name.ToUpper().Contains(animationState.ToString().ToUpper())); }
        }

        return null;
    }

    /// <summary>
    /// Used to check whether a certain animation is loopable
    /// </summary>
    /// <param name="animationState"></param>
    /// <returns></returns>
    bool IsAnimationClipLoopable(EnumCharacterAnimationStateName animationState)
    {
        // animation clip name, should contain a substring of the animation state name (ex. Animation State: Idling; Animation Clip: Player_Idling)
        AnimationClip clip = animations?.FirstOrDefault(animClip => animClip.name.ToUpper().Contains(animationState.ToString().ToUpper()));

        // check if anmation exists in list
        if (clip != null)
        {
            return clip.isLooping;
        }
        else
        {
            Debug.Log($"{animationState.ToString()} Animation clip not found in animator (from IsAnimationClipLoopable)");
        }

        return true;
    }

    /// <summary>
    /// Checks if animation exists in animator
    /// </summary>
    /// <param name="animationState"></param>
    /// <returns></returns>
    bool DoesAnimationExist(EnumCharacterAnimationStateName animationState)
    {
        // animation clip name, should contain a substring of the animation state name (ex. Animation State: Idling; Animation Clip: Player_Idling)
        AnimationClip clip = animations?.FirstOrDefault(animClip => animClip.name.ToUpper().Contains(animationState.ToString().ToUpper()));

        // check if anmation exists in list
        if (clip != null)
        {
            return true;
        }
        else
        {
            Debug.Log($"{animationState.ToString()} Animation clip not found in animator (from DoesAnimationExist())");
        }

        return false;
    }

    /// <summary>
    /// Called when an animation is completed, to set the bool back to true
    /// </summary>
    IEnumerator WaitForAnimationToComplete(float numberOfSeconds)
    {
        yield return new WaitForSeconds(numberOfSeconds);
        waitingForAnimationToComplete = false;
    }
    #endregion

}
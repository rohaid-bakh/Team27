using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    Animator animator;
    public EnumPlayerAnimationState currentAnimationState { get; private set; }
    public bool waitingForAnimationToComplete { get; private set; }

    List<AnimationClip> animations;

    #region Awake, Start, etc,
    void Awake()
    {
        waitingForAnimationToComplete = false;
        animator = GetComponent<Animator>();
        
    }

    void Start()
    {
        SetAnimClipTimesFromAnimator();
    }
    #endregion

    #region public functions
    /// <summary>
    /// Used to set the animation state and play the animations.
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeAnimationState(EnumPlayerAnimationState newState)
    {
        //stop the same animation from interuptting itself
        if (currentAnimationState == newState || waitingForAnimationToComplete) return;

        //if we need to wait for animation to complete 
        if (!IsAnimationClipLoopable(newState))
        {
            waitingForAnimationToComplete = true;

            // get animation clip length, and wait x seconds to set wait bool to false
            float? animationClipTime = GetAnimationClipTime(newState);
            if (animationClipTime != null)
                Invoke("AnimationComplete", (float)animationClipTime);
            else
                waitingForAnimationToComplete = false;
        }

        //play the animation
        animator.Play(newState.ToString());

        //reassign current state
        currentAnimationState = newState;
    }
    #endregion

    #region private functions
    /// <summary>
    /// Used to get the duration of a specific animation clip
    /// </summary>
    /// <param name="animationState"></param>
    /// <returns></returns>
    float? GetAnimationClipTime(EnumPlayerAnimationState animationState)
    {
        AnimationClip clip = animations.FirstOrDefault(animClip => animClip.name == animationState.ToString());

        // check if anmation exists in list
        if (clip != null)
        {
            return clip.length;
        }

        return null;
    }

    /// <summary>
    /// Used to check whether a certain animation is loopable
    /// </summary>
    /// <param name="animationState"></param>
    /// <returns></returns>
    bool IsAnimationClipLoopable(EnumPlayerAnimationState animationState)
    {
        AnimationClip clip = animations.FirstOrDefault(animClip => animClip.name == animationState.ToString());

        // check if anmation exists in list
        if (clip != null)
        {
            return clip.isLooping;
        }

        return true;
    }

    /// <summary>
    /// Creates a dictionary of animation clips & their length for reference
    /// </summary>
    void SetAnimClipTimesFromAnimator()
    {
        animations = animator.runtimeAnimatorController.animationClips.ToList();
    }

    /// <summary>
    /// Called when an animation is completed, to set the bool back to true
    /// </summary>
    void AnimationComplete()
    {
        waitingForAnimationToComplete = false;
    }
    #endregion

}
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Handles sprite animation management.
/// Keeps a list of all sprite animations for the object.
/// </summary>
public class SpriteAnimationManager : MonoBehaviour
{
    [Tooltip("Whether the first animation in the animations list should play when the object is instantiated.")]
    public bool playOnStartup = false;
    [Tooltip("Whether the object this animation manager is on should be disabled after an animation has finished playing.")]
    public bool disableObjectWhenFinished = false;
    [Tooltip("The list of animations for this script to manage.")]
    public List<SpriteAnimation> animations = new List<SpriteAnimation>();
    private SpriteAnimation initialAnimation;
    private SpriteAnimation activeAnimation;
    
    void Awake()
    {
        initialAnimation = animations[0];
        activeAnimation = initialAnimation;

        if (playOnStartup)
        {
            Play(initialAnimation.animationName);
        }
    }

    void Update()
    {
        if (disableObjectWhenFinished)
        {
            if (HasActiveAnimationFinished)
            {
                gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Resume playback of the active animation.
    /// </summary>
    public void Resume()
    {
        activeAnimation.Resume();
    }

    /// <summary>
    /// Start playback of the active animation from its starting frame.
    /// </summary>
    public void Play()
    {
        activeAnimation.Play();
    }
    /// <summary>
    /// Reset the active animation and start playing a new one.
    /// </summary>
    /// <param name="animationName">The name of the animation to play.</param>
    public void Play(string animationName)
    {
        SpriteAnimation animationToPlay = GetAnimation(animationName);
        if (animationToPlay != null)
        {
            activeAnimation = animationToPlay;
            activeAnimation.Play();
        }
        else
        {
            Debug.LogError("Object " + gameObject.name + " does not contain a SpriteAnimation script with the animation name \"" + animationName + "\".");
        }
    }
    /// <summary>
    /// Reset the active animation and start playing a new one.
    /// </summary>
    /// <param name="animationName">The name of the animation to play.</param>
    /// <param name="startingFrame">The frame to start playing the animation from.</param>
    public void Play(string animationName, int startingFrame)
    {
        SpriteAnimation animationToPlay = GetAnimation(animationName);
        if (animationToPlay != null)
        {
            if (startingFrame < animationToPlay.sprites.Length)
            {
                activeAnimation = animationToPlay;
                activeAnimation.Play(startingFrame);
            }
            else
            {
                Debug.LogError("\"" + animationToPlay.animationName + "\" animation does not have " + startingFrame + " frames. Cannot play animation from this point.");
            }
        }
        else
        {
            Debug.LogError("Object " + gameObject.name + " does not contain a SpriteAnimation script with the animation name \"" + animationName + "\".");
        }
    }

    /// <summary>
    /// Freeze the active animation on its current frame.
    /// </summary>
    public void Pause()
    {
        activeAnimation.Pause();
    }

    /// <summary>
    /// Reset the animation manager to the first animation in its animations list.
    /// </summary>
    public void Reset()
    {
        activeAnimation = initialAnimation;
    }

    /// <summary>
    /// Returns the SpriteAnimation connected to the given name.
    /// </summary>
    /// <param name="animationName">The name of the animation to play.</param>
    /// <returns>The animation with the specified name, or null if none is found.</returns>
    private SpriteAnimation GetAnimation(string animationName)
    {
        foreach (SpriteAnimation animation in animations)
        {
            // Compare string names in lowercase to prevent typo errors from 
            if (string.Equals(animation.animationName.ToLower(), animationName.ToLower()))
            {
                return animation;
            }
        }
        
        return null;
    }

    /// <summary>
    /// Whether the active animation has reached the last frame and has stopped playing.
    /// </summary>
    public bool HasActiveAnimationFinished
    {
        get
        {
            return activeAnimation.HasFinishedPlaying;
        }
    }
}

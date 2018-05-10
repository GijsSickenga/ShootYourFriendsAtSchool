using UnityEngine;

/// <summary>
/// Plays a sprite animation at a set framerate.
/// Can play once or loop indefinitely.
/// Can be paused and resumed.
/// </summary>
public class SpriteAnimation : MonoBehaviour
{
    [Tooltip("The name of this animation. Used to play this specific script from this object's sprite animation manager. Must be unique.")]
    public string animationName = "New Animation";

    public enum AnimationState { Playing, Paused, Finished };
    private AnimationState state = AnimationState.Paused;

    public bool loop = false;
    public float frameRate = 12;

    // The amount of seconds a frame is active before going to the next one
    private float frameTime;
    // Timer used to monitor the amount of time the current frame has been active
    private float timer;

    // The current frame of the animation
    private int currentFrame = 0;

    // The animation to play
    [Tooltip("The sequence of sprites in the animation.")]
    public Sprite[] sprites;

    // Sprite on this GameObject
    private SpriteRenderer spriteRenderer;
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        frameTime = 1f / frameRate;
        timer = frameTime;
    }
    
    void Update()
    {
        if (state == AnimationState.Playing)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                int frames = (int)Mathf.Floor(-timer / frameTime);
                // Compensate for Time.deltaTime overshoot by adding instead of setting
                timer += frameTime * frames;

                AdvanceAnimation(frames);
            }
        }
    }

    /// <summary>
    /// Increments the animation by a specified amount of frames. Rewinds and continues playback if looping, otherwise freezes on final frame.
    /// </summary>
    /// <param name="amountOfFrames">The amount of frames to advance.</param>
    void AdvanceAnimation(int amountOfFrames)
    {
        currentFrame += amountOfFrames;

        // Check whether the end of the animation has been reached
        if (currentFrame >= sprites.Length)
        {
            if (loop)
            {
                // Animation wraps back to start
                SkipToFrame(currentFrame - sprites.Length);
            }
            else
            {
                // Animation freezes on the final frame
                SkipToFrame(sprites.Length - 1);
                // No need to compensate for Time.deltaTime overshoot anymore if we're not looping, better to reset to full frameTime
                timer = frameTime;
                // Switch to final state
                state = AnimationState.Finished;
            }
        }
        
        ChangeSprite(currentFrame);
    }

    /// <summary>
    /// Change sprite to the specified one in the animation.
    /// </summary>
    /// <param name="frame">The frame of the animation to change the sprite to.</param>
    void ChangeSprite(int frame)
    {
        spriteRenderer.sprite = sprites[frame];
    }

    /// <summary>
    /// Change sprite to current frame and resume animation playback from there.
    /// </summary>
    public void Resume()
    {
        state = AnimationState.Playing;
        ChangeSprite(currentFrame);
    }

    /// <summary>
    /// Change sprite to initial frame and start animation playback from there.
    /// </summary>
    public void Play()
    {
        Rewind();
        Resume();
    }
    /// <summary>
    /// Change sprite to given frame and start animation playback from there.
    /// </summary>
    /// <param name="frame">The frame to start playing from.</param>
    public void Play(int frame)
    {
        state = AnimationState.Playing;
        SkipToFrame(frame);
    }

    /// <summary>
    /// Freeze animation playback on the current frame.
    /// </summary>
    public void Pause()
    {
        state = AnimationState.Paused;
    }

    /// <summary>
    /// Skip the animation to the specified frame.
    /// </summary>
    /// <param name="frame">The frame to skip to.</param>
    void SkipToFrame(int frame)
    {
        currentFrame = frame;
        ChangeSprite(currentFrame);
    }

    /// <summary>
    /// Rewind to the starting frame of the animation.
    /// </summary>
    public void Rewind()
    {
        SkipToFrame(0);
    }

    /// <summary>
    /// Whether the animation is currently playing.
    /// </summary>
    public bool IsPlaying
    {
        get
        {
            return state == AnimationState.Playing;
        }
    }

    /// <summary>
    /// Whether the animation has reached the last frame and has stopped playing.
    /// </summary>
    public bool HasFinishedPlaying
    {
        get
        {
            return state == AnimationState.Finished;
        }
    }
}

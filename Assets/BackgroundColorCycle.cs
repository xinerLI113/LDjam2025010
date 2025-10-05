using UnityEngine;

public class BackgroundColorCycle : MonoBehaviour
{
    [Header("Color Cycle Settings")]
    [Tooltip("Define your color spectrum here")]
    public Gradient colorGradient;
    
    [Header("Time Settings")]
    [Tooltip("Duration of one complete cycle in seconds")]
    public float cycleDuration = 60f;
    
    [Tooltip("Starting position in the cycle (0-1)")]
    [Range(0f, 1f)]
    public float startingTime = 0f;
    
    [Tooltip("Speed multiplier for the cycle")]
    public float timeSpeed = 1f;
    
    [Header("Options")]
    [Tooltip("Should the cycle pause?")]
    public bool isPaused = false;
    
    [Tooltip("Loop the cycle")]
    public bool loop = true;
    
    private SpriteRenderer spriteRenderer;
    private float currentTime;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer == null)
        {
            Debug.LogError("BackgroundColorCycle requires a SpriteRenderer component!");
            enabled = false;
            return;
        }
        
        currentTime = startingTime;
        UpdateColor();
    }
    
    void Update()
    {
        if (isPaused) return;
        
        currentTime += (Time.deltaTime / cycleDuration) * timeSpeed;
        
        if (loop)
        {
            currentTime %= 1f;
        }
        else
        {
            currentTime = Mathf.Clamp01(currentTime);
        }
        
        UpdateColor();
    }
    
    void UpdateColor()
    {
        if (spriteRenderer != null && colorGradient != null)
        {
            Color newColor = colorGradient.Evaluate(currentTime);
            spriteRenderer.color = newColor;
        }
    }
    
    public void SetTime(float normalizedTime)
    {
        currentTime = Mathf.Clamp01(normalizedTime);
        UpdateColor();
    }
    
    public float GetCurrentTime()
    {
        return currentTime;
    }
    
    public void Pause()
    {
        isPaused = true;
    }
    
    public void Resume()
    {
        isPaused = false;
    }
}
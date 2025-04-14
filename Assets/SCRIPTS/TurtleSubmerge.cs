using UnityEngine;

public class TurtleSubmerge : MonoBehaviour
{
    private float originalY;
    private Quaternion originalRotation;
    private Renderer[] renderers;
    private Color[] originalColors;

    [Header("Submerge Settings")]
    public float submergeDepth = 1f;
    public float submergeDownTime = 1f;
    public float stayUnderwaterTime = 2f;
    public float submergeUpTime = 1f;
    private float waitTime;  // Randomized wait time before submerge
    [Header("Submerge Control")]
    public bool canSubmerge = true; // gets randomly set when spawned
    [Range(0f, 1f)] public float submergeChance = 0.5f; // 0.5 = 50% chance

    private enum SubmergeState
    {
        Waiting,
        Warning,
        GoingDown,
        Underwater,
        ComingUp
    }

    private SubmergeState state = SubmergeState.Waiting;
    private float stateTimer = 0f;
    private float lerpStartY;
    private float lerpEndY;
    private float lerpDuration;

    void Start()
    {
        // Randomly decide if this turtle will submerge
        canSubmerge = Random.value < submergeChance;

        if (!canSubmerge)
        {
            // Disable this script if it's not allowed to submerge
            enabled = false;
            return;
        }

        originalY = transform.position.y;
        originalRotation = transform.rotation;

        renderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].material.color;
        }

        SetNewWaitTime();
    }


    void Update()
    {
        stateTimer += Time.deltaTime;

        switch (state)
        {
            case SubmergeState.Waiting:
                if (stateTimer >= waitTime - 1f)
                {
                    SetColor(Color.red); // Flash red before submerge
                    state = SubmergeState.Warning;
                    stateTimer = 0f;
                }
                break;

            case SubmergeState.Warning:
                if (stateTimer >= 1f)
                {
                    StartSubmerge(originalY, originalY - submergeDepth, submergeDownTime, SubmergeState.GoingDown);
                }
                break;

            case SubmergeState.GoingDown:
                SubmergeLerp();
                if (stateTimer >= lerpDuration)
                {
                    state = SubmergeState.Underwater;
                    stateTimer = 0f;
                }
                break;

            case SubmergeState.Underwater:
                if (stateTimer >= stayUnderwaterTime)
                {
                    StartSubmerge(originalY - submergeDepth, originalY, submergeUpTime, SubmergeState.ComingUp);
                }
                break;

            case SubmergeState.ComingUp:
                SubmergeLerp();
                if (stateTimer >= lerpDuration)
                {
                    ResetColor();
                    transform.position = new Vector3(transform.position.x, originalY, transform.position.z);
                    transform.rotation = originalRotation;

                    SetNewWaitTime();
                    state = SubmergeState.Waiting;
                    stateTimer = 0f;
                }
                break;
        }
    }

    void StartSubmerge(float fromY, float toY, float duration, SubmergeState nextState)
    {
        lerpStartY = fromY;
        lerpEndY = toY;
        lerpDuration = duration;
        state = nextState;
        stateTimer = 0f;
    }

    void SubmergeLerp()
    {
        float t = Mathf.Clamp01(stateTimer / lerpDuration);
        float newY = Mathf.Lerp(lerpStartY, lerpEndY, t);
        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x, newY, pos.z);
    }

    void SetNewWaitTime()
    {
        waitTime = Random.Range(5f, 8f);
    }

    void SetColor(Color color)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = color;
        }
    }

    void ResetColor()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = originalColors[i];
        }
    }
}

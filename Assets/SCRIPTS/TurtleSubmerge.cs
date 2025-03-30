/*using UnityEngine;
using System.Collections;

public class TurtleSubmerge : MonoBehaviour
{
    private float originalY;
    private Quaternion originalRotation; // Save the rotation
    private bool isSubmerging = false;

    [Header("Submerge Settings")]
    public float submergeDepth = 1f;
    public float submergeDownTime = 1f;
    public float stayUnderwaterTime = 2f;
    public float submergeUpTime = 1f;


    private Renderer[] renderers;             // all renderers in object + children
    private Color[] originalColors;           // original colors for reset

    void Start()
    {
        originalY = transform.position.y;
        originalRotation = transform.rotation; // Save original rotation

        // Cache renderers and original colors
        renderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].material.color;
        }

        StartCoroutine(SubmergeCycle());
    }

    IEnumerator SubmergeCycle()
    {
        while (true)
        {
            float waitBeforeSubmerge = Random.Range(3f, 6f);

            // Wait up until 1 second before submerge
            yield return new WaitForSeconds(waitBeforeSubmerge - 1f);

            // Change to red 1 second before submerging
            SetColor(Color.red);

            yield return new WaitForSeconds(1f);

            isSubmerging = true;

            // Go down
            yield return MoveY(originalY, originalY - submergeDepth, submergeDownTime);

            // Stay under
            yield return new WaitForSeconds(stayUnderwaterTime);

            // Go back up
            yield return MoveY(originalY - submergeDepth, originalY, submergeUpTime);

            // Reset position + rotation (forcefully)
            transform.position = new Vector3(transform.position.x, originalY, transform.position.z);
            transform.rotation = originalRotation;

            // Restore original color
            ResetColor();

            isSubmerging = false;
        }
    }

    IEnumerator MoveY(float startY, float endY, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newY = Mathf.Lerp(startY, endY, elapsed / duration);

            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x, newY, pos.z);

            yield return null;
        }

        transform.position = new Vector3(transform.position.x, endY, transform.position.z);
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
*/
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

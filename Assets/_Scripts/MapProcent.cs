using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapProgress : MonoBehaviour
{
    public Transform player;
    public Transform startPoint;
    public Transform endPoint;
    public TextMeshProUGUI progressText;

    private float startYPosition;
    private float totalYDistance;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        startYPosition = startPoint.position.y;
        totalYDistance = endPoint.position.y - startYPosition;
    }

    void Update()
    {
        // Calculate progress percentage
        float currentProgress = (player.position.y - startYPosition) / totalYDistance;

        // Clamp progress between 0 and 100%
        currentProgress = Mathf.Clamp01(currentProgress) * 100f;

        // Update UI text
        if (progressText != null)
        {
            progressText.text = string.Format("{0:0}%", currentProgress);
        }

        // Optionally log to console
        Debug.Log("Progress: " + currentProgress.ToString("0") + "%");
    }
}
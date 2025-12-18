using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SegmentedXPBar : MonoBehaviour
{
    public LevelSystem levelScript;
    public GameObject segmentPrefab; 
    public Color activeColor = Color.cyan;
    public Color inactiveColor = new Color(0.2f, 0.2f, 0.2f, 1f);

    public float xpPerSegment = 10f; 
    private List<Image> segments = new List<Image>();

    void Update()
    {
        if (levelScript == null) return;

        int requiredSegments = Mathf.CeilToInt(levelScript.xpToNextLevel / xpPerSegment);

        UpdateSegmentCount(requiredSegments);

        float currentXp = levelScript.currentXp;
        for (int i = 0; i < segments.Count; i++)
        {
            float segmentStartBound = i * xpPerSegment;

            if (currentXp >= segmentStartBound + xpPerSegment)
                segments[i].color = activeColor; 
            else if (currentXp > segmentStartBound)
                segments[i].color = Color.Lerp(inactiveColor, activeColor, (currentXp - segmentStartBound) / xpPerSegment); // Parcial
            else
                segments[i].color = inactiveColor;
        }
    }

    void UpdateSegmentCount(int count)
    {
        while (segments.Count < count)
        {
            GameObject newSeg = Instantiate(segmentPrefab, transform);
            segments.Add(newSeg.GetComponent<Image>());
        }
        while (segments.Count > count)
        {
            Destroy(segments[segments.Count - 1].gameObject);
            segments.RemoveAt(segments.Count - 1);
        }
    }
}
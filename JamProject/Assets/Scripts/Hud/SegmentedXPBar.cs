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
    public int maxPossibleSegments = 100; 

    private List<Image> segments = new List<Image>();

    void Start()
    {
        for (int i = 0; i < maxPossibleSegments; i++)
        {
            GameObject newSeg = Instantiate(segmentPrefab, transform);
            newSeg.SetActive(false); 
            segments.Add(newSeg.GetComponent<Image>());
        }
    }

    void Update()
    {
        if (levelScript == null || segments.Count == 0) return;

        int requiredSegments = Mathf.CeilToInt(levelScript.xpToNextLevel / xpPerSegment);

        int limit = Mathf.Min(requiredSegments, segments.Count);

        float currentXp = levelScript.currentXp;

        for (int i = 0; i < segments.Count; i++)
        {
            if (i < limit)
            {
                segments[i].gameObject.SetActive(true);
                float segmentStartBound = i * xpPerSegment;

                if (currentXp >= segmentStartBound + xpPerSegment)
                    segments[i].color = activeColor;
                else if (currentXp > segmentStartBound)
                    segments[i].color = Color.Lerp(inactiveColor, activeColor, (currentXp - segmentStartBound) / xpPerSegment);
                else
                    segments[i].color = inactiveColor;
            }
            else
            {
                segments[i].gameObject.SetActive(false);
            }
        }
    }
}
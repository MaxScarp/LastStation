using System.Collections.Generic;
using UnityEngine;

public class BulletContainer : MonoBehaviour
{
    [SerializeField] private float timerMax = 15.0f;

    private int currentChildAmount;

    private List<Transform> bulletTransformList;
    private List<float> timerList;

    private void Awake()
    {
        timerList = new List<float>();
        bulletTransformList = new List<Transform>();
    }

    private void Update()
    {
        if (currentChildAmount != transform.childCount)
        {
            currentChildAmount = transform.childCount;
            foreach (Transform bulletTransform in transform)
            {
                timerList.Add(timerMax);
                bulletTransformList.Add(bulletTransform);
            }
        }

        for (int i = 0; i < bulletTransformList.Count; i++)
        {
            if (bulletTransformList[i])
            {
                timerList[i] -= Time.deltaTime;
                if (timerList[i] <= 0.0f)
                {
                    Destroy(bulletTransformList[i].gameObject);

                    bulletTransformList.RemoveAt(i);
                    timerList.RemoveAt(i);
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestruct : MonoBehaviour
{
    public float duration;
    protected float dur;

    protected void OnEnable()
    {
        dur = duration;
    }

    protected void Update()
    {
        if (dur > 0)
        {
            dur -= Time.deltaTime;
            if (dur < 0)
                gameObject.SetActive(false);
        }
    }
    public static void AddDestruct(GameObject obj, float time)
    {
        AutoDestruct autoDestruct = obj.GetComponent<AutoDestruct>();
        if (autoDestruct == null)
            autoDestruct = obj.AddComponent<AutoDestruct>();
        autoDestruct.duration = time;
    }

}

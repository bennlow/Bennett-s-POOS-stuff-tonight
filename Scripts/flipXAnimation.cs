using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flipXAnimation : MonoBehaviour
{
    private int i = 1;
    public int animSpeed = 60;

    // Update is called once per frame
    void Update()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        i++;
        if (i % animSpeed == 0) {
            currentScale.x *= -1;
        }
        gameObject.transform.localScale = currentScale;
    }

}

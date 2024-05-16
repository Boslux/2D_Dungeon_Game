using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMessage : MonoBehaviour
{
    public void TextStatus()
    {
            StartCoroutine(GetFalse(3f));
    }

    IEnumerator GetFalse(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}

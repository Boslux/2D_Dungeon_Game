using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorChildControl : MonoBehaviour
{
    DoorMessage _doorMessage;
    GameObject _text;
    float doorOpenTime=5f;
    private void Awake() 
    {
        _doorMessage = GameObject.Find("DoorMessage").GetComponent<DoorMessage>();
        _text=GameObject.Find("DoorMessage").GetComponent<GameObject>();
    }
    private void Start() 
    {
        // if (!gameObject.activeSelf)
        // {
        //     gameObject.SetActive(true); // Kapıyı başlangıçta aktif hale getir
        //     Debug.Log("Door was inactive, now set to active.");
        // }
        StartCoroutine(DoorAccess(doorOpenTime));
    }

    void Message()
    {   
        _doorMessage.gameObject.SetActive(true);
        _doorMessage.TextStatus();
        Debug.Log("Message displayed.");
    }

    IEnumerator DoorAccess(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(true);
        Message();
        Debug.Log("Door is now active.");
    }
}

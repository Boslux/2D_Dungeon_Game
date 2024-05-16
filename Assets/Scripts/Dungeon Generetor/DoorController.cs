using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    private DungeonGenerator dungeonGenerator;

    private Text _text;

    private void Awake()
    {
        _text = GameObject.Find("DoorMessage").GetComponent<Text>();
        Debug.Log("DoorController Awake - GameObject: " + gameObject.name + " isActive: " + gameObject.activeSelf);
    }

    private void Start()
    {
        StartCoroutine(TextControl());
    }
    IEnumerator TextControl()
    {
        _text.text="Find The Door";
        yield return new WaitForSeconds(2);
        _text.text="";
    }

    public void Initialize(DungeonGenerator generator, Vector2 dir)
    {
        dungeonGenerator = generator;
        Debug.Log("DoorController Initialized");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dungeonGenerator.OnDoorTriggered();
            Debug.Log("Player triggered the door.");
        }
    }
}

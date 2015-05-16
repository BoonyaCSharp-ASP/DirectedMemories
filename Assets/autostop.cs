using UnityEngine;
using System.Collections;

public class autostop : MonoBehaviour
{

    public GameObject player;
    public AudioSource second;
    bool stop;

    // Use this for initialization
    void Start()
    {
        stop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!stop && ((player.transform.position - gameObject.transform.position).magnitude < 7))
        {
            Debug.Log("Swap");
            stop = true;
            gameObject.audio.Stop();
            second.Play();
        }
    }
}

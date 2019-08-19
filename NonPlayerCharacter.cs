using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour
{
    public float display_time = 4.0f;
    public GameObject dialog_box;
    float timer_display;

    // Start is called before the first frame update
    void Start()
    {
        dialog_box.SetActive(false);
        timer_display = -1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer_display >= 0)
        {
            timer_display -= Time.deltaTime;
            if (timer_display < 0)
            {
                dialog_box.SetActive(false);
            }
        }
    }

    public void displayDialog()
    {
        timer_display = display_time;
        dialog_box.SetActive(true);
    }
}

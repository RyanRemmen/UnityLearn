using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public static UIHealthBar instance { get; private set; }

    public Image mask;
    float original_size;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        original_size = mask.rectTransform.rect.width;
    }

    public void setValue(float value)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(
               RectTransform.Axis.Horizontal, original_size * value);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    public Sprite selectionTexture;

    public bool isSelected = false;

    public bool isControllable = false;

    public void setSelectionProps()
    {
        isSelected = true;
    }

    public void setDeselectionProps()
    {
        isSelected = false;
    }
}

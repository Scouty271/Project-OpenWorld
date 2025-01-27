using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : MonoBehaviour
{
    public bool isEnabled;

    public float moveSpeed;

    private float rotationX;

    private void Update()
    {
        if (isEnabled)
        {
            rotationX += moveSpeed / 100;

            if (rotationX >= 1f)
            {
                if (transform.eulerAngles.y > 90 || transform.eulerAngles.y < -90)
                {
                    GetComponent<Light>().intensity = 1;
                    transform.Rotate(0, 0.1f, 0, Space.World);
                }
                else
                {
                    GetComponent<Light>().intensity = 2;
                    transform.Rotate(0, 0.15f, 0, Space.World);
                }

                rotationX = 0;
            }
        }
    }
}

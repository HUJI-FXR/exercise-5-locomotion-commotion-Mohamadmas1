using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    public void ApplyRecoil(float intensity)
    {
        intensity = Random.Range(intensity * 0.5f, intensity);

        // push the gun back along the z axis a random amount
        transform.localPosition += new Vector3(0, 0, -intensity / 50);

        // rotate the gun around the y axis a random amount
        transform.localRotation *= Quaternion.Euler(0, intensity / 100, 0);

        // rotate the gun around the x axis a random amount
        transform.localRotation *= Quaternion.Euler(-intensity, 0, 0);
    }

    private void Update()
    {
        // lerp the gun back to its original position
        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * 10);

        // lerp the gun back to its original rotation
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, Time.deltaTime * 10);
    }
}

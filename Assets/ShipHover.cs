using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHover : MonoBehaviour
{
    public new Rigidbody rigidbody;
    [Header("HoverSettings")]
    public float hoverHeight;
    public float hoverDamp;
    public float hoverForce;

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;
        Ray downRay = new Ray(transform.position, -Vector3.up);

        // Cast a ray straight downwards.
        if (Physics.Raycast(downRay, out hit)) {

            // The "error" in height is the difference between the desired height
            // and the height measured by the raycast distance.
            float hoverError = hoverHeight - hit.distance;

            // Only apply a lifting force if the object is too low (ie, let
            // gravity pull it downward if it is too high).
            if (hoverError > 0) {
                // Subtract the damping from the lifting force and apply it to
                // the rigidbody.
                float upwardSpeed = rigidbody.velocity.y;
                float lift = hoverError * hoverForce - upwardSpeed * hoverDamp;
                rigidbody.AddForce(lift * Vector3.up);
            }
        }
    }
}

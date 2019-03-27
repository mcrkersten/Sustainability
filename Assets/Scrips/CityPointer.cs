using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityPointer : MonoBehaviour
{
    // The target marker.
    public GameObject target;
    public GameObject sphere;
    public GameObject holder;

    public float max;

    private Vector3 targetPoint;
    private Quaternion targetRotation;

    void Start() {

    }

    void Update() {
        holder.transform.LookAt(target.gameObject.transform);
        this.transform.eulerAngles = new Vector3(0, holder.transform.eulerAngles.y, 0);

        float distance = Vector3.Distance(target.transform.position, transform.position);
        distance /= 1000;
        float size = .75f - distance;

        size /= 3;
        sphere.transform.localScale = new Vector3(size, size, size);
    }
}
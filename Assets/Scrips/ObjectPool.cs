using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public int quadSize;
    public GameObject lightX;
    public GameObject lightZ;
    public PointList verticals = new PointList();
    public PointList horizontals = new PointList();
    private Ship ship;
    private int stepsOnX;
    private int stepsOnZ;

    public float distX;
    public float distZ;

    private void Start() {
        ship = Ship.Instance;
    }

    private void Update() {
        lightZ.transform.localEulerAngles = new Vector3(0, 0, ship.gameObject.transform.position.x);
        lightX.transform.localEulerAngles = new Vector3(ship.gameObject.transform.position.z, 0, 0);

        distX = Mathf.Abs(this.gameObject.transform.position.x - ship.gameObject.transform.position.x);
        distZ = Mathf.Abs(this.gameObject.transform.position.z - ship.gameObject.transform.position.z);

        if(distX > 100 && ship.gameObject.transform.position.x > this.gameObject.transform.position.x) {
            foreach(GameObject g in verticals.rows[stepsOnX].objects) {
                g.transform.position = new Vector3((g.transform.position.x + quadSize) + 400, 0, g.transform.position.z);
            }
            this.gameObject.transform.position = new Vector3(this.transform.position.x + quadSize, this.transform.position.y, this.transform.position.z);
            stepsOnX++;
            if(stepsOnX == verticals.rows.Count) {
                stepsOnX = 0;
            }
        }
        else if(distX > 100 && ship.gameObject.transform.position.x < this.gameObject.transform.position.x) {
            if(stepsOnX <= 0) {
                stepsOnX = verticals.rows.Count - 1;
            }
            else {
                stepsOnX--;
            }

            foreach (GameObject g in verticals.rows[stepsOnX].objects) {
                g.transform.position = new Vector3((g.transform.position.x - quadSize) - 400, 0, g.transform.position.z);
            }
            this.gameObject.transform.position = new Vector3(this.transform.position.x - quadSize, this.transform.position.y, this.transform.position.z);
        }




        if (distZ > 100 && ship.gameObject.transform.position.z > this.gameObject.transform.position.z) {
            foreach (GameObject g in horizontals.rows[stepsOnZ].objects) {
                g.transform.position = new Vector3(g.transform.position.x, 0, (g.transform.position.z + quadSize) + 400);
            }
            this.gameObject.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + quadSize);
            stepsOnZ++;
            if (stepsOnZ == verticals.rows.Count) {
                stepsOnZ = 0;
            }
        }

        else if (distZ > 100 && ship.gameObject.transform.position.z < this.gameObject.transform.position.z) {
            if (stepsOnZ <= 0) {
                stepsOnZ = verticals.rows.Count - 1;
            }
            else {
                stepsOnZ--;
            }

            foreach (GameObject g in horizontals.rows[stepsOnZ].objects) {
                g.transform.position = new Vector3(g.transform.position.x, 0, (g.transform.position.z - quadSize) - 400);
            }
            this.gameObject.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - quadSize);
        }
    }
}

[System.Serializable]
public class Point {
    public List<GameObject> objects;
}

[System.Serializable]
public class PointList {
    public List<Point> rows;
}

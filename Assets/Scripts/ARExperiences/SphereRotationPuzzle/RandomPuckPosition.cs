using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPuckPosition : MonoBehaviour
{
    public GameObject targetObject;

    [SerializeField] float xPos;
    [SerializeField] float yPos;
    [SerializeField] float zPos;

    public void SetXPosition(float x) { xPos = x; }
    public void SetYPosition(float y) { yPos = y; }
    public void SetZPosition(float z) { zPos = z; }

    [ContextMenu("Randomize Position")]
    public void RandomizePosition()
    {
        var pos = Random.onUnitSphere * 0.30f;
        Vector3 posRevised = new Vector3(pos.x, pos.y, pos.z + 1);
        // For visibility:
        xPos = pos.x;
        yPos = pos.y;
        zPos = pos.z;
        transform.position = posRevised;
        transform.LookAt(targetObject.transform.position);
    }

    private void Start()
    {
        RandomizePosition();
    }
}

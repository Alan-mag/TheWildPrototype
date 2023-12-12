using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationArrow : MonoBehaviour
{
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*float angle = Vector3.SignedAngle(this.gameObject.transform.parent.transform.forward, target.position, this.gameObject.transform.parent.transform.position);
        Debug.Log(angle);
        transform.RotateAround(this.gameObject.transform.parent.transform.position, Vector3.up, angle * Time.deltaTime);*/
        this.gameObject.transform.LookAt(target);
    }
}

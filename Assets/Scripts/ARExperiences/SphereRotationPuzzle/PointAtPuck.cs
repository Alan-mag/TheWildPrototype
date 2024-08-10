using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointAtPuck : MonoBehaviour
{
    public GameObject indicator;
    public Transform targetObjectTransform;
    public float pointingAtThreshold = 10.0f;
    public TMP_Text canvasText;

    private bool _isPointingAtPuck;
    private bool _incrementedExp;

    private void CheckIfPointingAtPuck()
    {
        var targetRot = (targetObjectTransform.transform.position - transform.position).normalized;
        var pointerRot = transform.forward;
        var angle = Vector3.Angle(pointerRot, targetRot);
        if (canvasText != null) canvasText.text = angle.ToString(); // tutorial check
        _isPointingAtPuck = angle < pointingAtThreshold;
        if (indicator != null)
        {
            indicator.SetActive(_isPointingAtPuck); // success object
        }
        if (_isPointingAtPuck)
        {
            Debug.Log("Pointing at target");
            // IncrementExp();
            // StartCoroutine(GoToNextScene());
        }
    }

    private void IncrementExp()
    {
        if (!_incrementedExp)
        {
            GameObject.Find("FirebaseSaveTest").GetComponent<FirebaseManager>().UpdatePlayerExperience(EXPERIENCE_TYPE.Adventurer, 0.5f);
            _incrementedExp = true;
        }
    }

    IEnumerator GoToNextScene()
    {
        yield return new WaitForSeconds(2);
        GameObject.Find("PuzzleSceneManager").GetComponent<SceneChangeHandler>().ChangeScene(); // probably just better to handle with observable pattern
    }

    private void Start()
    {
        // Todo: will need to update this on generating the tunnels
        StartCoroutine(PairTunnelToTarget());
    }

    IEnumerator PairTunnelToTarget()
    {
        yield return new WaitForSeconds(2);
        targetObjectTransform = GameObject.Find("Target").transform;
        yield return null;
    }

    private void Update()
    {
        if (targetObjectTransform != null) 
        {
            CheckIfPointingAtPuck();
        }
    }
}

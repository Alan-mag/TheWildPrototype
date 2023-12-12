using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointAtPuck : MonoBehaviour
{
    public GameObject indicator;
    public Transform puck;
    public float pointingAtThreshold = 10.0f;
    public TMP_Text canvasText;

    private bool _isPointingAtPuck;
    private bool _incrementedExp;

    private void CheckIfPointingAtPuck()
    {
        var puckRot = (puck.transform.position - transform.position).normalized;
        var pointerRot = transform.forward;
        var angle = Vector3.Angle(pointerRot, puckRot);
        if (canvasText != null) canvasText.text = angle.ToString(); // tutorial check
        _isPointingAtPuck = angle < pointingAtThreshold;
        indicator.SetActive(_isPointingAtPuck);
        if (_isPointingAtPuck)
        {
            IncrementExp();
            StartCoroutine(GoToNextScene());
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

    private void Update()
    {
        CheckIfPointingAtPuck();
    }
}

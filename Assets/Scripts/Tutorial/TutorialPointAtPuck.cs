using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialPointAtPuck : MonoBehaviour
{
    [SerializeField] private Animator companionAnimator;
    [SerializeField] private ParticleSystem companionFire;

    [SerializeField] private TutorialManager tutorialManager;

    public Transform puck;
    public float pointingAtThreshold = 10.0f;

    private bool _isPointingAtPuck;
    private bool _incrementedExp;
    private bool _isSolved;

    private void CheckIfPointingAtPuck()
    {
        var puckRot = (puck.transform.position - transform.position).normalized;
        var pointerRot = transform.forward;
        var angle = Vector3.Angle(pointerRot, puckRot);
        _isPointingAtPuck = angle < pointingAtThreshold;
        if (_isPointingAtPuck)
        {
            // IncrementExp();
            // StartCoroutine(GoToNextScene());
            HandleSolvedPuzzle();
        }
    }

    private void IncrementExp()
    {
        if (!_incrementedExp)
        {
            GameObject.Find("FirebaseSaveTest").GetComponent<FirebaseManager>().UpdatePlayerExperience(EXPERIENCE_TYPE.Adventurer, 10f);
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
        if (!_isSolved)
            CheckIfPointingAtPuck();
    }

    private void HandleSolvedPuzzle()
    {
        Debug.Log("Solved!");
        companionFire.Stop();
        companionAnimator.SetTrigger("TrRepairCompanion");
        _isSolved = true;
        // tutorialManager.HandleFinishedTutorial();
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScannerExpdManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _channelNameText = null;

    [SerializeField]
    private Text _successText = null;

    [SerializeField]
    private GameObject _finishBtn;

    private Texture2D _semanticTexture;
    private bool _incrementedExp;

    SceneChangeHandler sceneChangeHandler;
    [SerializeField] ExpeditionLevelHandler expeditionSceneHandler;

    [SerializeField]
    HandleFmodEvent handleFmodEvent;

    private void Awake()
    {
        if (sceneChangeHandler == null)
            sceneChangeHandler = gameObject.GetComponent<SceneChangeHandler>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /********************
     * Firebase test
     */

    private void IncrementExp()
    {
        if (!_incrementedExp && GameObject.Find("FirebaseSaveTest") != null)
        {
            GameObject.Find("FirebaseSaveTest").GetComponent<FirebaseManager>().UpdatePlayerExperience(EXPERIENCE_TYPE.Explorer, 1);
            GameObject.Find("FirebaseSaveTest").GetComponent<FirebaseManager>().UpdatePlayerExperience(EXPERIENCE_TYPE.Adventurer, 0.5f); // todo: clean up [progression manager, no need to ref twice, etc.]
            _incrementedExp = true;
        }
    }

    public void HandleSuccessfullScan()
    {

        // handle scene change to map
        IncrementExp();
        if (expeditionSceneHandler != null)
        {
            _successText.text = "Scanned: Japanese Maple";
            PlayScanningAudio();
            StartCoroutine(PlaySuccessfulScanEffect(3f));
        }
        else
        {
            _successText.text = "Scan Complete";
            StartCoroutine(PlaySuccessfulScanEffect(3f));
        }
    }

    private void HandleSuccessSceneChange()
    {
        /*if (expeditionSceneHandler != null)
        {
            expeditionSceneHandler.CompleteStage();
        }
        else
        {
            sceneChangeHandler.ChangeScene();
        }*/
    }

    IEnumerator PlaySuccessfulScanEffect(float waitForSeconds)
    {
        yield return new WaitForSeconds(waitForSeconds);
        _finishBtn.SetActive(true);
        // HandleSuccessSceneChange();
    }

    private void PlayScanningAudio()
    {
        // AmazonPollyUtil pollyUtil = this.GetComponent<AmazonPollyUtil>();
        // pollyUtil.PlayMessageWithPolly("Japanese maple has long been cultivated in Japan and was introduced into cultivation in Europe in the early 1800s. It is one of the most versatile small trees for use in the landscape. It exists in a multitude of forms that provide a wide range of sizes, shapes, and colors.");
        handleFmodEvent.PlayFmodEvent("/VO/HQ Expedition Japanese Maple");
    }

    private void OnDestroy()
    {
        if (_semanticTexture != null)
            Destroy(_semanticTexture);
    }
}

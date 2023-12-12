using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Niantic.Lightship.AR.Semantics;
using Niantic.Lightship.AR;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ScannerManager : MonoBehaviour
{
    /*[SerializeField]
    private ARSession _arSessionManager = null;

    [SerializeField]
    private ARSemanticSegmentationManager _semanticSegmentationManager;

    [Header("Rendering")]
    // The UI image that the camera overlay is rendered in.
    [SerializeField]
    private RawImage _segmentationOverlayImage = null;

    [Header("UI")]
    [SerializeField]
    private GameObject _togglesParent = null;

    [SerializeField]
    private Text _toggleFeaturesButtonText = null;

    [SerializeField]
    private Text _toggleInterpolationText = null;

    [SerializeField]
    private Text _channelNameText = null;

    [SerializeField]
    private Text _successText = null;

    [SerializeField]
    private int channelToScan = 7;

    private Texture2D _semanticTexture;

    SceneChangeHandler sceneChangeHandler;
    [SerializeField] ExpeditionLevelHandler expeditionSceneHandler;

    // The current active channel that is painted white. -1 means that no semantic is used.
    private int _featureChannel = -1;
    private bool _isTextureDirty;
    private bool _incrementedExp;

    // added to start animation for companion:
    // [SerializeField] private Animator companionAnimator;

    // test state implementation
    // [SerializeField] CompanionController companionController;

    private void Awake()
    {
        if (sceneChangeHandler == null)
            sceneChangeHandler = gameObject.GetComponent<SceneChangeHandler>();
    }

    private void Start()
    {
        if (_togglesParent != null)
            _togglesParent.SetActive(false);

        // TODO: this should be set from renderer
        Application.targetFrameRate = 60;

        _semanticSegmentationManager.EnableFeatures();
        _semanticSegmentationManager.SemanticBufferInitialized += OnSemanticBufferInitialized;
        _semanticSegmentationManager.SemanticBufferUpdated += OnSemanticBufferUpdated;

        // companionController.CurrentStateVal = new ScanState();
        // companionAnimator.SetTrigger("StartScan");
    }

    private void Update()
    {
        // Should update the semantics representation?
        if (_isTextureDirty)
        {
            // Update
            _semanticSegmentationManager.SemanticBufferProcessor.CopyToAlignedTextureARGB32
            (
              texture: ref _semanticTexture,
              channel: _featureChannel,
              orientation: Screen.orientation
            );

            // Assign
            _segmentationOverlayImage.texture = _semanticTexture;
            _isTextureDirty = false;
        }

        *//*if (_featureChannel == -1)
        {*//*
            var detectedChannels = string.Empty;
            if (Input.touchCount > 0)
            {
                // Display the names of the channels the user is touching on the screen
                var touchPosition = Input.touches[0].position;
                var channelsForPixel =
                  _semanticSegmentationManager.SemanticBufferProcessor.GetChannelNamesAt
                  (
                    (int)touchPosition.x,
                    (int)touchPosition.y
                  );

                detectedChannels = channelsForPixel.Aggregate
                (
                  detectedChannels,
                  (current, channelName) =>
                    string.IsNullOrEmpty(current)
                      ? (current + channelName)
                      : (current + ", " + channelName)
                );
            }

        if (detectedChannels.Contains("foliage"))
        {
            HandleSuccessfulScan();

        }
        // _successText.text = detectedChannels.Contains("foliage") ? "Successful Scan!" : "";
        // }
    }


    *//********************
     * Firebase test
     *//*

    private void IncrementExp()
    {
        if (!_incrementedExp)
        {
            GameObject.Find("FirebaseSaveTest").GetComponent<FirebaseManager>().UpdatePlayerExperience(EXPERIENCE_TYPE.Explorer, 1);
            GameObject.Find("FirebaseSaveTest").GetComponent<FirebaseManager>().UpdatePlayerExperience(EXPERIENCE_TYPE.Adventurer, 0.5f); // todo: clean up [progression manager, no need to ref twice, etc.]
            _incrementedExp = true;
        }
    }
    *//********************//*

    // Todo:
    // Companion should go toward selected plant/animal
    // allow scan state to play
    // if audio message, play audio message
    // either show a button to "finish" scene
    // Or - exit scene and go to following scene
    private void HandleSuccessfulScan()
    {

        // handle scene change to map
        IncrementExp();
        // todo:
        // for expedition prototype:
        // play audio message about specific tree [maybe show some text]
        // after done, wait a few seconds...then move back to map
        // stretch goal:
        // update scanning effects, have companion point towards plant

        // methods
        // if expedition - PlayScanningAudio
        // SuccessfulScanEffect [for now just wait]
        // HandleSceneChange
        if (expeditionSceneHandler != null)
        {
            _successText.text = "Scanned: Japanese Maple";
            PlayScanningAudio();
            StartCoroutine(PlaySuccessfulScanEffect(22f));
        } else
        {
            _successText.text = "Scan Complete";
            StartCoroutine(PlaySuccessfulScanEffect(3f));
        }
    }

    private void HandleSuccessSceneChange()
    {
        if (expeditionSceneHandler != null)
        {
            expeditionSceneHandler.CompleteStage();
        }
        else
        {
            sceneChangeHandler.ChangeScene();
        }
    }

    IEnumerator PlaySuccessfulScanEffect(float waitForSeconds)
    {
        yield return new WaitForSeconds(waitForSeconds);
        HandleSuccessSceneChange();
    }

    private void PlayScanningAudio()
    {
        AmazonPollyUtil pollyUtil =  this.GetComponent<AmazonPollyUtil>();
        pollyUtil.PlayMessageWithPolly("Japanese maple has long been cultivated in Japan and was introduced into cultivation in Europe in the early 1800s. It is one of the most versatile small trees for use in the landscape. It exists in a multitude of forms that provide a wide range of sizes, shapes, and colors.");
    }

    private void OnDestroy()
    {
        _semanticSegmentationManager.SemanticBufferUpdated -= OnSemanticBufferUpdated;

        // Release semantic overlay texture
        if (_semanticTexture != null)
            Destroy(_semanticTexture);
    }

    private void OnSemanticBufferInitialized(ContextAwarenessArgs<ISemanticBuffer> args)
    {
        _semanticSegmentationManager.SemanticBufferInitialized -= OnSemanticBufferInitialized;
        if (_togglesParent != null)
            _togglesParent.SetActive(true);
    }

    private void OnSemanticBufferUpdated(ContextAwarenessStreamUpdatedArgs<ISemanticBuffer> args)
    {
        _isTextureDirty = _isTextureDirty || _featureChannel != -1;
    }

    public void ChangeFeatureChannel()
    {
        // The exact channel names and the order they are returned in may change with
        // new versions of ARDK, so you should avoid caching information about channels in your app.
        // Instead, get the list of channel indices and names at runtime.
        var channelNames = _semanticSegmentationManager.SemanticBufferProcessor.Channels;

        // If the channels aren't yet known, we can't change off the initial default channel.
        if (channelNames == null)
            return;

        // Increment the channel count with wraparound.
        _featureChannel += 1;
        if (_featureChannel == channelNames.Length)
            _featureChannel = -1;

        // Update the displayed name of the channel, and enable or disable the overlay.
        if (_featureChannel == -1)
        {
            _channelNameText.text = "None";
            _segmentationOverlayImage.enabled = false;
        }
        else
        {
            _channelNameText.text = FormatChannelName(channelNames[_featureChannel]);
            if (_semanticSegmentationManager.AreFeaturesEnabled)
            {
                _segmentationOverlayImage.enabled = true;
            }

            _isTextureDirty = true;
        }
    }

    public void SetFeatureChannelToFoliage()
    {
        var channelNames = _semanticSegmentationManager.SemanticBufferProcessor.Channels;

        _featureChannel = channelToScan; // foliage is 7 for mobile device
        _channelNameText.text = FormatChannelName(channelNames[_featureChannel]);
        if (_semanticSegmentationManager.AreFeaturesEnabled)
        {
            _segmentationOverlayImage.enabled = true;
        }

        _isTextureDirty = true;
    }

    public void ToggleSessionSemanticFeatures()
    {
        var newEnabledState = !_semanticSegmentationManager.enabled;

        _toggleFeaturesButtonText.text = newEnabledState ? "Disable Features" : "Enable Features";

        _semanticSegmentationManager.enabled = newEnabledState;
        _segmentationOverlayImage.enabled = newEnabledState;

        if (!newEnabledState)
        {
            Destroy(_semanticTexture);
            _semanticTexture = null;
        }
    }

    public void ToggleInterpolation()
    {
        var provider = _semanticSegmentationManager.SemanticBufferProcessor;
        var current = provider.InterpolationMode;
        provider.InterpolationMode = current == InterpolationMode.None
          ? InterpolationMode.Smooth
          : InterpolationMode.None;

        _toggleInterpolationText.text =
          provider.InterpolationMode != InterpolationMode.None
            ? "Disable Interpolation"
            : "Enable Interpolation";
    }

    private string FormatChannelName(string text)
    {
        var parts = text.Split('_');
        List<string> displayParts = new List<string>();
        foreach (var part in parts)
        {
            displayParts.Add(char.ToUpper(part[0]) + part.Substring(1));
        }

        return String.Join(" ", displayParts.ToArray());
    }*/
}

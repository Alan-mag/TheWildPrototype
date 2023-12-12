using Niantic.Lightship.AR.Semantics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class SemanticQuerying : MonoBehaviour
{
    public ARCameraManager _cameraMan;
    public ARSemanticSegmentationManager _semanticMan;

    public TMP_Text _text;
    public RawImage _image;
    public Material _material;

    [SerializeField]
    private bool isScanning;

    private string _channel = "foliage";
    private float _timer = 0.0f;

    void OnEnable()
    {
        _cameraMan.frameReceived += OnCameraFrameUpdate;
    }

    private void OnDisable()
    {
        _cameraMan.frameReceived -= OnCameraFrameUpdate;
    }

    public void StartScan()
    {
        isScanning = true;
    }

    private void OnCameraFrameUpdate(ARCameraFrameEventArgs args)
    {
        if (!_semanticMan.subsystem.running)
        {
            return;
        }

        //get the semantic texture
        Matrix4x4 mat = Matrix4x4.identity;
        var texture = _semanticMan.GetSemanticChannelTexture(_channel, out mat);

        if (texture && isScanning)
        {
            _image.gameObject.SetActive(true);
            //the texture needs to be aligned to the screen so get the display matrix
            //and use a shader that will rotate/scale things.
            Matrix4x4 cameraMatrix = args.displayMatrix ?? Matrix4x4.identity;
            _image.material = _material;
            _image.material.SetTexture("_SemanticTex", texture);
            _image.material.SetMatrix("_SemanticMat", mat);
        }
    }

    void Update()
    {
        if (!_semanticMan.subsystem.running)
        {
            return;
        }

        var pos = new Vector3();

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            pos = Input.mousePosition;
        }

        /*if (EventSystem.current.IsPointerOverGameObject()) // hitting rawimage i'm guessing
        {
            // todo: this doesn't seem to work, can't select UI
            EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
            return;
        }*/
#else
        if (Input.touches.Length > 0)
            {
            pos = Input.touches[0].position;
            }
#endif

        if (pos.x > 0 && pos.x < Screen.width)
        {
            if (pos.y > 0 && pos.y < Screen.height)
            {
                _timer += Time.deltaTime;
                if (_timer > 0.05f)
                {
                    var list = _semanticMan.GetChannelNamesAt((int)pos.x, (int)pos.y);

                    if (list.Count > 0 && list[0] == "foliage") // just  foliage check
                    {
                        _channel = list[0];
                        _text.text = _channel;

                        GameObject scannerMngGameObj = GameObject.Find("ScannerExpdManager"); // TODO: decouple this  from any particular scan scene
                        if (scannerMngGameObj != null)
                            {
                                ScannerExpdManager scannerMngr = scannerMngGameObj.GetComponent<ScannerExpdManager>();
                                scannerMngr.HandleSuccessfullScan();
                            }
                    }
                    else
                    {
                        _text.text = "?";
                    }

                    _timer = 0.0f;
                }
            }
        }
    }
}
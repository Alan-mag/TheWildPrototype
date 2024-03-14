using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Camera))]
public class PhysicsRaycastOnTouch : MonoBehaviour
{
    [SerializeField]
    Transform playerTransform;

    [SerializeField]
    TMP_Text debugButtonText;

    public float interactionDistance = 50.0f;
    public float interactionDistanceExpedition = 15.0f;
    public bool debugMode;

    Camera m_Camera;

    void OnEnable()
    {
        m_Camera = GetComponent<Camera>();
    }

    void Update()
    {
        var pos = new Vector3();

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            pos = Input.mousePosition;
        }
#else
if (Input.touches.Length > 0)
        {
            pos = Input.touches[0].position;
        }
#endif
        if (pos != new Vector3(0, 0, 0))
            OnTouchScreen(pos);
    }
    
    public void ToggleDebug() // Todo: save and pull from playerprefs
    {
        debugMode = !debugMode; 
        if (debugMode)
        {
            debugButtonText.text = "Debug On";
        }
        else
        {
            debugButtonText.text = "Debug Off";
        }
    }

    private void ChangeSceneOnGameObject(GameObject selectedGameObject)
    {
        var sceneChangeScript = selectedGameObject.GetComponent<SceneChangeHandler>();
        sceneChangeScript.ChangeScene();
    }

    // todo: would like to make generic
    // This is not great!!!! needs updating
    private void HandleHistoricalImageSetInfo(GameObject selectedGameObject)
    {
        if (selectedGameObject.GetComponent<HistoricalMapObject>() != null)
        {
            HistoricalImageInfo.ImageTitle = selectedGameObject.GetComponent<HistoricalMapObject>().imageTitle;
            HistoricalImageInfo.ImageSourceTitle = selectedGameObject.GetComponent<HistoricalMapObject>().imageSourceTitle;
            HistoricalImageInfo.Description = selectedGameObject.GetComponent<HistoricalMapObject>().imageDescription;
        }
    }

    private void SelectPathOnMapObject(GameObject selectedGameObject)
    {
        var pathSelectionScript = selectedGameObject.GetComponent<PathSelectionHandler>();
        pathSelectionScript.SelectPath();

    }

    // TODO: This needs big overhaul
    void OnTouchScreen(Vector3 position)
    {
        Ray ray = m_Camera.ScreenPointToRay(position);
        

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            var g = hitInfo.collider.gameObject;



            if (g != null)
            {

                if (g.tag == "MapSpawnObject")
                {
                    if (debugMode)
                    {
                        // no distance requirement from experience
                        HandleHistoricalImageSetInfo(g);
                        var sceneChangeScript = g.GetComponent<SceneChangeHandler>();
                        sceneChangeScript.ChangeScene();
                    }
                    else
                    {
                        float dist = Vector3.Distance(g.transform.position, playerTransform.position);
                        if (dist <= interactionDistance)
                        {
                            // historical image data check:
                            HandleHistoricalImageSetInfo(g);
                            ChangeSceneOnGameObject(g);
                        }
                        else
                        {
                            Debug.Log("interaction too far away " + dist.ToString());
                        }
                    }
                }

                // handle audio log
                if (g.name == "AudioLog") // todo: this isn't going to scale well!
                {
                    var audioScript = g.GetComponent<AudioLog>();
                    audioScript.CollectAudioLog();
                }

                // handle audio log test
                if (g.name == "AudioLogTest2") // todo: this isn't going to scale well!
                {
                    var audioScript = g.GetComponent<AudioLog_To_AudioPlayer>();
                    audioScript.ToAudioPlayer();
                }

                /*if (g.name == "TutorialAudioLog") // todo: this isn't going to scale well!
                {
                    var audioScript = g.GetComponent<AudioLog>();
                    audioScript.CollectTutorialAudioLog();
                }*/

                // handle thread
                if (g.name.Contains("Thread")) // for now, just thread example update if need more
                {
                    // todo: check if audio or thread
                    var threadScript = g.GetComponent<ThreadObject>();
                    threadScript.CollectThread();
                }


                ////////////////////////////////////////////// EXPEDITION ///////////////////////////////////////////
                if (playerTransform != null) // on map? but not mapspawnobject
                {
                    float distanceToObject = Vector3.Distance(g.transform.position, playerTransform.position);
                    Debug.Log(g.name + " is " + distanceToObject + " meters away.");

                    if (g.name == "TestExpeditionMapCube(Clone)")
                    {
                        SceneManager.LoadScene("ExpeditionScene"); // could just call scene change handler from object component
                    }

                    // todo update: for now Expd_ map objects are all expedition
                    if (g.name.Contains("Expd_"))
                    {
                        if (g.name == "Expd_MapObj_Signal(Clone)" && distanceToObject <= interactionDistanceExpedition)
                        {
                            SelectPathOnMapObject(g);
                            ChangeSceneOnGameObject(g);
                        }
                        if (distanceToObject <= interactionDistanceExpedition) // TODO: make this it's own param, also spin object or something -- communicate to player it's selectable
                        {
                            ChangeSceneOnGameObject(g);
                        }
                    }
                }
                //////////////////////////////////////////////  ///////////////////////////////////////////
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ExpeditionInputHandler : MonoBehaviour
{
    [SerializeField]
    Camera mainCamera;
    [SerializeField]
    Transform playerTransform;

    public float interactionDistanceExpedition = 15.0f;
    public bool debugMode;

    private void Update()
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
        if (pos != new Vector3(0,0,0))
            OnTouchScreen(pos);
    }

    // handle map interactions
    // handle AR scene interactions

    // signal interactions
    // etc.
    void OnTouchScreen(Vector3 position)
    {
        Ray ray = mainCamera.ScreenPointToRay(position);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            var gameObjectHitWithRaycast = hitInfo.collider.gameObject;

            if (gameObjectHitWithRaycast.name == "AudioLogTest2")
            {
                var audioScript = gameObjectHitWithRaycast.GetComponent<AudioLog_To_AudioPlayer>();
                audioScript.ToAudioPlayer();
            }

            if (gameObjectHitWithRaycast.name.Contains("Thread"))
            {
                var threadScript = gameObjectHitWithRaycast.GetComponent<ThreadObject>();
                threadScript.CollectThread();
            }

            ////////////////////////////////////////////// EXPEDITION ///////////////////////////////////////////
            if (playerTransform != null) // on map? but not mapspawnobject
            {
                float distanceToObject = Vector3.Distance(gameObjectHitWithRaycast.transform.position, playerTransform.position);
                Debug.Log(gameObjectHitWithRaycast.name + " is " + distanceToObject + " meters away.");

                if (gameObjectHitWithRaycast.name == "TestExpeditionMapCube(Clone)")
                {
                    SceneManager.LoadScene("ExpeditionScene"); // could just call scene change handler from object component
                }

                // todo update: for now Expd_ map objects are all expedition
                if (gameObjectHitWithRaycast.name.Contains("Expd_"))
                {
                    if (gameObjectHitWithRaycast.name == "Expd_MapObj_Signal(Clone)" && distanceToObject <= interactionDistanceExpedition)
                    {
                        SelectPathOnMapObject(gameObjectHitWithRaycast);
                        ChangeSceneOnGameObject(gameObjectHitWithRaycast);
                    }
                    if (distanceToObject <= interactionDistanceExpedition) // TODO: make this it's own param, also spin object or something -- communicate to player it's selectable
                    {
                        ChangeSceneOnGameObject(gameObjectHitWithRaycast);
                    }
                }
            }
        }
        //////////////////////////////////////////////  ///////////////////////////////////////////
    }

    private void ChangeSceneOnGameObject(GameObject selectedGameObject)
    {
        var sceneChangeScript = selectedGameObject.GetComponent<SceneChangeHandler>();
        sceneChangeScript.ChangeScene();
    }

    private void SelectPathOnMapObject(GameObject selectedGameObject)
    {
        var pathSelectionScript = selectedGameObject.GetComponent<PathSelectionHandler>();
        pathSelectionScript.SelectPath();
    }

    private static bool IsTouchOverUIObject(Touch touch)
    {
        var eventDataCurrentPosition =
          new PointerEventData(EventSystem.current)
          {
              position = new Vector2(touch.position.x, touch.position.y)
          };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}

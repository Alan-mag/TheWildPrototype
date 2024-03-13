using System;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TutorialPhysicsRaycastOnTouch : MonoBehaviour
{
    /*[SerializeField] private TutorialManager tutorialManager;
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
    }*/

    /*void OnTouchScreen(Vector3 pos)
    {
        Ray ray = m_Camera.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            var g = hitInfo.collider.gameObject;
            Debug.Log(g.name);

            if (g != null)
            {
                // handle audio log
                if (g.name == "AudioLog") // todo: this isn't going to scale well!
                {
                    var audioScript = g.GetComponent<AudioLog>();
                    audioScript.CollectAudioLog();
                }

                if (g.name == "TutorialAudioLog") // todo: this isn't going to scale well!
                {
                    var audioScript = g.GetComponent<AudioLog>();
                    audioScript.CollectTutorialAudioLog();
                }

                // handle thread
                if (g.name.Contains("Thread")) // for now, just thread example update if need more
                {
                    // todo: check if audio or thread
                    // tutorialManager.GetComponent<TutorialManager>().CollectThread(g);
                }
            }
        }
    }*/
}

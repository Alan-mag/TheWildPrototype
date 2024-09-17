using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCreationManager : MonoBehaviour
{
    [SerializeField] GameObject mCamera;
    [SerializeField] GameObject puzzleSphere;
    [SerializeField] GameObject tunnelPrefab;
    [SerializeField] GameObject targetPrefab;

    [SerializeField] List<GameObject> tunnels;
    [SerializeField] List<GameObject> targets;

    [SerializeField] List<string> targetData;

    [SerializeField] int tunnelLimit = 5;

    [Header("Progression")]
    [SerializeField] GameProgressionSO gameProgressionSO;

    private int currentTunnelCount = 0;

    private void Start()
    {
        if (targetData == null)
            targetData = new List<string>();
    }

    public void CreateTunnel()
    {
        if (currentTunnelCount >= tunnelLimit)
        {
            Debug.Log("Too many tunnels [add this text to canvas]");
            return;
        }

        // calculate distance to center
        float distance = Vector3.Distance(puzzleSphere.transform.position, mCamera.transform.position);
        Debug.Log("distance " + distance);
        float centerOfSphereRadius = distance - (puzzleSphere.GetComponentInChildren<SphereCollider>().radius / 4); // divided by 2 should work here right? use two + .3? for puck placement
        Debug.Log("center of sphere from camera " + centerOfSphereRadius);
        Vector3 tunnelPosition = new Vector3(mCamera.transform.position.x, mCamera.transform.position.y, mCamera.transform.position.z + centerOfSphereRadius);

        Vector3 relPos = puzzleSphere.transform.position - mCamera.transform.position;
        Quaternion rotation = Quaternion.LookRotation(relPos, Vector3.up);
        Debug.Log("rotation " + rotation);

        GameObject newTunnel = Instantiate(tunnelPrefab, tunnelPosition, rotation);
        CreateTarget(tunnelPosition, rotation);

        // make new tunnel child of sphere - so rotation works
        newTunnel.transform.LookAt(puzzleSphere.transform);
        newTunnel.transform.parent = puzzleSphere.transform;
        tunnels.Add(newTunnel);
        currentTunnelCount++;
    }

    private void CreateTarget(Vector3 tunnelPos, Quaternion rot)
    {
        GameObject target = Instantiate(targetPrefab, new Vector3(tunnelPos.x, tunnelPos.y, tunnelPos.z - 0.15f), rot);
        target.transform.LookAt(puzzleSphere.transform);
        target.transform.parent = puzzleSphere.transform;
        // set x y z on RandomPuckPosition
        // save that to db
        target.GetComponent<RandomPuckPosition>().SetXPosition(target.transform.position.x);
        target.GetComponent<RandomPuckPosition>().SetYPosition(target.transform.position.y);
        target.GetComponent<RandomPuckPosition>().SetZPosition(target.transform.position.z);

        targets.Add(target);
    }

    private void DestroyTargets()
    {
        foreach (GameObject t in targets)
        {
            Destroy(t);
        }
    }

    public void ResetPuzzleSphereRotation()
    {
        StartCoroutine(AnimateRotationTowards(puzzleSphere.transform, Quaternion.identity, 1f));
    }
 
    IEnumerator AnimateRotationTowards(Transform target, Quaternion rot, float dur)
    {
        float t = 0f;
        Quaternion start = target.rotation;
        while (t < dur)
        {
            target.rotation = Quaternion.Slerp(start, rot, t / dur);
            yield return null;
            t += Time.deltaTime;
        }
        target.rotation = rot;
    }

    // Destroy all tunnel game objects
    public void StartOver() 
    {
        foreach (GameObject t in tunnels)
        {
            Destroy(t);
        }
        currentTunnelCount = 0;
        DestroyTargets();
        ResetPuzzleSphereRotation();
    }

    // TODO: Add test puzzle functionality
    public void TestPuzzle() { }

    public void SavePuzzle()
    {
        StartCoroutine(StorePuzzle());
    }

    // need to save rotation data of each tunnel to db
    private IEnumerator StorePuzzle() 
    {
        float t = 0f;
        Quaternion start = puzzleSphere.transform.rotation;
        List<PuzzleSphereTarget> puzzleTargets = new List<PuzzleSphereTarget>();
        string creatorName;
        if (PlayerPrefs.GetString("username") != null)
        {
            creatorName = PlayerPrefs.GetString("username");
        } 
        else
        {
            creatorName = "default";
        }

        while (t < 1f)
        {
            puzzleSphere.transform.rotation = Quaternion.Slerp(start, Quaternion.identity, t / 1f);
            yield return null;
            t += Time.deltaTime;
        }
        puzzleSphere.transform.rotation = Quaternion.identity;

        foreach (GameObject targ in targets)
        {
            targ.GetComponent<RandomPuckPosition>().SetXPosition(targ.transform.position.x);
            targ.GetComponent<RandomPuckPosition>().SetYPosition(targ.transform.position.y);
            targ.GetComponent<RandomPuckPosition>().SetZPosition(targ.transform.position.z);

            PuzzleSphereTarget targetLocationData = new PuzzleSphereTarget(targ.transform.position.x, targ.transform.position.y, targ.transform.position.z);
            puzzleTargets.Add(targetLocationData);
        }
        PuzzleSphereInformation puzzleInfo = new PuzzleSphereInformation(creatorName, puzzleTargets);
        string jsonData = JsonConvert.SerializeObject(puzzleInfo); // todo: can clean up with just data.ToJson() works now that public members are accessible
        Debug.Log(jsonData);
        targetData.Add(jsonData);

        Debug.Log(JsonConvert.SerializeObject(targetData));
        // call to save:
        FirebaseManager firebaseManager = GameObject.FindObjectOfType<FirebaseManager>();
        firebaseManager.AddPlayerCreatedPuzzle(JsonConvert.SerializeObject(targetData));
        firebaseManager.UpdatePlayerExperience(EXPERIENCE_TYPE.Creator, (float)gameProgressionSO.creatorExperienceLarge);
    }
}

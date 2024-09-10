using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PuzzleSceneManager : MonoBehaviour
{
    [SerializeField] GameObject puzzleSphere;
    [SerializeField] GameObject puzzleTargetObject;
    [SerializeField] GameObject puzzleTunnelObject;
    [SerializeField] SphereMapExperiencesManagerSO sphereMapExperiencesManagerSO;
    [SerializeField] ChosenSphereExperienceSO chosenSphereExperienceSO;

    [Header("Canvas References")]
    [SerializeField] TextMeshProUGUI creatorNameText;

    [Header("Rotation Text References")]
    [SerializeField] TextMeshProUGUI xRotationText;
    [SerializeField] TextMeshProUGUI yRotationText;
    [SerializeField] TextMeshProUGUI zRotationText;

    private void Awake()
    {
        if (puzzleSphere == null)
            throw new UnassignedReferenceException("No puzzle sphere set in PuzzleSceneManager");
    }

    void Start()
    {
        Debug.Log("PuzzleSceneManager:: " + chosenSphereExperienceSO.chosenSpherePuzzle);
        if (chosenSphereExperienceSO.chosenSpherePuzzle != null)
        {
            HandleChosenSphereExperience();
            return;
        }
#if UNITY_IPHONE
    Debug.Log("Running on Apple Device.");
            Debug.Log("Running on iOS Device.");
        if (sphereMapExperiencesManagerSO.sphereInformationCollection.Count > 0)
        {
            var s = Resources.Load<SphereMapExperiencesManagerSO>("ScriptableObjects/Sphere Map Experiences Manager SO");
            creatorNameText.text = s.sphereInformationCollection.First().creatorName.ToString();
            PuzzleSphereInformation chosenPuzzle = s.sphereInformationCollection.First();
            foreach (PuzzleSphereTarget target in chosenPuzzle.puzzleSphereTarget) // Todo: get random puzzleinfo
            {
                CreateTargetObject
                    (
                        target.x,
                        target.y,
                        target.z
                    );
            }
        }
        else
        {
            CreateTargetObject(null, null, null);
        }
        SetInitialPuzzleSphereRotation();
#elif UNITY_ANDROID
        Debug.Log("Running on Android Device.");
        if (sphereMapExperiencesManagerSO.sphereInformationCollection.Count > 0)
        {
            var s = Resources.Load<SphereMapExperiencesManagerSO>("ScriptableObjects/Sphere Map Experiences Manager SO");
            creatorNameText.text = s.sphereInformationCollection.First().creatorName.ToString();
            PuzzleSphereInformation chosenPuzzle = s.sphereInformationCollection.First();
            foreach (PuzzleSphereTarget target in chosenPuzzle.puzzleSphereTarget) // Todo: get random puzzleinfo
            {
                CreateTargetObject
                    (
                        target.x,
                        target.y,
                        target.z
                    );
            }
        }
        else
        {
            CreateTargetObject(null, null, null);
        }
        SetInitialPuzzleSphereRotation();
#elif UNITY_WP8
    Debug.Log("Running on WP Device.");
#else
    if (sphereMapExperiencesManagerSO.sphereInformationCollection.Count > 0)
        {
            creatorNameText.text = sphereMapExperiencesManagerSO.sphereInformationCollection.First().creatorName.ToString();
            // use signal sequence from SO todo: randomize which gets picked
            System.Random rand = new System.Random();
            int index = rand.Next(sphereMapExperiencesManagerSO.sphereInformationCollection.Count);
            var sphereTargetCollectionArray = sphereMapExperiencesManagerSO.sphereInformationCollection.ToArray();
            PuzzleSphereInformation chosenPuzzle = sphereTargetCollectionArray[index];
            foreach (PuzzleSphereTarget target in chosenPuzzle.puzzleSphereTarget) // Todo: get random puzzleinfo
            {
                CreateTargetObject
                    (
                        target.x,
                        target.y,
                        target.z
                    );
            }
        }
        else
        {
            CreateTargetObject(null, null, null);
        }
        SetInitialPuzzleSphereRotation();
#endif
    }

    private void HandleChosenSphereExperience()
    {
        PuzzleSphereInformation chosenPuzzle = chosenSphereExperienceSO.chosenSpherePuzzle;
        creatorNameText.text = chosenPuzzle.creatorName;
        foreach (PuzzleSphereTarget target in chosenPuzzle.puzzleSphereTarget) // Todo: get random puzzleinfo
        {
            CreateTargetObject
                (
                    target.x,
                    target.y,
                    target.z
                );
        }
        
        SetInitialPuzzleSphereRotation();
        chosenSphereExperienceSO.chosenSpherePuzzle = null;
    }

    private void CreateTargetObject(float? x, float? y, float? z)
    {
        var targetObj = Instantiate(puzzleTargetObject);
        if (x == null || y == null || z == null)
        {
            var pos = Random.onUnitSphere * 0.30f;
            Vector3 posRevised = new Vector3(pos.x, pos.y, pos.z + 1);
            targetObj.transform.position = posRevised;
            targetObj.transform.LookAt(puzzleSphere.transform.position);
            // Vector3 tunnelPos = new Vector3((pos.x / 2), (pos.y / 2), (pos.z + 1) / 2);
            CreateTunnelObject(new Vector3(0f, 0f, 1f), targetObj.transform.position);
        }
        else
        {
            var pos = Random.onUnitSphere * 0.30f;
            Vector3 posRevised = new Vector3((float)x, (float)y, (float)z);
            targetObj.transform.position = posRevised;
            targetObj.transform.LookAt(puzzleSphere.transform.position);
            CreateTunnelObject(new Vector3(0f, 0f, 1f), targetObj.transform.position);
        }
    }

    private void CreateTunnelObject(Vector3 tunnelPosition, Vector3 targetPosition)
    {
        var tunnelObject = Instantiate(puzzleTunnelObject);
        tunnelObject.transform.parent = puzzleSphere.transform;
        tunnelObject.transform.position = tunnelPosition;
        tunnelObject.transform.LookAt(targetPosition);
        // Todo: need to set "target" for tunnel here
        // Also, need logic to check total targets and make sure they
        // are are aligned for success
    }

    private void SetInitialPuzzleSphereRotation()
    {
        float randomXRot = Random.Range(0f, 360f);
        float randomYRot = Random.Range(0f, 360f);
        float randomZRot = Random.Range(0f, 360f);
        puzzleSphere.transform.Rotate(randomXRot, randomYRot, randomZRot);
    }

    void Update()
    {
        // PrintRotation();
    }

    // todo: observable here? handle material changes, vfx, etc.
    void HandleSolvedPuzzle()
    {

    }

    void PrintRotation()
    {
        xRotationText.text = "x: " + puzzleSphere.transform.localRotation.eulerAngles.x.ToString();
        yRotationText.text = "y: " + puzzleSphere.transform.localRotation.eulerAngles.y.ToString();
        zRotationText.text = "z: " + puzzleSphere.transform.localRotation.eulerAngles.z.ToString();
    }

}

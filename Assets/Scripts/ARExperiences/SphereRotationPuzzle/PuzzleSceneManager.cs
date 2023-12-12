using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PuzzleSceneManager : MonoBehaviour
{
    [SerializeField] GameObject puzzleSphere;

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
        
    }

    void Update()
    {
        PrintRotation();
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

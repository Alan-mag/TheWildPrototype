using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpeditionFinalStage : MonoBehaviour
{
    [SerializeField] ExpeditionSO expeditionData = default;

    [SerializeField] GameObject pathOneElement = default;
    [SerializeField] GameObject pathTwoElement = default;
    [SerializeField] GameObject pathThreeElement = default;

    private void Start()
    {
        if (expeditionData.GetPathTaken == 1)
        {
            pathOneElement.SetActive(true);
        }

        if (expeditionData.GetPathTaken == 2)
        {
            pathTwoElement.SetActive(true);
        }

        if (expeditionData.GetPathTaken == 3)
        {
            pathThreeElement.SetActive(true);
        }
    }
}

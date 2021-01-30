using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraUpdater : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera vCamera;
    private CinemachineConfiner confiner;

    private void Awake()
    {
        confiner = vCamera.GetComponent<CinemachineConfiner>();
    }

    public void SetSize(float size)
    {
        vCamera.m_Lens.OrthographicSize = size;
    }

    public void SetSizeAnimated(float size)
    {
        StartCoroutine(AnimateSize(size));
    }

    public void SetNewBounds(PolygonCollider2D newBounds)
    {
        confiner.m_BoundingShape2D = newBounds;
    }

    public void RemoveCameraTarget()
    {
        vCamera.m_Follow = null;
    }

    IEnumerator AnimateSize(float newSize)
    {
        var iSize = vCamera.m_Lens.OrthographicSize;
        var numberOfSteps = 20;

        for(int i = 1; i <= numberOfSteps; i++)
        {
            float v = Mathf.Lerp(iSize, newSize, i / (float)numberOfSteps);
            vCamera.m_Lens.OrthographicSize = v;
            yield return new WaitForSeconds(0.02f);
        }
    }
}

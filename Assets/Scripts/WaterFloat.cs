using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class WaterFloat : MonoBehaviour
{
    public WaterSurface water;

    [Header("Float settings")]
    public float floatHeightOffset = 0.0f;
    public float smooth = 5f;

    Vector3 velocity;

    void Update()
    {
        if (water == null)
            return;

        Vector3 pos = transform.position;

        // HDRP water query
        WaterSearchParameters search = new WaterSearchParameters();
        search.startPositionWS = pos;
        search.targetPositionWS = pos;
        search.error = 0.01f;
        search.maxIterations = 8;

        WaterSearchResult result;

        if (water.ProjectPointOnWaterSurface(search, out result))
        {
            float targetY = result.projectedPositionWS.y + floatHeightOffset;

            Vector3 targetPos = new Vector3(
                transform.position.x,
                targetY,
                transform.position.z
            );

            transform.position = Vector3.Lerp(
                transform.position,
                targetPos,
                Time.deltaTime * smooth
            );
        }
    }
}
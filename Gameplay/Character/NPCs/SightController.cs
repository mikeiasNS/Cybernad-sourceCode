using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightController : MonoBehaviour
{
    [SerializeField]
    private float sightRangeHeight;
    [SerializeField]
    private float sightRangeWidth;
    [SerializeField]
    private Transform sightOriginPosition;
    [SerializeField]
    private LayerMask layerMask;

    public Collider2D[] OnSight()
    {
        var dir = transform.localScale.x < 0 ? -1 : 1;

        var center = new Vector2(sightOriginPosition.position.x + sightRangeWidth * .5f * dir,
            sightOriginPosition.position.y - sightRangeHeight * .5f);

        var filter = new ContactFilter2D();
        filter.SetLayerMask(layerMask);

        Collider2D[] results = new Collider2D[6];
        Physics2D.OverlapBox(center, new Vector2(sightRangeHeight, sightRangeWidth), 90, filter, results);

        return results;
    }

    private void OnDrawGizmos()
    {
        var dir = transform.localScale.x < 0 ? -1 : 1;

        var center = new Vector2(sightOriginPosition.position.x + sightRangeWidth * .5f * dir,
            sightOriginPosition.position.y - sightRangeHeight * .5f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, new Vector2(sightRangeWidth, sightRangeHeight));
    }
}

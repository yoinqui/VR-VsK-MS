using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExtPhysics
{
    /// <summary>
    /// Performs a curved raycast.
    /// </summary>
    public static bool CurveCast(Vector3 origin, Vector3 direction, Vector3 gravityDirection, int smoothness, out RaycastHit hitInfo, float maxDistance, out List<Vector3> points)
    {
        if (maxDistance == Mathf.Infinity) maxDistance = 500;
        Vector3 currPos = origin, hypoPos = origin, hypoVel = direction.normalized / smoothness;
        List<Vector3> v = new List<Vector3>();
        RaycastHit hit;
        float curveCastLength = 0;

        do
        {
            v.Add(hypoPos);
            currPos = hypoPos;
            hypoPos = currPos + hypoVel + (gravityDirection * Time.fixedDeltaTime / (smoothness * smoothness));
            hypoVel = hypoPos - currPos;
            curveCastLength += hypoVel.magnitude;
        }
        while (Physics.Raycast(currPos, hypoVel, out hit, hypoVel.magnitude) == false && curveCastLength < maxDistance);
        hitInfo = hit;
        points = v;
        return Physics.Raycast(currPos, hypoVel, hypoVel.magnitude);
    }
}

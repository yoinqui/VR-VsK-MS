using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EInterpolationType
{
    Custom,
    Linear,
    Exponential,
    Sine,
    None
}

public enum ERaycastType
{
    Straight,
    Curved
}

public class ControllerTeleport : MonoBehaviour
{
    [Header("Raycast")]
    public ERaycastType raycastType = ERaycastType.Straight;
    [Range(1, 15)]
    public int gravitationForce = 8;
    [Range(50f, 500f)]
    public float maxDistance = 100f;
    public float tpCooldownTime = 5.0f;
    

    [Header("Interpolation")]
    public EInterpolationType interpolationType = EInterpolationType.Sine;
    public AnimationCurve interpolationCurve = AnimationCurve.Linear(0, 0, 10, 10);


    [Header("Visuals")]
    public Color color;
    [Range(0.02f, 0.05f)]
    public float thickness = 0.2f;
    [Range(1, 50)]
    public int smoothness = 10;
    public Material lineMaterial;
    public bool Scrolling;
    public AnimationCurve lineWidthCurve = AnimationCurve.Linear(0, 0, 10, 10);

    List<Vector3> pointsPrev;
    private int appearOffset;
    LineRenderer lineRenderer;

    [HideInInspector]
    public Vector3 TargetPosition;
    [HideInInspector]
    public bool CanTeleport;

    private bool tpAvailable = true;

    GameObject pointer;
    GameObject cursor;


    // Use this for initialization
    void Start()
    {
        pointsPrev = new List<Vector3>();
        
    }

    void Update()
    {
        bool output;
        RaycastHit curvedHit;
        // Create a Vector3 List, this will be used to store the points along the curved RayCast; You can use this to map a LineRenderer to the points in order to visualize the CurveCast;
        List<Vector3> curvePoints;
        
        if (raycastType == ERaycastType.Straight)
        {
            output = ExtPhysics.CurveCast(transform.position, transform.forward, new Vector3(0, 0, 0), smoothness, out curvedHit, maxDistance, out curvePoints);
        } 
        else
        {
            output = ExtPhysics.CurveCast(transform.position, transform.forward, Vector3.down * gravitationForce, smoothness, out curvedHit, maxDistance, out curvePoints);
        }

       

        if (pointsPrev.Count == 0)
        {
            pointsPrev = curvePoints.Select(v => new Vector3(v.x, v.y + Mathf.InverseLerp(0, curvePoints.Count, curvePoints.IndexOf(v)) * appearOffset, v.z)).ToList();
            appearOffset = 0;
        }
        float lengthI = curvePoints.Count;
        for (int i = 0; i < lengthI; i++)
        {
            float t = default(float);
            float interpolVal = i / lengthI;
            switch (interpolationType)
            {
                case EInterpolationType.Custom:
                    t = interpolationCurve.Evaluate(interpolVal);
                    break;
                case EInterpolationType.Linear:
                    t = interpolVal;
                    break;
                case EInterpolationType.Exponential:
                    t = Mathf.Pow(interpolVal, 2);
                    break;
                case EInterpolationType.Sine:
                    t = Mathf.Sin(interpolVal * Mathf.PI) * .9f;
                    break;
                case EInterpolationType.None:
                    t = 0;
                    break;
            }
            curvePoints[i] = Vector3.Lerp(curvePoints[i], pointsPrev.Count >= 1 ? pointsPrev[i > pointsPrev.Count - 1 ? pointsPrev.Count - 1 : i] : curvePoints[i], t * .9f);
        }
        pointsPrev = curvePoints;
        lineRenderer.positionCount = curvePoints.Count;
        lineRenderer.SetPositions(curvePoints.ToArray());
        if (output)
        {
            if (curvedHit.collider.gameObject.GetComponent<TeleportPlane>() && tpAvailable)
            {
                CanTeleport = true;
                TargetPosition = curvedHit.point;
                lineMaterial.color = Color.cyan;
                cursor.transform.position = curvePoints.Last();
                cursor.transform.position = Vector3.MoveTowards(cursor.transform.position, curvePoints.Last(), 1);
                cursor.SetActive(true);
            }
            else if (curvedHit.collider.gameObject.GetComponent<TeleportPlane>() && !tpAvailable)
            {
                CanTeleport = false;
                lineMaterial.color = Color.yellow;
                cursor.transform.position = curvePoints.Last();
                cursor.transform.position = Vector3.MoveTowards(cursor.transform.position, curvePoints.Last(), 1);
                cursor.SetActive(true);
            }
            else
            {
                CanTeleport = false;
                lineMaterial.color = tpAvailable ? Color.red : Color.yellow;
                cursor.SetActive(false);
            }
        }
    }


    public void ActivatePointer()
    {
        enabled = true;
        pointer = new GameObject();
        pointer.name = "Pointer";
        pointer.transform.parent = this.transform;
        pointer.transform.localPosition = Vector3.zero;
        lineRenderer = pointer.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.widthMultiplier = thickness;

        cursor = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        cursor.SetActive(false);
        cursor.GetComponent<MeshRenderer>().material = lineMaterial;
        Destroy(cursor.GetComponent<CapsuleCollider>());
        cursor.transform.localScale = new Vector3((float)0.5, (float)0.02, (float)0.5);
    }

    public void DesactivatePointer()
    {
        enabled = false;
        Destroy(pointer);
        Destroy(cursor);
    }

    public IEnumerator WaitTeleportReloaded()
    {
        tpAvailable = false;
        yield return new WaitForSeconds(tpCooldownTime);
        tpAvailable = true;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour
{
    public static Quaternion FaceTarget(Vector3 targetTransform, Vector3 selfTransform, float constant)
    {
        float AngleRad = Mathf.Atan2(targetTransform.y - selfTransform.y, targetTransform.x - selfTransform.x);
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        return Quaternion.Euler(0, 0, AngleDeg + constant);
    }
}

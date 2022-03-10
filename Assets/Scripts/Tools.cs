using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tools
{
    public static Quaternion FaceTarget(Vector3 targetTransform, Vector3 selfTransform, float constant)
    {
        float AngleRad = Mathf.Atan2(targetTransform.y - selfTransform.y, targetTransform.x - selfTransform.x);
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        return Quaternion.Euler(0, 0, AngleDeg + constant);
    }

    public static CreatureDatabase GetCreatureDatabase()
    {
        return Resources.Load<CreatureDatabase>("Data/CreatureDatabase.asset");
    }

    public static float DeviateByPercent(float num, float percent)
    {
        return DeviateByPercent(num, percent, 0);
    }
    public static float DeviateByPercent(float num, float percent, float min)
    {
        return Mathf.Max(min, num + Random.Range(-num * percent, num * percent));
    }
}

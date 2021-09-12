using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 本来用于检测子弹是否飞出Main Camera，但考虑到性能还是换用碰撞体解决
public static class CameraExtension
{
    public static bool IsObjectVisible(this UnityEngine.Camera @this, Renderer renderer)
    {
        return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(@this), renderer.bounds);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �������ڼ���ӵ��Ƿ�ɳ�Main Camera�������ǵ����ܻ��ǻ�����ײ����
public static class CameraExtension
{
    public static bool IsObjectVisible(this UnityEngine.Camera @this, Renderer renderer)
    {
        return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(@this), renderer.bounds);
    }
}

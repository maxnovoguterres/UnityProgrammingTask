using UnityEngine;

public static class IsometricUtils
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 225, 0));

    public static Vector3 ToIso(this Vector3 input)
    {
        var cameraController = GameManagerUtils.CameraController;
        if (cameraController is not null)
        {
            return _isoMatrix.MultiplyPoint3x4(input);
        }
        
        return Vector3.zero;
    }
}
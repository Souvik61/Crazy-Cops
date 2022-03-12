using UnityEngine;
using Cinemachine;

/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's X co-ordinate
/// </summary>
[SaveDuringPlay]
[AddComponentMenu("")] // Hide in menu
public class FollowCameraClampExt : CinemachineExtension
{
    [Tooltip("Lock the camera's Z position to this value")]
    public float m_XPosition = 10;

    public float minX;
    public float maxX;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            var pos = state.RawPosition;
            pos.x = m_XPosition;
            state.RawPosition = pos;
        }
    }
}

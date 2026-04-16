using UnityEngine;
using UnityEngine.Rendering;

namespace SmartCityDemo.VR
{
    public sealed class QuestRuntimeConfigurator : MonoBehaviour
    {
        [SerializeField] private int targetFps = 72;
        [SerializeField] private float lodBias = 0.65f;
        [SerializeField] private float shadowDistance = 20f;

        private void Awake()
        {
            Application.targetFrameRate = targetFps;
            QualitySettings.vSyncCount = 0;
            QualitySettings.lodBias = lodBias;
            QualitySettings.shadowDistance = shadowDistance;
            QualitySettings.shadowCascades = 1;
            QualitySettings.realtimeReflectionProbes = false;
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;

            var camera = Camera.main;
            if (camera != null)
            {
                camera.useOcclusionCulling = true;
                camera.allowMSAA = true;
                camera.allowHDR = false;
            }

            if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Vulkan)
            {
                // Vulkan path is preferred on modern Quest builds.
            }
        }
    }
}

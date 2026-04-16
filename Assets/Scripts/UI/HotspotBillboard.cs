using UnityEngine;

namespace SmartCityDemo.UI
{
    public sealed class HotspotBillboard : MonoBehaviour
    {
        private Transform target;

        private void LateUpdate()
        {
            if (target == null)
            {
                var cam = Camera.main;
                if (cam != null)
                {
                    target = cam.transform;
                }
                else
                {
                    return;
                }
            }

            transform.LookAt(transform.position + target.forward);
        }
    }
}

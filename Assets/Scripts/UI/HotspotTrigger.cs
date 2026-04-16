using UnityEngine;

namespace SmartCityDemo.UI
{
    public sealed class HotspotTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private float activationDistance = 2.2f;

        private Transform viewer;

        public void Configure(GameObject infoPanel)
        {
            panel = infoPanel;
        }

        private void Update()
        {
            if (panel == null)
            {
                return;
            }

            if (viewer == null)
            {
                var cam = Camera.main;
                if (cam != null)
                {
                    viewer = cam.transform;
                }
                else
                {
                    return;
                }
            }

            var shouldShow = Vector3.Distance(viewer.position, transform.position) <= activationDistance;
            if (panel.activeSelf != shouldShow)
            {
                panel.SetActive(shouldShow);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace SmartCityDemo.DemoFlow
{
    public sealed class DemoFlowController : MonoBehaviour
    {
        [SerializeField] private List<Transform> waypoints = new();
        [SerializeField] private float transitionDuration = 10f;
        [SerializeField] private float[] holdDurations = { 35f, 100f, 100f, 100f, 50f };

        private Transform rigRoot;
        private Camera mainCamera;
        private Coroutine flowRoutine;
        private int currentSegment;

        public void Configure(List<Transform> waypointList)
        {
            waypoints = waypointList;
        }

        private void Start()
        {
            SetupRigReferences();
            flowRoutine = StartCoroutine(RunFlow());
        }

        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current.nKey.wasPressedThisFrame)
            {
                SkipToNextSegment();
            }
        }

        private IEnumerator RunFlow()
        {
            yield return new WaitForSeconds(0.5f);
            for (currentSegment = 0; currentSegment < waypoints.Count; currentSegment++)
            {
                yield return MoveRigTo(waypoints[currentSegment].position, transitionDuration);
                var hold = currentSegment < holdDurations.Length ? holdDurations[currentSegment] : 45f;
                yield return new WaitForSeconds(hold);
            }

            yield return new WaitForSeconds(4f);
            SceneManager.LoadScene("MainMenu");
        }

        private IEnumerator MoveRigTo(Vector3 destination, float duration)
        {
            var start = rigRoot.position;
            var timer = 0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                rigRoot.position = Vector3.Lerp(start, destination, Mathf.SmoothStep(0f, 1f, timer / duration));
                yield return null;
            }
            rigRoot.position = destination;
        }

        private void SkipToNextSegment()
        {
            if (flowRoutine != null)
            {
                StopCoroutine(flowRoutine);
            }

            currentSegment = Mathf.Clamp(currentSegment + 1, 0, waypoints.Count - 1);
            rigRoot.position = waypoints[currentSegment].position;
            flowRoutine = StartCoroutine(RunFlow());
        }

        private void SetupRigReferences()
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                var cameraGo = new GameObject("Main Camera");
                mainCamera = cameraGo.AddComponent<Camera>();
                cameraGo.tag = "MainCamera";
                cameraGo.transform.position = new Vector3(0f, 1.7f, -18f);
            }

            rigRoot = mainCamera.transform.root;
            if (rigRoot == mainCamera.transform)
            {
                var rig = new GameObject("RuntimeXRRig");
                rig.transform.position = mainCamera.transform.position;
                mainCamera.transform.SetParent(rig.transform, true);
                rigRoot = rig.transform;
            }
        }
    }
}

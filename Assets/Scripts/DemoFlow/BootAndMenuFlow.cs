using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

namespace SmartCityDemo.DemoFlow
{
    public sealed class BootAndMenuFlow : MonoBehaviour
    {
        private static BootAndMenuFlow instance;
        private const string BootScene = "Boot";
        private const string MainMenuScene = "MainMenu";
        private const string DemoScene = "SmartCityDemo";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void EnsureRuntime()
        {
            if (instance != null)
            {
                return;
            }

            var host = new GameObject("SmartCityRuntime");
            DontDestroyOnLoad(host);
            instance = host.AddComponent<BootAndMenuFlow>();
            SceneManager.sceneLoaded += instance.OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == BootScene)
            {
                StartCoroutine(LoadMenuAfterBootDelay());
                return;
            }

            if (scene.name == MainMenuScene)
            {
                MainMenuBuilder.BuildMenu(() => SceneManager.LoadScene(DemoScene));
                return;
            }

            if (scene.name == DemoScene)
            {
                SmartCitySceneBuilder.Build();
            }
        }

        private void Update()
        {
            if (!DidPressDebugInput())
            {
                return;
            }

            Debug.Log($"[BootAndMenuFlow] Debug input alindi. Aktif sahne: {SceneManager.GetActiveScene().name}");
        }

        private IEnumerator LoadMenuAfterBootDelay()
        {
            yield return new WaitForSeconds(1.25f);
            SceneManager.LoadScene(MainMenuScene);
        }

        private static bool DidPressDebugInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                return true;
            }

            return IsAnyControllerTriggerPressed();
        }

        private static bool IsAnyControllerTriggerPressed()
        {
            var leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            if (leftHand.isValid && leftHand.TryGetFeatureValue(CommonUsages.triggerButton, out var leftTriggerPressed) && leftTriggerPressed)
            {
                return true;
            }

            var rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            return rightHand.isValid && rightHand.TryGetFeatureValue(CommonUsages.triggerButton, out var rightTriggerPressed) && rightTriggerPressed;
        }
    }

    internal static class MainMenuBuilder
    {
        public static void BuildMenu(UnityEngine.Events.UnityAction onStart)
        {
            if (GameObject.Find("MainMenuCanvas") != null)
            {
                return;
            }

            var canvasGo = new GameObject("MainMenuCanvas");
            var canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvasGo.AddComponent<CanvasScaler>().dynamicPixelsPerUnit = 20f;
            canvasGo.AddComponent<GraphicRaycaster>();
            canvasGo.transform.position = new Vector3(0f, 1.6f, 2.2f);
            canvasGo.transform.localScale = Vector3.one * 0.0022f;

            var panel = CreateImage("Panel", canvasGo.transform, new Color(0.04f, 0.07f, 0.1f, 0.92f));
            SetRect(panel.rectTransform, Vector2.zero, new Vector2(900f, 460f));

            var title = CreateText("Title", panel.transform, "VR AKILLI SEHIR DEMOSU", 56, FontStyle.Bold);
            title.color = new Color(0.58f, 0.9f, 1f, 1f);
            SetRect(title.rectTransform, new Vector2(0f, 130f), new Vector2(820f, 120f));

            var subtitle = CreateText("Subtitle", panel.transform, "Gelecegin ulasim, enerji ve dijital yasam turu", 30, FontStyle.Normal);
            subtitle.color = new Color(0.85f, 0.92f, 0.97f, 1f);
            SetRect(subtitle.rectTransform, new Vector2(0f, 50f), new Vector2(820f, 80f));

            var buttonImage = CreateImage("StartButton", panel.transform, new Color(0.07f, 0.45f, 0.72f, 1f));
            SetRect(buttonImage.rectTransform, new Vector2(0f, -95f), new Vector2(360f, 100f));
            var button = buttonImage.gameObject.AddComponent<Button>();
            button.targetGraphic = buttonImage;
            button.onClick.AddListener(onStart);

            var buttonLabel = CreateText("Label", buttonImage.transform, "DEMOYU BASLAT", 34, FontStyle.Bold);
            buttonLabel.color = Color.white;
            SetRect(buttonLabel.rectTransform, Vector2.zero, new Vector2(320f, 70f));

            if (Object.FindFirstObjectByType<EventSystem>() == null)
            {
                var eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<InputSystemUIInputModule>();
            }
        }

        private static Image CreateImage(string name, Transform parent, Color color)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var image = go.AddComponent<Image>();
            image.color = color;
            return image;
        }

        private static Text CreateText(string name, Transform parent, string content, int size, FontStyle style)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var text = go.AddComponent<Text>();
            text.text = content;
            text.alignment = TextAnchor.MiddleCenter;
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = size;
            text.fontStyle = style;
            return text;
        }

        private static void SetRect(RectTransform rt, Vector2 anchoredPos, Vector2 size)
        {
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = anchoredPos;
            rt.sizeDelta = size;
        }
    }
}

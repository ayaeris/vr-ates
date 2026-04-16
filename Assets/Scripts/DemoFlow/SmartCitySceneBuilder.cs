using System.Collections.Generic;
using SmartCityDemo.UI;
using SmartCityDemo.VR;
using UnityEngine;

namespace SmartCityDemo.DemoFlow
{
    public static class SmartCitySceneBuilder
    {
        public static void Build()
        {
            if (GameObject.Find("SmartCityRoot") != null)
            {
                return;
            }

            var root = new GameObject("SmartCityRoot");
            var flow = root.AddComponent<DemoFlowController>();
            root.AddComponent<QuestRuntimeConfigurator>();

            var sun = new GameObject("Directional Light");
            var light = sun.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 0.95f;
            sun.transform.rotation = Quaternion.Euler(38f, 35f, 0f);

            RenderSettings.fog = true;
            RenderSettings.fogColor = new Color(0.08f, 0.11f, 0.16f);
            RenderSettings.fogDensity = 0.0055f;
            RenderSettings.ambientIntensity = 0.9f;

            var waypoints = new List<Transform>
            {
                CreateWaypoint("IntroPoint", new Vector3(0f, 1.7f, -18f)),
                CreateWaypoint("MobilityPoint", new Vector3(0f, 1.7f, 20f)),
                CreateWaypoint("EnergyPoint", new Vector3(65f, 1.7f, 20f)),
                CreateWaypoint("LifePoint", new Vector3(130f, 1.7f, 20f)),
                CreateWaypoint("FinalPoint", new Vector3(180f, 2.2f, -8f))
            };

            BuildGround();
            BuildIntroSkyline();
            BuildMobilityZone(new Vector3(0f, 0f, 26f));
            BuildEnergyZone(new Vector3(65f, 0f, 26f));
            BuildDigitalLifeZone(new Vector3(130f, 0f, 26f));
            BuildFinalPanorama(new Vector3(180f, 0f, -4f));

            flow.Configure(waypoints);
        }

        private static Transform CreateWaypoint(string name, Vector3 position)
        {
            var go = new GameObject(name);
            go.transform.position = position;
            return go.transform;
        }

        private static void BuildGround()
        {
            var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "CityGround";
            ground.transform.localScale = new Vector3(30f, 1f, 30f);
            ground.transform.position = Vector3.zero;
            ground.isStatic = true;
            ground.GetComponent<Renderer>().material.color = new Color(0.06f, 0.07f, 0.08f);
        }

        private static void BuildIntroSkyline()
        {
            for (var i = 0; i < 25; i++)
            {
                var tower = GameObject.CreatePrimitive(PrimitiveType.Cube);
                tower.name = $"SkylineTower_{i:00}";
                tower.transform.position = new Vector3(-40f + i * 3.3f, 1.5f + (i % 6), -8f - (i % 3) * 4f);
                tower.transform.localScale = new Vector3(2.2f, 3f + (i % 5) * 3f, 2.2f);
                tower.isStatic = true;
                tower.GetComponent<Renderer>().material.color = new Color(0.13f, 0.17f, 0.24f);
            }
        }

        private static void BuildMobilityZone(Vector3 origin)
        {
            CreateZoneLabel("Akilli Ulasim", origin + new Vector3(0f, 5.5f, 0f), "Otonom araclar ve hava taksi koridoru");

            for (var i = 0; i < 12; i++)
            {
                var roadLight = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                roadLight.transform.position = origin + new Vector3(-22f + i * 4f, 1f, -4f);
                roadLight.transform.localScale = new Vector3(0.2f, 1f, 0.2f);
                roadLight.GetComponent<Renderer>().material.color = new Color(0.18f, 0.95f, 0.95f);
            }

            for (var i = 0; i < 8; i++)
            {
                var vehicle = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                vehicle.transform.position = origin + new Vector3(-16f + i * 4f, 0.8f, 3f);
                vehicle.transform.localScale = new Vector3(0.8f, 0.5f, 1.4f);
                vehicle.GetComponent<Renderer>().material.color = new Color(0.22f, 0.6f, 0.94f);
            }

            CreateHotspot(origin + new Vector3(0f, 1.8f, -1.4f), "AI trafik agi", "Gercek zamanli sensor agi ile trafik yogunlugu optimize edilir.");
        }

        private static void BuildEnergyZone(Vector3 origin)
        {
            CreateZoneLabel("Yesil Enerji", origin + new Vector3(0f, 5.5f, 0f), "Dagitik enerji ve sifir emisyon");

            for (var i = 0; i < 14; i++)
            {
                var panel = GameObject.CreatePrimitive(PrimitiveType.Cube);
                panel.transform.position = origin + new Vector3(-20f + i * 3f, 0.25f, -4f - (i % 2) * 2.5f);
                panel.transform.localScale = new Vector3(2f, 0.08f, 1.3f);
                panel.transform.rotation = Quaternion.Euler(15f, 0f, 0f);
                panel.GetComponent<Renderer>().material.color = new Color(0.1f, 0.25f, 0.37f);
                panel.isStatic = true;
            }

            for (var i = 0; i < 4; i++)
            {
                var turbine = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                turbine.transform.position = origin + new Vector3(-16f + i * 10f, 3f, 6f);
                turbine.transform.localScale = new Vector3(0.5f, 3f, 0.5f);
                turbine.GetComponent<Renderer>().material.color = new Color(0.74f, 0.86f, 0.9f);
            }

            CreateHotspot(origin + new Vector3(2f, 2f, 1f), "Enerji paylasimi", "Mahalle mikro-sebekeleri fazla enerjiyi birbirine aktarir.");
        }

        private static void BuildDigitalLifeZone(Vector3 origin)
        {
            CreateZoneLabel("Dijital Yasam", origin + new Vector3(0f, 5.5f, 0f), "Saglik, lojistik ve kamu hizmetlerinde otomasyon");

            for (var i = 0; i < 9; i++)
            {
                var kiosk = GameObject.CreatePrimitive(PrimitiveType.Cube);
                kiosk.transform.position = origin + new Vector3(-15f + i * 4f, 1.8f, -2f + (i % 3) * 2f);
                kiosk.transform.localScale = new Vector3(1.6f, 3.2f, 1.1f);
                kiosk.GetComponent<Renderer>().material.color = new Color(0.15f, 0.45f, 0.64f);
                kiosk.isStatic = true;
            }

            for (var i = 0; i < 6; i++)
            {
                var drone = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                drone.transform.position = origin + new Vector3(-12f + i * 4f, 5.3f + (i % 2), 5f);
                drone.transform.localScale = Vector3.one * 0.8f;
                drone.GetComponent<Renderer>().material.color = new Color(0.82f, 0.92f, 0.98f);
            }

            CreateHotspot(origin + new Vector3(0f, 2.2f, 0f), "Akilli saglik kiosku", "Anlik tarama ve uzaktan uzman baglantisi ile hizli yonlendirme saglanir.");
        }

        private static void BuildFinalPanorama(Vector3 origin)
        {
            var arc = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            arc.transform.position = origin + new Vector3(0f, 2.5f, 0f);
            arc.transform.localScale = new Vector3(8f, 2.5f, 8f);
            arc.GetComponent<Renderer>().material.color = new Color(0.1f, 0.18f, 0.26f);

            CreateZoneLabel("Final Panorama", origin + new Vector3(0f, 7f, 0f), "Insan odakli, yesil ve bagli bir gelecek");
        }

        private static void CreateZoneLabel(string title, Vector3 position, string subtitle)
        {
            var label = new GameObject($"{title}_Label");
            label.transform.position = position;
            var text = label.AddComponent<TextMesh>();
            text.text = $"{title}\n{subtitle}";
            text.fontSize = 42;
            text.anchor = TextAnchor.MiddleCenter;
            text.alignment = TextAlignment.Center;
            text.characterSize = 0.09f;
            text.color = new Color(0.72f, 0.94f, 1f);
            label.AddComponent<HotspotBillboard>();
        }

        private static void CreateHotspot(Vector3 position, string headline, string body)
        {
            var hotspot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            hotspot.name = $"Hotspot_{headline}";
            hotspot.transform.position = position;
            hotspot.transform.localScale = Vector3.one * 0.35f;
            hotspot.GetComponent<Renderer>().material.color = new Color(0.95f, 0.82f, 0.25f);

            var info = new GameObject("InfoPanel");
            info.transform.SetParent(hotspot.transform, false);
            info.transform.localPosition = new Vector3(0f, 0.75f, 0f);
            var text = info.AddComponent<TextMesh>();
            text.text = $"{headline}\n{body}";
            text.fontSize = 36;
            text.characterSize = 0.05f;
            text.anchor = TextAnchor.MiddleCenter;
            text.alignment = TextAlignment.Center;
            text.color = Color.white;

            var trigger = hotspot.AddComponent<HotspotTrigger>();
            trigger.Configure(info);
            info.SetActive(false);
            info.AddComponent<HotspotBillboard>();
        }
    }
}

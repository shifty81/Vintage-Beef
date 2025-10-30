using UnityEngine;

namespace VintageBeef.World
{
    /// <summary>
    /// Day/night cycle system with dynamic lighting
    /// Creates immersive time-of-day atmosphere
    /// Optimized for Palia-style performance
    /// </summary>
    public class DayNightCycle : MonoBehaviour
    {
        [Header("Time Settings")]
        [SerializeField] private float dayLengthInMinutes = 20f; // Real-time minutes for full day
        [SerializeField] private float startTimeOfDay = 6f; // 6 AM start
        [SerializeField] private bool pauseTime = false;

        [Header("Sun Settings")]
        [SerializeField] private Light sunLight;
        [SerializeField] private Gradient sunColorGradient;
        [SerializeField] private AnimationCurve sunIntensityCurve;

        [Header("Moon Settings")]
        [SerializeField] private Light moonLight;
        [SerializeField] private Color moonColor = new Color(0.5f, 0.6f, 0.8f);
        [SerializeField] private float moonIntensity = 0.3f;

        [Header("Ambient Settings")]
        [SerializeField] private Gradient ambientColorGradient;
        [SerializeField] private Gradient fogColorGradient;
        [SerializeField] private AnimationCurve fogDensityCurve;

        private float timeOfDay = 6f; // 0-24 hour format
        private float timeScale = 1f;

        private void Start()
        {
            timeOfDay = startTimeOfDay;
            
            if (sunLight == null)
            {
                sunLight = CreateSunLight();
            }

            if (moonLight == null)
            {
                moonLight = CreateMoonLight();
            }

            // Create default gradients if not set
            if (sunColorGradient == null || sunColorGradient.colorKeys.Length == 0)
            {
                CreateDefaultSunGradient();
            }

            if (ambientColorGradient == null || ambientColorGradient.colorKeys.Length == 0)
            {
                CreateDefaultAmbientGradient();
            }

            if (fogColorGradient == null || fogColorGradient.colorKeys.Length == 0)
            {
                CreateDefaultFogGradient();
            }

            if (sunIntensityCurve == null || sunIntensityCurve.length == 0)
            {
                CreateDefaultIntensityCurve();
            }

            if (fogDensityCurve == null || fogDensityCurve.length == 0)
            {
                CreateDefaultFogCurve();
            }

            // Enable fog
            RenderSettings.fog = true;

            UpdateLighting();
        }

        private void Update()
        {
            if (pauseTime) return;

            // Calculate time progression
            timeScale = 24f / (dayLengthInMinutes * 60f); // Convert minutes to seconds
            timeOfDay += Time.deltaTime * timeScale;

            // Wrap around 24 hours
            if (timeOfDay >= 24f)
            {
                timeOfDay -= 24f;
            }

            UpdateLighting();
        }

        private void UpdateLighting()
        {
            float normalizedTime = timeOfDay / 24f; // 0-1

            // Update sun rotation
            if (sunLight != null)
            {
                float sunAngle = (timeOfDay - 6f) * 15f - 90f; // Sun rises at 6 AM, sets at 6 PM
                sunLight.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);

                // Update sun properties
                sunLight.color = sunColorGradient.Evaluate(normalizedTime);
                sunLight.intensity = sunIntensityCurve.Evaluate(normalizedTime);

                // Sun is active during day
                sunLight.enabled = timeOfDay >= 5f && timeOfDay <= 19f;
            }

            // Update moon rotation
            if (moonLight != null)
            {
                float moonAngle = (timeOfDay - 18f) * 15f - 90f; // Moon rises at 6 PM
                moonLight.transform.rotation = Quaternion.Euler(moonAngle, 170f, 0f);

                // Moon is active at night
                moonLight.enabled = timeOfDay < 5f || timeOfDay > 19f;
            }

            // Update ambient light
            RenderSettings.ambientLight = ambientColorGradient.Evaluate(normalizedTime);

            // Update fog
            RenderSettings.fogColor = fogColorGradient.Evaluate(normalizedTime);
            RenderSettings.fogDensity = fogDensityCurve.Evaluate(normalizedTime);
        }

        private Light CreateSunLight()
        {
            GameObject sunObj = new GameObject("Sun");
            sunObj.transform.SetParent(transform);
            sunObj.transform.localPosition = Vector3.zero;

            Light light = sunObj.AddComponent<Light>();
            light.type = LightType.Directional;
            light.color = Color.white;
            light.intensity = 1f;
            light.shadows = LightShadows.Soft;

            return light;
        }

        private Light CreateMoonLight()
        {
            GameObject moonObj = new GameObject("Moon");
            moonObj.transform.SetParent(transform);
            moonObj.transform.localPosition = Vector3.zero;

            Light light = moonObj.AddComponent<Light>();
            light.type = LightType.Directional;
            light.color = moonColor;
            light.intensity = moonIntensity;
            light.shadows = LightShadows.Soft;

            return light;
        }

        private void CreateDefaultSunGradient()
        {
            sunColorGradient = new Gradient();
            GradientColorKey[] colorKeys = new GradientColorKey[5];
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];

            // Midnight - Dark blue
            colorKeys[0] = new GradientColorKey(new Color(0.2f, 0.2f, 0.4f), 0.0f);
            // Sunrise - Orange
            colorKeys[1] = new GradientColorKey(new Color(1.0f, 0.7f, 0.4f), 0.25f);
            // Noon - Bright yellow-white
            colorKeys[2] = new GradientColorKey(new Color(1.0f, 0.95f, 0.9f), 0.5f);
            // Sunset - Orange-red
            colorKeys[3] = new GradientColorKey(new Color(1.0f, 0.6f, 0.3f), 0.75f);
            // Midnight - Dark blue
            colorKeys[4] = new GradientColorKey(new Color(0.2f, 0.2f, 0.4f), 1.0f);

            alphaKeys[0] = new GradientAlphaKey(1.0f, 0.0f);
            alphaKeys[1] = new GradientAlphaKey(1.0f, 1.0f);

            sunColorGradient.SetKeys(colorKeys, alphaKeys);
        }

        private void CreateDefaultAmbientGradient()
        {
            ambientColorGradient = new Gradient();
            GradientColorKey[] colorKeys = new GradientColorKey[5];
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];

            // Night - Dark blue
            colorKeys[0] = new GradientColorKey(new Color(0.1f, 0.1f, 0.2f), 0.0f);
            // Dawn - Purple-pink
            colorKeys[1] = new GradientColorKey(new Color(0.5f, 0.3f, 0.4f), 0.25f);
            // Day - Bright blue
            colorKeys[2] = new GradientColorKey(new Color(0.6f, 0.7f, 0.8f), 0.5f);
            // Dusk - Orange
            colorKeys[3] = new GradientColorKey(new Color(0.6f, 0.4f, 0.3f), 0.75f);
            // Night - Dark blue
            colorKeys[4] = new GradientColorKey(new Color(0.1f, 0.1f, 0.2f), 1.0f);

            alphaKeys[0] = new GradientAlphaKey(1.0f, 0.0f);
            alphaKeys[1] = new GradientAlphaKey(1.0f, 1.0f);

            ambientColorGradient.SetKeys(colorKeys, alphaKeys);
        }

        private void CreateDefaultFogGradient()
        {
            fogColorGradient = new Gradient();
            GradientColorKey[] colorKeys = new GradientColorKey[5];
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];

            // Night - Dark blue fog
            colorKeys[0] = new GradientColorKey(new Color(0.1f, 0.1f, 0.3f), 0.0f);
            // Dawn - Pink fog
            colorKeys[1] = new GradientColorKey(new Color(0.7f, 0.5f, 0.6f), 0.25f);
            // Day - Light blue fog
            colorKeys[2] = new GradientColorKey(new Color(0.7f, 0.8f, 0.9f), 0.5f);
            // Dusk - Orange fog
            colorKeys[3] = new GradientColorKey(new Color(0.8f, 0.6f, 0.4f), 0.75f);
            // Night - Dark blue fog
            colorKeys[4] = new GradientColorKey(new Color(0.1f, 0.1f, 0.3f), 1.0f);

            alphaKeys[0] = new GradientAlphaKey(1.0f, 0.0f);
            alphaKeys[1] = new GradientAlphaKey(1.0f, 1.0f);

            fogColorGradient.SetKeys(colorKeys, alphaKeys);
        }

        private void CreateDefaultIntensityCurve()
        {
            sunIntensityCurve = new AnimationCurve();
            sunIntensityCurve.AddKey(0.0f, 0.0f);   // Midnight
            sunIntensityCurve.AddKey(0.25f, 0.8f);  // Sunrise
            sunIntensityCurve.AddKey(0.5f, 1.2f);   // Noon
            sunIntensityCurve.AddKey(0.75f, 0.8f);  // Sunset
            sunIntensityCurve.AddKey(1.0f, 0.0f);   // Midnight
        }

        private void CreateDefaultFogCurve()
        {
            fogDensityCurve = new AnimationCurve();
            fogDensityCurve.AddKey(0.0f, 0.02f);   // Night - some fog
            fogDensityCurve.AddKey(0.25f, 0.03f);  // Dawn - more fog
            fogDensityCurve.AddKey(0.5f, 0.01f);   // Day - less fog
            fogDensityCurve.AddKey(0.75f, 0.03f);  // Dusk - more fog
            fogDensityCurve.AddKey(1.0f, 0.02f);   // Night - some fog
        }

        /// <summary>
        /// Get current time of day (0-24)
        /// </summary>
        public float GetTimeOfDay()
        {
            return timeOfDay;
        }

        /// <summary>
        /// Set time of day (0-24)
        /// </summary>
        public void SetTimeOfDay(float time)
        {
            timeOfDay = Mathf.Clamp(time, 0f, 24f);
            UpdateLighting();
        }

        /// <summary>
        /// Check if it's currently daytime
        /// </summary>
        public bool IsDaytime()
        {
            return timeOfDay >= 6f && timeOfDay <= 18f;
        }

        /// <summary>
        /// Check if it's currently nighttime
        /// </summary>
        public bool IsNighttime()
        {
            return !IsDaytime();
        }

        /// <summary>
        /// Pause or resume time progression
        /// </summary>
        public void SetPauseTime(bool pause)
        {
            pauseTime = pause;
        }

        /// <summary>
        /// Get formatted time string (12-hour format)
        /// </summary>
        public string GetFormattedTime()
        {
            int hour = Mathf.FloorToInt(timeOfDay);
            int minute = Mathf.FloorToInt((timeOfDay - hour) * 60f);
            
            bool isPM = hour >= 12;
            int displayHour = hour > 12 ? hour - 12 : (hour == 0 ? 12 : hour);
            
            string period = isPM ? "PM" : "AM";
            return $"{displayHour:00}:{minute:00} {period}";
        }
    }
}

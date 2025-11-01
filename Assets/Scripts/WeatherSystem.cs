using UnityEngine;
using System.Collections;

namespace VintageBeef.World
{
    /// <summary>
    /// Weather system with rain, fog, and visual effects
    /// Creates dynamic atmospheric conditions
    /// Optimized for Palia-style performance
    /// </summary>
    public class WeatherSystem : MonoBehaviour
    {
        [Header("Weather Settings")]
        [SerializeField] private float weatherCheckInterval = 60f; // Check every minute
        [SerializeField] private float weatherTransitionTime = 10f; // 10 seconds to transition
        [SerializeField] private float rainChance = 0.3f;
        [SerializeField] private float foggyChance = 0.2f;

        [Header("Rain Settings")]
        [SerializeField] private ParticleSystem rainParticles;
        [SerializeField] private AudioSource rainAudio;
        [SerializeField] private int maxRainParticles = 1000;
        [SerializeField] private float rainIntensity = 1f;

        [Header("Fog Settings")]
        [SerializeField] private float normalFogDensity = 0.01f;
        [SerializeField] private float foggyWeatherDensity = 0.05f;
        [SerializeField] private Color foggyColor = new Color(0.6f, 0.6f, 0.65f);

        [Header("Audio")]
        [SerializeField] private AudioClip rainSound;

        private WeatherType currentWeather = WeatherType.Clear;
        private WeatherType targetWeather = WeatherType.Clear;
        private float transitionProgress = 1f;
        private float checkTimer = 0f;

        private DayNightCycle dayNightCycle;

        private void Start()
        {
            dayNightCycle = GetComponent<DayNightCycle>();
            
            if (rainParticles == null)
            {
                CreateRainParticles();
            }

            if (rainAudio == null && rainSound != null)
            {
                rainAudio = gameObject.AddComponent<AudioSource>();
                rainAudio.clip = rainSound;
                rainAudio.loop = true;
                rainAudio.playOnAwake = false;
                rainAudio.volume = 0f;
            }

            StartCoroutine(WeatherCheckRoutine());
        }

        private void Update()
        {
            // Update weather transition
            if (transitionProgress < 1f)
            {
                transitionProgress += Time.deltaTime / weatherTransitionTime;
                transitionProgress = Mathf.Clamp01(transitionProgress);
                
                UpdateWeatherEffects(transitionProgress);
            }
        }

        private IEnumerator WeatherCheckRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(weatherCheckInterval);
                
                // Randomly determine next weather
                float roll = Random.value;
                
                if (roll < rainChance)
                {
                    SetWeather(WeatherType.Rain);
                }
                else if (roll < rainChance + foggyChance)
                {
                    SetWeather(WeatherType.Foggy);
                }
                else
                {
                    SetWeather(WeatherType.Clear);
                }
            }
        }

        public void SetWeather(WeatherType weather)
        {
            if (targetWeather == weather) return;

            Debug.Log($"Weather changing to: {weather}");
            
            targetWeather = weather;
            transitionProgress = 0f;
        }

        private void UpdateWeatherEffects(float progress)
        {
            switch (targetWeather)
            {
                case WeatherType.Clear:
                    TransitionToClear(progress);
                    break;
                
                case WeatherType.Rain:
                    TransitionToRain(progress);
                    break;
                
                case WeatherType.Foggy:
                    TransitionToFoggy(progress);
                    break;
            }

            if (progress >= 1f)
            {
                currentWeather = targetWeather;
            }
        }

        private void TransitionToClear(float progress)
        {
            // Fade out rain
            if (rainParticles != null)
            {
                var emission = rainParticles.emission;
                float rate = Mathf.Lerp(maxRainParticles, 0, progress);
                emission.rateOverTime = rate;

                if (progress >= 1f && rainParticles.isPlaying)
                {
                    rainParticles.Stop();
                }
            }

            // Fade out rain audio
            if (rainAudio != null)
            {
                rainAudio.volume = Mathf.Lerp(rainAudio.volume, 0f, progress);
                
                if (progress >= 1f && rainAudio.isPlaying)
                {
                    rainAudio.Stop();
                }
            }

            // Return fog to normal
            if (RenderSettings.fog)
            {
                RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, normalFogDensity, progress);
            }
        }

        private void TransitionToRain(float progress)
        {
            // Fade in rain
            if (rainParticles != null)
            {
                if (!rainParticles.isPlaying)
                {
                    rainParticles.Play();
                }

                var emission = rainParticles.emission;
                float rate = Mathf.Lerp(0, maxRainParticles * rainIntensity, progress);
                emission.rateOverTime = rate;
            }

            // Fade in rain audio
            if (rainAudio != null)
            {
                if (!rainAudio.isPlaying)
                {
                    rainAudio.Play();
                }
                rainAudio.volume = Mathf.Lerp(0f, 0.5f, progress);
            }

            // Slightly increase fog during rain
            if (RenderSettings.fog)
            {
                float targetFog = normalFogDensity * 1.5f;
                RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, targetFog, progress);
            }
        }

        private void TransitionToFoggy(float progress)
        {
            // Fade out rain if it was raining
            if (rainParticles != null && rainParticles.isPlaying)
            {
                var emission = rainParticles.emission;
                float rate = Mathf.Lerp(emission.rateOverTime.constant, 0, progress);
                emission.rateOverTime = rate;

                if (progress >= 1f)
                {
                    rainParticles.Stop();
                }
            }

            // Fade out rain audio
            if (rainAudio != null && rainAudio.isPlaying)
            {
                rainAudio.volume = Mathf.Lerp(rainAudio.volume, 0f, progress);
                
                if (progress >= 1f)
                {
                    rainAudio.Stop();
                }
            }

            // Increase fog density
            if (RenderSettings.fog)
            {
                RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, foggyWeatherDensity, progress);
                
                // Slightly adjust fog color for foggy weather
                Color currentFog = RenderSettings.fogColor;
                RenderSettings.fogColor = Color.Lerp(currentFog, foggyColor, progress * 0.3f);
            }
        }

        private void CreateRainParticles()
        {
            GameObject rainObj = new GameObject("RainParticles");
            rainObj.transform.SetParent(transform);
            rainObj.transform.localPosition = new Vector3(0, 20, 0);

            rainParticles = rainObj.AddComponent<ParticleSystem>();
            
            // Main module
            var main = rainParticles.main;
            main.startLifetime = 2f;
            main.startSpeed = 15f;
            main.startSize = 0.1f;
            main.startColor = new Color(0.7f, 0.7f, 0.8f, 0.5f);
            main.maxParticles = maxRainParticles;

            // Emission
            var emission = rainParticles.emission;
            emission.rateOverTime = 0; // Start with no rain

            // Shape
            var shape = rainParticles.shape;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = new Vector3(50, 1, 50);

            // Renderer
            var renderer = rainParticles.GetComponent<ParticleSystemRenderer>();
            renderer.renderMode = ParticleSystemRenderMode.Stretch;
            renderer.lengthScale = 2f;

            rainParticles.Stop();
        }

        /// <summary>
        /// Get current weather type
        /// </summary>
        public WeatherType GetCurrentWeather()
        {
            return currentWeather;
        }

        /// <summary>
        /// Force set weather immediately (for debugging)
        /// </summary>
        public void ForceWeather(WeatherType weather)
        {
            currentWeather = weather;
            targetWeather = weather;
            transitionProgress = 1f;
            UpdateWeatherEffects(1f);
        }

        /// <summary>
        /// Check if it's currently raining
        /// </summary>
        public bool IsRaining()
        {
            return currentWeather == WeatherType.Rain || 
                   (targetWeather == WeatherType.Rain && transitionProgress < 1f);
        }

        /// <summary>
        /// Check if it's currently foggy
        /// </summary>
        public bool IsFoggy()
        {
            return currentWeather == WeatherType.Foggy ||
                   (targetWeather == WeatherType.Foggy && transitionProgress < 1f);
        }
    }

    public enum WeatherType
    {
        Clear,
        Rain,
        Foggy,
        // Future: Snow, Storm, etc.
    }
}

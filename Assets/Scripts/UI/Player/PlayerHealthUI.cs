using UnityEngine;
using System.Collections;
using FluxFramework.UI;
using FluxFramework.Core;
using Events;

public class PlayerHealthUI : FluxUIComponent
{
    [Header("Health Vignette")]
    [Tooltip("Drag the Quad Renderer here.")]
    [SerializeField] private Renderer quadRenderer;
    [SerializeField] private Color healthyColor = new Color(1, 1, 1, 0);
    [SerializeField] private Color criticalColor = new Color(1, 0, 0, 1);
    [SerializeField] private float pulseSpeed = 5f;
    [SerializeField] private float pulseIntensity = 0.5f;

    [Header("Damage Streaks Particles")]
    [Tooltip("Drag the Damage Streaks Particle System here.")]
    [SerializeField] private ParticleSystem damageStreaksSystem;
    [SerializeField] private float particleSpeed = 50f;
    private ParticleSystem.EmissionModule streaksEmission;
    private ParticleSystem.VelocityOverLifetimeModule streaksVelocity;

    [Header("Advanced Effects")]
    [SerializeField] private Color healColor = Color.green;
    [SerializeField] private float criticalHealthThreshold = 15f;
    [SerializeField] private float glitchStrengthAtCritical = 0.01f;

    private Material vignetteMaterial;
    private Camera mainCamera;


    private float currentHealth;
    private float maxHealth;

    protected override void OnFluxStart()
    {
        base.OnFluxStart();
        if (quadRenderer != null)
        {
            vignetteMaterial = quadRenderer.material;
        }
        else
        {
            Debug.LogError("Quad Renderer not assigned in PlayerHealthUI.");
        }

        mainCamera = Camera.main;

        if (damageStreaksSystem != null)
        {
            streaksEmission = damageStreaksSystem.emission;
            streaksVelocity = damageStreaksSystem.velocityOverLifetime;
            streaksEmission.rateOverTime = 0;
        }
        
        FluxFramework.Core.Flux.Manager.EventBus.Subscribe<GetDamageEvent>(OnGetDamage);
    }

    void Update()
    {
        currentHealth = FluxFramework.Core.Flux.Manager.Properties.GetProperty<float>("Player.health");
        maxHealth = FluxFramework.Core.Flux.Manager.Properties.GetProperty<float>("Player.maxHealth");

        float healthPercent = currentHealth / maxHealth;
        float effectIntensity = 1.0f - healthPercent;

        UpdateVignette(effectIntensity);
        UpdateDamageStreaks(effectIntensity);
        UpdateGlitchEffect();
    }
    
    private void OnGetDamage(GetDamageEvent evt)
    {
        TriggerDamageIndicator(evt.fromDirection);
    }

    public void TriggerDamageIndicator(Vector3 damageSourceWorldPosition)
    {
        if (vignetteMaterial == null || mainCamera == null) return;

        // 1. Calculate the true 3D direction to the damage source
        Vector3 directionToDamage = (damageSourceWorldPosition - mainCamera.transform.position);

        // 2. Verify if the damage source is in front or behind the player
        // Vector3.Dot > 0  => Target in front
        // Vector3.Dot < 0  => Target behind
        float dotProduct = Vector3.Dot(mainCamera.transform.forward, directionToDamage.normalized);

        Vector2 screenDirection;
        Vector3 screenPos = mainCamera.WorldToScreenPoint(damageSourceWorldPosition);

        if (dotProduct > 0) // --- NORMAL CASE : ENEMY IS IN FRONT ---
        {
            screenDirection = new Vector2(screenPos.x / Screen.width, screenPos.y / Screen.height);
            screenDirection = (screenDirection - new Vector2(0.5f, 0.5f)).normalized;
        }
        else // --- SPECIAL CASE : ENEMY IS BEHIND ---
        {
            // We invert the projected position to get the true off-screen direction
            screenDirection = new Vector2(screenPos.x / Screen.width, screenPos.y / Screen.height);
            screenDirection = (screenDirection - new Vector2(0.5f, 0.5f));
            screenDirection *= -1; // Crucial inversion step
            screenDirection.Normalize();
        }

        vignetteMaterial.SetVector("_Damage_Direction", screenDirection);
        StartCoroutine(FadeEffect("_Damage_Intensity", 2.0f, 0.5f));

        if (damageStreaksSystem != null)
        {
            Vector3 directionFromDamage = (mainCamera.transform.position - damageSourceWorldPosition).normalized;

            streaksVelocity.x = new ParticleSystem.MinMaxCurve(particleSpeed * directionFromDamage.x);
            streaksVelocity.y = new ParticleSystem.MinMaxCurve(particleSpeed * directionFromDamage.y);
            streaksVelocity.z = new ParticleSystem.MinMaxCurve(particleSpeed * directionFromDamage.z);
            
            damageStreaksSystem.Emit(Random.Range(20, 31));
        }
    }

    public void TriggerHealEffect()
    {
        if (vignetteMaterial == null) return;
        vignetteMaterial.SetColor("_Heal_Color", healColor);
        StartCoroutine(FadeEffect("_Heal_Intensity", 1.0f, 0.4f));
    }

    private IEnumerator FadeEffect(string propertyName, float startValue, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float value = Mathf.Lerp(startValue, 0f, timer / duration);
            vignetteMaterial.SetFloat(propertyName, value);
            yield return null;
        }
        vignetteMaterial.SetFloat(propertyName, 0f);
    }
    
    private void UpdateVignette(float intensity)
    {
        if (vignetteMaterial == null) return;

        // 1. We calculate the base color based on health
        Color baseColor = Color.Lerp(healthyColor, criticalColor, intensity);

        // 2. We calculate a safe pulse factor oscillating between 0 and 1
        // (Mathf.Sin(...) + 1) gives a range of [0, 2], we divide by 2 to get [0, 1]
        float pulseFactor = (Mathf.Sin(Time.time * pulseSpeed) + 1) / 2f;

        // 3. We apply this pulse to increase the alpha of the base color
        // We only add it when the effect is already a bit visible (intensity > 0.1f)
        if (intensity > 0.1f)
        {
            // We add the pulse to the base alpha. pulseIntensity controls the strength of the "breathing"
            float pulsedAlpha = baseColor.a + (pulseFactor * pulseIntensity * intensity);
            baseColor.a = Mathf.Clamp01(pulsedAlpha); // We ensure that the alpha never exceeds 1
        }

        // 4. We send the final and clean color to the material
        vignetteMaterial.SetColor("_Vignette_Color", baseColor);
    }

    private void UpdateGlitchEffect()
    {
        if (vignetteMaterial == null) return;

        if (currentHealth <= criticalHealthThreshold)
        {
            float flicker = Mathf.PerlinNoise(Time.time * 10f, 0f) * 0.5f + 0.5f;
            vignetteMaterial.SetFloat("_Glitch_Strength", glitchStrengthAtCritical * flicker);
        }
        else
        {
            vignetteMaterial.SetFloat("_Glitch_Strength", 0f);
        }
    }
    
    private void UpdateDamageStreaks(float intensity)
    {
        // if (damageStreaksSystem == null) return;

        // streaksEmission.rateOverTime = intensity * 100f;
    }
}
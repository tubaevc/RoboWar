using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrailSystem : MonoBehaviour
{
    //Trail Settings
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private float trailTime = 0.2f;
    [SerializeField] private Color trailColor = Color.yellow;

    //Particle Settings
    [SerializeField] private ParticleSystem bulletParticles;
    [SerializeField] private float particleLifetime = 0.5f;
    [SerializeField] private int emissionRate = 10;

    private void Awake()
    {
        SetupTrailRenderer();
        SetupParticleSystem();
    }

    private void SetupTrailRenderer()
    {
        if (trailRenderer == null)
            trailRenderer = gameObject.AddComponent<TrailRenderer>();

        Material trailMaterial = new Material(Shader.Find("Sprites/Default"));
        trailMaterial.color = trailColor;
        trailRenderer.material = trailMaterial;

        trailRenderer.time = trailTime;
        trailRenderer.startWidth = 0.1f;
        trailRenderer.endWidth = 0.05f;

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(trailColor, 0.0f),
                new GradientColorKey(trailColor, 1.0f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1.0f, 0.0f),
                new GradientAlphaKey(0.0f, 1.0f)
            }
        );
        trailRenderer.colorGradient = gradient;
    }

    private void SetupParticleSystem()
    {
        if (bulletParticles == null)
            bulletParticles = gameObject.AddComponent<ParticleSystem>();

        bulletParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        var main = bulletParticles.main;
        main.duration = particleLifetime;
        main.loop = true;
        main.startSpeed = 2f;
        main.startSize = 0.1f;
        main.startColor = trailColor;

        var emission = bulletParticles.emission;
        emission.rateOverTime = emissionRate;

        var shape = bulletParticles.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.1f;

        bulletParticles.Play();
    }

    public void ChangeColor(Color newColor)
    {
        trailColor = newColor;
        trailRenderer.material.color = newColor;

        var main = bulletParticles.main;
        main.startColor = newColor;
    }
}

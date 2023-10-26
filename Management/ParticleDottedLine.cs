using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleDottedLine : MonoBehaviour
{

	public Vector3 begin;
	public Vector3 end;
	public float interval = 0.24f;
	public float offset = 0f;
	public float particleSize = 0.08f;
	public Material particleMaterial;

#if UNITY_EDITOR
    public bool drawGizmo = true;
	public Color gizmoColor = Color.yellow;
	public float gizmoSize = 0.1f;
#endif

    private new ParticleSystem particleSystem;
	private new ParticleSystemRenderer renderer;
	private ParticleSystem.Particle[] particles;


	void Start()
	{
		particleSystem = GetComponent<ParticleSystem>();

		var emission = particleSystem.emission;
		emission.enabled = false;
		var shape = particleSystem.shape;
		shape.enabled = false;

		particleSystem.startLifetime = float.MaxValue;
		particleSystem.startSpeed = 0;
		particleSystem.startSize = particleSize;
		particleSystem.playOnAwake = false;
		particleSystem.maxParticles = 300;
        particleSystem.simulationSpace = ParticleSystemSimulationSpace.World;

        renderer = GetComponent<ParticleSystemRenderer>();

		if (particleMaterial != null)
		{
			renderer.sharedMaterial = particleMaterial;
		}
	}

	void LateUpdate()
	{
		float distance = Vector3.Distance(begin, end);
		int count = (int)((distance + interval * 0.5f) / interval);

		int particleCount = particleSystem.particleCount;
		if (count > particleCount)
		{
			particleSystem.Emit(count - particleCount);
		}

        if (particles == null || particles.Length < particleSystem.maxParticles)
        {
            particles = new ParticleSystem.Particle[particleSystem.maxParticles];
        }

        particleCount = particleSystem.GetParticles(particles);

		Vector3 pos = begin;
		Vector3 dir = (end - begin).normalized;
		Vector3 increment = dir * interval;
		offset %= interval;
		if (offset < 0)
		{
			offset += interval;
		}
		pos += dir * offset;

		for (int i = 0; i < particleCount; ++i)
		{
			particles[i].position = pos;
			particles[i].velocity = Vector3.zero;
			particles[i].startSize = particleSize;

            if (i >= count)
			{
				particles[i].startLifetime = 0;
				particles[i].lifetime = 0;
			}

			pos += increment;
		}

		// Apply the particle changes to the particle system
		particleSystem.SetParticles(particles, particleCount);
	}

#if UNITY_EDITOR
    void OnDrawGizmos()
	{
        if (drawGizmo)
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawSphere(begin, gizmoSize);
            Gizmos.DrawSphere(end, gizmoSize);
        }
    }
#endif
}

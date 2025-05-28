using UnityEngine;

public class ParticleManager : CoreComponent
{
    private Transform particleContainer;

    protected override void Awake()
    {
        base.Awake();

        particleContainer = GameObject.FindGameObjectWithTag("ParticleContainer").transform;
    }

    public GameObject StartParticle(GameObject particlePrefab, Vector2 position, Quaternion rotation)
    {
        return Instantiate(particlePrefab, position, rotation, particleContainer);
        // ParticleSystem ps = particle.GetComponent<ParticleSystem>();

        // if (ps != null)
        // {
        //     ps.Play();
        // }

        // return particle;
    }

    public GameObject StartParticle(GameObject particlePrefab)
    {
        return StartParticle(particlePrefab, transform.position, Quaternion.identity);
    }

    public GameObject StartParticleWithRandomRotation(GameObject particlePrefab)
    {
        var randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        return StartParticle(particlePrefab, transform.position, randomRotation);
    }

}

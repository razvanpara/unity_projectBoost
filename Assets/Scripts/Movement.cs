using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DeltaTimeValuesExt
{
    public static Vector3 DtV(this Vector3 vector) => vector * Time.deltaTime;
    public static float DtV(this float n) => n * Time.deltaTime;
    public static float DtV(this double n) => (float)(n * Time.deltaTime);
    public static float DtV(this int n) => (float)n * Time.deltaTime;
}

public static class Arrays
{
    public static T[] Create<T>(params T[] items) => items;
}

public enum RotationDirection
{
    RIGHT = -1,
    LEFT = 1
}

public partial class Movement : MonoBehaviour
{
    [SerializeField] float mainThrust = 1000;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] float rotationThrust = 90;
    [SerializeField] ParticleSystem leftThrusterParticles;
    [SerializeField] ParticleSystem rightThrusterParticles;
    [SerializeField] ParticleSystem mainThrusterParticles;
    private Rigidbody rb;
    private AudioSource aSrc;
    private float dt;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        aSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        dt = Time.deltaTime;
        var playerInput = GetComponent<PlayerInput>();
        ProcessInput(playerInput);
    }

    internal class AudioMapping
    {
        private AudioMapping(AudioSource aSrc, AudioClip aClip)
        {
            AudioSource = aSrc;
            AudioClip = aClip;
        }
        public AudioSource AudioSource { get; }
        public AudioClip AudioClip { get; }
        public static AudioMapping Create(AudioSource aSrc, AudioClip aClip) => new AudioMapping(aSrc, aClip);
    }
    private void ProcessInput(PlayerInput input)
    {
        ProcessThrust(input);
        ProcessRotation(input);
    }
    private void ProcessThrust(PlayerInput input)
    {
        if (input.Thrust)
            StartThrusting();
        else
            StopThrusting();
    }
    private void ProcessRotation(PlayerInput input)
    {
        if (input.RotateLeft)
            Rotate(RotationDirection.LEFT, leftThrusterParticles, rightThrusterParticles);
        else if (input.RotateRight)
            Rotate(RotationDirection.RIGHT, rightThrusterParticles, leftThrusterParticles);
        else
            StopParticles(Arrays.Create(rightThrusterParticles, leftThrusterParticles));
    }
    private void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up.DtV() * mainThrust);
        PlayParticles(Arrays.Create(mainThrusterParticles));
        PlaySounds(Arrays.Create(AudioMapping.Create(aSrc, mainEngine)));
    }
    private void StopThrusting()
    {
        StopParticles(Arrays.Create(mainThrusterParticles));
        StopSounds(Arrays.Create(aSrc));
    }
    private void Rotate(RotationDirection direction, ParticleSystem toStop, ParticleSystem toStart)
    {
        StopParticles(Arrays.Create(toStop));
        ApplyRotation((float)direction * rotationThrust);
        PlayParticles(Arrays.Create(toStart));
    }
    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward.DtV() * rotationThisFrame);
        rb.freezeRotation = false;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
    }

    private static void PlayParticles(ParticleSystem[] particles)
    {
        foreach (var particleSys in particles)
            if (!particleSys.isPlaying)
                particleSys.Play();
    }
    private static void StopParticles(ParticleSystem[] particles)
    {
        foreach (var particleSys in particles)
            particleSys.Stop();
    }
    private static void PlaySounds(AudioMapping[] audioMappings)
    {
        Debug.Log(String.Join(" ", audioMappings.Select(map => $"{map.AudioClip} {map.AudioSource}")));
        foreach (var mapping in audioMappings)
            if (!mapping.AudioSource.isPlaying)
                mapping.AudioSource.PlayOneShot(mapping.AudioClip);
    }
    private static void StopSounds(AudioSource[] audioSources)
    {
        foreach (var audioSource in audioSources)
            audioSource.Stop();
    }
}

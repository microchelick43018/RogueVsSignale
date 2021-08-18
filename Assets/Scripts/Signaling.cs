using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(BoxCollider2D))]

public class Signaling : MonoBehaviour
{
    [SerializeField] private float _soundTransitTime;
    [SerializeField] private List<Transform> _owners = new List<Transform>();

    private UnityEvent _outsiderEntered = new UnityEvent();
    private UnityEvent _outsiderCameOut = new UnityEvent();
    private AudioSource _signalAudio;

    public event UnityAction OutsiderCameOut
    {
        add => _outsiderCameOut.AddListener(value);
        remove => _outsiderCameOut.RemoveListener(value);
    }
    public event UnityAction OutSiderEntered
    {
        add => _outsiderEntered.AddListener(value);
        remove => _outsiderEntered.RemoveListener(value);
    }

    public void CheckForOwner(Transform incomingPerson)
    {
        if (_signalAudio.isPlaying)
            return;

        bool isOwner = false;
        foreach (var owner in _owners)
        {
            if (owner.name == incomingPerson.name)
            {
                return;
            }
        }
        if (isOwner == false)
        {
            _outsiderEntered?.Invoke();
        }
    }

    public void MakeNoise()
    {
        if (_signalAudio.isPlaying == true)
            return;
        _signalAudio.volume = 0;
        _signalAudio.Play();
        StartCoroutine(VarySound());
    }

    private void Awake()
    {
        _signalAudio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        OutSiderEntered += MakeNoise;
        OutsiderCameOut += _signalAudio.Stop;
    }

    private void OnDisable()
    {
        OutSiderEntered -= MakeNoise;
        OutsiderCameOut -= _signalAudio.Stop;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_signalAudio.isPlaying == true && collision.TryGetComponent(out CharacterMover player))
        {
            _outsiderCameOut?.Invoke();
        }
    }

    private IEnumerator VarySound()
    {
        WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
        float timeCounter = 0;
        while (_signalAudio.isPlaying)
        {
            while (timeCounter < _soundTransitTime)
            {
                timeCounter += Time.deltaTime;
                _signalAudio.volume = timeCounter / _soundTransitTime;
                yield return endOfFrame;
            }
            while (timeCounter > 0)
            {
                timeCounter -= Time.deltaTime;
                _signalAudio.volume = timeCounter / _soundTransitTime;
                yield return endOfFrame;
            }
            yield return endOfFrame;
        }
    }
}

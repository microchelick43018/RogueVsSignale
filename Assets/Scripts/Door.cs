using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class Door : MonoBehaviour
{
    [SerializeField] private float _openingForce;
    [SerializeField] private float _maxOpenAngle;

    private Signaling _houseSignaling;
    private CharacterMover _incomingPlayer;
    private bool _isOpening = false;

    public bool IsOpen { get; private set; }

    private void Awake()
    {
        _houseSignaling = GetComponentInParent<Signaling>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsOpen == false && collision.TryGetComponent(out _incomingPlayer))
        {
            _isOpening = true;
            StartCoroutine(Opening());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out CharacterMover outComingPlayer))
        {
            IsOpen = false;
            _isOpening = false;
            StartCoroutine(Closing());
        }
    }

    private IEnumerator Opening()
    {
        var endOfFrame = new WaitForEndOfFrame();
        while (transform.localEulerAngles.y <= _maxOpenAngle && _isOpening)
        {
            transform.Rotate(0, _openingForce, 0);
            yield return endOfFrame;
        }
        IsOpen = transform.localEulerAngles.y >= _maxOpenAngle;
        if (IsOpen)
        {
            _isOpening = false;
            _houseSignaling.CheckForOwner(_incomingPlayer.transform);
        }
    }

    private IEnumerator Closing()
    {
        var endOfFrame = new WaitForEndOfFrame();
        while (transform.localEulerAngles.y >= 0.01f && _isOpening == false)
        {
            transform.Rotate(0, _openingForce * -1, 0);
            yield return endOfFrame;
        }
    }
}

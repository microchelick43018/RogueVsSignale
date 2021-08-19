using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerAnimController))]

public class CharacterMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Sides _startTurnDirection;

    private Animator _animator;

    private enum Sides
    {
        left = 0,
        right = 1
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float axisValue = Input.GetAxis("Horizontal");
        TurnToCorrectSide(axisValue);
        transform.Translate(transform.right * axisValue * Time.deltaTime * _speed);
        _animator.SetFloat(PlayerAnimController.Params.Speed, _speed * Mathf.Abs(axisValue));
        
    }

    private void TurnToCorrectSide(float axisValue)
    {
        if (axisValue > 0)
        {
            transform.localRotation = new Quaternion(0, (int)_startTurnDirection * 180, 0, 0);
        }
        else if (axisValue < 0)
        {
            transform.localRotation = new Quaternion(0, (int)(_startTurnDirection - 1) * 180, 0, 0);
        }
    }
}

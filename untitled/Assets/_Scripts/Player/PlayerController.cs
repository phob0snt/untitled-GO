using UnityEngine;
using Zenject;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController _controller;
    [SerializeField] private DynamicJoystick _joystick;
    private Vector3 _moveDirection;

    [SerializeField] private float _moveSpeed = 6f;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        _moveDirection = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
        float rotationAngle = Mathf.Atan2(_joystick.Horizontal, _joystick.Vertical) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, rotationAngle, 0);
        _controller.Move(_moveDirection * _moveSpeed * Time.deltaTime);
    }
}

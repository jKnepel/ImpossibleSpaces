using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    private Rigidbody _rb;
    private Vector2 _movementInput;
    private Vector2 _rotationInput;
    private bool _isFlying = false;

    [Header("Player Objects")]
    [SerializeField] private Transform _camera;
    [SerializeField] private GameObject _playerVisuals;
    
    [Header("Player Attributes")]
    private float _speed;
    [SerializeField] private float _walkingSpeed;
    [SerializeField] private float _runningSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Vector3 _firstPersonPosition;
    [SerializeField] private Vector3 _flyingPosition;

    [Header("External Objects")]
    [SerializeField] private RectTransform _UICoordinate;
    [SerializeField] private Image _UIButtonQ;

	private void Awake()
	{
        _speed = _walkingSpeed;
        _rb = transform.GetComponent<Rigidbody>();
    }

	private void Update()
	{
        ManageInput();

        if (_isFlying)
		{
            _camera.LookAt(transform);
		}
        else
		{
            Vector3 deltaRotation = new Vector3(-_rotationInput.y, 0, 0) * _rotationSpeed * 100 * Time.deltaTime;
            Vector3 targetRotation = _camera.transform.eulerAngles + deltaRotation;
            targetRotation.x = targetRotation.x < 180 ? Mathf.Clamp(targetRotation.x, -1, 50) : Mathf.Clamp(targetRotation.x, 330, 361);
            _camera.eulerAngles = targetRotation;

            _UICoordinate.rotation = Quaternion.LookRotation(new Vector3(0, 0, 1), Vector3.up);
		}
    }

	private void FixedUpdate()
    {
        Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, _rotationInput.x, 0) * _rotationSpeed * 100 * Time.fixedDeltaTime);
        _rb.MoveRotation(_rb.rotation * deltaRotation);

        Vector3 direction = _isFlying
            ? new Vector3(_movementInput.x, 0, _movementInput.y).normalized
            : (_movementInput.x * transform.right + _movementInput.y * transform.forward).normalized;
        _rb.MovePosition(transform.position + direction * Time.fixedDeltaTime * _speed);
    }

    private void ManageInput()
	{
        _movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _rotationInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if (Input.GetKeyDown(KeyCode.Escape))
		{
            Application.Quit();
		}
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _speed = _runningSpeed;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed = _walkingSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _UIButtonQ.color = new Color(1, 1, 1, 1);
            _isFlying = !_isFlying;
            _playerVisuals.SetActive(_isFlying);
            _UICoordinate.gameObject.SetActive(!_isFlying);
            _camera.localEulerAngles = new Vector3(0, 0, 0);
            _camera.localPosition = _isFlying ? _flyingPosition : _firstPersonPosition;
            _UIButtonQ.transform.GetChild(1).GetComponent<TMP_Text>().text = _isFlying ? "First Person View" : "Top-Down View";
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            _UIButtonQ.color = new Color(1, 1, 1, 0.5f);
        }
    }
}

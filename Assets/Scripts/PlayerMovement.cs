using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(Animator))]
public class PlayerMovement : MonoBehaviour {

	public float speed = 7f;
	public float jumpForce = 10f;

	public float jumpTime;
	
	public float checkRadius;
	public LayerMask groundMask;
	
	private Rigidbody2D _rigidbody;
	private BoxCollider2D _collider;
	private Animator _animator;
	
	private float _moveInput;
	private bool _isGrounded;
	private float _jumpTimeCount;
	private bool _isJumping;
	private static readonly int IsJumping = Animator.StringToHash("isJumping");
	private static readonly int IsMoving = Animator.StringToHash("isMoving");


	private void Start() {
		_rigidbody = GetComponent<Rigidbody2D>();
		_collider = GetComponent<BoxCollider2D>();
		_animator = GetComponent<Animator>();
		_isJumping = false;
	}

	private void FixedUpdate() {
		_moveInput = Input.GetAxis("Horizontal");
		if (_moveInput < 0) {
			// ReSharper disable once Unity.InefficientPropertyAccess
			transform.eulerAngles = Vector3.up * 180f;
		}
		if (_moveInput > 0) {
			transform.eulerAngles = Vector3.zero;
		}

		if (_moveInput != 0) {
			_animator.SetBool(IsMoving, true);
		} else if (_moveInput == 0) {
			_animator.SetBool(IsMoving, false);
		}
		_rigidbody.velocity = new Vector2(_moveInput * speed, _rigidbody.velocity.y);
	}

	private void Update() {
		_isGrounded = Physics2D.OverlapCircle((new Vector2(transform.position.x, _collider.bounds.min.y)) , checkRadius, groundMask);

		if (_isGrounded && Input.GetKeyDown(KeyCode.Space)) {
			_jumpTimeCount = jumpTime;
			_rigidbody.velocity = Vector2.up * jumpForce;
			_isJumping = true;
		}

		if (Input.GetKey(KeyCode.Space) && _isJumping) {
			if (_jumpTimeCount > 0) {
				_rigidbody.velocity = Vector2.up * jumpForce;
				_jumpTimeCount -= Time.deltaTime;
			}
			else
				_isJumping = false;
		}

		if (Input.GetKeyUp(KeyCode.Space)) {
			_isJumping = false;
		}
		
		_animator.SetBool(IsJumping, !_isGrounded);
	}



}
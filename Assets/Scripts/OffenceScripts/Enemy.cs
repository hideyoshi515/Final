using System;
using System.Collections;

using UnityEngine;

#pragma warning disable CS0108, CS0114

public class Enemy : MonoBehaviour {
	[SerializeField] private float speed;
	[SerializeField] private float health;
	[SerializeField] private float maxHealth;

	[SerializeField] private RuntimeAnimatorController[] runtimeAnimatorControllers;

	[SerializeField] private Rigidbody2D target;

	private bool isLive;

	private Rigidbody2D rigidbody2D;
	private Collider2D collider2D;
	private Animator animator;
	private SpriteRenderer spriteRenderer;
	private WaitForFixedUpdate waitForFixedUpdate;

	private GameManager gameManager;
	private Player player;

	private Vector3 scale;

	private void Awake() {
		rigidbody2D = GetComponent<Rigidbody2D>();
		collider2D = GetComponent<Collider2D>();
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		waitForFixedUpdate = new WaitForFixedUpdate();

		gameManager = GameManager.Instance;
		player = GameManager.Instance.Player[GameManager.Instance.PlayerId];

		scale = transform.localScale;

		animator.SetFloat("RunState", 0.25f);
	}

	private void Update() {
		if (player.gameObject.name != "Dummy") return;

		player = GameManager.Instance.Player[GameManager.Instance.PlayerId];
	}

	private void FixedUpdate() {
		if (!gameManager.IsLive) return;

		if (!isLive || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")) return;

		Vector2 directionVector2 = target.position - rigidbody2D.position;
		Vector2 nextVector2 = directionVector2.normalized * speed * Time.fixedDeltaTime;

		rigidbody2D.MovePosition(rigidbody2D.position + nextVector2);
		rigidbody2D.velocity = Vector2.zero;
	}

	private void LateUpdate() {
		if (!gameManager.IsLive) return;

		if (!isLive) return;

		if (target.position.x < rigidbody2D.position.x) scale.x = 1;
		else if (target.position.x > rigidbody2D.position.x) scale.x = -1;

		transform.localScale = scale;
	}

	private void OnEnable() {
		target = player.GetComponent<Rigidbody2D>();

		isLive = true;
		collider2D.enabled = true;
		rigidbody2D.simulated = true;
		spriteRenderer.sortingOrder = 2;
		animator.SetBool("Die", false);

		health = maxHealth;
	}

	public void Initialized(SpawnData spawnData) {
		speed = spawnData.Speed;
		maxHealth = spawnData.Health;
		health = spawnData.Health;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (!other.CompareTag("Bullet") || !isLive) return;

		health -= other.GetComponent<Bullet>().Damage();

		StartCoroutine(KnockBack());
		// StartCoroutine("KnockBack");

		if (health > 0) {
			animator.SetFloat("RunState", 1);

			AudioManager.Instance.PlaySfx(AudioManager.Sfx.Hit);
		} else {
			isLive = false;
			collider2D.enabled = false;
			rigidbody2D.simulated = false;
			spriteRenderer.sortingOrder = 1;
			animator.SetBool("Die", true);

			if (gameManager.IsLive) {
				gameManager.Kill++;
				gameManager.GetExp();

				AudioManager.Instance.PlaySfx(AudioManager.Sfx.Dead);
			}
		}
	}

	IEnumerator KnockBack() {
		// yield return null; // 1프레임 쉬기

		// yield return new WaitForSeconds(2f); // 2초 쉬기

		yield return waitForFixedUpdate; // 다음 하나의 무리 프레임 딜레이

		Vector3 playerPosition = player.transform.position;
		Vector3 directionVector3 = transform.position - playerPosition;

		rigidbody2D.AddForce(directionVector3.normalized * 3, ForceMode2D.Impulse);
	}

	private void Dead() {
		transform.parent.gameObject.SetActive(false);
	}
}
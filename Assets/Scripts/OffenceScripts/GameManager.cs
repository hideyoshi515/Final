using System;

using UnityEngine;
using UnityEngine.Animations;

public class GameManager : MonoBehaviour {
	#pragma warning disable CS0618
	private static GameManager instance;

	[SerializeField] private Player player;

	[SerializeField] private PoolManager poolManager;

	[SerializeField] private float gameTime;
	[SerializeField] private float maxGameTime = 2 * 10f;

	public static GameManager Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType<GameManager>();

				if (instance == null) {
					CustomLogger.LogError("No Singleton Object");
				} else {
					instance.Initialize();
				}
			}

			return instance;
		}
	}

	public Player Player => player;
	public PoolManager PoolManager => poolManager;
	public float GameTime => gameTime;

	private void Awake() {
		if (instance == null) instance = this;
		else if (instance != this) Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}

	private void Update() {
		gameTime += Time.deltaTime;

		if (gameTime > maxGameTime) {
			gameTime = maxGameTime;
		}
	}

	private void Initialize() {
		if (player == null) {
			player = FindObjectOfType<Player>();

			if (player == null) {
				CustomLogger.LogError("Player instance not found");
			} else {
				CustomLogger.Log("Player instance found and assigned");
			}
		}

		if (poolManager == null) {
			poolManager = FindObjectOfType<PoolManager>();

			if (poolManager == null) {
				CustomLogger.LogError("PoolManager instance not found");
			} else {
				CustomLogger.Log("PoolManager instance found and assigned");
			}
		}
	}
}
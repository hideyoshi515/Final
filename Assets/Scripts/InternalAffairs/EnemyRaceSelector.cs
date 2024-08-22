﻿using System;

using UnityEngine;

using Random = UnityEngine.Random;

namespace InternalAffairs {
	public class EnemyRaceSelector : MonoBehaviour {
		public static EnemyRaceSelector Instance { get; private set; }
		[SerializeField] public string[] enemyRaces;
		[SerializeField] public string SelectedRace;
		private int randomIndex;
		public int stageCount;
		public int weekCount;

		private void Awake() {
			CustomLogger.Log("EnemyRaceSelector Awake()진입", "black");

			if (Instance == null) {
				Instance = this;
				DontDestroyOnLoad(this.gameObject);
			} else if (Instance != this) {
				Destroy(this.gameObject);
			}

			weekCount = PlayerSyncManager.Instance.Repeat;
			stageCount = PlayerLocalManager.Instance.L_Stage;
			CustomLogger.Log("위크카운트  : " + weekCount + ", 업데이트 전 스테이지카운트 : " + stageCount, "white");

			CustomLogger.Log("save상 선택된 종족: " + PlayerLocalManager.Instance.lSelectedRace, Color.cyan);

			if (string.IsNullOrEmpty(PlayerLocalManager.Instance.lSelectedRace)) {
				CustomLogger.Log("종족 선택되지 않음. 종족 선택으로 이행", Color.cyan);

				if (stageCount == 0) {
					PlayerLocalManager.Instance.ResetHealthData();
				}

				// 종족 선택 및 저장
				SelectAndSaveRace();
			} else {
				SelectedRace = PlayerLocalManager.Instance.lSelectedRace;
				PlayerLocalManager.Instance.Save();
				CustomLogger.Log("Save데이터 상에 종족이 선택되어 있으므로 그 값을 받아옴 : " + SelectedRace, Color.cyan);
			}
		}

		private void SelectAndSaveRace() {
			SelectRandomRace();
			CustomLogger.Log("SelectedRace:" + SelectedRace, Color.cyan);

			// 선택된 종족을 PlayerLocalManager에 저장
			PlayerLocalManager.Instance.lSelectedRace = SelectedRace;
			CustomLogger.Log("로컬매니저에 저장된 종족 : " + PlayerLocalManager.Instance.lSelectedRace, "white");

			PlayerLocalManager.Instance.Save();
		}

		private void SelectRandomRace() {
			// 랜덤으로 lStageRace 배열에서 종족 선택
			randomIndex = Random.Range(0, PlayerLocalManager.Instance.lStageRace.Length);
			SelectedRace = PlayerLocalManager.Instance.lStageRace[randomIndex];
		}
	}
}
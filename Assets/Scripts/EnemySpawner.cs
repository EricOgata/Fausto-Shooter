using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] bool looping = false;

    int startingWave = 0; // waveIndex

	// Use this for initialization
	IEnumerator Start () {
        do {
            yield return StartCoroutine(SpawnAllWaves());
        } while (looping);
	}

    private IEnumerator SpawnAllWaves() {
        for(int i = startingWave; i < waveConfigs.Count; i++) {
            var currentWave = waveConfigs[i];
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
        }
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig currentWave) {
        for(var i = 0; i < currentWave.GetNumberOfEnemies(); i++) {
            var newEnemy = Instantiate(
                currentWave.GetEnemyPrefab(),
                currentWave.GetWaypoints()[0].transform.position,
                Quaternion.identity);
            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(currentWave);
            yield return new WaitForSeconds(currentWave.GetTimeBetweenSpanws());
        }
    }
}

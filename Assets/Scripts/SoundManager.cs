using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomSoundSpawner : MonoBehaviour {

    [System.Serializable]
    public class Clip {
        public AudioClip audio_clip;
        public float volume;
    }

    public List<Clip> soundClips; // Массив возможных звуков
    public float minDistance = 20f; // Минимальное расстояние от игрока
    public float maxDistance = 40f; // Максимальное расстояние от игрока
    public GameObject soundPrefab; // Префаб с AudioSource

    private Transform player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(SpawnSoundRoutine());
    }

    private IEnumerator SpawnSoundRoutine() {
        while (true) {
            float minTime = MalfunctionManager.Instance.GetTimeInt();
            yield return new WaitForSeconds(Random.Range(minTime, minTime * 2));
            SpawnSound();
        }
    }

    private void SpawnSound() {
        if (player == null) return;

        float angle = Random.Range(0f, 360f);
        Vector3 direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));

        float distance = Random.Range(minDistance, maxDistance);
        Vector3 spawnPosition = player.position + direction * distance;

        GameObject soundObject = Instantiate(soundPrefab, spawnPosition, Quaternion.identity);
        AudioSource audioSource = soundObject.GetComponent<AudioSource>();

        Clip sourceClip = soundClips[Random.Range(0, soundClips.Count)];
        audioSource.clip = sourceClip.audio_clip;
        audioSource.volume = sourceClip.volume;
        audioSource.Play();

        Destroy(soundObject, audioSource.clip.length);
    }
}
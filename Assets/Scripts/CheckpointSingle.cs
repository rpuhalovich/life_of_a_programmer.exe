using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSingle : MonoBehaviour
{
    [SerializeField] private Material triggered;
    [SerializeField] private Transform respawnPoint;

    private LevelCheckpoints levelCheckpoints;

    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<PlayerController>(out PlayerController player)) {
            foreach (Transform child in transform) {
                // Set light beams to green.
                child.GetComponent<MeshRenderer>().material = triggered;
                // Set respawn point to this checkpoint.
                respawnPoint.transform.position = this.transform.position;
                levelCheckpoints.PlayerThroughCheckpoint(this);
            }
        }
    }

    public void SetLevelCheckpoints(LevelCheckpoints levelCheckpoints) {
        this.levelCheckpoints = levelCheckpoints;
    }
}

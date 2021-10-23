using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSingle : MonoBehaviour
{
    [SerializeField] private Material triggered;

    private LevelCheckpoints levelCheckpoints;

    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<PlayerController>(out PlayerController player)) {
            foreach (Transform child in transform) {
                child.GetComponent<MeshRenderer>().material = triggered;
                levelCheckpoints.PlayerThroughCheckpoint(this);
            }
        }
    }

    public void SetLevelCheckpoints(LevelCheckpoints levelCheckpoints) {
        this.levelCheckpoints = levelCheckpoints;
    }
}

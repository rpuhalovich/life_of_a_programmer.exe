using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSingle : MonoBehaviour
{
    private LevelCheckpoints levelCheckpoints;
    private MeshRenderer meshRenderer;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start() {
        Show();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<PlayerController>(out PlayerController player )) {
            levelCheckpoints.PlayerThroughCheckpoint(this);
        }
    }

    public void setTrackCheckpoints(LevelCheckpoints levelCheckpoints) {
        this.levelCheckpoints = levelCheckpoints;
    }

    public void Show() {
        meshRenderer.enabled = true;
    }

    public void Hide() {
        meshRenderer.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCheckpoints : MonoBehaviour
{
    private void Awake() {
        Transform checkpointsTransform = transform.Find("Checkpoints");

        foreach (Transform checkpointSingleTransform in checkpointsTransform) {
            CheckpointSingle checkpointSingle = checkpointSingleTransform.GetComponent<CheckpointSingle>();
            checkpointSingle.setTrackCheckpoints(this);
        }
    }

    public void PlayerThroughCheckpoint(CheckpointSingle checkpointSingle) {
        Debug.Log(checkpointSingle.transform.name);
    }
}

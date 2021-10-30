using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCheckpoints : MonoBehaviour
{
    private List<CheckpointSingle> checkpointSingles;
    private int nextCheckpointSingleIndex;
    [SerializeField] private Stopwatch stopwatch;
    [SerializeField] private Material triggered;
    [SerializeField] private Transform respawnPoint;

    private void Awake()
    {
        Transform checkpointsTransform = transform.Find("Checkpoints");

        checkpointSingles = new List<CheckpointSingle>();
        foreach (Transform checkpointSingleTransform in checkpointsTransform)
        {
            CheckpointSingle checkpointSingle = checkpointSingleTransform.GetComponent<CheckpointSingle>();
            checkpointSingle.SetLevelCheckpoints(this);
            checkpointSingles.Add(checkpointSingle);
        }

        nextCheckpointSingleIndex = 0;
    }

    public void PlayerThroughCheckpoint(CheckpointSingle checkpointSingle)
    {
        if (checkpointSingles.IndexOf(checkpointSingle) == nextCheckpointSingleIndex) {
            // Set light beams to green.
            foreach (Transform child in checkpointSingle.transform) {
                child.GetComponent<MeshRenderer>().material = triggered;
            }
            // Set respawn point to this checkpoint.
            respawnPoint.transform.position = checkpointSingle.transform.position;
            nextCheckpointSingleIndex++;
        }

        if (checkpointSingles.IndexOf(checkpointSingle) == 0)
        {
            stopwatch.StartStopwatch();
        }

        if (nextCheckpointSingleIndex == checkpointSingles.Count)
        {
            stopwatch.StopStopwatch();
        }
    }
}

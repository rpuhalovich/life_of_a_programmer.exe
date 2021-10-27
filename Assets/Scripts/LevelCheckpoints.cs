using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCheckpoints : MonoBehaviour
{

    private List<CheckpointSingle> checkpointSingles;
    [SerializeField] private Stopwatch stopwatch;

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
    }

    public void PlayerThroughCheckpoint(CheckpointSingle checkpointSingle)
    {
        if (checkpointSingles.IndexOf(checkpointSingle) == 0)
        {
            stopwatch.StartStopwatch();
        }
        else if (checkpointSingles.IndexOf(checkpointSingle) == checkpointSingles.Count - 1)
        {
            stopwatch.StopStopwatch();
        }
    }
}

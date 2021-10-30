using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSingle : MonoBehaviour
{
    private LevelCheckpoints levelCheckpoints;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out PlayerController player))
        {
            levelCheckpoints.PlayerThroughCheckpoint(this);
        }
    }

    public void SetLevelCheckpoints(LevelCheckpoints levelCheckpoints)
    {
        this.levelCheckpoints = levelCheckpoints;
    }
}

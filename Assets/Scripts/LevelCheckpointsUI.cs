using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCheckpointsUI : MonoBehaviour
{
    [SerializeField] private LevelCheckpoints levelCheckpoints;

    private void Start() {
        levelCheckpoints.OnPlayerCorrectCheckpoint += LevelCheckpoints_OnPlayerCorrectCheckpoint;
        levelCheckpoints.OnPlayerWrongCheckpoint += LevelCheckpoints_OnPlayerWrongCheckpoint;

        Hide();
    }

    private void LevelCheckpoints_OnPlayerWrongCheckpoint(object sender, System.EventArgs e) {
        Show();
    }

    private void LevelCheckpoints_OnPlayerCorrectCheckpoint(object sender, System.EventArgs e) {
        Hide();
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}

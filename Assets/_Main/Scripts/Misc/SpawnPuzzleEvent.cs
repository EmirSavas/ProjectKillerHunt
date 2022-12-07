using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPuzzleEvent : MonoBehaviour
{
    public GameObject puzzleCanvasWorld;

    private GameObject spawnedPuzzle;

    public void SpawnPuzzleWorldSpace(Vector3 position, string keycode)
    {
        var puzzle = Instantiate(puzzleCanvasWorld, position, Quaternion.identity);
        var text = puzzle.GetComponentInChildren<TextMeshProUGUI>();

        text.text = keycode;
        puzzle.transform.LookAt(Camera.main.transform); //todo Change

        spawnedPuzzle = puzzle;
    }

    public void Finished(bool result)
    {
        if (result)
        {
            spawnedPuzzle.GetComponentInChildren<Image>().color = Color.green;
        }
        else
        {
            spawnedPuzzle.GetComponentInChildren<Image>().color = Color.red;
        }
        
        Invoke(nameof(DestroyDelay), 2f);
    }

    private void DestroyDelay()
    {
        Destroy(spawnedPuzzle.gameObject);
    }
}

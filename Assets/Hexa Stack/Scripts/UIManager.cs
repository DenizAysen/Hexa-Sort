using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private LevelManagerSo LevelManagerSo;
    [SerializeField] private TextMeshProUGUI targetHexagonText;
    [SerializeField] private TextMeshProUGUI collectedHexagonText;
    [SerializeField] private GameObject hexagonImage;

    private char _slash = '/';
    private LevelDataSo _currentLevel;
    private int _collectedHexagonCount;
    private void OnEnable()
    {
        MergeManager.onStackCompleted += OnStackCompleted;
    }

    private void OnStackCompleted(int collectedHexagons)
    {
        _collectedHexagonCount += collectedHexagons;
        collectedHexagonText.text = _collectedHexagonCount.ToString();
        if (_collectedHexagonCount >= _currentLevel.LevelData.RequiredHexagons)
        {
            StartCoroutine(LoadIslandScene());
        }
        else
            PlayHexagonAnimation();
    }
    private IEnumerator LoadIslandScene()
    {
        float _duration = 1.5f;
        PlayerPrefs.SetInt("Hexagon", _collectedHexagonCount);
        yield return new WaitForSeconds(_duration);
        SceneManager.LoadScene(2);
    }
    private void PlayHexagonAnimation()
    {
        LeanTween.scale(hexagonImage, Vector3.one * 1.2f, .25f).setOnComplete(() =>
        {
            LeanTween.scale(hexagonImage, Vector3.one, .25f);
        });
    }

    private void OnDisable()
    {
        MergeManager.onStackCompleted -= OnStackCompleted;
    }
    void Start()
    {
        _collectedHexagonCount = 0;
        collectedHexagonText.text = _collectedHexagonCount.ToString();
        _currentLevel = LevelManagerSo.GetCurrentLevel();
        targetHexagonText.text = _slash + _currentLevel.LevelData.RequiredHexagons.ToString();
    }

    
}

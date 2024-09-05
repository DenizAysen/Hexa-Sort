using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Renderer))]
public class Tower : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private float fillIncrement;
    [SerializeField] private Animator _animator;
    [SerializeField] private BuildingDataSo buildingData;
    #endregion

    #region Privates
    private float _fillIncrement;
    private int _fillCount;
    private int _startFillCount;
    private float _fillPercent;

    private Renderer _renderer;

    #endregion
    public static Action<float> onFillIncreased;
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _fillIncrement = 1 / buildingData.Health;
        _startFillCount = PlayerPrefs.GetInt("Hexagon");
        _fillCount = _startFillCount;
        _fillPercent = PlayerPrefs.GetFloat("Percentage");
        if (_fillPercent >= 1f)
            _fillPercent = 0f;
    }
    private void OnEnable()
    {
        FillButton.onFilling += OnFilling;
    }

    private void OnFilling()
    {
        Fill();
    }
    private void OnDisable()
    {
        FillButton.onFilling -= OnFilling;
    }

    void Start()
    {
        UpdateMaterials();
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    //if(Input.GetMouseButton(0))
    //    //{
    //    //    Fill();
    //    //}
    //}
    public void ButonaBasti()
    {
        Debug.Log("Basti");
    }
    private void Fill()
    {
        if (_fillPercent >= 1)
        {
            PlayerPrefs.SetFloat("Percentage", _fillPercent);
            StartCoroutine(LoadNextLevel());
            return;
        }

        if(_fillCount > 0)
        {
            _fillPercent += _fillIncrement;
            UpdateMaterials();

            _animator.Play("Bump");
            _fillCount--;
            float _fillButtonFillAmount = _fillCount/(float)_startFillCount;
            Debug.Log(_fillButtonFillAmount);
            onFillIncreased?.Invoke(_fillButtonFillAmount);
        }
        else
        {
            PlayerPrefs.SetInt("Hexagon", 0);
            PlayerPrefs.SetFloat("Percentage", _fillPercent);
            StartCoroutine(LoadNextLevel());
        }
    }

    private void UpdateMaterials()
    {
        foreach (Material material in _renderer.materials)
        {
            material.SetFloat("_Fill_Percent", _fillPercent);
        }
    }
    private IEnumerator LoadNextLevel()
    {
        float duration = 1f;
        int levelIndex = PlayerPrefs.GetInt("Level");
        Debug.Log(levelIndex);
        yield return new WaitForSeconds(duration);
        PlayerPrefs.SetInt("Level", (levelIndex + 1)%2);
        SceneManager.LoadScene((levelIndex + 1) % 2);
    }
}

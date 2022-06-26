using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIPopUp : MonoBehaviour
{
    public string equipmentName;
    public Transform equipmentPosition;

    private RectTransform _popUpPos;
    private TextMeshProUGUI _popUpText;

    public Vector3 offset;

    private void Awake()
    {
        _popUpPos = GetComponent<RectTransform>();
        _popUpText = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        _popUpPos.transform.position = GetComponentInParent<Equipment>().transform.position + offset;
        _popUpText.text = equipmentName;
    }
}

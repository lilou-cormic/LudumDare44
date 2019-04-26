using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIVersion : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI VersionText = null;

    private void Awake()
    {
        VersionText.text = Application.version;
    }
}

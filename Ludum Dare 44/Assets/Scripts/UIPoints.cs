using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIPoints : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI PointsText = null;

    private float Timer = 0.4f;

    public void SetPointsText(int points)
    {
        PointsText.text = $"+{points}";
    }

    private void FixedUpdate()
    {
        if (Timer <= 0)
        {
            Destroy(gameObject);
            Timer = 0;
            return;
        }

        Timer -= Time.deltaTime;
        transform.position += Vector3.up * 0.02f;
    }
}

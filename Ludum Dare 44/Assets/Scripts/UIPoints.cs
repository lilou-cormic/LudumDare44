using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIPoints : MonoBehaviour
{
    private static Color BadColor = Color.red;
    private static Color GoodColor = new Color(255 / 255f, 252 / 255f, 64 / 255f);

    [SerializeField]
    private TextMeshProUGUI PointsText = null;

    private float Timer = 0.4f;

    public void SetPointsText(int points)
    {
        if (points >= 0)
        {
            PointsText.color = GoodColor;
            PointsText.text = $"+{points}";
        }
        else
        {
            PointsText.color = BadColor;
            PointsText.text = $"{points}";
        }
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

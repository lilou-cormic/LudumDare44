using UnityEngine;
using TMPro;

public class UIHealth : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI HealthValueText = null;

    private static Color BadColor = Color.red;
    private static Color NormalColor = new Color(250 / 255f, 106 / 255f, 10 / 255f);
    private static Color GoodColor = new Color(255 / 255f, 252 / 255f, 64 / 255f);
    private static Color MaxColor = new Color(148 / 255f, 227 / 255f, 68 / 255f);

    private void Start()
    {
        Health.ValueChanged += Health_ValueChanged;
    }

    private void OnDestroy()
    {
        Health.ValueChanged -= Health_ValueChanged;
    }

    private void Health_ValueChanged()
    {
        HealthValueText.text = Health.Value.ToString();

        if (Health.Value < Health.MaxValue / 8)
            HealthValueText.color = BadColor;
        else if (Health.Value < Health.MaxValue / 2)
            HealthValueText.color = NormalColor;
        else if (Health.Value < Health.MaxValue - 1)
            HealthValueText.color = GoodColor;
        else if (Health.Value == Health.MaxValue)
            HealthValueText.color = MaxColor;
        else
            HealthValueText.color = NormalColor;
    }
}

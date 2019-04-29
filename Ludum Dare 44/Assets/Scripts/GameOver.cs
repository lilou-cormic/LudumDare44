using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    private Button[] Buttons = null;

    private void Start()
    {
        StartCoroutine(WaitToActivateButtons());
    }

    private IEnumerator WaitToActivateButtons()
    {
        yield return new WaitForSeconds(1f);

        foreach (var button in Buttons)
        {
            Navigation navigation = button.navigation;
            navigation.mode = Navigation.Mode.Automatic;

            button.navigation = navigation;
        }
    }
}

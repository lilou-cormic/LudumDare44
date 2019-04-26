using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButton : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    private Button Button;

    [SerializeField]
    private AudioClip ClickSound = null;

    [SerializeField]
    private AudioClip SelectedSound = null;

    private void Awake()
    {
        Button = GetComponent<Button>();
        Button.onClick.AddListener(() => PlayClickSound());
    }

    public void PlayClickSound()
    {
        SoundPlayer.Play(ClickSound);
    }

    public void PlaySelectedSound()
    {
        SoundPlayer.Play(SelectedSound);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlaySelectedSound();
    }

    public void OnSelect(BaseEventData eventData)
    {
        PlaySelectedSound();
    }
}

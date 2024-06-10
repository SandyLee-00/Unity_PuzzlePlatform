using UnityEngine;
using UnityEngine.UI;

public class UIPopup_Setting : MonoBehaviour
{
    public Slider BGMSlider;
    public Slider SESlider;

    private void Start()
    {
        BGMSlider.onValueChanged.AddListener((value) => { SoundManager.Instance.SetVolume(Define.Sound.Bgm, value); });
        SESlider.onValueChanged.AddListener((value) => { SoundManager.Instance.SetVolume(Define.Sound.Effect, value); });

        BGMSlider.value = SoundManager.Instance.GetVolume(Define.Sound.Bgm);
        SESlider.value = SoundManager.Instance.GetVolume(Define.Sound.Effect);
    }
}

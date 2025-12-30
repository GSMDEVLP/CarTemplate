using TMPro;
using UnityEngine;

public sealed class SpeedometerView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI gearText;
    [SerializeField] private RectTransform arrow;

    private SpeedometerViewModel _vm;

    public void Bind(SpeedometerViewModel vm)
    {
        _vm = vm;
        _vm.SpeedText.OnChanged += OnSpeedChanged;
        _vm.GearText.OnChanged += OnGearChanged;
        _vm.ArrowAngle.OnChanged += OnArrowChanged;

        OnSpeedChanged(_vm.SpeedText.Value);
        OnGearChanged(_vm.GearText.Value);
        OnArrowChanged(_vm.ArrowAngle.Value);
    }

    private void OnDestroy()
    {
        if (_vm == null) return;
        _vm.SpeedText.OnChanged -= OnSpeedChanged;
        _vm.GearText.OnChanged -= OnGearChanged;
        _vm.ArrowAngle.OnChanged -= OnArrowChanged;
    }

    private void OnSpeedChanged(string value)
    {
        if (speedText != null) speedText.text = value;
    }

    private void OnGearChanged(string value)
    {
        if (gearText != null) gearText.text = value;
    }

    private void OnArrowChanged(float angle)
    {
        if (arrow != null) arrow.localEulerAngles = new Vector3(0f, 0f, angle);
    }
}

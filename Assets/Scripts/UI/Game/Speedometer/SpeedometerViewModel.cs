using UnityEngine;

public sealed class SpeedometerViewModel : ViewModelBase
{
    public ObservableProperty<string> SpeedText { get; } = new ObservableProperty<string>("0");
    public ObservableProperty<float> ArrowAngle { get; } = new ObservableProperty<float>(0f);
    public ObservableProperty<string> GearText { get; } = new ObservableProperty<string>("1");

    private readonly IVehicleTelemetrySource _telemetry;
    private readonly float _minAngle;
    private readonly float _maxAngle;
    private readonly float _maxSpeed;

    public SpeedometerViewModel(
        IVehicleTelemetrySource telemetry,
        float minAngle,
        float maxAngle,
        float maxSpeed)
    {
        _telemetry = telemetry;
        _minAngle = minAngle;
        _maxAngle = maxAngle;
        _maxSpeed = maxSpeed;

        if (_telemetry != null)
        {
            GearText.Value = _telemetry.CurrentGear.ToString();
            _telemetry.GearChanged += OnGearChanged;
        }
    }

    public void RefreshSpeed()
    {
        if (_telemetry == null) return;
        float speed = _telemetry.SpeedKph;

        SpeedText.Value = ((int)speed).ToString();
        float t = _maxSpeed <= 0f ? 0f : speed / _maxSpeed;
        ArrowAngle.Value = Mathf.Lerp(_minAngle, _maxAngle, t);
    }

    private void OnGearChanged(int gear)
    {
        GearText.Value = gear.ToString();
    }

    public override void Dispose()
    {
        if (_telemetry != null)
            _telemetry.GearChanged -= OnGearChanged;
    }

}

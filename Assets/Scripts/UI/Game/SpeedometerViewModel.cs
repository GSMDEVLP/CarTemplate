using System;
using UnityEngine;

public sealed class SpeedometerViewModel : ViewModelBase
{
    public ObservableProperty<string> SpeedText { get; } = new ObservableProperty<string>("0");
    public ObservableProperty<float> ArrowAngle { get; } = new ObservableProperty<float>(0f);
    public ObservableProperty<string> GearText { get; } = new ObservableProperty<string>("1");

    private readonly Func<float> _getSpeed;
    private readonly float _minAngle;
    private readonly float _maxAngle;
    private readonly float _maxSpeed;
    private readonly Ashsvp.GearSystem _gearSystem;

    public SpeedometerViewModel(Func<float> getSpeed, float minAngle, float maxAngle, float maxSpeed, Ashsvp.GearSystem gearSystem)
    {
        _getSpeed = getSpeed;
        _minAngle = minAngle;
        _maxAngle = maxAngle;
        _maxSpeed = maxSpeed;
        _gearSystem = gearSystem;

        if (_gearSystem != null)
        {
            GearText.Value = _gearSystem.currentGear.ToString();
            _gearSystem.OnGearChanged += OnGearChanged;
        }
    }

    public void RefreshSpeed()
    {
        if (_getSpeed == null) return;
        float speed = _getSpeed();

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
        if (_gearSystem != null)
            _gearSystem.OnGearChanged -= OnGearChanged;
    }

}

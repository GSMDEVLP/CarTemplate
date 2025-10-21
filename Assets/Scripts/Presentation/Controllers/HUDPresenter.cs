
using UnityEngine;
using TMPro;

public class HUDPresenter : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private GameObject player; 
    private ITakesDamage _hp;
    private IEventBus _bus;

    void Awake()
    {
        _hp = player.GetComponent<ITakesDamage>();
        _bus = CompositionRoot.Instance.Events; 
    }
    
    void Start()
    {
        Refresh();       
    }
    void OnEnable() 
    {
        _bus.Subscribe<DamageTaken>(OnDamage);
    }
    void OnDisable() 
    {
        _bus.Unsubscribe<DamageTaken>(OnDamage);
    }
    void OnDamage(DamageTaken e)
     {
        if (e.Target == _hp) Refresh();
    }
    void Refresh() 
    {
        if (_hp != null && hpText != null) 
        {
            hpText.text = $"{Mathf.CeilToInt(_hp.CurrentHP)}/{Mathf.CeilToInt(_hp.MaxHP)}";
        }
    }
}

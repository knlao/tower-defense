using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    public GameObject ui;
    public GameObject upgradeUI;

    public Button rangeButton;
    public Button damageButton;
    public Button fireRateButton;
    public Button damageOverButtonButton;

    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI fireRateText;
    public TextMeshProUGUI damageOverTimeText;

    public TextMeshProUGUI sellText;

    public LineRenderer lr;
    public Transform lineStart;
    
    private Node _target;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            Hide();
    }

    public void SetTarget(Node target)
    {
        _target = target;
        transform.position = _target.GetBuildPosition();
        
        var turret = _target.turret.GetComponent<Turret>();
        var turretUseLaser = turret.useLaser;
        
        lr.SetPosition(0, lineStart.position);
        lr.SetPosition(1, _target.transform.position + Vector3.up * 2);
        lr.enabled = true;
        
        rangeText.text = "<b>Range</b>\nLevel " + (turret.rangeLevel >= turret.rangeList.Length ? "MAX" : "" + turret.rangeLevel) + " $" + turret.rangeUpgradeCost[turret.rangeLevel-1];
        rangeButton.interactable = true;

        if (turretUseLaser)
        {
            damageOverTimeText.text = "<b>Damage Rate</b>\nLevel " + (turret.damageOverTimeLevel >= turret.damageOverTimeList.Length ? "MAX" : "" + turret.damageOverTimeLevel) + " $" + turret.damageOverTimeUpgradeCost[turret.damageOverTimeLevel-1];
            damageButton.interactable = false;
            fireRateButton.interactable = false;
            damageOverButtonButton.interactable = true;
            damageText.text = "Not Available\nOn This Turret";
            fireRateText.text = "Not Available\nOn This Turret";
        }
        else
        {
            damageText.text = "<b>Damage</b>\nLevel " + (turret.damageLevel >= turret.damageList.Length ? "MAX" : "" + turret.damageLevel) + " $" + turret.damageUpgradeCost[turret.damageLevel-1];
            fireRateText.text = "<b>Fire Rate</b>\nLevel " + (turret.fireRateLevel >= turret.fireRateList.Length ? "MAX" : "" + turret.fireRateLevel) + " $" + turret.fireRateUpgradeCost[turret.fireRateLevel-1];
            damageButton.interactable = true;
            fireRateButton.interactable = true;
            damageOverButtonButton.interactable = false;
            damageOverTimeText.text = "Not Available\nOn This Turret";
        }

        sellText.text = "<b>SELL</b>\n$" + _target.turretBlueprint.sellCost;
        
        
        ui.SetActive(true);
    }

    public void Hide()
    {
        if (_target == null) return;
        _target.rend.material = _target.startMat;
        lr.enabled = false;
        upgradeUI.SetActive(false);
        ui.SetActive(false);
    }

    public void ToggleUpgradePanel()
    {
        upgradeUI.SetActive(!upgradeUI.activeSelf);
    }
    
    public void UpgradeTurretRange()
    {
        var turret = _target.turret.GetComponent<Turret>();
        turret.Upgrade("range");
        rangeText.text = "<b>Range</b>\nLevel " + (turret.rangeLevel >= turret.rangeList.Length ? "MAX" : "" + turret.rangeLevel) + " $" + turret.rangeUpgradeCost[turret.rangeLevel-1];
        _target.DoBuildEffect();
    }

    public void UpgradeTurretDamage()
    {
        var turret = _target.turret.GetComponent<Turret>();
        turret.Upgrade("damage");
        damageText.text = "<b>Damage</b>\nLevel " + (turret.damageLevel >= turret.damageList.Length ? "MAX" : "" + turret.damageLevel) + " $" + turret.damageUpgradeCost[turret.damageLevel-1];
        _target.DoBuildEffect();
    }

    public void UpgradeTurretFireRate()
    {
        var turret = _target.turret.GetComponent<Turret>();
        turret.Upgrade("fire_rate");
        fireRateText.text = "<b>Fire Rate</b>\nLevel " + (turret.fireRateLevel >= turret.fireRateList.Length ? "MAX" : "" + turret.fireRateLevel) + " $" + turret.fireRateUpgradeCost[turret.fireRateLevel-1];
        _target.DoBuildEffect();
    }

    public void UpgradeTurretDamageOverTime()
    {
        var turret = _target.turret.GetComponent<Turret>();
        turret.Upgrade("damage_over_time");
        damageOverTimeText.text = "<b>Damage Rate</b>\nLevel " + (turret.damageOverTimeLevel >= turret.damageOverTimeList.Length ? "MAX" : "" + turret.damageOverTimeLevel) + " $" + turret.damageOverTimeUpgradeCost[turret.damageOverTimeLevel-1];
        _target.DoBuildEffect();
    }

    public void Sell()
    {
        PlayerStats.Money += _target.turretBlueprint.sellCost;
        _target.DoSellEffect();
        Destroy(_target.turret);
        _target.turretBlueprint = null;
        Hide();
    }
}

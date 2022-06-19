using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    /*
     * Shop Script
     * A shop for turrets
     */

    [Header("Turrets")]
    public TurretBlueprint standardTurret;
    public TurretBlueprint miniMissileLauncher;
    public TurretBlueprint missileLauncher;
    public TurretBlueprint laserBeamer;

    [Header("Select")]
    public GameObject selectBox;
    public GameObject firstPos;
    public GameObject secondPos;
    public GameObject thridPos;
    public GameObject fourthPos;
    public Vector2 offset;

    private BuildManager _buildManager;

    private void Start()
    {
        _buildManager = BuildManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1)) SelectStandardTurret();
        if (Input.GetKey(KeyCode.Alpha2)) SelectMiniMissileLauncher();
        if (Input.GetKey(KeyCode.Alpha3)) SelectMissileLauncher();
        if (Input.GetKey(KeyCode.Alpha4)) SelectLaserBeamer();
    }

    public void SelectStandardTurret()
    {
        _buildManager.SelectTurretToBuild(standardTurret);
        SelectBox(1);
    }

    public void SelectMiniMissileLauncher()
    {
        _buildManager.SelectTurretToBuild(miniMissileLauncher);
        SelectBox(2);
    }

    public void SelectMissileLauncher()
    {
        _buildManager.SelectTurretToBuild(missileLauncher);
        SelectBox(3);
    }

    public void SelectLaserBeamer()
    {
        _buildManager.SelectTurretToBuild(laserBeamer);
        SelectBox(4);
    }

    public void SelectBox(int idx)
    {
        selectBox.SetActive(true);
        switch (idx)
        {
            case 1:
                selectBox.GetComponent<RectTransform>().SetParent(firstPos.GetComponent<RectTransform>());
                selectBox.GetComponent<RectTransform>().SetSiblingIndex(0);
                break;
            case 2:
                selectBox.GetComponent<RectTransform>().SetParent(secondPos.GetComponent<RectTransform>());
                selectBox.GetComponent<RectTransform>().SetSiblingIndex(0);
                break;
            case 3:
                selectBox.GetComponent<RectTransform>().SetParent(thridPos.GetComponent<RectTransform>());
                selectBox.GetComponent<RectTransform>().SetSiblingIndex(0);
                break;
            case 4:
                selectBox.GetComponent<RectTransform>().SetParent(fourthPos.GetComponent<RectTransform>());
                selectBox.GetComponent<RectTransform>().SetSiblingIndex(0);
                break;
        }
        selectBox.GetComponent<RectTransform>().anchoredPosition = offset;
    }

    public void DeselectBox()
    {
        selectBox.SetActive(false);
    }
}

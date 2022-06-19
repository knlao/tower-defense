using System;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    /*
     * Build Manager Script
     * Manages all the turrets we want to build
     */

    [Header("Miscellaneous")]
    public GameObject buildEffect;
    public GameObject sellEffect;
    public GameObject tempTurret;
    public NodeUI nodeUI;
    
    public static BuildManager Instance;
    
    private TurretBlueprint _turretToBuild;
    private Node _selectedNode;
    
    public bool CanBuild
    {
        get { return _turretToBuild != null; }
    }
    
    public bool HasMoney
    {
        get { return PlayerStats.Money >= _turretToBuild.cost; }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one BuildManager in the scene!");
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _turretToBuild = null;
            FindObjectOfType<Shop>().DeselectBox();
            DeselectNode();
        }
    }

    public void SelectNode(Node node)
    {
        if (_selectedNode == node)
        {
            //Debug.Log("Deselect node");
            DeselectNode();
            return;
        }
        
        _selectedNode = node;
        FindObjectOfType<Shop>().DeselectBox();
        _turretToBuild = null;

        nodeUI.SetTarget(node);
    }

    public void DeselectNode()
    {
        _selectedNode = null;
        nodeUI.Hide();
    }

    public void SelectTurretToBuild(TurretBlueprint turret)
    {
        _turretToBuild = turret;
        DeselectNode();
    }

    public TurretBlueprint GetTurretToBuild()
    {
        return _turretToBuild;
    }
}

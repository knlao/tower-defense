using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    /*
     * Node Script
     * A place which we can build our turrets
     */

    [Header("Colors")]
    public Material startMat;
    public Material hoverMat;

    [Header("Turret")]
    public Vector3 offset;

    [Header("Misc")]
    public LayerMask ignoreMask;

    [HideInInspector] public GameObject turret;
    [HideInInspector] public TurretBlueprint turretBlueprint;
    [HideInInspector] public Renderer rend;


    private BuildManager _buildManager;
    private GameObject _tempTurretPrefab;
    private GameObject _tempTurret;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material = startMat;
        _buildManager = BuildManager.Instance;
        _tempTurretPrefab = _buildManager.tempTurret;
    }

    public Vector3 GetBuildPosition()
    {
        return transform.position + offset;
    }

    Ray ray;
    RaycastHit hit;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f, ~ignoreMask) && hit.collider.gameObject == gameObject && turret != null) // On Selected
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;
                if (turret != null)
                {
                    _buildManager.SelectNode(this);
                    return;
                }
            }
        }
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (!_buildManager.CanBuild) return;

        // Check
        BuildTempTurret();
        PathCheckingRequestManager.instance.RequestPath(CheckIfCanBuild);
    }

    private void BuildTempTurret()
    {
        _tempTurret = Instantiate(_tempTurretPrefab, GetBuildPosition(), Quaternion.identity);
    }

    private void RemoveTempTurret()
    {
        Destroy(_tempTurret);
    }

    private void CheckIfCanBuild(bool canBuild)
    {
        //print("Received, status: " + canBuild);
        RemoveTempTurret();
        if (canBuild)
            // Build a Turret
            BuildTurret(_buildManager.GetTurretToBuild());
        else
        {
            Debug.LogWarning("You can't build here");
        }
    }

    private void BuildTurret(TurretBlueprint blueprint)
    {
        if (PlayerStats.Money < blueprint.cost)
        {
            Debug.LogWarning("Not enough money to build!");
            return;
        }

        PlayerStats.Money -= blueprint.cost;

        var tempTurret = Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);
        turret = tempTurret;
        turretBlueprint = blueprint;

        NodeGrid.instance.CreateGrid();

        DoBuildEffect();
    }

    private void OnMouseOver()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        rend.material = hoverMat;
    }

    private void OnMouseExit()
    {
        rend.material = startMat;
    }

    public void DoBuildEffect()
    {
        var effect = Instantiate(_buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);
    }

    public void DoSellEffect()
    {
        var effect = Instantiate(_buildManager.sellEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);
    }
}
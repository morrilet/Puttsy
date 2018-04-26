using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower_Menu : MonoBehaviour
{
    public GameObject windmillTower;
    public GameObject putterTower;
    public GameObject cannonTower;
    [Space]
    public Text cannonPar;
    public Text cannonUsed;
    [Space]
    public Text putterPar;
    public Text putterUsed;
    [Space]
    public Text windmillPar;
    public Text windmillUsed;
    [Space]
    public Color underParColor;
    public Color overParColor;
    [Space]
    public GameObject arrowPrefab;

    private void Update()
    {
        cannonPar.text = "Par: " + GameManager.instance.par.parCannons;
        cannonUsed.text = "Used: " + GameManager.instance.currentCannonsUsed;
        cannonPar.color = (GameManager.instance.currentCannonsUsed < GameManager.instance.par.parCannons) ? underParColor : overParColor;
        cannonUsed.color = (GameManager.instance.currentCannonsUsed < GameManager.instance.par.parCannons) ? underParColor : overParColor;

        putterPar.text = "Par: " + GameManager.instance.par.parPutters;
        putterUsed.text = "Used: " + GameManager.instance.currentPuttersUsed;
        putterPar.color = (GameManager.instance.currentPuttersUsed < GameManager.instance.par.parPutters) ? underParColor : overParColor;
        putterUsed.color = (GameManager.instance.currentPuttersUsed < GameManager.instance.par.parPutters) ? underParColor : overParColor;

        windmillPar.text = "Par: " + GameManager.instance.par.parWindmills;
        windmillUsed.text = "Used: " + GameManager.instance.currentWindmillsUsed;
        windmillPar.color = (GameManager.instance.currentWindmillsUsed < GameManager.instance.par.parWindmills) ? underParColor : overParColor;
        windmillUsed.color = (GameManager.instance.currentWindmillsUsed < GameManager.instance.par.parWindmills) ? underParColor : overParColor;

        if (GameManager.instance.planningMode)
        {
            DrawTowerArrows();
        }
    }

    public void SpawnTower(GameObject towerPrefab)
    {
        GameObject tower = Instantiate(towerPrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.Euler(-90, 0, 0), null) as GameObject;

        ClickAndDrag drag = tower.AddComponent<ClickAndDrag>();
        //drag.sortingLayerName = "Tower";
        //drag.sortingOrder = 0;
        drag.snapMask = 1 << LayerMask.NameToLayer("Wall");

        Rigidbody rb = tower.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;

        drag.Grab();
    }

    public void SpawnCannonTower()
    {
        if (GameManager.instance.currentCannonsUsed 
            < GameManager.instance.par.parCannons)
        {
            SpawnTower(cannonTower);
        }
    }

    public void SpawnWindmillTower()
    {
        if (GameManager.instance.currentWindmillsUsed
            < GameManager.instance.par.parWindmills)
        {
            SpawnTower(windmillTower);
        }
    }

    public void SpawnPutterTower()
    {
        if (GameManager.instance.currentPuttersUsed
            < GameManager.instance.par.parPutters)
        {
            SpawnTower(putterTower);
        }
    }

    public void Putt()
    {
        //Remove menus, change camera view, and begin the game!
        this.GetComponent<Animator>().SetTrigger("ExitMenu");

        //Remove all click&drag components.
        foreach(ClickAndDrag obj in GameObject.FindObjectsOfType<ClickAndDrag>())
        {
            Destroy(obj);
        }

        RemoveTowerArrows();
        GameManager.instance.planningMode = false;
    }

    /// <summary>
    /// Draws arrows to indicate how to use towers.
    /// </summary>
    private void DrawTowerArrows()
    {
        foreach(GameObject tower in GameObject.FindGameObjectsWithTag("Tower"))
        {
            if(tower.GetComponentInChildren<SpriteRenderer>() == null) //Arrow is the only thing with this.
            {
                GameObject arrow = Instantiate(arrowPrefab, tower.transform);
                Vector3 scale = new Vector3(0.25f, 0.25f, 1);
                Vector3 rotation = Vector3.zero;
                Vector3 position = Vector3.zero;

                if(tower.GetComponentInChildren<PutterPusher>() != null
                    || tower.GetComponentInChildren<WindmillPusher>() != null)
                {
                    position = new Vector3(0.0f, -0.25f, 0.275f);
                    rotation = new Vector3(90, 0, 0);
                }
                else if (tower.GetComponentInChildren<CannonShooter>() != null)
                {
                    position = new Vector3(0.6f, -0.25f, 0.0f);
                    rotation = new Vector3(90, -90, 0);
                }

                arrow.transform.localScale = scale;
                arrow.transform.localRotation = Quaternion.Euler(rotation);
                arrow.transform.localPosition = position;
            }
        }
    }

    /// <summary>
    /// Removes all the indicators from tower objects.
    /// </summary>
    private void RemoveTowerArrows()
    {
        foreach(GameObject arrow in GameObject.FindGameObjectsWithTag("TowerIndicator"))
        {
            Destroy(arrow);
        }
    }
}

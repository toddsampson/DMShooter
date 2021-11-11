using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.OfTomorrowInc.DMShooter;

public class UnitSelector : MonoBehaviour
{
    public List<EnemyGeneric> selectedUnits = new List<EnemyGeneric>();

    public Camera cam;

    public bool demoMode;

    public GameObject holdingShift;

    private Texture2D _whiteTexture;

    private bool isDragging;

    private Vector3 mousePos;

    public Texture2D WhiteTexture
    {
        get
        {
            if(_whiteTexture == null)
            {
                _whiteTexture = new Texture2D(1, 1);
                _whiteTexture.SetPixel(0, 0, Color.white);
                _whiteTexture.Apply();
            }

            return _whiteTexture;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleInputs();
    }

    private void OnGUI()
    {
        if(isDragging)
        {
            Rect rect = GetScreenRect(mousePos, Input.mousePosition);
            DrawScreenRect(rect, new Color(0f, 0f, 0f, 0.25f));
            DrawScreenRectBorder(rect, 3, Color.blue);
        }
    }

    private void HandleInputs()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            mousePos = Input.mousePosition;
            isDragging = true;

            if(!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
            {
                RemoveSelections();
            }
            
            TrySelect();
        }

        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            foreach(GameObject enemy in GameManager.enemies)
            {
                if(enemy != null && IsWithinSelectionBounds(enemy.transform))
                {
                    EnemyGeneric e = enemy.GetComponent<EnemyGeneric>();
                    
                    if(e.selectionHighlight != null)
                    {
                        e.selectionHighlight.SetActive(true);
                        selectedUnits.Add(e);
                    }
                }
            }

            isDragging = false;
        }

        if(demoMode && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            holdingShift.SetActive(true);
        }

        else if(demoMode)
        {
            holdingShift.SetActive(false);
        }
    }    

    private void TrySelect()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            if(hit.transform.gameObject.TryGetComponent(out EnemyGeneric e) && e.selectionHighlight != null)
            {
                e.selectionHighlight.SetActive(true);
                selectedUnits.Add(e);
            }
        }
    }

    private void RemoveSelections()
    {
        foreach(EnemyGeneric e in selectedUnits)
        {
            e.selectionHighlight.SetActive(false);
        }

        selectedUnits.Clear();
    }

    private void DrawScreenRect(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, WhiteTexture);
    }

    private bool IsWithinSelectionBounds(Transform tf)
    {
        if(!isDragging)
        {
            return false;
        }

        Bounds vpBounds = GetVPBounds(cam, mousePos, Input.mousePosition);
        return vpBounds.Contains(cam.WorldToViewportPoint(tf.position));
    }

    private void DrawScreenRectBorder(Rect rect, float thickness, Color color)
    {
        // Top Border
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        // Bottom Border
        DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
        // Left Border
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        // Right Border
        DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
    }

    private Rect GetScreenRect(Vector3 screenPos1, Vector3 screenPos2)
    {
        screenPos1.y = Screen.height - screenPos1.y;
        screenPos2.y = Screen.height - screenPos2.y;
        Vector3 bR = Vector3.Max(screenPos1, screenPos2);
        Vector3 tL = Vector3.Min(screenPos1, screenPos2);

        return Rect.MinMaxRect(tL.x, tL.y, bR.x, bR.y);
    }

    private Bounds GetVPBounds(Camera cam, Vector3 screenPos1, Vector3 screenPos2)
    {
        Vector3 pos1 = cam.ScreenToViewportPoint(screenPos1);
        Vector3 pos2 = cam.ScreenToViewportPoint(screenPos2);

        Vector3 min = Vector3.Min(pos1, pos2);
        Vector3 max = Vector3.Max(pos1, pos2);

        min.z = cam.nearClipPlane;
        max.z = cam.farClipPlane;

        Bounds bounds = new Bounds();
        bounds.SetMinMax(min, max);

        return bounds;
    }
}
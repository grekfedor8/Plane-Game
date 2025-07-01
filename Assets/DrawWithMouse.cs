using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawWithMouse : MonoBehaviour
{
    public LineRenderer line;
    private Vector3 previousPosition;

    [SerializeField]
    private float minDistance = 0.1f;

    [SerializeField, Range(0.1f, 2f)]
    private float width;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 1;
        line.startWidth = line.endWidth = width;
        previousPosition = transform.position;
    }

    public void StartLine(Vector2 position) // начало
    {
        line.positionCount = 1;
        line.SetPosition(0, position);
    }
    public void UpdateLine() // продолжение
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentPosition.z = 0f;

            if (previousPosition == transform.position)
            {
                line.SetPosition(0, currentPosition);
            }
            else
            {

                if (Vector3.Distance(currentPosition, previousPosition) > minDistance) // добавление точки в кривую
                {
                    line.positionCount += 1;
                    line.SetPosition(line.positionCount - 1, currentPosition);
                }

            }


            previousPosition = currentPosition;
        }
    }
}

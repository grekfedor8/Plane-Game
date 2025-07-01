using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class plane : MonoBehaviour
{
    public DrawWithMouse drawControl; //�����
    public float speed = 1f; //��������
    float pi = Mathf.Acos(-1);
    Vector3[] positions; //�������
    Vector2 vecMovement; //������-������ ��������

    bool startMovement = false; // �������� �� �������� �� �����
    int moveIndex = 0; // ������� ������
    float prevTime = 0; // ��������� ������ �������, ����� ���� �������� �� ����

    private void get(Vector2 pos) // ����������� ���������� ��������
    {
        System.Random rnd = new System.Random();
        int an;
        if (pos.y > 3)
        {
            an = rnd.Next(210, 329);
        } else if (pos.y < -3)
        {
            an = rnd.Next(30, 149);
        } else if (pos.x > 10)
        {
            an = rnd.Next(120, 239);
        }
        else
        {
            an = (rnd.Next(300, 419)) % 360;
        }
        float angle = ((float)an / 180) * pi;
        vecMovement = new Vector2(0.1f * Mathf.Cos(angle), 0.1f * Mathf.Sin(angle));
    }

    private void Start()
    {   //�������� �����������
        System.Random rnd = new System.Random();
        int an = rnd.Next(0, 359);
        float angle = ((float)an / 180) * pi;
        vecMovement = new Vector2(0.1f * Mathf.Cos(angle), 0.1f * Mathf.Sin(angle));
        get(transform.position);

        prevTime = Time.time;
    }
    private void OnMouseDown() //������ ��������� �����
    {
        drawControl.StartLine(transform.position); 
    }

    private void OnMouseDrag() //����������� ��������� �����
    {
        drawControl.UpdateLine(); 
    }

    private void OnMouseUp() //��������� ��������� �����
    {
        positions = new Vector3[drawControl.line.positionCount];
        drawControl.line.GetPositions(positions); //��������� ����� ������
        startMovement = true;
        moveIndex = 0;
        for(int i = 0; i < positions.Length; ++i) // ����� ������ ��������
        {
            float distance = Vector2.Distance(positions[i], transform.position);
            if (distance <= 0.05f)
            {
                moveIndex = i;
            }
        }
    }

    public void check() // �������� �� ����� �� ����
    {

        if (Time.time - prevTime < 5)
        {
            return;
        }

        bool ok1 = Mathf.Abs(transform.position.x) > 11;
        bool ok2 = Mathf.Abs(transform.position.y) > 4.5;

        Vector2 pos = (Vector2)transform.position;

        if (ok1)
        {
            if (pos.x > 0)
            {
                pos.x = 12;
            }
            else
            {
                pos.x = -12;
            }
            get(pos);
            prevTime = Time.time;

        }
        else if (ok2) { 
            if (pos.y > 0)
            {
                pos.y = 6;
            }
            else
            {
                pos.y = -6;
            }
            get(pos);
            prevTime = Time.time;
        }
    }

    private void Update()
    {

        check(); // �������, ����� ������ �� ������� �� ����

        if (startMovement) // �������� �� �����
        {
            Vector2 currentPos = positions[moveIndex];
            transform.position = Vector2.MoveTowards(transform.position, currentPos, speed * Time.deltaTime); // ��������

            Vector2 dir = currentPos - (Vector2)transform.position;
            vecMovement = dir;
            float angle = Mathf.Atan2(dir.normalized.y, dir.normalized.x);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg - 90f), speed); // ����������� ��������� ����

            float distance = Vector2.Distance(currentPos, transform.position);
            if (distance <= 0.05f) // ������� �� ��������� �����
            {
                moveIndex += 1;
            }

            if (moveIndex > positions.Length - 1) // ��������� �����
            {
                startMovement = false;
            }
        }
        else
        {
            Vector2 currentPos = (Vector2)transform.position + vecMovement;
            transform.position = Vector2.MoveTowards(transform.position, currentPos, speed * Time.deltaTime);

            Vector2 dir = vecMovement;
            float angle = Mathf.Atan2(dir.normalized.y, dir.normalized.x);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg - 90f), speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) // ����������� ����������� � ��������
    {
        if (other.gameObject.tag == "Plane") // � ��������
        {
            isDefeat.isDef = true; // ���������
        }
        if (other.gameObject.tag == "Target") // � �����
        {
            Score.score += 1; 
            gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class plane : MonoBehaviour
{
    public DrawWithMouse drawControl; //линия
    public float speed = 1f; //скорость
    float pi = Mathf.Acos(-1);
    Vector3[] positions; //позиции
    Vector2 vecMovement; //радиус-вектор движения

    bool startMovement = false; // проверка на движение по линии
    int moveIndex = 0; // текущий индекс
    float prevTime = 0; // последний момент времени, когда тело выходило за поле

    private void get(Vector2 pos) // определение начального движения
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
    {   //задается направление
        System.Random rnd = new System.Random();
        int an = rnd.Next(0, 359);
        float angle = ((float)an / 180) * pi;
        vecMovement = new Vector2(0.1f * Mathf.Cos(angle), 0.1f * Mathf.Sin(angle));
        get(transform.position);

        prevTime = Time.time;
    }
    private void OnMouseDown() //начало рисования линии
    {
        drawControl.StartLine(transform.position); 
    }

    private void OnMouseDrag() //продолжение рисования линии
    {
        drawControl.UpdateLine(); 
    }

    private void OnMouseUp() //окончание рисования линии
    {
        positions = new Vector3[drawControl.line.positionCount];
        drawControl.line.GetPositions(positions); //получение точек кривой
        startMovement = true;
        moveIndex = 0;
        for(int i = 0; i < positions.Length; ++i) // поиск начала движения
        {
            float distance = Vector2.Distance(positions[i], transform.position);
            if (distance <= 0.05f)
            {
                moveIndex = i;
            }
        }
    }

    public void check() // проверка на выход из поля
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

        check(); // функция, чтобы самолёт не вылетал из поля

        if (startMovement) // движение по линии
        {
            Vector2 currentPos = positions[moveIndex];
            transform.position = Vector2.MoveTowards(transform.position, currentPos, speed * Time.deltaTime); // движение

            Vector2 dir = currentPos - (Vector2)transform.position;
            vecMovement = dir;
            float angle = Mathf.Atan2(dir.normalized.y, dir.normalized.x);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg - 90f), speed); // определение положение тела

            float distance = Vector2.Distance(currentPos, transform.position);
            if (distance <= 0.05f) // перенос на следующую точку
            {
                moveIndex += 1;
            }

            if (moveIndex > positions.Length - 1) // окончание линии
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

    private void OnTriggerEnter2D(Collider2D other) // определение пересечения с объектом
    {
        if (other.gameObject.tag == "Plane") // с самолётом
        {
            isDefeat.isDef = true; // Поражение
        }
        if (other.gameObject.tag == "Target") // с целью
        {
            Score.score += 1; 
            gameObject.SetActive(false);
        }
    }
}

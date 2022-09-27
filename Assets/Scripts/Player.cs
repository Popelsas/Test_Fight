using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private GameObject _target;
    private float _health;
    private float _force;
    private Player _targetPl;

    public float speed;
    public bool fight;
    
    public float Health { 
        get 
        { 
            return _health; 
        } 
        set 
        {
            if (_health > 0)
                _health = value;
            else 
                _health = 0;
        } 
    }

    void Start()
    {
        //Рандомные значения ХП и силы атаки
        _health = Random.Range(50, 100);
        _force = Random.Range(0.01f, 0.05f);
        //Флаг ведения боя
        fight = false;
    }

    void Update()
    {
        if(_target == null)
        {
            fight = false;
            //Поиск ближайшей цели
            _target = FindClosestTarget();
        }
        else
        {        
            //Поворот лицом к цели
            LookAt(transform, _target.transform.position);
            if (GetDistance() > 2)
            {    
                //Остановка, если цель уже в бою
                if (_targetPl.fight == true)
                    _target = null;
                //Движение к цели, если дистанция недостаточна для атаки
                transform.Translate(Vector3.forward * speed);
            }
            else
            {
                //Атака, если позволяет дистанция
                float targetHealth = _targetPl.Health;
                fight = true;
                if (targetHealth > 0)
                    _targetPl.Health -= _force;
            }
        }
        //Смерть
        if(_health <= 0)
        {
            GameController.instance.players.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    private float GetDistance()
    {
        var heading = _target.transform.position - transform.position;
        float distance = heading.magnitude;
        return distance;        
    }

    private GameObject FindClosestTarget()
    {
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        if(GameController.instance.players.Count > 1)
        {
            foreach (GameObject go in GameController.instance.players)
            {
                if (go == this.gameObject || go.GetComponent<Player>().fight)
                    continue;
                Vector3 diff = go.transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    closest = go;
                    distance = curDistance;
                }
            }
        }
        if (closest != null)
            _targetPl = closest.GetComponent<Player>();
        return closest;
    }

    private void LookAt(Transform transform, Vector3 point)
    {
        var direction = (point - transform.position).normalized;
        direction.y = 0f;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}

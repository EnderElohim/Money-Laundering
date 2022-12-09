using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class scrMoveWithWaypoints : MonoBehaviour
{
    public Vector3[] waypoints;
    public float actionSpeed;
    public Ease moveStyle;

    private int currentWaypointId;

    public void Setup(Vector3[] _waypoints, float _actionSpeed, Ease _moveStyle)
    {
        waypoints = _waypoints;
        actionSpeed = _actionSpeed;
        moveStyle = _moveStyle;
        MoveToPoint();
    }

    public void RemoveThis()
    {
        transform.DOKill();
        Destroy(this);
    }

    private void MoveToPoint()
    {
        transform.DOMove(waypoints[currentWaypointId], actionSpeed).SetEase(moveStyle).OnComplete(() =>{
            currentWaypointId++;
            if (currentWaypointId < waypoints.Length)
            {
                MoveToPoint();
            }
            else
            {
                Destroy(this);
            }
        });
    }
   
}

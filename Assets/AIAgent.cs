using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : Agent
{
    Rigidbody rBody;
    public GameObject otherPlayer;
    public float Speed = 1;
    //float previousDistance = 100f;

    void Start ()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public override void AgentReset()
    {
        Debug.Log("reset");
        rBody.velocity = Vector3.zero;
    }

    public override void CollectObservations()
    {
        var fruits = GameObject.FindGameObjectsWithTag("Ball");
        for (int i = 0; i < 1; i++) //TODO: make bigger?
        {
            if (fruits.Length > i)
            {
                float x = (fruits[0].transform.position.x - transform.position.x - 12) / 45;
                float y = (fruits[0].transform.position.y - transform.position.y - 15) / 30;

                AddVectorObs(x);
                AddVectorObs(y);

                AddVectorObs(fruits[0].GetComponent<Rigidbody>().velocity.x);
                AddVectorObs(fruits[0].GetComponent<Rigidbody>().velocity.y);
            }
            else
            {
                AddVectorObs(0);
                AddVectorObs(0);
                AddVectorObs(0);
                AddVectorObs(0);
            }
        }
        AddVectorObs(this.transform.position.x);
        //AddVectorObs(otherPlayer.gameObject.transform.position.x);

        //Monitor.Log("Reward", GetCumulativeReward(), MonitorType.text);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        int movement = Mathf.FloorToInt(vectorAction[0]);
        int directionX = 0;
        if (movement == 0) { directionX = -1; }
        if (movement == 1) { directionX = 1; }

        rBody.velocity = new Vector3(directionX * Speed, 0, 0);

        //float minDistance = 50f;
        //Vector3 closest = new Vector3(100, 100, 100);
        //foreach (var fruit in GameObject.FindGameObjectsWithTag("Ball"))
        //{
        //    float distanceToTarget = Vector3.Distance(fruit.transform.position, this.transform.position);
        //    if (distanceToTarget < minDistance)
        //    {
        //        minDistance = distanceToTarget;
        //        closest = fruit.transform.position;
        //    }
        //}

        //Vector3 distanceToClosest = closest - this.transform.position;
        //if (distanceToClosest.x <= previousDistance)
        //{
        //    AddReward(0.1f);
        //}
        //else
        //{
        //    AddReward(-0.1f);
        //}
        //previousDistance = distanceToClosest.x;
    }
}

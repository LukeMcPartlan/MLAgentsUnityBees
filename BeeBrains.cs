using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class BeeBrains : Agent
{
    Rigidbody rBody;
    float rewardy;

    //on start
    void Start () {
        rBody = GetComponent<Rigidbody>();
    }
    public Transform Goal;
    public Transform Target;
    public Rigidbody BallBodey;
    public override void OnEpisodeBegin()
    {
    
       // If the Agent fell, zero its momentum
        rewardy = .01f;
        
        if (this.transform.localPosition.y < -11 || this.transform.localPosition.y >11 || this.transform.localPosition.z < -11 || this.transform.localPosition.z >11 || this.transform.localPosition.x < -11 || this.transform.localPosition.x >11 )
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3( 0, 0.5f, 2);
        }

        // Move the Goal to a new spot
        Goal.localPosition = new Vector3(0,0,0);
        // Move the Target to a new spot
        BallBodey.velocity = Vector3.zero;
        BallBodey.angularVelocity = Vector3.zero;

        float buggyX = Random.value * 5 - 4;
        float buggyZ = Random.value * 5 - 4;
        float buggyY = Random.value * 5 - 4;
        if(buggyX < 2)
        {
            buggyX+=2;
        }
        if(buggyY < 2)
        {
            buggyY+=2;
        }
        if(buggyZ < 2)
        {
            buggyZ+=2;
        }


        Target.localPosition = new Vector3(buggyX,buggyY,buggyZ);
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent and goal positions
        sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(Goal.localPosition);
        sensor.AddObservation(this.transform.localPosition);

        // Agent velocity
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);

        // Ball velocity
        sensor.AddObservation(BallBodey.velocity.x);
        sensor.AddObservation(BallBodey.velocity.z);
        sensor.AddObservation(Vector3.Distance(this.transform.localPosition, Target.localPosition));
        sensor.AddObservation(Vector3.Distance(Goal.localPosition, this.transform.localPosition));
        sensor.AddObservation(Vector3.Distance(Goal.localPosition, Target.localPosition));
    }
    public float forceMultiplier = 10;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 3
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];
        controlSignal.y = actionBuffers.ContinuousActions[2];
        rBody.AddForce(controlSignal * forceMultiplier);

        // Rewards
        float distanceToTarget = Vector3.Distance(Goal.localPosition, Target.localPosition);
         //close to bug
        float touchTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);
       
        if (touchTarget < 1.5f)
        {
            rewardy = 1f;
            //Debug.log("ok");
        }

        // Reached target
        if (distanceToTarget < 2f)
        {
            rewardy = 100.0f;
            SetReward(rewardy);
            EndEpisode();
        }

        //Bug is free
        if (distanceToTarget > 11f)
        {
            SetReward(rewardy);
            EndEpisode();
        }
       

        // Fell off platform
        else if (this.transform.localPosition.y < -11 || this.transform.localPosition.y >11 || this.transform.localPosition.z < -11 || this.transform.localPosition.z >11 || this.transform.localPosition.x < -11 || this.transform.localPosition.x >11 )
        {
            SetReward(0f);
            EndEpisode();
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
        continuousActionsOut[2] = Input.GetAxis("Zeddy");
        Debug.Log(continuousActionsOut[1]);
        Debug.Log(Input.GetAxis("Zeddy"));
    }
}
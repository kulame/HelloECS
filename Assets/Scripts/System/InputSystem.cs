using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;


public partial class InputSystem : SystemBase
{
    private InputActions inputs = null;
    EntityQuery m_VehicleInputQuery;



    protected override void OnCreate()
    {

        inputs = new InputActions();
        m_VehicleInputQuery = GetEntityQuery(typeof(VehicleInput));
        Debug.Log("OnCreate");

    }

    protected override void OnStartRunning()
    {
        Debug.Log("OnStartRunning");
        inputs.Vehicle.Enable();


    }

    protected override void OnUpdate()
    {
        Debug.Log("onUpdate");
        Vector2 m_VehicleLooking = inputs.Vehicle.Look.ReadValue<Vector2>();
        Vector2 m_VehicleSteering = inputs.Vehicle.Steering.ReadValue<Vector2>();
        float m_VehicleThrottle = inputs.Vehicle.Throttle.ReadValue<float>();
        int m_VehicleChanged = 0;
        if (inputs.Vehicle.Previous.WasPerformedThisFrame())
        {
            m_VehicleChanged = -1;
        }
        if (inputs.Vehicle.Next.WasPerformedThisFrame())
        {
            m_VehicleChanged = 1;
        }


        if (m_VehicleInputQuery.CalculateEntityCount() == 0)
            EntityManager.CreateEntity(typeof(VehicleInput));

        m_VehicleInputQuery.SetSingleton(new VehicleInput
        {
            Looking = m_VehicleLooking,
            Steering = m_VehicleSteering,
            Throttle = m_VehicleThrottle,
            Change = m_VehicleChanged
        });

    }
}


struct VehicleInput : IComponentData
{
    public float2 Looking;
    public float2 Steering;
    public float Throttle;
    public int Change;
}



using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class AiCarHandler : MonoBehaviour
{
    public Transform pathGroup;
    public Vector3 centerOfMass;

    //Move varibles

    public float maxSteer = 15;
    public float maxTorque = 50;
    public float currentSpeed;
    public float topSpeed = 150;
    public float decelerationSpeed = 10;

    //end of move

    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;

    public int currentPathObj;
    public float distFromPath;

    //Sensor varibles

    public Color sensorColor = Color.magenta;
    public float sensorLength = 5;
    public float frontSensorStartPoint = 5;
    public float frontSensorSideDistance = 2;
    public float frontSensorsAngle = 30;
    public float sidewaySensorLength = 5;

    private int flag = 0;

    //end of sensor

    private List<Transform> path;
    private Rigidbody rb;

    void Start()
    {
        path = new List<Transform>();
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass;
        getPath();
    }

    void Update()
    {
        if (flag == 0)
            getSteer();

        Move();
        Sensors();
    }

    void getPath()
    {
        Transform[] childObejects = pathGroup.GetComponentsInChildren<Transform>();

        for (int i = 0; i < childObejects.Length; i++)
        {
            Transform temp = childObejects[i];
            if (temp.gameObject.GetInstanceID() != GetInstanceID())
                path.Add(temp);
        }

    }

    void getSteer()
    {
        Vector3 steerVector = transform.InverseTransformPoint(new Vector3(path[currentPathObj].position.x, transform.position.y, path[currentPathObj].position.z));
        float newSteer = maxSteer * (steerVector.x / steerVector.magnitude);
        wheelFL.steerAngle = newSteer;
        wheelFR.steerAngle = newSteer;

        if (steerVector.magnitude <= distFromPath)
        {
            currentPathObj++;
        }

        if (currentPathObj >= path.Count)
        {
            currentPathObj = 0;
        }
    }

    void Move()
    {
        currentSpeed = 2 * (22 / 7) * wheelRL.radius * wheelRL.rpm * 60 / 1000;
        currentSpeed = Mathf.Round(currentSpeed);

        if (currentSpeed <= topSpeed)
        {
            wheelRL.motorTorque = maxTorque;
            wheelRR.motorTorque = maxTorque;
            wheelRL.brakeTorque = 0;
            wheelRR.brakeTorque = 0;
        }
        else
        {
            wheelRL.motorTorque = 0;
            wheelRR.motorTorque = 0;
            wheelRL.brakeTorque = decelerationSpeed;
            wheelRR.brakeTorque = decelerationSpeed;
        }
    }

    void Sensors()
    {
        Vector3 pos;
        RaycastHit hit;
        flag = 0;
        float AvoidSensivety = 0;
        Vector3 rightAngle = Quaternion.AngleAxis(frontSensorsAngle, transform.up) * transform.forward;
        Vector3 leftAngle = Quaternion.AngleAxis(-frontSensorsAngle, transform.up) * transform.forward;

        // ======================== Mid Sensor ======================

        pos = transform.position;
        pos += transform.forward * frontSensorStartPoint;

        if (Physics.Raycast(pos, transform.forward, out hit, sensorLength))
        {
            Debug.DrawLine(pos, hit.point, sensorColor);
        }

        // ======================== Right ===========================

        pos += transform.right * frontSensorSideDistance;

        if (Physics.Raycast(pos, transform.forward, out hit, sensorLength))
        {
            if (hit.transform.tag != "Terrain")
            {
                flag++;
                AvoidSensivety -= 0.5f;
            }
            Debug.DrawLine(pos, hit.point, sensorColor);
        }

        if (Physics.Raycast(pos, rightAngle, out hit, sensorLength))
        {
            Debug.DrawLine(pos, hit.point, sensorColor);
        }

        // ========================= Left =============================

        pos = transform.position;
        pos += transform.forward * frontSensorStartPoint;
        pos -= transform.right * frontSensorSideDistance;

        if (Physics.Raycast(pos, transform.forward, out hit, sensorLength))
        {
            if (hit.transform.tag != "Terrain")
            {
                flag++;
                AvoidSensivety += 0.5f;
            }
            Debug.DrawLine(pos, hit.point, sensorColor);
        }

        if (Physics.Raycast(pos, leftAngle, out hit, sensorLength))
        {
            Debug.DrawLine(pos, hit.point, sensorColor);
        }

        // ======================== SideRight ===========================

        if (Physics.Raycast(transform.position, transform.right, out hit, sidewaySensorLength))
        {
            Debug.DrawLine(transform.position, hit.point, sensorColor);
        }

        // ======================== SideLeft ===========================

        if (Physics.Raycast(transform.position, -transform.right, out hit, sidewaySensorLength))
        {
            Debug.DrawLine(transform.position, hit.point, sensorColor);
        }

        if (flag != 0)
        {
            AvoidSteer(AvoidSensivety);
        }
    } //end of Sensors

    void AvoidSteer(float sensivety)
    {
        wheelFL.steerAngle = maxSteer * sensivety;
        wheelFR.steerAngle = maxSteer * sensivety;
    }
}

using UnityEngine;

public class AICar : MonoBehaviour
{
    #region --- helpers ---
    private struct structAI
    {
        public Transform checkpoints;
        public int idx;
        public Vector3 directionSteer;
        public Quaternion rotationSteer;
    }
    #endregion

    public float MoveSpeed = 0f;
    public float TurnSpeed = 0.1f;
    [SerializeField] int FinishTimes = 0;
    private Rigidbody rb = null;
    private structAI ai;


    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();

        ai.checkpoints = GameObject.FindWithTag("Checkpoints").transform;
        ai.idx = 0;
    }
    private void FixedUpdate()
    {
        if (RaceStart.RaceStarted == true)
        {
            //turn
            ai.directionSteer = ai.checkpoints.GetChild(ai.idx).position - this.transform.position;
            ai.rotationSteer = Quaternion.LookRotation(ai.directionSteer);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, ai.rotationSteer, TurnSpeed);

            //move
            rb.AddRelativeForce(Vector3.forward * MoveSpeed, ForceMode.VelocityChange);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall") == true)
        {
            ai.idx = CalcNextCheckpoint();
        }
        if (other.CompareTag("Finish"))
        { 
            FinishTimes++;
        }
        if (FinishTimes > 2)
        {
            Debug.Log("AI won");
        }

        if (other.CompareTag("0.2") == true)
        {
            MoveSpeed = 0.2f;
            ai.idx = CalcNextCheckpoint();
        }
        if (other.CompareTag("0.3") == true)
        {
            MoveSpeed = 0.3f;
            ai.idx = CalcNextCheckpoint();
        }
        if (other.CompareTag("0.4") == true)
        {
            MoveSpeed = 0.4f;
            ai.idx = CalcNextCheckpoint();
        }
        if (other.CompareTag("0.5") == true)
        {
            MoveSpeed = 0.5f;
            ai.idx = CalcNextCheckpoint();
        }
        if (other.CompareTag("0.7") == true)
        {
            MoveSpeed = 0.7f;
            ai.idx = CalcNextCheckpoint();
        }
        if (other.CompareTag("0.9") == true)
        {
            MoveSpeed = 0.9f;
            ai.idx = CalcNextCheckpoint();
        }
        if (other.CompareTag("1.0") == true)
        {
            MoveSpeed = 1.0f;
            ai.idx = CalcNextCheckpoint();
        }
    }
    private int CalcNextCheckpoint()
    {
        int curr = ExtractNumberFromString(ai.checkpoints.GetChild(ai.idx).name);
        int next = curr + 1;
        if (next > ai.checkpoints.childCount - 1)
            next = 0;

        Debug.Log(string.Format("current checkpoint {0}, next {1}", curr, next));

        return next;
    }
    private int ExtractNumberFromString(string s1)
    {
        return System.Convert.ToInt32(System.Text.RegularExpressions.Regex.Replace(s1, "[^0-9]", ""));
    }
}
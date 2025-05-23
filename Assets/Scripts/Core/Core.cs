using UnityEngine;

public class Core : MonoBehaviour
{
    public Movement Movement { get; private set; }
    public CollisionSenses CollisionSenses { get; private set; }

    private void Awake()
    {
        Movement = GetComponentInChildren<Movement>();
        CollisionSenses = GetComponentInChildren<CollisionSenses>();

        if (!Movement || !CollisionSenses)
        {
            Debug.LogError("Missing core component");
        }
    }

    public void LogicUpdate()
    {
        Movement.LogicUpdate();
    }

}

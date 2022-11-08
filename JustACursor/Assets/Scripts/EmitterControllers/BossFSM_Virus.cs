using UnityEngine;

public class BossFSM_Virus : BasicBossFSM
{
    [Header("- VIRUS -")]
    [SerializeField] private Energy energy;
    [SerializeField] private BoxCollider2D levelHeight;
    [SerializeField] private BoxCollider2D levelLength;
    private Vector2 levelCenter;
    private bool move;
    private readonly float moveSpeed = 5f; 

    private void Start()
    {
        Init();
        
        levelCenter = new Vector2(levelLength.bounds.center.x, levelHeight.bounds.center.y + levelHeight.bounds.extents.x);
    } 

    private void Update()
    {
        // DEBUG
        UpdateDebugInput();

        if (Input.GetKeyDown(KeyCode.M))
        {
            move = true;
            // energy.SpeedUpTime();
        }

        if (move) GoToCenter();
    }

    private void GoToCenter()
    {
        move = Vector3.Distance(levelCenter, transform.position) >= 0.05f;       
        transform.position = Vector3.MoveTowards(transform.position, levelCenter, moveSpeed * Time.deltaTime); 
    }
}

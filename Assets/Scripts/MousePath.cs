using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePath : MonoBehaviour
{
    public DrawWithMouse drawControl;
    public bool startMovement;
    Vector3[] positions;
    int moveIndex;
    public float speed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if( startMovement == true)
        {
            Vector2 currentPos = positions[moveIndex];
            transform.position = Vector2.MoveTowards(transform.position, currentPos, speed*Time.deltaTime);

            float distance = Vector2.Distance(currentPos, transform.position);

            if(distance <= 0.05f)
            {
                moveIndex++;
            }

            if(moveIndex > positions.Length -1)
            {
                startMovement = false;
            }
        }
        
    }

    private void OnMouseDown()
    {
        drawControl.StartLine(transform.position);

        
    }

    private void OnMouseDrag()
    {
        drawControl.UpdateLine();
        
    }

    private void OnMouseUp()
    {
        positions = new Vector3[drawControl.line.positionCount];
        drawControl.line.GetPositions(positions);
        startMovement = true;
        moveIndex = 0;

        
    }
}

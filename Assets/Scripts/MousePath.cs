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
    public Camera cam;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if( startMovement == true)
        {

            // Set gravity scale to 0 to disable gravity while moving
            rb.gravityScale = 0f;
            //rb.isKinematic = true; // Disable gravity initially


            Vector2 currentPos = positions[moveIndex];
            //cam.fieldOfView = cam.fieldOfView * 2;
            transform.position = Vector2.MoveTowards(transform.position, currentPos, speed*Time.deltaTime);

            float distance = Vector2.Distance(currentPos, transform.position);

            if(distance <= 0.05f)
            {
                moveIndex++;
            }

            if(moveIndex > positions.Length -1)
            {
                startMovement = false;

                // Activate gravity when it reaches the end of the path
                rb.gravityScale = 2f;
            }
        }
        
    }

    private void OnMouseDown()
    {
        //Camera.main.fieldOfView = Camera.main.fieldOfView * 2;
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

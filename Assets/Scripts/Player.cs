using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool Move(Vector2 direction) // avoid ability to move diagonally
    {
        if (Mathf.Abs(direction.x) < 0.5)
        {
            direction.x = 0;
        }
        else
        {
            direction.y = 0;
        }

        direction.Normalize();

        Vector2 newPos = NewPosition(transform.position, direction);

        if (newPos.x == transform.position.x && newPos.y == transform.position.y)
        {
            return false;
        }
        else
        {
            //transform.TransformPoint()
            transform.Translate(new Vector2(newPos.x - transform.position.x, newPos.y - transform.position.y));
            return true;
        }
    }

    Vector2 NewPosition(Vector3 position, Vector2 direction)
    {
        Vector2 currentPos = new Vector2(position.x, position.y);
        Vector2 newPos = currentPos + direction;

        GameObject portal1 = GameObject.FindGameObjectsWithTag("Portal")[0];
        GameObject portal2 = GameObject.FindGameObjectsWithTag("Portal")[1];

        // see if player will move through portal (if they can move at all, which is checked later)
        if (portal1.transform.position.x == newPos.x && portal1.transform.position.y == newPos.y)
        {
            // set newPos to the other portal's position + direction
            newPos = new Vector2(portal2.transform.position.x, portal2.transform.position.y) + direction;
        } else if (portal2.transform.position.x == newPos.x && portal2.transform.position.y == newPos.y)
        {
            newPos = new Vector2(portal1.transform.position.x, portal1.transform.position.y) + direction;
        }


        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

        foreach (var wall in walls)
        {
            if (wall.transform.position.x == newPos.x && wall.transform.position.y == newPos.y)
            {
                // blocked by wall; don't move
                return currentPos;
            }
        }

        GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");

        foreach(var box in boxes)
        {
            if (box.transform.position.x == newPos.x && box.transform.position.y == newPos.y)
            {
                Box bx = box.GetComponent<Box>();
                if (bx && bx.Move(direction))
                {
                    // both player & box move
                    return newPos;
                }
                else
                {
                    // blocked because box can't move; don't move player
                    return currentPos;
                }
            }
        }
        return newPos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public bool m_OnTarget;

    public bool Move(Vector2 direction) // avoid ability to move diagonally
    {
        Vector2 newPos = NewPosition(transform.position, direction);

        if (newPos.x == transform.position.x && newPos.y == transform.position.y)
        {
            return false;
        }
        else
        {
            transform.Translate(new Vector2(newPos.x - transform.position.x, newPos.y - transform.position.y));
            TestForOnTarget();
            return true;
        }
    }

    Vector2 NewPosition(Vector3 position, Vector2 direction)
    {
        Vector2 currentPos = new Vector2(position.x, position.y);
        Vector2 newPos = new Vector2(position.x, position.y) + direction;


        GameObject portal1 = GameObject.FindGameObjectsWithTag("Portal")[0];
        GameObject portal2 = GameObject.FindGameObjectsWithTag("Portal")[1];

        // see if player will move through portal (if they can move at all, which is checked later)
        if (portal1.transform.position.x == newPos.x && portal1.transform.position.y == newPos.y)
        {
            // set newPos to the other portal's position + direction
            newPos = new Vector2(portal2.transform.position.x, portal2.transform.position.y) + direction;
        }
        else if (portal2.transform.position.x == newPos.x && portal2.transform.position.y == newPos.y)
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

        foreach (var box in boxes)
        {
            if (box.transform.position.x == newPos.x && box.transform.position.y == newPos.y)
            {
                // blocked by another box
                return currentPos;
            }
        }

        return newPos;
    }

    void TestForOnTarget()
    {
        GameObject[] crosses = GameObject.FindGameObjectsWithTag("Target");
        foreach (var cross in crosses)
        {
            if (transform.position.x == cross.transform.position.x && transform.position.y == cross.transform.position.y)
            {
                GetComponent<SpriteRenderer>().color = Color.red;
                m_OnTarget = true;
                return;
            }
        }
        GetComponent<SpriteRenderer>().color = Color.white;
        m_OnTarget = false;
    }
}

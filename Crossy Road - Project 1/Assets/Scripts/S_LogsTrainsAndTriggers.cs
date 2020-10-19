using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_LogsTrainsAndTriggers : MonoBehaviour
{
    GridMovement gridMove;

    private void Start()
    {
        gridMove = this.GetComponent<GridMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Exit")) //ending platform
        {
            gridMove.ActivateFlip();
        }

        if (other.gameObject.CompareTag("Car"))
        {
            //gridMove.Die();
            Debug.Log("Hit by a vehicle");
        }

        if (other.gameObject.CompareTag("Log"))
        {
            transform.parent = other.transform; //follow log path
            gridMove.targetPos.transform.parent = other.transform;
            CancelInvoke();
        }
        else if (other.gameObject.CompareTag("Water"))
        {
            Invoke("InvokeDie", .1f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Log")) //for jumping between logs
        {
            transform.parent = other.transform;
            gridMove.targetPos.transform.parent = other.transform;
            CancelInvoke();
        }
        else if (other.gameObject.CompareTag("Water")) //die if jump from log to water
        {
            Invoke("InvokeDie", .1f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Log"))
        {
            transform.parent = null;
            gridMove.targetPos.transform.parent = null;
            CancelInvoke();
        }
    }

    void InvokeDie()
    {
        gridMove.Die();
    }


}

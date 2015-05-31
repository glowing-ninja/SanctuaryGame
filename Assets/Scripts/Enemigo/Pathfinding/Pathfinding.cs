using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : ENComportamiento {
    
	protected List<Vector3> Path = new List<Vector3>();
	protected Pathfinder pathfinder;
	//public float smothRotation = 6.0f;

	void Awake() {
		pathfinder = GetComponentInParent<Pathfinder> ();
	}

    public void FindPath(Vector3 startPosition, Vector3 endPosition)
    {
		pathfinder.InsertInQueue(startPosition, endPosition, SetList);
    }

    //A test move function, can easily be replaced
    public void Move(float velocidad, bool reset)
    {
        if (!reset && PuedoVerTarget())
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * velocidad);
            RotarModelo(target.transform.position);
        }
        else
        {
            if (Path.Count > 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, Path[0], Time.deltaTime * velocidad);
                RotarModelo(Path[0]);
                if (Vector3.Distance(transform.position, Path[0]) < 0.4F)
                {
                    Path.RemoveAt(0);
                }
            }
        }
    }
	public void RotarModelo(Vector3 look) {
		Quaternion rot = Quaternion.LookRotation(look-modelo.transform.position,Vector3.up);
		modelo.transform.rotation = Quaternion.Slerp(modelo.transform.rotation,rot,Time.deltaTime*6.0f);
	}

    protected virtual void SetList(List<Vector3> path)
    {
        if (path == null)
        {
            return;
        }

        Path.Clear();
        Path = path;
        if (Path.Count > 0)
        {
            //Path[0] = new Vector3(Path[0].x, Path[0].y - 1, Path[0].z);
            //Path[Path.Count - 1] = new Vector3(Path[Path.Count - 1].x, Path[Path.Count - 1].y - 1, Path[Path.Count - 1].z);
        }
    }

    private bool PuedoVerTarget()
    {
        RaycastHit hit;
        Vector3 direccion = target.transform.position - transform.position;
        direccion.y += 1.0f;
        float dist = Vector3.Distance(target.transform.position, transform.position) + 5f;
        Ray lookRay = new Ray(transform.position, direccion);

        if (Physics.Raycast(lookRay, out hit, dist))
        {
            if (hit.collider.tag != "Destructible")
            {
                return true;
            }
        }
        return false;
    }
}
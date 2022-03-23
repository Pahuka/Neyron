using Neyron.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

public class Boid
{

    private Dot dot;

    public Vector3 target = new Vector3(0f, 0f, 0f);
    private Vector3 transform;

    // Start is called before the first frame update
    void Start(Dot dot)
    {
        this.dot = dot;
    }

    // Update is called once per frame
    void Update()
    {
        transform = new Vector3(0f, 0f, (float)(Math.Atan2(dot.Y, dot.X) * Math.PI/180));
    }

    void FixedUpdate(Dot[] dots)
    {
        //float vision = 5f;
        //Collider2D[] dots = Physics2D.OverlapCircleAll(transform.position, vision);
        for (int i = 0; i < dots.Length; i++)
        {
            if (dots[i].X != dot.X)
                if(dots[i].Y != dot.Y)
                    continue;
            //Boid boid = dots[i].gameObject.GetComponent<Boid>();
            //if (boid)
            //{
                var pos = new Vector3((float)dots[i].X, (float)dots[i].Y, (float)(Math.Atan2(dots[i].Y, dots[i].X) * Math.PI / 180));
                float dist = Vector3.Distance(transform, pos);
                pos -= transform;
                dist = Math.Max(1f, dist);
                if (dist < 3) target -= pos / dist * 100;
                target += pos / dist;
                //target += boid.target * 10f;
            //}
        }
        // мышь
        // Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // pz.z = 0;
        // pz -= transform.position;
        // float sign = 10f;
        // if(pz.magnitude < 5f) sign = -1000f;
        // pz.Normalize();
        // target += pz * sign;
        //target.Normalize();
        dot.X += target.X;
        dot.Y += target.Y;
        //dot.velocity *= 0.9f;
    }

}
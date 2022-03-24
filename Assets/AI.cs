using Neyron.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Windows.Media;
using System.Windows.Shapes;

public class AI
{
    public static int[] skillsTotal = new int[4];
    //private Random random = new Random();
    private Vector2 transform;
    Dot dot;
    //public GameObject bacteriumPrefab;

    public int foodSkill = 0;
    public int attackSkill;
    public int defSkill = 0;
    public float energy = 10;
    //public float age = 0;

    private int inputsCount = 4;
    private Genome genome;
    private NN nn;

    //private Rigidbody2D dot;

    // Start is called before the first frame update
    public AI(Dot dot)
    {
        this.dot = dot;
        attackSkill = dot.Attack;
    }

    // Update is called once per frame
    void Update()
    {
        transform = new Vector2(0f, 0f);
        //age++;
    }

    public void FixedUpdate(Dot[] targets)
    {
        float vision = 5f + attackSkill;
        float[] inputs = new float[inputsCount];

        // количество соседних объектов
        float[] neighboursCount = new float[4];

        // вектара к центрам масс еды, красного, зеленого и синего
        Vector2[] vectors = new Vector2[4];
        for (int i = 0; i < 4; i++)
        {
            neighboursCount[i] = 0;
            vectors[i] = new Vector2(0f, 0f);
        }
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i].Id == dot.Id) continue;
            if (targets[i].DotClass == "food")
            {
                neighboursCount[0]++;
                vectors[0] += targets[i].Position - transform;
            }
            //else if (targets[i].DotClass == "bacterium")
            //{
            //    AI ai = targets[i].gameObject.GetComponent<AI>();
            //    neighboursCount[1] += ai.attackSkill / 3f;
            //    vectors[1] += (targets[i].gameObject.transform.position - transform) * ai.attackSkill;
            //    neighboursCount[2] += ai.foodSkill / 3f;
            //    vectors[2] += (targets[i].gameObject.transform.position - transform) * ai.foodSkill;
            //    neighboursCount[3] += ai.defSkill / 3f;
            //    vectors[3] += (targets[i].gameObject.transform.position - transform) * ai.defSkill;
            //}
        }
        for (int i = 0; i < 4; i++)
        {
            if (neighboursCount[i] > 0)
            {
                var divide = neighboursCount[i] * vision;
                vectors[i].X /= divide;
                vectors[i].Y /= divide;
                inputs[i] = MagnitudeVector2(vectors[i]);
            }
            else
            {
                inputs[i] = 0f;
            }
        }

        float[] outputs = nn.FeedForward(inputs);
        Vector2 target = new Vector2(0, 0);
        for (int i = 0; i < 4; i++)
        {
            if (neighboursCount[i] > 0)
            {
                Vector2 dir = new Vector2(vectors[i].X, vectors[i].Y);
                //dir.Normalize();
                target += dir * outputs[i];
            }
        }
        //if (MagnitudeVector2(target) > 1f) 
        //    target.Normalize();
        Vector2 velocity = new Vector2();
        velocity += target * (0.25f + attackSkill * 0.05f);
        velocity *= 0.98f;
        dot.Position = velocity;
        dot.Move();
        //float antibiotics = 1f;
        // концентрация антибиотиков
        // if(transform.position.x < -39) antibiotics = 4;
        // else if(transform.position.x < -20) antibiotics = 3;
        // else if(transform.position.x < -1) antibiotics = 2;
        // antibiotics = Mathf.Max(1f, antibiotics - defSkill);
        //energy -= Time.deltaTime * antibiotics * antibiotics;
        //energy--;
        //if (energy < 0f)
        //{
        //    Kill();
        //}
    }

    private float MagnitudeVector2(Vector2 vector)
    {
        return (float)(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2));
    }

    private float MagnitudeVector3(Vector3 vector)
    {
        return (float)(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2) + Math.Pow(vector.Z, 2));
    }

    //void OnTriggerEnter2D(Collider2D col)
    //{
    //    if (foodSkill == 0) return;
    //    if (col.gameObject.name == "food")
    //    {
    //        Eat(foodSkill);
    //        Destroy(col.gameObject);
    //    }
    //}

    //void OnCollisionEnter2D(Collision2D col)
    //{
    //    if (age < 1f) return;
    //    if (attackSkill == 0) return;
    //    if (col.gameObject.name == "bacterium")
    //    {
    //        AI ai = col.gameObject.GetComponent<AI>();
    //        if (ai.age < 1f) return;
    //        float damage = Math.Max(0f, attackSkill - ai.defSkill);
    //        damage *= 4f;
    //        damage = Math.Min(damage, ai.energy);
    //        ai.energy -= damage * 1.25f;
    //        Eat(damage);
    //        if (ai.energy == 0f) ai.Kill();
    //    }
    //}

    public void Init(Genome g)
    {
        genome = g;
        //Color col = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
        float size = 0.75f;
        for (int i = 0; i < Genome.skillCount; i++)
        {
            skillsTotal[g.skills[i]]++;
            if (g.skills[i] == 0)
            {
                foodSkill++;
                //col.G += 0.2f;
            }
            else if (g.skills[i] == 1)
            {
                attackSkill++;
                //col.r += 0.25f;
            }
            else if (g.skills[i] == 2)
            {
                defSkill++;
                //col.b += 0.25f;
            }
            else if (g.skills[i] == 3)
            {
                size += 0.5f;
            }
        }
        transform = new Vector2(size, size);
        //gameObject.GetComponent<SpriteRenderer>().color = col;
        nn = new NN(inputsCount, 8, 4);
        for (int i = 0; i < inputsCount; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                nn.layers[0].weights[i, j] = genome.weights[i + j * inputsCount];
            }
        }
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                nn.layers[1].weights[i, j] = genome.weights[i + j * 8 + inputsCount * 8];
            }
        }
    }

    //public void Kill()
    //{
    //    for (int i = 0; i < Genome.skillCount; i++)
    //    {
    //        skillsTotal[genome.skills[i]]--;
    //    }
    //    Destroy(gameObject);
    //}

    //private void Eat(float food)
    //{
    //    energy += food;
    //    if (energy > 16)
    //    {
    //        energy *= 0.5f;
    //        //GameObject b = (GameObject)Object.Instantiate(Resources.Load("m1", typeof(GameObject)), new Vector3(0, 0, 0), Quaternion.identity);
    //        var b = new Dot(new Ellipse()
    //        {
    //            Height = 25,
    //            Width = 25,
    //            Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(200, 0, 0))
    //        }, "food")
    //        { X = random.Next(0, (int)myCanvas.Height), Y = random.Next(0, (int)myCanvas.Width) };);
    //        b.transform.position = transform.position;
    //        b.name = "bacterium";
    //        Genome g = new Genome(genome);
    //        g.Mutate(0.5f);
    //        AI ai = b.GetComponent<AI>();
    //        ai.Init(g);
    //        ai.energy = energy;
    //    }
    //}
}
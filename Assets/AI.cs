using Neyron;
using Neyron.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

public class AI
{
    public static int[] skillsTotal = new int[4];
    private Random random = new Random();
    private Vector3 transform;
    Pixel pixel;

    public int foodSkill = 0;
    public int attackSkill = 0;
    public int defSkill = 0;
    public float energy = 100;
    public float age = 0;

    private int inputsCount = 4;
    private Genome genome;
    private NN nn;

    //private Rigidbody2D dot;

    // Start is called before the first frame update
    public AI(Pixel pixel)
    {
        this.pixel = pixel;
        //attackSkill = dot.Attack;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg - 90);
    //    age += Time.deltaTime;
    //}
    public void FixedUpdate(Pixel[] pixels)
    {
        transform = new Vector3(0f, 0f, (float)(Math.Atan2(pixel.Y, pixel.X) * (360 / (Math.PI * 2))) - 90);
        age++;
        //Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, vision);
        var colliders = new List<Pixel>();
        float vision = 5f + attackSkill;
        foreach (var item in pixels.Where(x => x.Id != pixel.Id))
        {
            if (pixel.DotClass != "food")
            {
                pixel.RectForm = new Rect(pixel.X, pixel.Y, vision, vision);
                if (pixel.RectForm.IntersectsWith(item.RectForm))
                    colliders.Add(item);
            }
        }

        float[] inputs = new float[inputsCount];

        // количество соседних объектов
        float[] neighboursCount = new float[4];

        // вектара к центрам масс еды, красного, зеленого и синего
        Vector3[] vectors = new Vector3[4];
        for (int i = 0; i < 4; i++)
        {
            neighboursCount[i] = 0;
            vectors[i] = new Vector3(0f, 0f, 0f);
        }
        for (int i = 0; i < colliders.Count; i++)
        {
            if (colliders[i].Id == pixel.Id) continue;
            if (colliders[i].DotClass == "food")
            {
                neighboursCount[0]++;
                //vectors[0] += targets[i].Position - transform;
                vectors[0] += colliders[i].Position - transform;
                //pixel.RectForm = new Rect(Canvas.GetLeft(pixel.Show()), Canvas.GetTop(pixel.Show()), pixel.Show().Width, pixel.Show().Height);
                //if (pixel.RectForm.IntersectsWith(pixels[i].RectForm))
                //{
                    FindFood(colliders[i]);
                //}

            }

            else if (colliders[i].DotClass == "bacterium")
            {
                AI ai = colliders[i].PixelAI;
                neighboursCount[1] += ai.attackSkill / 3f;
                vectors[1] += (colliders[i].Position - transform) * ai.attackSkill;
                neighboursCount[2] += ai.foodSkill / 3f;
                vectors[2] += (colliders[i].Position - transform) * ai.foodSkill;
                neighboursCount[3] += ai.defSkill / 3f;
                vectors[3] += (colliders[i].Position - transform) * ai.defSkill;
                //pixel.RectForm = new Rect(Canvas.GetLeft(pixel.Show()), Canvas.GetTop(pixel.Show()), pixel.Show().Width, pixel.Show().Height);
                //if (pixel.RectForm.IntersectsWith(pixels[i].RectForm))
                //{
                    Fight(colliders[i]);
                //}
            }
        }

        for (int i = 0; i < 4; i++)
        {
            if (neighboursCount[i] > 0)
            {
                var divide = neighboursCount[i] * vision;
                //vectors[i].X /= divide;
                //vectors[i].Y /= divide;
                vectors[i] /= divide;
                inputs[i] = MagnitudeVector3(vectors[i]);
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
                dir = Vector2.Normalize(dir);
                target += dir * outputs[i];
            }
        }
        if (MagnitudeVector2(target) > 1f)
            target = Vector2.Normalize(target);
        Vector2 velocity = new Vector2(pixel.Position.X, pixel.Position.Y);
        //var velocity = 10f;
        velocity += target * (0.25f + attackSkill * 0.05f);
        velocity *= 0.98f;
        velocity = Vector2.Normalize(velocity);
        pixel.X = velocity.X;
        pixel.Y = velocity.Y;
        pixel.Move();

        //float antibiotics = 1f;
        // концентрация антибиотиков
        // if(transform.position.x < -39) antibiotics = 4;
        // else if(transform.position.x < -20) antibiotics = 3;
        // else if(transform.position.x < -1) antibiotics = 2;
        // antibiotics = Mathf.Max(1f, antibiotics - defSkill);
        //energy -= age * antibiotics * antibiotics;
        energy--;
        if (energy < 0f & pixel.DotClass != "food")
        {
            pixel.Show().Fill = Brushes.Black;
            pixel.Health = 0;
            //Kill();
        }
    }

    private float MagnitudeVector2(Vector2 vector)
    {
        return (float)(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2));
    }

    private float MagnitudeVector3(Vector3 vector)
    {
        return (float)(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2) + Math.Pow(vector.Z, 2));
    }

    void FindFood(Pixel col)
    {
        if (foodSkill == 0) return;
        Eat(foodSkill);
        //Destroy(col.gameObject);
    }

    void Fight(Pixel enemy)
    {
        if (age < 1f) return;
        if (enemy.PixelAI.attackSkill == 0) return;
        if (enemy.DotClass == "bacterium")
        {
            AI ai = enemy.PixelAI;
            if (ai.age < 1f) return;
            float damage = Math.Max(0f, attackSkill - ai.defSkill);
            damage *= 4f;
            damage = Math.Min(damage, ai.energy);
            ai.energy -= damage * 1.25f;
            Eat(damage);
            if (ai.energy == 0f)
            {
                enemy.Show().Fill = Brushes.Black;
                enemy.Health = 0;
            }
            //ai.Kill();
        }
    }

    public void Init(Genome g)
    {
        genome = g;
        //var col = new SolidColorBrush().;
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
        transform = new Vector3(size, size, size);
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

    private void Eat(float food)
    {
        energy += food;
        if (energy > 16)
        {
            energy *= 0.5f;
            var b = new Pixel(new System.Windows.Shapes.Rectangle()
            {
                Height = 25,
                Width = 25,
                Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 0, (byte)random.Next(100, 256), 0))
            }, "bacterium")
            { Y = pixel.Y, X = pixel.X };
            b.Position += transform;
            b.DotClass = "bacterium";
            Genome g = new Genome(genome);
            g.Mutate(0.5f);
            //AI ai = b.GetComponent<AI>();
            pixel.PixelAI.Init(g);
            pixel.PixelAI.energy = energy;
        }
    }
}
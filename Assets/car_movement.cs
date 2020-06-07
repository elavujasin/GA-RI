using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
public class car_movement : MonoBehaviour
{
    private GameObject CarCopy;
    public GameObject Car;
    private GameObject CarCollider;
    private GameObject CarCollider1;
    private int parent1;
    private int parent2;
    private List<float> timeArray = new List<float>();
    public static int br = 0;
    public static int generation_br = 0;
    private Vector3 targetPos;
    private float speed = 0.2f;
    private int Car_count = 0;
    private int update_num = 0;
    public static bool flag = false;
    private int generation_num = 5;
    public GameObject text;
    public struct DNA
    {
        public int position;

    }
    public class CarDNA
    {
        public List<DNA> model;
        public double fitness;

    }
    public static int NumberOfPositions = 100;
    public List<DNA> CarPositions = new List<DNA>();
    public static List<CarDNA> Population = new List<CarDNA>();
    public List<CarDNA> PopulationCopy = new List<CarDNA>();
    public static List<CarDNA> BestCars = new List<CarDNA>();


   


    void Awake()
    {



        if (Car.name == "car")

        {
            for (int i = 0; i < 100; i++)
            {
                CarCopy = Instantiate(Car);
                CarCopy.name = "car" + i;
                Physics.IgnoreCollision(CarCopy.GetComponent<Collider>(), GetComponent<Collider>());
            }

            for (int i = 0; i < 100; i++)
                for (int j = 0; j < 100; j++)
                {
                    if (i != j)
                    {
                        CarCollider1 = GameObject.Find("car" + i);
                        CarCollider = GameObject.Find("car" + j);
                        Physics.IgnoreCollision(CarCollider1.GetComponent<Collider>(), CarCollider.GetComponent<Collider>());
                    }
                }

            if (flag)
                for (int i = 0; i < 100; i++)
                    GameObject.Find("car" + i).SetActive(false);






        }



        

    }

    // Start is called before the first frame update
    void Start()
    {
        Car_count = 0;
        br = 0;
        update_num = 0;
        Rigidbody car = GetComponent<Rigidbody>();
        targetPos = car.transform.position;
        if (!flag)
            text.GetComponent<UnityEngine.UI.Text>().text = "Generation: " + (generation_br + 1).ToString();

        else
            text.GetComponent<UnityEngine.UI.Text>().text = "Best car after " + generation_num.ToString() + " generations";
           
        DNA entity = new DNA();

        if (generation_br == 0)
            for (var i = 0; i < NumberOfPositions - 1; i++)
            {
                entity.position = Random.Range(0, 3);
                CarPositions.Add(entity);
            }


        else if (generation_br < generation_num)
        {


            if (Car.name == "car")
                Population[0].model.ForEach(x => CarPositions.Add(x));

            for (int i = 0; i < 100; i++)
                if (Car.name == ("car" + i))
                    Population[i + 1].model.ForEach(x => CarPositions.Add(x));

        }
        else
        {

            BestCars.Sort((p1, p2) => p1.fitness.CompareTo(p2.fitness));
            BestCars.Reverse();
            BestCars[0].model.ForEach(x => CarPositions.Add(x));
        }
    }

    // Update is called once per frame
    void Update()
    {


        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed); //* Time.fixedDeltaTime);



        if (Car_count < CarPositions.Count)
        {
            if (CarPositions[Car_count].position == 1)
                targetPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.75f);
            else if (CarPositions[Car_count].position == 0)
                targetPos = new Vector3(transform.position.x + 0.75f, transform.position.y, transform.position.z);
            else if (CarPositions[Car_count].position == 2)
                targetPos = new Vector3(transform.position.x - 0.75f, transform.position.y, transform.position.z);
        }

        else
        {
            if (CarPositions.ElementAt(CarPositions.Count - 1).position == 1)
                targetPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.75f);
            else if (CarPositions.ElementAt(CarPositions.Count - 1).position == 0)
                targetPos = new Vector3(transform.position.x + 0.75f, transform.position.y, transform.position.z);
            else if (CarPositions.ElementAt(CarPositions.Count - 1).position == 2)
                targetPos = new Vector3(transform.position.x - 0.75f, transform.position.y, transform.position.z);
        }

        if (update_num % 10 == 0)
            Car_count++;

        update_num++;


    }

    private void OnTriggerEnter(Collider other)
    {
        if (br == 0)
            Population.Clear();

        br++;
        if (flag)
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
        BestCars_fun();


        if (br == 101)
        {
            NaturalSelection();
            Crossover();
            Mutacija();
            generation_br++;
            if (generation_br == generation_num)
                flag = true;
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);

        }


    }

    void OnCollisionEnter(Collision e)
    {
        if (br == 0)
            Population.Clear();
        br++;
        if (flag)
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
        gameObject.SetActive(false);

        CreatePopulation();
        if (br == 101)
        {
            NaturalSelection();
            Crossover();
            Mutacija();
            generation_br++;
            if (generation_br == generation_num)
                flag = true;
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

    }
    void BestCars_fun()
    {
        CarDNA entity = new CarDNA();
        entity.model = new List<DNA>();
        entity.fitness = 200 - Car_count;
        CarPositions.ForEach(x => entity.model.Add(x));

        BestCars.Add(entity);
        Population.Add(entity);
        gameObject.SetActive(false);
    }
    void CreatePopulation()
    {
        double distance = System.Math.Sqrt(System.Math.Pow(transform.position.z - 6.23, 2));
        double distance_all = System.Math.Sqrt(System.Math.Pow(transform.position.z - 6.23, 2) + System.Math.Pow(transform.position.x + 12.68, 2));
        CarDNA entity = new CarDNA();
        entity.model = new List<DNA>();
        entity.fitness = -distance * 0.2 - distance_all * 0.8;
        CarPositions.ForEach(x => entity.model.Add(x));
        Population.Add(entity);
    }
    void NaturalSelection()
    {
        PopulationCopy.Clear();
        Population.ForEach(x => PopulationCopy.Add(x));
        PopulationCopy.Sort((p1, p2) => p1.fitness.CompareTo(p2.fitness));
        PopulationCopy.Reverse();
        Population.Clear();
        for (int i = 0; i < 30; i++)
            Population.Add(PopulationCopy[i]);
        
    }
    void Mutacija()
    {

        float mutation_probability = 0.7f;
        for (int i = 6; i < Population.Count; i++)
            for (int j = 0; j < Population[i].model.Count; j++)
            {
                DNA temp = new DNA();
                float probability = Random.Range(0f, 1f);
                if (probability > mutation_probability)
                 {
                    do
                    {
                        temp.position = Random.Range(0, 3);
                    } while (temp.position == Population[i].model[j].position);

                    Population[i].model[j] = temp;
                }
            }
    }
    void Crossover()
    {
        for (int i = 30; i < 101; i++)
        {
            List<CarDNA> parent_DNA = new List<CarDNA>();
            parent1 = Random.Range(0, 30);
            do
            {
                parent2 = Random.Range(0, 30);
            } while (parent1 == parent2);

            parent_DNA.Add(PopulationCopy[parent1]);
            parent_DNA.Add(PopulationCopy[parent2]);
            CarDNA Child = new CarDNA();
            Child.model = new List<DNA>();
            for (int j = 0; j < NumberOfPositions - 1; j++)
            {
                DNA child = new DNA();
                int num = Random.Range(0, 2);
                child.position = parent_DNA[num].model[j].position;
                Child.model.Add(child);

            }
            Population.Add(Child);
            Population[i].fitness = -1000.0;


        }
    }
}

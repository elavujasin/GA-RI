using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class car_movement : MonoBehaviour
{
    private Vector3 targetPos;
    private float speed = 0.2f;
    public float br=0;
    public struct DNA
    {
       public Position position;
       public float time;
    }
    List<DNA> Lista = new List<DNA>();
    public struct Model
    {   List<DNA> model;
        int fitness;
    }
    List<Model> Pokusaji = new List<Model>();
    public enum Position
    {
        x,
        z
    };
   public static List<Model> Pool = new List<Model>();
    // Start is called before the first frame update
    void Start()
    {
        br = 0;
    }

    // Update is called once per frame
    void Update()
    {
        br = Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed );
        //??
        Debug.Log(br);

       
       if(br>0.01) 
          targetPos = new Vector3(transform.position.x , transform.position.y, transform.position.z + 0.75f);
        else
            targetPos = new Vector3(transform.position.x + 0.75f, transform.position.y, transform.position.z);




    }
    void OnCollisionEnter(Collision e)
    {
        Debug.Log("collision");
      
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);

    }
}

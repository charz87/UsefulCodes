using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RayCastOculus : MonoBehaviour {

    public Image LoadingProgress;
    public float speed = 5;
    private float currentAmount = 0;

    Camera camera;

    Ray ray;

    void Start()
    {
        AudioMusic.PlayMenuMusic();
        camera = GetComponent<Camera>();
    }

    void Update()
    {
        
        ray = camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if(hit.transform.tag == "Trigger")
            {
               
                currentAmount += speed * Time.deltaTime;

                if (currentAmount >= 1)
                    SceneManager.LoadScene(1);
            }
        }
            

        else
            currentAmount = 0;

        LoadingProgress.GetComponent<Image>().fillAmount = currentAmount;
    }




}

using UnityEngine;
using UnityEngine.SceneManagement;


public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f; //rcs means reaction control system
    [SerializeField] float mainThrust = 100f;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
    }

    void OnCollisionEnter(Collision collision) //type Collision, variable collision
    {
        print("Collided");
        if (state != State.Alive) { return; } //ignore collisions when dead

            switch (collision.gameObject.tag) //tag of the game object e.g friendly, respawn, etc. use "default" if need to use without the tag
            {
                case "Friendly":

                    break;
                case "Finish":
                    state = State.Transcending;
                    Invoke("LoadNextLevel", 1f); //parameterise time
                    break;
                default:
                    print("Hit something deadly");
                    state = State.Dying;
                    Invoke("LoadFirstLevel", 1f);
                    break;
            }


    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1); // to do - make for more than 2 levels
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }


    private void Thrust()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust); //can thrust while rotating
            if (!audioSource.isPlaying) //so it doesnt layer
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true;
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false;

    }


}

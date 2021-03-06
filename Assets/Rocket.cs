using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f; //rcs means reaction control system
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

    bool collisionsDisabled = false;

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
            RespondToThrustInput();
            Rotate();
        }
        // to do only if debug on
        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
        
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            // toggle collision
            collisionsDisabled = !collisionsDisabled;
        }
    }

        void OnCollisionEnter(Collision collision) //type Collision, variable collision
        {
            print("Collided");
            if (state != State.Alive || collisionsDisabled) { return; }

            switch (collision.gameObject.tag) //tag of the game object e.g friendly, respawn, etc. use "default" if need to use without the tag
            {
                case "Friendly":
                    break;
                case "Finish":
                    StartSuccessSequence();
                    break;
                default:
                    StartDeathSequence();
                    break;
            }

        }

        private void StartSuccessSequence()
        {
            state = State.Transcending;
            successParticles.Play();
            audioSource.Stop();
            audioSource.PlayOneShot(success);
            Invoke("LoadNextLevel", levelLoadDelay); //parameterise time
        }

        private void StartDeathSequence()
        {
            state = State.Dying;
            deathParticles.Play();
            audioSource.Stop();
            audioSource.PlayOneShot(death);
            Invoke("LoadFirstLevel", levelLoadDelay);
        }



        private void LoadNextLevel()
        {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings) 
        {
            nextSceneIndex = 0;
        }
            SceneManager.LoadScene(nextSceneIndex); // to do - make for more than 2 levels
        }

        private void LoadFirstLevel()
        {
            SceneManager.LoadScene(0);
        }


        private void RespondToThrustInput()
        {
            if (Input.GetKey(KeyCode.W))
            {
                ApplyThrust();
            }
            else
            {
                audioSource.Stop();
                mainEngineParticles.Stop();
            }
        }

        private void ApplyThrust()
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust); //can thrust while rotating
            if (!audioSource.isPlaying) //so it doesnt layer
            {
                audioSource.PlayOneShot(mainEngine);
            }
            mainEngineParticles.Play();
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

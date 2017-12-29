using UnityEngine;


public class EnemyAnimation : MonoBehaviour
{
    public Animation Anim;
    private bool Grip;
    private Rigidbody2D _playerBody;


    // Use this for initialization
    void Start()
    {
        _playerBody = GetComponentInParent<Rigidbody2D>();
        Anim = GetComponent<Animation>();
        Anim.Play("Armature|Chill");
    }

    public Animation GetAnim()
    {
        return Anim;
    }

    public void PlayDeath()
    {
        Anim.Play("Armature|Death.001");
    }
    void PlayWalk()
    {
        Anim.Play("Armature|Action");
    }

    void PlayJump()
    {
        Anim.Play("Armature|Jump");
    }
    void PlayChill()
    {
        Anim.Play("Armature|Chill");
    }

    void PlayRun()
    {
        Anim.Play("Armature|Run");
    }

    void PlaySlide()
    {
        Anim.Play("Armature|SlideRight");
    }

    // Update is called once per frame
    void Update()
    {

        Grip = PlayerController._isWallGrip;

        //Anim.Play(Mathf.Abs(_playerBody.velocity.y) > 0.5f ? "Armature|Jump" : "Armature|Chill");

        if (Input.GetKey(KeyCode.Space))
        {
            Anim.Play("Armature|Jump");
        }

        else if (Grip == true)
        {
            Anim.Play("Armature|SlideRight");
        }

        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            Anim.Play("Armature|Run");
        }


        /*else if (Grip == true )
        {
            Anim.Play("Armature|SlideRight");
        }*/

        /*else if (Grip == true || ((Grip == true && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)))))
        {
            //Anim.Stop();
            Anim.Play("Armature|SlideRight");
        }*/

        else if (Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.LeftArrow))
        {
            Anim.Play("Armature|Jump");
        }

        else if (Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.RightArrow))
        {
            Anim.Play("Armature|Jump");
        }

        else if (Input.GetKey(KeyCode.S))
        {
            Anim.Play("Armature|Atack");
        }

        else if (Input.GetKey(KeyCode.D))
        {
            Anim.Play("Armature|Atack2");
        }

        else
        {
            Anim.Play("Armature|Chill");
        }

    }
}

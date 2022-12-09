using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrPlayer : MonoBehaviour
{

    #region Singleton
    public static scrPlayer manager;
    private void Awake()
    {
        manager = this;
    }
    #endregion

    [Header("Stats")]
    public float deadSpaceRange;
    public float slideSpeed;
    public float moveSpeed;

    [Header("Assignment")]
    public AudioClip[] soundEffects;
    public AudioSource speaker;
    public Animator anim;

    //Private Variables
    private Vector2 mousePosStarting;
    private Vector2 mousePosCurrent;
    private float dirX, dirY;
    private bool isWin, isLose, isPaused;
 
    private void DetectMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePosStarting = Input.mousePosition;
            dirX = 0;
            dirY = 0;
        }

        if (Input.GetMouseButton(0))
        {
            
            mousePosCurrent = Input.mousePosition;
            if (Mathf.Abs(mousePosCurrent.x - mousePosStarting.x) > deadSpaceRange)
            {

                if (mousePosCurrent.x > mousePosStarting.x) //right
                {
                    dirX = 1;
                }
                else //left
                {
                    dirX = -1;
                }


            }
            else
            {


                dirX = 0;
            }

            if (Mathf.Abs(mousePosCurrent.y - mousePosStarting.y) > deadSpaceRange)
            {

                if (mousePosCurrent.y > mousePosStarting.y) //right
                {
                    dirY = 1;
                }
                else //left
                {
                    dirY = -1;
                }


            }
            else
            {


                dirY = 0;
            }

            //if moved
            if(dirX !=0 || dirY != 0)
            {
                mousePosStarting = Input.mousePosition;
            }

        }

    }

    private void LeftRightMovement()
    {
        Vector3 moveDir = (Vector3.forward * (moveSpeed == 1 ? 1 : 0)) + (Vector3.right * dirX * slideSpeed);
        transform.Translate(moveDir * Time.deltaTime * moveSpeed);
    }

    private void MoveFourDirection()
    {
        transform.Translate(new Vector3(dirX, 0, dirY) * Time.deltaTime * slideSpeed);
    }

    private void MoveCamera(Vector3 _camMove, float _speed)
    {
        Camera.main.transform.Translate(_camMove * Time.deltaTime * _speed, Space.World);
    }

    public void PlaySoundEffect(int _soundId)
    {
        speaker.PlayOneShot(soundEffects[_soundId], 1);
    }

    public void Win()
    {
        anim.SetTrigger("Win");
    }

    public void Lose()
    {
        anim.SetTrigger("Lose");

    }
}

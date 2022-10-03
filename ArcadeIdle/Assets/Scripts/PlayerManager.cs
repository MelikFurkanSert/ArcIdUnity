using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public float speed;
    [SerializeField] private FloatingJoystick floatingJoystick;
    [SerializeField] private List<Transform> papers = new List<Transform>();
    [SerializeField] private GameObject paperPlace;
    private Rigidbody rb;
    private Animator anim;
    private float paperYAxis,paperJumpDelay;
    private Coroutine buyCoroutine = null;
    [SerializeField] private Vector2 paperRotationRange;
    [SerializeField] private float collectPaperSpeedXpos, collectPaperSpeedYpos , paperToDeskJumpPower , paperToDeskJumpDuration;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        papers.Add(paperPlace.transform);
    }

    public void FixedUpdate()
    {
        // Vector3 direction = Vector3.forward * floatingJoystick.Vertical + Vector3.right * floatingJoystick.Horizontal;
        //  rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange); */
        rb.velocity = new Vector3(floatingJoystick.Horizontal * speed, rb.velocity.y, floatingJoystick.Vertical * speed);

        if (floatingJoystick.Horizontal != 0 || floatingJoystick.Vertical != 0)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (papers.Count > 1)
            {
                anim.SetBool("isCarrying", true);
            }
            else
            {
                anim.SetBool("isCarrying", false);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            anim.SetBool("isWalking", false);
            if (papers.Count > 1)
            {
                anim.SetBool("isCarrying", true);
            }
            else
            {
                anim.SetBool("isCarrying", false);
            }
        }

        if (papers.Count > 1)
        {
            PapersMovement();
        }
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.forward, out var hit, 1f))
        {
            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y+0.5f,transform.position.z), transform.forward * 1f, Color.green);

            if (hit.collider.CompareTag("table") && papers.Count < 21)
            {
                if (hit.collider.transform.childCount > 2)
                {
                    var paper = hit.collider.transform.GetChild(0);
                    paper.rotation = Quaternion.Euler(paper.rotation.x, Random.Range(paperRotationRange.x, paperRotationRange.y), paper.rotation.z);
                    papers.Add(paper);
                    paper.parent = paperPlace.transform;

                    if (hit.collider.transform.parent.GetComponent<Printer>().CountPapers > 1)
                        hit.collider.transform.parent.GetComponent<Printer>().CountPapers--;

                    if (hit.collider.transform.parent.GetComponent<Printer>().YAxis > 0f)
                        hit.collider.transform.parent.GetComponent<Printer>().YAxis -= 0.07f;

                    anim.SetBool("isCarrying", true);
                    anim.SetBool("isWalking", false);
                }
            }
            if (hit.collider.CompareTag("Worker") && papers.Count > 1)
            {
                paperJumpDelay = 0;
                var WorkDesk = hit.collider.transform;
                PapersMoveToWorker(WorkDesk);
            }
        }
        else
        {
            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.forward * 1f, Color.red);
        }
    }
    void PapersMovement()
    {
          for (int i = 1; i < papers.Count; i++)
          {
              var firstPaper = papers.ElementAt(i - 1);
              var secondPaper = papers.ElementAt(i);
              if (secondPaper.position != new Vector3 (firstPaper.position.x , firstPaper.position.y+0.07f , firstPaper.position.z))
              {
                  secondPaper.position = new Vector3(Mathf.Lerp(secondPaper.position.x, firstPaper.position.x, Time.deltaTime * collectPaperSpeedXpos),
                  Mathf.Lerp(secondPaper.position.y, firstPaper.position.y + 0.07f, Time.deltaTime * 15f),
                  Mathf.Lerp(secondPaper.position.z, firstPaper.position.z, Time.deltaTime * collectPaperSpeedYpos));

            }
          } 
    }
    void PapersMoveToWorker(Transform WorkDesk)
    {
       
        

        if (WorkDesk.childCount > 0)
        {
            paperYAxis = WorkDesk.GetChild(WorkDesk.childCount - 1).position.y;
        }
        else
        {
            paperYAxis = WorkDesk.position.y;
        }

        for (var index = papers.Count - 1; index >= 1; index--)
        {
            papers[index].DOJump(new Vector3(WorkDesk.position.x, paperYAxis, WorkDesk.position.z), paperToDeskJumpPower, 1, paperToDeskJumpDuration)
                .SetDelay(paperJumpDelay).SetEase(Ease.Flash);

            papers.ElementAt(index).parent = WorkDesk;
            papers.RemoveAt(index);

            paperYAxis += 0.07f;
            paperJumpDelay += (paperToDeskJumpDuration/10);
        }

        WorkDesk.GetComponent<workDesk>().Work();

        WorkDesk.parent.GetChild(WorkDesk.parent.childCount - 2).gameObject.SetActive(false);

        if (papers.Count <= 1)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isCarrying", false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("dollar"))
        {
            other.GetComponent<CollectableCodes>().SetCollected();
          //  MoneyUI.instance.AddCount(5);
        }
        if (other.CompareTag("BuyArea"))
        {
           buyCoroutine = StartCoroutine(other.GetComponent<BuyArea>().BuyWithDelay());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BuyArea"))
        {
            if (other != null)
            {
                if (buyCoroutine != null)
                {
                    StopCoroutine(buyCoroutine);

                }
            }
        }
    }
}
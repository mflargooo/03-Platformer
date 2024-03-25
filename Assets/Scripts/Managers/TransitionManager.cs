using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    [SerializeField] private PlayerController2D playerController;
    [SerializeField] private GameObject blackPanel;
    private static Animator bpAnim;
    private static PlayerController2D pc;
    private static Rigidbody2D playerRB;
    public static Transform playerTransform;
    public static bool isTransitioning;
    // Start is called before the first frame update
    void Start()
    {
        bpAnim = blackPanel.GetComponent<Animator>();
        pc = playerController;
        playerTransform = pc.transform;
        playerRB = pc.GetComponent<Rigidbody2D>();
    }

    public static IEnumerator DoTransition(Transform target, GameObject levelFrom, GameObject levelTo, int lookDir)
    {
        isTransitioning = true;
        bpAnim.Play("FadeOut");
        pc.enabled = false;
        playerRB.gravityScale = 2;
        playerTransform.localScale = new Vector3(lookDir, playerTransform.localScale.y, playerTransform.localScale.z);
        playerRB.velocity = Vector2.zero;
        playerRB.velocity = Vector2.right * pc.GetSpeed() * lookDir;

        yield return new WaitForSeconds(bpAnim.GetCurrentAnimatorStateInfo(0).length);
        if (levelFrom) levelFrom.SetActive(false);
        if (levelTo) levelTo.SetActive(true);
        playerTransform.GetChild(0).transform.eulerAngles = Vector3.zero;
        playerTransform.GetChild(0).GetComponent<Animator>().SetFloat("Speed X", pc.GetSpeed());
        playerTransform.GetChild(0).GetComponent<Animator>().SetFloat("Velocity Y", 0);
        playerTransform.GetChild(0).GetComponent<Animator>().SetBool("isGrounded", true);
        playerTransform.GetChild(0).GetComponent<Animator>().Play("player_running");
        pc.transform.position = target.position + Vector3.right * lookDir;
        playerRB.velocity = Vector2.right * pc.GetSpeed() * lookDir;

        yield return new WaitForSeconds(.75f);
        bpAnim.Play("FadeIn");
        yield return new WaitForSeconds(.25f);

        playerRB.gravityScale = 0;
        pc.enabled = true;
        isTransitioning = false;

    }
}

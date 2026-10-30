using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
public class Transition : MonoBehaviour
{
    [SerializeField] private Animator transitionAnimator;
    [SerializeField] Transform spawn;
    [SerializeField] MenuController menu;
    [SerializeField] float fade = 0.5f;
    [SerializeField] CinemachineCamera cam;
    [SerializeField] PolygonCollider2D mapBoundary;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            StartCoroutine(FadeRoutine(player));
        }
    }
    private IEnumerator FadeRoutine(Player player)
    {
        player.enabled = false;
        menu.enabled = false;
        if (player.TryGetComponent(out Rigidbody2D rb))
        {
            rb.linearVelocity = Vector2.zero;
        }
        transitionAnimator.SetTrigger("StartTransition");
        transitionAnimator.SetTrigger("EndTransition");
        yield return new WaitForSeconds(fade);
        Vector3 oldPosition = player.transform.position;
        player.transform.position = spawn.position;
        CinemachineConfiner2D confiner = cam.GetComponent<CinemachineConfiner2D>();
        float dampingOriginal = confiner.Damping;
        float slowingOriginal = confiner.SlowingDistance;
        confiner.Damping = 0f;
        confiner.SlowingDistance = 0f;
        confiner.BoundingShape2D = mapBoundary;
        confiner.InvalidateBoundingShapeCache();
        cam.OnTargetObjectWarped(player.transform, spawn.position - oldPosition);
        yield return null;
        confiner.Damping = dampingOriginal;
        confiner.SlowingDistance = slowingOriginal;
        player.enabled = true;
        menu.enabled = true;
    }
}

using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region 내부 
    // 플레이어 
    public float moveSpeed;
    public int pooptakeCount = 0;
    private int maxpooptakeCount = 5;
    [SerializeField] private float cleaningRange;
    [SerializeField] float slowSpeed = 2f;
    [SerializeField] float slowTime = 2f;
    private Transform player;
    private Vector3 moveInput;
    // 컴포넌트 연결 
    private Rigidbody rigidbody;
    private Animator animator;
    // 싱글톤
    public static Player Instance;
    #endregion

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // 컴포넌트 연결 
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // 커서 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 플레이어 초기화
        player = transform;
    }

    void Update()
    {
        //플레이 중 일때만 이동 가능
        if (GameManager.Instance.currentGameDirection == GameManager.GameDirection.Playing) HandleMovement();

        // 똥 먹은 개수가 최대치에 도달하면 엄마 화남 게이지 조정
        if (pooptakeCount == maxpooptakeCount)
        {
            Mom.Instance.ReduceAngryGaze();
            pooptakeCount = 0;

            // Icon 초기화
            CountPoop.Instance.CountIcon(pooptakeCount);
        }
    }

    #region 이동 로직 / 물리 처리 
    void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // 이동 방향을 플레이어가 바라보는 방향 기준으로 변환
        moveInput = (transform.forward * verticalInput + transform.right * horizontalInput).normalized;
    }

    void FixedUpdate()
    {
        if (moveInput != Vector3.zero)
        {
            Vector3 targetVelocity = moveInput * moveSpeed;
            targetVelocity.y = rigidbody.linearVelocity.y;
            rigidbody.linearVelocity = targetVelocity;

            // 애니메이션
            animator.SetBool("isWalk", true);
        }
        else
        {
            rigidbody.linearVelocity = new Vector3(0, rigidbody.linearVelocity.y, 0);

            // 애니메이션
            animator.SetBool("isWalk", false);
        }
    }
    #endregion

    #region 플레이어 똥 밟음
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("poop"))
        {
            StartCoroutine(PlayerMoveSlow());
            pooptakeCount += 1;
            CountPoop.Instance.CountIcon(pooptakeCount);
        }
    }

    IEnumerator PlayerMoveSlow()
    {
        moveSpeed = slowSpeed;
        yield return new WaitForSeconds(slowTime);
        moveSpeed = 5f;
    }
    #endregion

    #region 게임 승리 / 오버
    public void Victroy()
    {
        IsNotPlaying();

        // 애니메이션
        animator.SetBool("isWalk", false);
    }

    public void GameOver()
    {
        IsNotPlaying();

        // 애니메이션
        animator.SetBool("isWalk", false);
        animator.SetTrigger("isDeath");
    }

    // 커서 / 물리 처리 
    void IsNotPlaying()
    {
        // 1. 발생할 입력 막기 
        moveInput = Vector3.zero;

        // 2. 마우스 커서 보이기
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 3. 물리 무시 
        rigidbody.linearVelocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }
    #endregion

    // Gizmo
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, cleaningRange);
    }
}

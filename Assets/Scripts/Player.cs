using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{

    [Header("Settings")]
    public float JumpForce;


    [Header("References")]
    public Rigidbody2D PlayerRigidBody;

    public Animator PlayerAnimator;


    private bool isGrounded = true;

    private int lives = 3;
    private bool isInvincible = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 유저가 스페이스바를 누름 && 땅에 있는지?
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {

            PlayerRigidBody.AddForceY(JumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            PlayerAnimator.SetInteger("state", 1);
        }
    }


    /* 플레이어 생명 감소 */
    void Hit(){
        lives -= 1;
    }

    /* 플레이어 생명 증가 */
    void Heal(){
        // 생명은 최대 3(초기값)으로 제한
        lives = Mathf.Min(3,lives + 1);
    }

    /* 무적(황금배추) 시작 */
    void StartInvincible(){
        isInvincible = true;
        // 5초 뒤, StopInvincible 메소드 실행  
        Invoke("StopInvincible", 5f);
    }

    /* 무적(황금배추) 정지 */   
    void StopInvincible(){
        isInvincible = false;
    }

    /**
        플레이어 캐릭터 점프 및 착지 애니메이션을 위한 상태값
    */
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌하는 곳이 Platform이라면?
        if(collision.gameObject.name == "Platform")
        {
            if(!isGrounded)
            {
                PlayerAnimator.SetInteger("state", 2);
            }
            isGrounded = true;
        }
    }

    /*
        플레이어 캐릭터와 오브젝트가 충돌 이벤트 발생 시, 
    */
    void OgerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "enemy")
        {
            if(!isInvincible){
                Destroy(collider.gameObject);
            }
            Hit();
        }
        else if(collider.gameObject.tag == "food")
        {
            Destroy(collider.gameObject);
            Heal();

        }
        else if (collider.gameObject.tag == "golden")
        {
            Destroy(collider.gameObject);
            StartInvincible();
        }
        
    }
}

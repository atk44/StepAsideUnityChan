using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnityChanController : MonoBehaviour
{
    //アニメーションするためのコンポーネントを入れる
    private Animator myAnimator;
    //Unityちゃんを移動させるコンポーネントを入れる（追加）
    private Rigidbody myRigidbody;
    //前進するための力
    private float forwardForce = 800.0f;

    private float turnForce = 500.0f;
    private float movableRange = 3.4f;
    //ジャンプするための力
    private float upForce = 500.0f;
    //動きを減速するための関数
    private float coefficient = 0.95f;

    private bool isEnd = false;

    private GameObject stateText;
    private GameObject scoreText;
    private int score = 0;

    //左ボタン判定
    private bool isLButtonDown = false;
    //右ボタン判定
    private bool isRButtonDown = false;

    // Use this for initialization
    void Start()
    {

        //アニメータコンポーネントを取得
        this.myAnimator = GetComponent<Animator>();

        //走るアニメーションを開始
        this.myAnimator.SetFloat("Speed", 1);

        //Rigidbodyコンポーネントを取得
        this.myRigidbody = GetComponent<Rigidbody>();

        //シーン中のstateTextオブジェクトを取得
        this.stateText = GameObject.Find("GameResultText");

        this.scoreText = GameObject.Find("ScoreText");
    }

    // Update is called once per frame
    void Update()
    {

        //isEnd=TrueでUnityちゃんの動きを減速
        if (this.isEnd)
        {
            this.forwardForce *= this.coefficient;
            this.turnForce *= this.coefficient;
            this.upForce *= this.coefficient;
            this.myAnimator.speed *= this.coefficient;
        }

        //Unityちゃんに前方向の力を加える
        this.myRigidbody.AddForce(this.transform.forward * this.forwardForce);

        if((Input.GetKey(KeyCode.LeftArrow) || this.isLButtonDown) && -this.movableRange < this.transform.position.x)
        {
            this.myRigidbody.AddForce(-this.turnForce, 0, 0);
        } else if ((Input.GetKey(KeyCode.RightArrow) || this.isRButtonDown) && this.transform.position.x < this.movableRange)
        {
            this.myRigidbody.AddForce(this.turnForce, 0, 0);

        }

        //Jumpステートの場合はJumpにfalseをセットする（追加）
        if (this.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            this.myAnimator.SetBool("Jump", false);
        }

        //ジャンプしていない時にスペースが押されたらジャンプする
        if (Input.GetKeyDown(KeyCode.Space) && this.transform.position.y < 0.5f)
        {
            //ジャンプアニメを再生（追加）
            this.myAnimator.SetBool("Jump", true);
            //Unityちゃんに上方向の力を加える（追加）
            this.myRigidbody.AddForce(this.transform.up * this.upForce);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "CarTag" || other.gameObject.tag == "TrafficConeTag")
        {
            this.isEnd = true;
            //stateTextにGAMEOVERを表示
            this.stateText.GetComponent<Text>().text = "GAME OVER";
        }

        if(other.gameObject.tag == "GoalTag")
        {
            this.isEnd = true;
            //stateTextにCLEAR!!を表示
            this.stateText.GetComponent<Text>().text = "CLEAR!!";
        }

        if(other.gameObject.tag == "CoinTag")
        {
            //スコアを加算
            this.score += 10;
            this.scoreText.GetComponent<Text>().text = "Score " + this.score + "pt";
            GetComponent<ParticleSystem>().Play();
            Destroy(other.gameObject);
        }
    }

    //ジャンプボタンを押した場合の処理
    public void GetMyJumpButtonDown()
    {
        if (this.transform.position.y < 0.5f)
        {
            this.myAnimator.SetBool("Jump", true);
            this.myRigidbody.AddForce(this.transform.up * this.upForce);
        }
    }

    //左ボタンを押し続けた場合の処理
    public void GetMyLeftButtonDown()
    {
        this.isLButtonDown = true;
    }
    //左ボタンを離した場合の処理
    public void GetMyLeftButtonUp()
    {
        this.isLButtonDown = false;
    }

    //右ボタンを押し続けた場合の処理
    public void GetMyRightButtonDown()
    {
        this.isRButtonDown = true;
    }
    //右ボタンを離した場合の処理
    public void GetMyRightButtonUp()
    {
        this.isRButtonDown = false;
    }
}